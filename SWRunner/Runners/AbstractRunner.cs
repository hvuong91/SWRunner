using SWEmulator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace SWRunner.Runners
{
    public abstract class AbstractRunner<T> : IRunner where T : RunnerConfig
    {
        public string LogFile { get; private set; }
        public T RunnerConfig { get; private set; }
        public AbstractEmulator Emulator { get; private set; }
        public DateTime modifiedTime { get; protected set; }

        public int MinEnergyRequired { get;  protected set; }
        public TimeSpan MaxRunTime { get; protected set; }
        

        public AbstractRunner(string logFile, T runnerConfig, AbstractEmulator emulator)
        {
            LogFile = logFile;
            Emulator = emulator;
            RunnerConfig = runnerConfig;
            Helper.UpdateRunConfig(emulator, runnerConfig);
        }

        public void CheckRefill()
        {
            if (NeedRefill())
            {
                // TODO: Refill
                Thread.Sleep(20000);


                Emulator.Click(RunnerConfig.ReplayPoint);
            }
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

        public bool IsFailed()
        {
            // Check last modification timestamp of log file
            DateTime lastModifiedTime = File.GetLastWriteTime(LogFile);
            return (DateTime.Now - lastModifiedTime) > MaxRunTime;
        }

        public abstract void Run();

        public void SkipRevive()
        {
            // TODO: Some runnners won't support this
            // TODO: Random click to pop up revive dialog
            Thread.Sleep(1000);
            Emulator.Click(RunnerConfig.NoRevivePoint); //TODO: This should be random click

            Thread.Sleep(1000);
            Emulator.Click(RunnerConfig.NoRevivePoint);

            Thread.Sleep(2000);
        }

        public void StartNewRun()
        {
            // TODO: Some runners won't support this
            Thread.Sleep(3000);
            RandomSleep();
            Emulator.Click(RunnerConfig.ReplayPoint);

            // Click twice to pop up replay option
            RandomSleep();
            CheckRefill();

            RandomSleep();
            Emulator.Click(RunnerConfig.StartPoint);
        }

        protected bool NeedRefill()
        {
            return GetCurrentEnergy() < MinEnergyRequired;
        }

        protected int GetCurrentEnergy()
        {
            // TODO

            return 10;
        }

        protected void RandomSleep()
        {
            int randomWaitTime = new Random().Next(200, 1000);
            Thread.Sleep(randomWaitTime);
        }
    }
}
