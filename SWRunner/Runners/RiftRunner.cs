using SWEmulator;
using SWRunner.Filters;
using SWRunner.Rewards;
using System;
using System.Diagnostics;
using System.Threading;

namespace SWRunner.Runners
{
    public class RiftRunner : AbstractRunner<RiftRunnerConfig>
    {
        private RiftFilter Filter { get; set; }

        public RiftRunner(RiftFilter filter,
                          string logFile,
                          string fullLogFile,
                          RiftRunnerConfig runnerConfig,
                          AbstractEmulator emulator,
                          RunnerLogger logger) : base(logFile, fullLogFile, runnerConfig, emulator, logger)
        {
            Filter = filter;
        }

        public override void Run()
        {
            // 1. Check run finish
            // 2. If not finish, wait 3-5s
            // 3. If finish, collect reward with filter
            // 4. Check for refill
            // 5. Start
            ModifiedTime = DateTime.Now;

            while (!Stop)
            {
                Debug.WriteLine("Checking run status ...");
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
                break;
            }
        }
        public override void Collect()
        {
            // Random click twice to pop up the reward dialog
            Thread.Sleep(8000); // Wait for end animation
            Emulator.RandomClick();

            Thread.Sleep(2000); // wait for treasure box to pop up
            Emulator.RandomClick();

            Thread.Sleep(4000); // wait for reward to pop up

            RunResult runResult = Helper.GetRunResult(LogFile);
            Reward reward = Helper.GetReward(runResult);

            bool getReward = Filter.ShouldGet(reward);

            if (!getReward)
            {
                Emulator.Click(RunnerConfig.RuneInfoPoint);
                Thread.Sleep(1500);

                Emulator.Click(RunnerConfig.SellRunePoint);
                Thread.Sleep(1000);
                Emulator.Click(RunnerConfig.ConfirmSellRunePoint);
                Thread.Sleep(1500);
                Logger.Log("Sell: " + Environment.NewLine + reward.ToString());
            }
            else
            {
                Logger.Log("Get: " + reward.ToString());
            }

            Emulator.PressEsc();
        }

    }
}
