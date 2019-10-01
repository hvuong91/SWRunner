using System;
using System.Drawing;

namespace SWEmulator
{
    interface IEmulator
    {

        void Click(PointF point);

        IntPtr GetMainWindow(string windowName);
    }
}
