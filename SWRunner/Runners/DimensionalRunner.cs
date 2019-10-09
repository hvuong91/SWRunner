using SWRunner.Filters;
using SWRunner.Rewards;
using System;

namespace SWRunner.Runners
{
    class DimensionalRunner : AbstractRunner<RunnerConfig>
    {
        DimensionalFilter filter;

        public DimensionalRunner(DimensionalFilter filter, string logFile, string fullLogFile) 
            : base(logFile, fullLogFile, null, null, null)
        {
            this.filter = filter;
        }

        public override void Collect()
        {
            throw new NotImplementedException();
        }

        public override void Run()
        {
            throw new NotImplementedException();
        }
    }
}
