using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using static SWRunner.Rewards.Rune;

namespace SWRunner.Rewards
{
    [XmlInclude(typeof(GrindStone))]
    [XmlInclude(typeof(EnchantedGem))]
    public abstract class GemStone : Reward
    {
        [XmlElement("Set")]
        public RUNESET Set { get; set; }
        [XmlElement("MainStat")]
        public string MainStat { get; set; }
        [XmlElement("Rarity")]
        public RARITY Rarity { get; set; }

        public GemStone(string dropItem) : base(dropItem)
        {
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                GemStone other = (GemStone)obj;
                return (Type == other.Type)
                    && (Set == other.Set || Set == RUNESET.ALL || other.Set == RUNESET.ALL)
                    && (MainStat.ToLower() == other.MainStat.ToLower() || MainStat.ToLower().Equals("all") || other.MainStat.ToLower().Equals("all"))
                    && (Rarity == other.Rarity || Rarity == RARITY.ALL || other.Rarity == RARITY.ALL);
            }
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ Set.GetHashCode() ^ MainStat.GetHashCode() ^ Rarity.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Type).Append(" - ").Append(Set).Append(" - ").Append(Rarity).Append(Environment.NewLine);

            return sb.ToString();
        }

    }
}
