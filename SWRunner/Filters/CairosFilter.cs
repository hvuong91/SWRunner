using SWRunner.Rewards;

namespace SWRunner.Filters
{
    public class CairosFilter : IFilter
    {
        // TOOD: Needs better logic for filter out runes
        public CairosFilter() { }

        public bool ShouldGet(Reward reward)
        {
            // Take all non-rune drops
            if (reward.GetType() != typeof(Rune))
            {
                return true;
            }

            Rune rune = (Rune)reward;

            if (!IsHeroOrLegendary(rune))
            {
                return false;
            }

            if (IsFlat246(rune) && !IsSlot2Speed(rune))
            {
                return false;
            }

            if (Is5StarHero(rune))
            {
                return false;
            }

            return true;
        }

        private bool IsHeroOrLegendary(Rune rune)
        {
            return rune.Rarity == Rune.RARITY.HERO || rune.Rarity == Rune.RARITY.LEGENDARY;
        }

        private bool IsFlat246(Rune rune)
        {
            return Is246(rune) && !rune.MainStat.Contains("%");
        }

        private static bool Is246(Rune rune)
        {
            return rune.Slot.Equals("2") || rune.Slot.Equals("4") || rune.Slot.Equals("6");
        }

        private bool IsSlot2Speed(Rune rune)
        {
            return rune.Slot.Equals("2") && rune.MainStat.Contains("SPD");
        }

        private bool HasSpeedSub(Rune rune)
        {
            return ContainsStat(rune, "SPD");
        }

        private bool ContainsStat(Rune rune, string stat)
        {
            return rune.SubStat1.Contains(stat)
                    || rune.SubStat2.Contains(stat)
                    || rune.SubStat3.Contains(stat)
                    || rune.SubStat4.Contains(stat);
        }

        private bool Is5StarHero(Rune rune)
        {
            return Is5Star(rune) && rune.Rarity == Rune.RARITY.HERO;
        }

        private bool Is5Star(Rune rune)
        {
            return rune.Grade.Equals("5*");
        }
    }
}