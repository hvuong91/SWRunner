using System;
using System.Drawing;

namespace SWEmulator
{
    interface IEmulator
    {

        void Click(PointF point);

        void RandomClick();

        IntPtr GetMainWindow(string windowName);
    }
}
