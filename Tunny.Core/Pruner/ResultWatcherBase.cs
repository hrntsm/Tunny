using System.IO;

namespace Tunny.Core.Pruner
{
    public abstract class ResultWatcherBase
    {
        internal readonly string ProcessName;
        internal readonly string TargetFilePath;
        internal readonly double WatchInterval;
        internal TargetProcessState TargetProcessState;

        public ResultWatcherBase(string processName, string targetFilePath, double watchInterval)
        {
            ProcessName = processName;
            TargetFilePath = targetFilePath;
            WatchInterval = watchInterval;
        }

        public abstract void Start();
        protected abstract void OnChanged(object source, FileSystemEventArgs e);
    }

    public enum TargetProcessState
    {
        Running,
        Stopped
    }
}
