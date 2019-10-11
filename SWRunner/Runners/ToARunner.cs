using SWEmulator;
using SWRunner.Rewards;
using System;
using System.Diagnostics;
using System.Threading;

namespace SWRunner.Runners
{
    public class ToARunner : AbstractRunner<ToaRunnerConfig>
    {
        public ToARunner(string logFile, string fullLogFill, ToaRunnerConfig runnerConfig, AbstractEmulator emulator,
                         RunnerLogger logger) : base(logFile, fullLogFill, runnerConfig, emulator, logger)
        {
            MaxRunTime = new TimeSpan(0, 5, 0);
        }

        public override void Collect()
        {
            Thread.Sleep(5000);
            Emulator.RandomClick();

            Thread.Sleep(2000); // wait for treasure box to pop up
            Emulator.RandomClick();

            Thread.Sleep(2000); // wait for reward to pop up

            Emulator.PressEsc(); // Collect reward
        }

        public override void Run()
        {
            // 1. Check fail. Hit replay
            // 2. Check run finish
            // 3. If not finish, wait 3-5s
            // 4. If finish, collect reward.
            // 5. Check for refill
            // 6. Start
            ModifiedTime = DateTime.Now;

            while (!Stop)
            {
                Debug.WriteLine("Checking run status ...");
                Thread.Sleep(3000);
                if (IsFailed())
                {
                    Debug.WriteLine("Run Failed");
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
    }
}
