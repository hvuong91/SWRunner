using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
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
            // TODO: This might be stuck forever. Use timer instead?
            //if(!GetWindowRect(Screen, out Rect rc))
            //{
            //    throw new Exception("Failed to Print Window");
            //}

            //Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppRgb);
            //Graphics gfxBmp = Graphics.FromImage(bmp);
            //IntPtr hdcBitmap = gfxBmp.GetHdc();

            //if (!PrintWindow(Screen, hdcBitmap, 0))
            //{
            //    throw new Exception("Failed to Print Window");
            //}

            //gfxBmp.ReleaseHdc(hdcBitmap);
            //gfxBmp.Dispose();

            //string test = $"C:\\TestWin32\\{DateTime.Now.ToString("hhmmss", DateTimeFormatInfo.InvariantInfo)}-test.png";
            //bmp.Save(test, ImageFormat.Png);

            //return bmp;

            // Test
            if (!GetWindowRect(Screen, out Rect rc))
            {
                throw new Exception("Failed to Print Window");
            }
            var testBmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppRgb);
            Graphics graphics = Graphics.FromImage(testBmp);
            graphics.CopyFromScreen(rc.left, rc.top, 0, 0, new Size(rc.Width, rc.Height), CopyPixelOperation.SourceCopy);

            string test = $"C:\\TestWin32\\{DateTime.Now.ToString("hhmmss", DateTimeFormatInfo.InvariantInfo)}-test.png";

            //testBmp.Save(test, ImageFormat.Png);

            return testBmp;

            // Test 2
            //GetWindowRect(Screen, out Rect rc);
            //Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);


            //Graphics gfxBmp = Graphics.FromImage(bmp);

            //IntPtr hdcBitmap = gfxBmp.GetHdc();
            ////hdcBitmap = GetWindowDC(Screen);
            //BitBlt(hdcBitmap, 0, 0, rc.Width, rc.Height, GetWindowDC(Screen), 0, 0, TernaryRasterOperations.SRCCOPY);
            //gfxBmp.ReleaseHdc(hdcBitmap);
            //gfxBmp.Dispose();

            //string test = $"C:\\TestWin32\\{DateTime.Now.ToString("hhmmss", DateTimeFormatInfo.InvariantInfo)}-test.png";
            //bmp.Save(test, ImageFormat.Png);

            //return bmp;

        }
    }
}
