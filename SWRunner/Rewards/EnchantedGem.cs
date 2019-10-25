using System;
using System.Collections.Generic;
using System.Text;
using static SWRunner.Rewards.Rune;

namespace SWRunner.Rewards
{
    public class EnchantedGem : GemStone
    {
        private EnchantedGem() : base("Enchanted Gem") { }

        public EnchantedGem(string set, string mainStat, string min, string max) : base("Enchanted Gem")
        {
            Set = Enum.TryParse(set, true, out RUNESET outSet) ? outSet : RUNESET.UNKNOWN;
            MainStat = mainStat;
            Rarity = GetRarirty(max, min, mainStat);
            Type = REWARDTYPE.ENCHANTEDGEM;
        }

        public EnchantedGem(RUNESET set, string mainStat, RARITY rarity) : base("Enchanted Gem")
        {
            Set = set;
            MainStat = mainStat;
            Rarity = rarity;
            Type = REWARDTYPE.ENCHANTEDGEM;
        }

        private RARITY GetRarirty(string max, string min, string mainStat)
        {
            if (mainStat.Contains("%"))
            {
                switch (max)
                {
                    case "7":
                        return RARITY.MAGIC;
                    case "9":
                        return RARITY.RARE;
                    case "11":
                        return RARITY.HERO;
                    case "13":
                        return RARITY.LEGENDARY;
                    default:
                        return RARITY.UNKNOWN;
                }
            }
            else if (mainStat.Contains("flat"))
            {
                switch (max)
                {
                    case "16":
                    case "220":
                        return RARITY.MAGIC;
                    case "23":
                    case "310":
                        return RARITY.RARE;
                    case "30":
                    case "420":
                        return RARITY.HERO;
                    case "40":
                    case "580":
                        return RARITY.LEGENDARY;
                }
            }
            else if (mainStat.Contains("SPD"))
            {
                switch (max)
                {
                    case "4":
                        return RARITY.MAGIC;
                    case "6":
                        return RARITY.RARE;
                    case "8":
                        return RARITY.HERO;
                    case "10":
                        return RARITY.LEGENDARY;
                }
            }
            else
            {
                // Ignore magic/rare
                if ((min == "4" && max == "7")
                    || (min == "5" && max == "8")
                    || (min == "6" && max == "9"))
                {
                    return RARITY.HERO;
                }
                else if ((min == "6" && max == "9")
                    || (min == "7" && max == "10")
                    || (min == "8" && max == "11"))
                {
                    return RARITY.LEGENDARY;
                }
            }

            return RARITY.UNKNOWN;
        }

    }
}
