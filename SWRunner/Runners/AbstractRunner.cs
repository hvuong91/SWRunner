using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SWRunner.Runners
{
    public abstract class AbstractRunner : IRunner
    {
        public string LogFile { get; private set; }
        public DateTime modifiedTime { get; private set; } = DateTime.Now;

        public AbstractRunner(string logFile)
        {
            LogFile = logFile;
        }

        public void CheckRefill()
        {
            throw new NotImplementedException();
        }

        public abstract void Collect();

        public bool IsEnd()
        {
            // Check last modification timestamp of log file
            DateTime lastModifiedTime = File.GetLastWriteTime(LogFile);
            if (lastModifiedTime > modifiedTime)
            {
                modifiedTime = lastModifiedTime;
                return true;
            }
            return false;
        }

        public abstract bool IsFailed();

        public abstract void Run();

        public void SkipRevive()
        {
            throw new NotImplementedException();
        }

        public void StartNewRun()
        {
            throw new NotImplementedException();
        }
    }
}
