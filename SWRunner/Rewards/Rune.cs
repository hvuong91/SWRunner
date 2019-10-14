using System;
using System.Collections.Generic;
using System.Text;

namespace SWRunner.Rewards
{
    public class Rune : Reward
    {
        public string Grade { get; private set; }
        public RUNESET Set { get; private set; }
        public string Slot { get; private set; }
        public RARITY Rarity { get; private set; }
        public string MainStat { get; private set; }
        public string PrefixStat { get; private set; }
        public string SubStat1 { get; private set; }
        public string SubStat2 { get; private set; }
        public string SubStat3 { get; private set; }
        public string SubStat4 { get; private set; }

        private Rune(string grade, RUNESET set, string slot, RARITY rarity,
            string mainStat, string prefixStat, string subStat1, string subStat2, string subStat3, string subStat4) : base("Rune")
        {
            Grade = grade;
            Set = set;
            Slot = slot;
            Rarity = rarity;
            MainStat = mainStat;
            PrefixStat = prefixStat;
            SubStat1 = subStat1;
            SubStat2 = subStat2;
            SubStat3 = subStat3;
            SubStat4 = subStat4;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Set).Append(" ").Append(Grade);
            sb.Append(" - ").Append(Rarity).Append(" ");
            sb.Append("(").Append(Slot).Append(")").Append(" - ");
            sb.Append(MainStat).Append(Environment.NewLine);
            if (!string.IsNullOrEmpty(PrefixStat))
            {
                sb.Append("Prefix: ").Append(PrefixStat).Append(Environment.NewLine);
            }
            if (!string.IsNullOrEmpty(SubStat1))
            {
                sb.Append("SubStat 1: ").Append(SubStat1).Append(Environment.NewLine);
            }
            if (!string.IsNullOrEmpty(SubStat2))
            {
                sb.Append("SubStat 2: ").Append(SubStat2).Append(Environment.NewLine);
            }
            if (!string.IsNullOrEmpty(SubStat3))
            {
                sb.Append("SubStat 3: ").Append(SubStat3).Append(Environment.NewLine);
            }
            if (!string.IsNullOrEmpty(SubStat4))
            {
                sb.Append("SubStat 4: ").Append(SubStat4).Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public class RuneBuilder
        {
            private string grade;
            private RUNESET set;
            private string slot;
            private RARITY rarity;
            private string mainStat;
            private string prefixStat;
            private string subStat1;
            private string subStat2;
            private string subStat3;
            private string subStat4;


            public RuneBuilder Grade(string grade)
            {
                this.grade = grade;
                return this;
            }

            public RuneBuilder Set(string set)
            {
                this.set = Enum.TryParse(set, true, out RUNESET runeSet) ? runeSet : RUNESET.UNKNOWN;
                return this;
            }

            public RuneBuilder Slot(string slot)
            {
                this.slot = slot;
                return this;
            }

            public RuneBuilder Rarity(string rarity)
            {
                this.rarity = Enum.TryParse(rarity, true, out RARITY outRarity) ? outRarity : RARITY.UNKNOWN;
                return this;
            }

            public RuneBuilder MainStat(string mainStat)
            {
                this.mainStat = mainStat;
                return this;
            }

            public RuneBuilder PrefixStat(string prefixStat)
            {
                this.prefixStat = prefixStat;
                return this;
            }

            public RuneBuilder SubStat1(string subStat1)
            {
                this.subStat1 = subStat1;
                return this;
            }

            public RuneBuilder SubStat2(string subStat2)
            {
                this.subStat2 = subStat2;
                return this;
            }

            public RuneBuilder SubStat3(string subStat3)
            {
                this.subStat3 = subStat3;
                return this;
            }

            public RuneBuilder SubStat4(string subStat4)
            {
                this.subStat4 = subStat4;
                return this;
            }

            public Rune Build()
            {
                return new Rune(grade, set, slot, rarity, mainStat, 
                    prefixStat, subStat1, subStat2, subStat3, subStat4);
            }
        }

        public enum RUNESET { VIOLENT, RAGE, ENERGY, FATAL, SWIFT, FOCUS, DESPAIR, ENDURE, 
            REVENGE, WILL, NEMESIS, VAMPIRE, DESTROY, FIGHT, UNKNOWN}

        public enum RARITY { NORMAL, RARE, MAGIC, HERO, LEGEND, UNKNOWN}
    }
    
}
