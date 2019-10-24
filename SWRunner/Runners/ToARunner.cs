using SWEmulator;
using SWRunner.Rewards;
using System;
using System.Diagnostics;
using System.Drawing;
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
                Thread.Sleep(3000);
                if (IsFailed())
                {
                    Debug.WriteLine("Run Failed");
                    Thread.Sleep(1000);
                    Emulator.RandomClick();
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
                Debug.WriteLine("Start new run ...");
                StartNewRun();
                break;
            }
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

        public override bool IsFailed()
        {
            Bitmap screenShot = Emulator.PrintWindow();
            Bitmap crop = BitmapUtils.CropImage(screenShot, new Rectangle(600, 75, 300, 200));
            return BitmapUtils.FindMatchImage(crop, new Bitmap(@"Resources\general\toaDefeat.PNG"), 0.92f);

        }
    }
}
