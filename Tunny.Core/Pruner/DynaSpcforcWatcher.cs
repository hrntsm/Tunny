using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tunny.Core.Pruner
{
    public class DynaSpcforcWatcher : ResultWatcherBase
    {
        private static readonly List<DynaSpcForces> _reactions = new List<DynaSpcForces>();

        public DynaSpcforcWatcher(string processName, string targetFilePath, double watchInterval)
        : base(processName, targetFilePath, watchInterval)
        {
        }

        public override void Start()
        {
            Process[] dashboardProcess = Process.GetProcessesByName(ProcessName);
            if (dashboardProcess.Length == 0)
            {
                throw new InvalidOperationException("The process is not running.");
            }

            var watcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(TargetFilePath),
                Filter = Path.GetFileName(TargetFilePath),
            };

            watcher.Changed += OnChanged;
            watcher.EnableRaisingEvents = true;
        }

        protected override void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine($"File {e.FullPath} {e.ChangeType}");
            var reaction = new DynaSpcForces();

            var lines = new List<string>();
            using (var fs = new FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(fs))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
            }

            const string timePattern = @"output at time =\s*([0-9.E+-]+)";
            const string forcePattern = @"node=\s*(\d+)\s+local\s+x,y,z\s+forces\s+=\s+(-?\d+(?:\.\d+)?(?:E[+-]?\d+)?)\s+(-?\d+(?:\.\d+)?(?:E[+-]?\d+)?)\s+(-?\d+(?:\.\d+)?(?:E[+-]?\d+)?)\s+setid=\s*(\d+)";
            const string momentPattern = @"node=\s*(\d+)\s+local\s+x,y,z\s+moments=\s+(-?\d+(?:\.\d+)?(?:E[+-]?\d+)?)\s+(-?\d+(?:\.\d+)?(?:E[+-]?\d+)?)\s+(-?\d+(?:\.\d+)?(?:E[+-]?\d+)?)\s+setid=\s*(\d+)";
            const string resultPattern = @"force resultants\s*=\s*-?\d+(\.\d+)?(E[+-]?\d+)?\s*-?\d+(\.\d+)?(E[+-]?\d+)?\s*-?\d+(\.\d+)?(E[+-]?\d+)?";

            bool isTimeRead = false;

            foreach (string line in lines)
            {
                Match timeMatch = Regex.Match(line, timePattern);
                Match forceMatch = Regex.Match(line, forcePattern);
                Match momentMatch = Regex.Match(line, momentPattern);

                if (timeMatch.Success)
                {
                    var times = _reactions.Select(x => x.Time).ToList();
                    double time = double.Parse(timeMatch.Groups[1].Value, CultureInfo.InvariantCulture);
                    if (times.Count > 0)
                    {
                        double lastTime = times.Last();
                        if (time <= lastTime)
                        {
                            isTimeRead = true;
                            continue;
                        }
                        else
                        {
                            isTimeRead = false;
                        }
                    }
                    reaction.Time = time;
                    continue;
                }

                if (isTimeRead)
                {
                    continue;
                }

                if (forceMatch.Success)
                {
                    NodeResult nodeResult = reaction.NodeResults
                        .FirstOrDefault(x => x.NodeId == int.Parse(forceMatch.Groups[1].Value, CultureInfo.InvariantCulture));
                    if (nodeResult != null)
                    {
                        nodeResult.Force = new double[]
                        {
                            double.Parse(forceMatch.Groups[2].Value, CultureInfo.InvariantCulture),
                            double.Parse(forceMatch.Groups[3].Value, CultureInfo.InvariantCulture),
                            double.Parse(forceMatch.Groups[4].Value, CultureInfo.InvariantCulture),
                        };
                    }
                    else
                    {
                        reaction.NodeResults.Add(new NodeResult
                        {
                            NodeId = int.Parse(forceMatch.Groups[1].Value, CultureInfo.InvariantCulture),
                            Force = new double[]
                            {
                                double.Parse(forceMatch.Groups[2].Value, CultureInfo.InvariantCulture),
                                double.Parse(forceMatch.Groups[3].Value, CultureInfo.InvariantCulture),
                                double.Parse(forceMatch.Groups[4].Value, CultureInfo.InvariantCulture),
                            },
                            SetId = int.Parse(forceMatch.Groups[5].Value, CultureInfo.InvariantCulture),
                        });
                    }
                }

                if (momentMatch.Success)
                {
                    NodeResult nodeResult = reaction.NodeResults
                        .FirstOrDefault(x => x.NodeId == int.Parse(momentMatch.Groups[1].Value, CultureInfo.InvariantCulture));
                    if (nodeResult != null)
                    {
                        nodeResult.Moment = new double[]
                        {
                            double.Parse(momentMatch.Groups[2].Value, CultureInfo.InvariantCulture),
                            double.Parse(momentMatch.Groups[3].Value, CultureInfo.InvariantCulture),
                            double.Parse(momentMatch.Groups[4].Value, CultureInfo.InvariantCulture),
                        };
                    }
                    else
                    {
                        reaction.NodeResults.Add(new NodeResult
                        {
                            NodeId = int.Parse(momentMatch.Groups[1].Value, CultureInfo.InvariantCulture),
                            Moment = new double[]
                            {
                                double.Parse(momentMatch.Groups[2].Value, CultureInfo.InvariantCulture),
                                double.Parse(momentMatch.Groups[3].Value, CultureInfo.InvariantCulture),
                                double.Parse(momentMatch.Groups[4].Value, CultureInfo.InvariantCulture),
                            },
                            SetId = int.Parse(momentMatch.Groups[5].Value, CultureInfo.InvariantCulture),
                        });
                    }
                    continue;
                }

                if (Regex.IsMatch(line, resultPattern))
                {
                    _reactions.Add(reaction);
                    if (_reactions.Last().GetForceSum(Direction.ABS, 0) > 5000)
                    {
                        TargetProcessState = TargetProcessState.Stopped;
                    }
                    reaction = new DynaSpcForces();
                    continue;
                }
            }
        }

        class DynaSpcForces
        {
            public double Time { get; set; }
            public List<NodeResult> NodeResults { get; set; } = new List<NodeResult>();

            public double GetForceSum(Direction dir, int setId)
            {
                IEnumerable<NodeResult> resultSet = NodeResults.Where(x => x.SetId == setId);

                switch (dir)
                {
                    case Direction.X:
                        return resultSet.Sum(x => x.Force[0]);
                    case Direction.Y:
                        return resultSet.Sum(x => x.Force[1]);
                    case Direction.Z:
                        return resultSet.Sum(x => x.Force[2]);
                    default:
                        double xDir = resultSet.Sum(x => x.Force[0]);
                        double yDir = resultSet.Sum(x => x.Force[1]);
                        double zDir = resultSet.Sum(x => x.Force[2]);
                        return Math.Sqrt(xDir * xDir + yDir * yDir + zDir * zDir);
                }
            }
        }

        enum Direction
        {
            X,
            Y,
            Z,
            ABS,
        }

        class NodeResult
        {
            public int NodeId { get; set; }
            public int SetId { get; set; }
            public double[] Force { get; set; } = new double[3];
            public double[] Moment { get; set; } = new double[3];
        }
    }
}
