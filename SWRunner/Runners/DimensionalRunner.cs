using SWRunner.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWRunner.Runners
{
    class DimensionalRunner : AbstractRunner
    {
        DimensionalFilter filter;

        public DimensionalRunner(DimensionalFilter filter)
        {
            this.filter = filter;
        }

        public override void Collect()
        {
            throw new NotImplementedException();
        }

        public override bool IsEnd()
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
