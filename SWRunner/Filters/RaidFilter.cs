using System;
using System.Collections.Generic;
using System.Text;
using SWRunner.Rewards;

namespace SWRunner.Filters
{
    class RaidFilter : IFilter
    {
        public bool ShouldGet(Reward reward)
        {
            if (reward.GetType() != typeof(Grindstone))
            {
                return ShouldGetGrindStone((Grindstone)reward);
            }
            else if (reward.GetType() != typeof(EnchantedGem))
            {
                return ShouldGetGem((EnchantedGem)reward);
            }
            else
            {
                return true; // Get other
            }
        }

        private bool ShouldGetGrindStone(Grindstone grindStone)
        {
            // TODO
            return true;
        }

        private bool ShouldGetGem(EnchantedGem gem)
        {
            // TODO
            return true;
        }
    }
}
