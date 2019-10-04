using SWEmulator;
using SWRunner.Filters;
using SWRunner.Rewards;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SWRunner.Runners
{
    // TODO: Ensure to wait for each click.
    public class CairosRunner : AbstractRunner<CairosRunnerConfig>
    {
        CairosFilter filter;

        public CairosRunner(CairosFilter filter, string logFile, CairosRunnerConfig runnerConfig, 
            AbstractEmulator emulator) : base(logFile, runnerConfig, emulator)
        {
            this.filter = filter;
            MinEnergyRequired = 8;
        }
        public override void Run()
        {
            // 1. Check fail, do not revive. Jump to check refill
            // 2. Check run finish
            // 3. If not finish, wait 3-5s
            // 4. If finish, collect reward with filter
            // 5. Check for refill
            // 6. Start
            modifiedTime = DateTime.Now;

            while (true)
            {
                Thread.Sleep(3000);

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
                modifiedTime = DateTime.Now; // ?
            }
        }

        public override void Collect()
        {
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
                Thread.Sleep(1500); // Wait for confirmation dialog
                Emulator.Click(RunnerConfig.ConfirmSellRunePoint);
            }
            Thread.Sleep(4000); // Wait for server response
        }

        public override bool IsFailed()
        {
            throw new NotImplementedException();
        }

    }
}
