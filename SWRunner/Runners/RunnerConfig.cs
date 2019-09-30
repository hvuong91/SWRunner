using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SWRunner.Runners
{
    public abstract class RunnerConfig
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Point StartPoint { get; set; }
        public Point ReplayPoint { get; set; }
    }
}
