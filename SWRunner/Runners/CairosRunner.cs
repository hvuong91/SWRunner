using SWRunner.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWRunner.Runners
{
    public class CairosRunner : AbstractRunner
    {
        CairosFilter filter;

        public CairosRunner(CairosFilter filter, string logFile) : base(logFile)
        {
            this.filter = filter;
        }

        public override void Collect()
        {
            throw new NotImplementedException();
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
