using SWRunner.Rewards;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SWRunner.Filters
{
   

    [XmlRoot("GemStoneFilter")]
    [XmlInclude(typeof(GemStone))]
    public class GemStoneFilter
    {
        [XmlElement("GemStone")]
        public List<GemStone> GemStoneList { get; set; }

        public GemStoneFilter()
        {
            GemStoneList = new List<GemStone>();
        }
    }
}
