using System;
using System.Collections.Generic;
using System.Text;
using SWRunner.Rewards;
using Rune = SWRunner.Rewards.Rune;

namespace SWRunner.Filters
{
    class RaidFilter : IFilter
    {
        public bool ShouldGet(Reward reward)
        {
            if (reward.GetType() != typeof(GrindStone))
            {
                return ShouldGetGrindStone((GrindStone)reward);
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

        private bool ShouldGetGrindStone(GrindStone grindStone)
        {
            // TODO
            return true;
        }

        private bool IsFlatGrindstone(GrindStone grindStone)
        {
            return !grindStone.MainStat.Contains("%") && !grindStone.MainStat.ToLower().Contains("spd");
        }

        private bool IsHeroOrLegendGrindStone(GrindStone grindStone)
        {
            return grindStone.Rarity == Rune.RARITY.HERO || grindStone.Rarity == Rune.RARITY.LEGENDARY;
        }



        private bool ShouldGetGem(EnchantedGem gem)
        {
            // TODO
            return true;
        }
    }
}
