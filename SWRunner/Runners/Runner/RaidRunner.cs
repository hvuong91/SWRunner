using SWEmulator;
using SWRunner.Filters;
using SWRunner.Rewards;
using System;
using System.Diagnostics;
using System.Threading;

namespace SWRunner.Runners
{
    public class RaidRunner : AbstractRunner<RaidRunnerConfig>
    {
        RaidFilter Filter { get; set; }

        public RaidRunner(RaidFilter filter, string logFile, string fullLogFile, RaidRunnerConfig runnerConfig,
            AbstractEmulator emulator, RunnerLogger logger) : base(logFile, fullLogFile, runnerConfig, emulator, logger)
        {
            Filter = filter;
            MinEnergyRequired = 8;
            MaxRunTime = new TimeSpan(0, 2, 0); //This would be overriden
        }

        public override void Collect()
        {
            // Random click twice to pop up the reward dialog
            Thread.Sleep(15000); // Wait for end animation
            Emulator.RandomClick();

            //Thread.Sleep(1500); // wait for treasure box to pop up
            //Emulator.RandomClick();

            //Thread.Sleep(2500); // wait for reward to pop up


            RunResult runResult = Helper.GetRunResult(LogFile);
            Reward reward = Helper.GetReward(runResult);

            bool getReward = Filter.ShouldGet(reward);

            Debug.WriteLine($"Should get {reward}: {getReward}");

            if (!getReward && typeof(GemStone).IsAssignableFrom(reward.GetType()))
            {
                Emulator.Click(RunnerConfig.SellStoneGemPoint);
                Thread.Sleep(1000);
                Emulator.Click(RunnerConfig.ConfirmStoneGemRunePoint);
                
            }
            else
            {
                Emulator.PressEsc();
            }

        }

        public override void Run()
        {
            // 1. Check run ends
            // 2. If succeed, collect reward. Otherwise, ignore
            // 3. Start new run
            // 4. Wait for party ready if play as host

            ModifiedTime = DateTime.Now;

            while (!Stop)
            {
                Debug.WriteLine("Checking raid run status ...");
                Thread.Sleep(3000);

                if (IsEnd())
                {
                    Debug.WriteLine("Collecting reward");
                    Collect();
                }
                else
                {
                    // Run is not completed yet
                    continue;
                }

                StartNewRun();

                Thread.Sleep(5000); // wait till it's auto ready
                break;
            }
        }

        public override void StartNewRun()
        {
            Thread.Sleep(2000);
            RandomSleep();
            Emulator.Click(RunnerConfig.ReplayPoint);

            Thread.Sleep(3500); // ensure refill window is pop up
            CheckRefill();

            RandomSleep();
            Thread.Sleep(3500); // wait till start button is enabled
            Emulator.Click(RunnerConfig.StartPoint);
        }

    }
}
