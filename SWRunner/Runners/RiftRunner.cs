using System;
using System.Collections.Generic;
using System.Text;

namespace SWRunner.Runners
{
    class RiftRunner : AbstractRunner<RunnerConfig>
    {
        public RiftRunner(string logFile) : base(logFile, null, null)
        {
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
