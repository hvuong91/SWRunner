using SWRunner.Rewards;

namespace SWRunner.Filters
{
    public class RiftFilter : IFilter
    {
        public bool ShouldGet(Reward reward)
        {
            // Take all non-rune drops, might not applies to rift
            if (reward.GetType() != typeof(Rune))
            {
                return true;
            }

            Rune rune = (Rune)reward;

            if (rune.Set == Rune.RUNESET.FIGHT)
            {
                // get trash fight rune for now
                return true;
            }

            // only get legendary rune
            if (!IsLegendary(rune))
            {
                return false;
            }

            // Ignore flat rune
            if (IsFlat246(rune) && !IsSlot2Speed(rune))
            {
                return false;
            }

            return true;
        }

        private bool IsLegendary(Rune rune)
        {
            return rune.Rarity == Rune.RARITY.LEGENDARY;
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
    }
}
