using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Optuna.Storage;
using Optuna.Study;
using Optuna.Trial;

using Tunny.Core.PostProcess;
using Tunny.Core.Util;
using Tunny.Handler;
using Tunny.UI;

namespace Tunny.Core.Solver
{
    public class Output
    {
        private readonly string _storagePath;

        public Output(string storagePath)
        {
            _storagePath = storagePath;
        }

        public ModelResult[] GetModelResult(int[] resultNum, string studyName, BackgroundWorker worker)
        {
            TLog.MethodStart();
            var modelResult = new List<ModelResult>();
            IOptunaStorage storage = GetStorage();
            Study targetStudy = storage.GetAllStudies().FirstOrDefault(s => s.StudyName == studyName);
            if (targetStudy == null)
            {
                TunnyMessageBox.Show("There are no output models. Please check study name.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return modelResult.ToArray();
            }
            SetTrialsToModelResult(resultNum, modelResult, targetStudy, worker);
            return modelResult.ToArray();
        }

        private IOptunaStorage GetStorage()
        {
            string ext = Path.GetExtension(_storagePath);
            IOptunaStorage storage;
            if (ext == ".db" || ext == ".sqlite")
            {
                storage = new Optuna.Storage.RDB.SqliteStorage(_storagePath, true);
            }
            else if (ext == ".log")
            {
                storage = new Optuna.Storage.Journal.JournalStorage(_storagePath, true);
            }
            else
            {
                throw new ArgumentException("Storage type not supported");
            }

            return storage;
        }

        private static void SetTrialsToModelResult(int[] resultNum, List<ModelResult> modelResult, Study study, BackgroundWorker worker)
        {
            TLog.MethodStart();
            if (resultNum[0] == -1)
            {
                ParatoSolutions(modelResult, study, worker);
            }
            else if (resultNum[0] == -10)
            {
                AllTrials(modelResult, study, worker);
            }
            else
            {
                UseModelNumber(resultNum, modelResult, study, worker);
            }
        }

        private static void UseModelNumber(IReadOnlyList<int> resultNum, List<ModelResult> modelResult, Study study, BackgroundWorker worker)
        {
            TLog.MethodStart();
            var trials = new List<Trial>();
            for (int i = 0; i < resultNum.Count; i++)
            {
                int res = resultNum[i];
                try
                {
                    trials.Add(study.Trials.FirstOrDefault(t => t.Number == res));
                }
                catch (Exception e)
                {
                    TunnyMessageBox.Show("Error\n\n" + e.Message, "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            SetAndReportModelResult(modelResult, worker, trials);
        }

        private static void AllTrials(List<ModelResult> modelResult, Study study, BackgroundWorker worker)
        {
            TLog.MethodStart();
            List<Trial> trials = study.Trials;
            SetAndReportModelResult(modelResult, worker, trials);
        }

        private static void ParatoSolutions(List<ModelResult> modelResult, Study study, BackgroundWorker worker)
        {
            TLog.MethodStart();
            Trial[] bestTrials = study.BestTrials;
            SetAndReportModelResult(modelResult, worker, bestTrials.ToList());
        }

        private static void SetAndReportModelResult(List<ModelResult> modelResult, BackgroundWorker worker, List<Trial> trials)
        {
            for (int i = 0; i < trials.Count; i++)
            {
                Trial trial = trials[i];
                if (OutputLoop.IsForcedStopOutput)
                {
                    break;
                }
                modelResult.Add(new ModelResult(trial));
                worker?.ReportProgress(i * 100 / trials.Count);
            }
        }
    }
}
