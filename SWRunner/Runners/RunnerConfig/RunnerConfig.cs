using System;
using System.Xml.Serialization;
using System.Windows;
using System.Drawing;

namespace SWRunner.Runners
{
    [XmlInclude(typeof(CairosRunnerConfig))]
    [Serializable, XmlRoot(ElementName = "RunConfig")]
    public abstract class RunnerConfig
    {
        public PointF StartPoint;
        public PointF ReplayPoint;
        public PointF NoRevivePoint;
    }

}
