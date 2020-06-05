using SWEmulator;
using SWRunner.Filters;
using SWRunner.Rewards;
using System;
using System.Diagnostics;
using System.Threading;
using static SWRunner.RunnerLogger;

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
            Thread.Sleep(10000); // Wait for end animation
            Emulator.RandomClick();

            Thread.Sleep(3000); // wait for treasure box to pop up
            Emulator.RandomClick();

            Thread.Sleep(5000); // wait for reward to pop up

            RunResult runResult = Helper.GetRunResult(LogFile);
            Reward reward = Helper.GetReward(runResult);

            bool getReward = Filter.ShouldGet(reward);

            if (!getReward)
            {
                Thread.Sleep(1000);
                Emulator.Click(RunnerConfig.RuneInfoPoint);
                Thread.Sleep(2000);

                Emulator.Click(RunnerConfig.SellRunePoint);
                Thread.Sleep(1000);

                // Rare gem/grindstone can be ignore
                if (IsRequiredConfirmSell(reward))
                {
                    Emulator.Click(RunnerConfig.ConfirmSellRunePoint);
                }
                
                Thread.Sleep(2000);
                Logger.Log(ACTION.SELL, reward);
            }
            else
            {
                Logger.Log(ACTION.GET, reward);
            }

            Thread.Sleep(3000);
            Emulator.PressEsc();
        }

        private bool IsRequiredConfirmSell(Reward reward)
        {
            if (reward is GemStone)
            {
                GemStone gemStone = reward as GemStone;
                if ( gemStone.Rarity != Rune.RARITY.HERO && gemStone.Rarity != Rune.RARITY.LEGENDARY)
                {
                    Debug.WriteLine($"Ignore confirm sale for Gem/Grindstone {gemStone.Rarity}");
                    return false;
                }
            }

            // A+ or such might returns blue 4* rune
            if (reward is Rune)
            {
                Rune rune = reward as Rune;
                if (rune.Grade.Contains("4"))
                {
                    Debug.WriteLine($"Ignore confirm sale for {rune.Grade} rune.");
                    return false;
                }

            }

            return true;
        }
    }
}
