using System;
using System.Collections.Generic;
using System.Text;

namespace SWEmulator
{
    public class NoxEmulator : AbstractEmulator
    {
        public NoxEmulator(string windowName) : base(windowName)
        {
            // Do nothing for now
        }

        public override IntPtr GetMainWindow(string windowName)
        {
            // 1. Caption: "Nox", Class: "Qt5QWindowIcon"
            // 2. Caption: "ScreenBoardClassWindow", Class: "Qt5QWindowIcon"
            // 3. Caption: "QWidgetClassWindow", Class: "Qt5QWindowIcon"

            IntPtr parent = FindWindow("Qt5QWindowIcon", "Nox");
            IntPtr subWindow = FindWindowEx(parent, IntPtr.Zero, "Qt5QWindowIcon", "ScreenBoardClassWindow");
            IntPtr mainWindow = FindWindowEx(subWindow, IntPtr.Zero, "Qt5QWindowIcon", "QWidgetClassWindow");

            return mainWindow;
        }

    }
}
