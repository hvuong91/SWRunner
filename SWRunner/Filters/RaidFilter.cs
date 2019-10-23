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

        private bool IsFlatGrindstone(Grindstone grindStone)
        {
            return !grindStone.MainStat.Contains("%") && !grindStone.MainStat.ToLower().Contains("spd");
        }

        private bool IsHeroOrLegendGrindStone(Grindstone grindStone)
        {
            return grindStone.Rarity == Rune.RARITY.HERO || grindStone.Rarity == Rune.RARITY.LEGEND;
        }



        private bool ShouldGetGem(EnchantedGem gem)
        {
            // TODO
            return true;
        }
    }
}
