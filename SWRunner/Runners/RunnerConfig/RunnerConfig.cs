using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;

namespace SWRunner.Runners
{
    [XmlInclude(typeof(CairosRunnerConfig))]
    [Serializable, XmlRoot(ElementName = "RunConfig")]
    public abstract class RunnerConfig
    {
        public int Width;
        public int Height;

        public Point StartPoint;
        public Point ReplayPoint;
    }
}
