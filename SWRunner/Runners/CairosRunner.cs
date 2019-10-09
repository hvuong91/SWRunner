using SWEmulator;
using SWRunner.Filters;
using SWRunner.Rewards;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SWRunner.Runners
{
    // TODO: Ensure to wait for each click.
    public class CairosRunner : AbstractRunner<CairosRunnerConfig>
    {
        CairosFilter filter;

        public CairosRunner(CairosFilter filter, string logFile, string fullLogFile, CairosRunnerConfig runnerConfig, 
            AbstractEmulator emulator) : base(logFile, fullLogFile, runnerConfig, emulator)
        {
            this.filter = filter;
            MinEnergyRequired = 8;
            MaxRunTime = new TimeSpan(0, 2, 0); // TODO: This should come from the constructor params
        }
        public override async Task Run(CancellationToken ct)
        {
            // 1. Check fail, do not revive. Jump to check refill
            // 2. Check run finish
            // 3. If not finish, wait 3-5s
            // 4. If finish, collect reward with filter
            // 5. Check for refill
            // 6. Start
            ModifiedTime = DateTime.Now;

            while (true)
            {
                await Task.Delay(3000, ct);
                //Thread.Sleep(3000);

                if (IsFailed())
                {
                    SkipRevive();
                }
                else if (IsEnd())
                {
                    Collect();
                }
                else
                {
                    // Run is not completed yet
                    continue;
                }

                StartNewRun();
                ModifiedTime = DateTime.Now; // ?
            }
        }

        public override void Collect()
        {
            // Random click twice to pop up the reward dialog
            Task.Delay(8000);
            //Thread.Sleep(8000); // Wait for end animation
            Emulator.RandomClick();
            Task.Delay(2000);
            //Thread.Sleep(2000); // wait for treasure box to pop up
            Emulator.RandomClick();
            Task.Delay(2000);
            //Thread.Sleep(2000); // wait for reward to pop up


            RunResult runResult = Helper.GetRunResult(LogFile);
            Reward reward = Helper.GetReward(runResult);

            if (filter.ShouldGet(reward))
            {
                switch (reward.Type)
                {
                    case RewardType.RUNE:
                        Emulator.Click(RunnerConfig.GetRunePoint);
                        break;
                    case RewardType.MYSTICAL_SCROLL:
                    case RewardType.SUMMON_STONE:
                        Emulator.Click(RunnerConfig.GetMysticalScrollPoint);
                        break;
                    default:
                        Emulator.Click(RunnerConfig.GetOtherPoint);
                        break;
                }
            }
            else
            {
                // Only Rune needs to be sold
                Emulator.Click(RunnerConfig.SellRunePoint);
                Task.Delay(1500);
                //Thread.Sleep(1500); // Wait for confirmation dialog
                Emulator.Click(RunnerConfig.ConfirmSellRunePoint);
            }
            Task.Delay(4000);
            //Thread.Sleep(4000); // Wait for server response
        }

    }
}
