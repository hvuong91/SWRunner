using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace SWEmulator
{
    public class NoxEmulator : AbstractEmulator
    {
        public IntPtr Screen { get; private set; }

        private const int topMenuHeight = 35;

        public NoxEmulator() { 
            Screen = GetScreen();
        }

        public override IntPtr GetMainWindow()
        {
            // 1. Caption: "Nox", Class: "Qt5QWindowIcon"
            // 2. Caption: "ScreenBoardClassWindow", Class: "Qt5QWindowIcon"
            // 3. Caption: "QWidgetClassWindow", Class: "Qt5QWindowIcon"

            IntPtr parent = FindWindow("Qt5QWindowIcon", "Nox");
            IntPtr subWindow = FindWindowEx(parent, IntPtr.Zero, "Qt5QWindowIcon", "ScreenBoardClassWindow");
            IntPtr mainWindow = FindWindowEx(subWindow, IntPtr.Zero, "Qt5QWindowIcon", "QWidgetClassWindow");

            return mainWindow;
        }

        private IntPtr GetScreen()
        {
            IntPtr parent = AbstractEmulator.FindWindow("Qt5QWindowIcon", "Nox");
            return parent;
        }

        public override Bitmap PrintWindow()
        {
            Rect rc;
            // TODO: This might be stuck forever. Use timer instead?
            int tries = 100;
            while (!GetWindowRect(Screen, out rc) && tries-- > 0) { };

            if (tries <= 0)
            {
                throw new Exception("Failed to Print Window");
            }

            Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format24bppRgb);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();

            PrintWindow(Screen, hdcBitmap, 0);

            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();

            bmp.Save("C:\\TestWin32\\test.png", ImageFormat.Png);
            return bmp;
        }
    }
}
