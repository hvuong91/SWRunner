using SWEmulator;
using SWRunner.Filters;
using SWRunner.Rewards;
using System;
using System.Collections.Generic;
using System.Text;

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
        }

        public override void Collect()
        {
            RunResult runResult = Helper.GetRunResult(LogFile);
            Reward reward = Helper.GetReward(runResult);

            if (filter.ShouldGet(reward))
            {
                Emulator.Click(RunnerConfig.GetRunePoint);
            }
            else
            {
                // TODO: Sell
            }
        }

        public override bool IsFailed()
        {
            throw new NotImplementedException();
        }

        public override void Run()
        {
            // 1. Check fail, do not revive. Jump to check refill
            // 2. Check run finish
            // 3. If not finish, wait 3-5s
            // 4. If finish, collect reward with filter
            // 5. Check for refill
            // 6. Start

            while (true)
            {
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
                    // Run is not completed
                    continue;
                }

                StartNewRun();
                CheckRefill();
                             
            }
        }

    }
}
