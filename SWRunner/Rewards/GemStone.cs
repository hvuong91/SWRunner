using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using static SWRunner.Rewards.Rune;

namespace SWRunner.Rewards
{
    [XmlType("GemStone")]
    [XmlInclude(typeof(Grindstone))]
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
                GemStone gemStone = (GemStone)obj;
                return (Type == gemStone.Type && Set == gemStone.Set && MainStat == gemStone.MainStat && Rarity == gemStone.Rarity);
            }
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ Set.GetHashCode() ^ MainStat.GetHashCode() ^ Rarity.GetHashCode();
        }

    }
}
