using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;

namespace SWRunner.Runners
{
    [XmlInclude(typeof(DungeonRunConfig))]
    [Serializable, XmlRoot(ElementName = "RunConfig")]
    public abstract class RunConfig
    {
        public int Width;
        public int Height;

        public Point StartPoint;
        public Point ReplayPoint;
    }
}
