using SWRunner.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWRunner.Runners
{
    class DimensionalRunner : IRunner
    {
        DimensionalFilter filter;

        public DimensionalRunner(DimensionalFilter filter)
        {
            this.filter = filter;
        }
        public void Collect()
        {
            throw new NotImplementedException();
        }

        public bool IsEnd()
        {
            throw new NotImplementedException();
        }

        public bool IsFailed()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
