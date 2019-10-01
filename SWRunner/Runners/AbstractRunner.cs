using SWEmulator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SWRunner.Runners
{
    public abstract class AbstractRunner<T> : IRunner where T : RunnerConfig
    {
        public string LogFile { get; private set; }
        public T RunnerConfig { get; private set; }
        public AbstractEmulator Emulator { get; private set; }
        public DateTime modifiedTime { get; private set; } = DateTime.Now;

        public AbstractRunner(string logFile, T runnerConfig, AbstractEmulator emulator)
        {
            LogFile = logFile;
            Emulator = emulator;
            RunnerConfig = runnerConfig;
            Helper.UpdateRunConfig(emulator, runnerConfig);
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
