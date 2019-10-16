using System;
using static SWRunner.Rewards.Rune;

namespace SWRunner.Rewards
{
    public class Grindstone : Reward
    {

        public RUNESET Set { get; private set; }
        public string MainStat { get; private set; }
        public RARITY Rarity { get; private set; }

        public Grindstone(string set, string mainStat, string min, string max) : base("Grindstone")

        {
            Set = Enum.TryParse(set, true, out RUNESET outSet) ? outSet : RUNESET.UNKNOWN;
            MainStat = mainStat;
            Rarity = GetRarirty(max, min);
        }

        private RARITY GetRarirty(string max, string min)
        {
            switch (max)
            {
                // % DEF/ATK/HP
                case "5":
                    if (min.Equals("2"))
                    {
                        return RARITY.MAGIC;
                    }
                    else
                        return RARITY.LEGEND; // SPD
                case "6":
                    return RARITY.RARE;
                case "7":
                    return RARITY.HERO;
                case "10":
                    return RARITY.LEGEND;
                // flat DEF/ATK
                case "12":
                    return RARITY.MAGIC;
                case "18":
                    return RARITY.RARE;
                case "22":
                    return RARITY.HERO;
                case "30":
                    return RARITY.LEGEND;
                // flat hp
                case "200":
                    return RARITY.MAGIC;
                case "250":
                    return RARITY.RARE;
                case "450":
                    return RARITY.HERO;
                case "550":
                    return RARITY.LEGEND;
                // SPD
                case "2":
                    return RARITY.MAGIC;
                case "3":
                    return RARITY.RARE;
                case "4":
                    return RARITY.HERO;
                default:
                    return RARITY.UNKNOWN;
            }

        }

    }
}
