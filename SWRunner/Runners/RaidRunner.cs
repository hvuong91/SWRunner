using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SWRunner.Runners
{
    class RaidRunner : AbstractRunner<RunnerConfig>
    {
        public RaidRunner(string logFile) : base(logFile, "",  null, null)
        {
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
