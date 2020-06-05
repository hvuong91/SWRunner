using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace SWEmulator
{
    public class BlueStacksEmulator : AbstractEmulator
    {
        public IntPtr Screen { get; private set; }

        public BlueStacksEmulator()
        {
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
            IntPtr parent = FindWindow(null, "BlueStacks");
            IntPtr subWindow = FindWindowEx(parent, IntPtr.Zero, null, "BlueStacks Android PluginAndroid");
            IntPtr mainWindow = FindWindowEx(subWindow, IntPtr.Zero, "BlueStacksApp", null);
            //IntPtr mainWindow = FindWindowEx(subWindow, IntPtr.Zero, null, "_ctl.Window");
            return mainWindow;
        }

        public override Bitmap PrintWindow()
        {
            //Rect rc;
            //// TODO: This might be stuck forever. Use timer instead?
            //int tries = 100;
            //while (!GetWindowRect(Screen, out rc) && tries-- > 0) { };

            //if (tries <= 0)
            //{
            //    throw new Exception("Failed to Print Window");
            //}

            //Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format24bppRgb);
            //Graphics gfxBmp = Graphics.FromImage(bmp);
            //IntPtr hdcBitmap = gfxBmp.GetHdc();

            //PrintWindow(Screen, hdcBitmap, 0);

            //gfxBmp.ReleaseHdc(hdcBitmap);
            //gfxBmp.Dispose();

            //bmp.Save("C:\\TestWin32\\test.png", ImageFormat.Png);

            //return bmp;

            // get te hDC of the target window
            IntPtr hdcSrc = GetWindowDC(Screen);
            // get the size
            Rect windowRect;
            GetWindowRect(Screen, out windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest, hOld);
            // clean up 
            GDI32.DeleteDC(hdcDest);
            ReleaseDC(Screen, hdcSrc);

            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);

            ((Bitmap)img).Save("C:\\TestWin32\\test.png", ImageFormat.Png);
            ((Bitmap)img).Save("C:\\TestWin32\\test2.jpeg", ImageFormat.Jpeg);

            return (Bitmap)img;
        }

        private class GDI32
        {

            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter

            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
                int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }
    }
}
