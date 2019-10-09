using SWEmulator;
using SWRunner.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SWRunner.Runners
{
    class DimensionalRunner : AbstractRunner<RunnerConfig>
    {
        DimensionalFilter filter;

        public DimensionalRunner(DimensionalFilter filter, string logFile, string fullLogFile) 
            : base(logFile, fullLogFile, null, null)
        {
            this.filter = filter;
        }

        public override void Collect()
        {
            throw new NotImplementedException();
        }

        public override async Task Run(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
