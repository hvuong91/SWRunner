﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SWRunner.Runners
{
    public class ToARunner : AbstractRunner<RunnerConfig>
    {
        public ToARunner(string logFile) : base(logFile, null, null)
        {
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
