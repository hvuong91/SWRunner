using System;
using System.Drawing;

namespace SWEmulator
{
    interface IEmulator
    {

        void Click(Point point);

        IntPtr GetMainWindow(string windowName);
    }
}
