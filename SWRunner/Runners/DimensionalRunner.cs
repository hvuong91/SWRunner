using SWEmulator;
using SWRunner.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWRunner.Runners
{
    class DimensionalRunner : AbstractRunner<RunnerConfig>
    {
        DimensionalFilter filter;

        public DimensionalRunner(DimensionalFilter filter, string logFile) 
            : base(logFile, null, null)
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
            throw new NotImplementedException();
        }
    }
}
