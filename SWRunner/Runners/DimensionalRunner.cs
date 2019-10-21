using SWEmulator;
using SWRunner.Filters;
using SWRunner.Rewards;
using System;
using System.Diagnostics;
using System.Threading;

namespace SWRunner.Runners
{
    class DimensionalRunner : AbstractRunner<AbstractRunnerConfig>
    {
        private DimensionalFilter Filter { get; set; }

        public DimensionalRunner(DimensionalFilter filter,
                                 string logFile,
                                 string fullLogFile,
                                 DimensionalRunnerConfig runnerConfig,
                                 AbstractEmulator emulator,
                                 RunnerLogger logger)
            : base(logFile, fullLogFile, runnerConfig, emulator, logger)
        {
            Filter = filter;
        }

        public override void Run()
        {
            // 1. Check fail, do not revive.
            // 2. Check run finish
            // 3. If not finish, wait 3-5s
            // 4. If finish, collect reward with filter
            // 6. Start
            ModifiedTime = DateTime.Now;

            while (!Stop)
            {
                Debug.WriteLine("Checking run status ...");
                Thread.Sleep(3000);
                if (IsFailed())
                {
                    Debug.WriteLine("Run Failed");
                    SkipRevive();
                }
                else if (IsEnd())
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

        public override void StartNewRun()
        {
            Thread.Sleep(3000);
            RandomSleep();
            Emulator.Click(RunnerConfig.ReplayPoint);

            Thread.Sleep(1000);

            RandomSleep();
            Emulator.Click(RunnerConfig.StartPoint);
        }

        public override void Collect()
        {
            // Random click twice to pop up the reward dialog
            Thread.Sleep(9000); // Wait for end animation
            Emulator.RandomClick();

            Thread.Sleep(1500); // wait for treasure box to pop up
            Emulator.RandomClick();

            Thread.Sleep(2500); // wait for reward to pop up

            // TODO: need filter once SWEX is updated
            Emulator.PressEsc();

            Thread.Sleep(2000); // Wait for server response
        }

    }
}
