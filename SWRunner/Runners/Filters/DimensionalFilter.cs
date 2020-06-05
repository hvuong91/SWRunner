using System;
using System.Collections.Generic;
using System.Text;
using SWRunner.Rewards;

namespace SWRunner.Filters
{
    public class DimensionalFilter : IFilter
    {
        public bool ShouldGet(Reward reward)
        {
            // Need to wait for SWEX to be updated
            return true;
        }
    }
}
