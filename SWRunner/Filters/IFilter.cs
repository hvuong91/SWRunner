using SWRunner.Rewards;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWRunner.Filters
{
    interface IFilter
    {
        bool ShouldGet(Reward reward);
    }
}
