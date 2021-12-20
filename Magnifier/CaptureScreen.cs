using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Magnifier
{
    class CaptureScreen
    {
        //This structure shall be used to keep the size of the screen.
        public struct SIZE
        {
            public int cx;
            public int cy;
        }

        static Bitmap CaptureDesktop()
        {
            SIZE size;
            IntPtr hBitmap;
            IntPtr hDC = Win32Stuff.GetDC(Win32Stuff.GetDesktopWindow());
            IntPtr hMemDC = GDIStuff.CreateCompatibleDC(hDC);

            size.cx = Win32Stuff.GetSystemMetrics
                      (Win32Stuff.SM_CXSCREEN);

            size.cy = Win32Stuff.GetSystemMetrics
                      (Win32Stuff.SM_CYSCREEN);

            hBitmap = GDIStuff.CreateCompatibleBitmap(hDC, size.cx, size.cy);

            if (hBitmap != IntPtr.Zero)
            {
                IntPtr hOld = (IntPtr)GDIStuff.SelectObject
                                       (hMemDC, hBitmap);

                GDIStuff.BitBlt(hMemDC, 0, 0, size.cx , size.cy, hDC,
                                               0, 0, GDIStuff.SRCCOPY);

                GDIStuff.SelectObject(hMemDC, hOld);
                GDIStuff.DeleteDC(hMemDC);
                Win32Stuff.ReleaseDC(Win32Stuff.GetDesktopWindow(), hDC);
                Bitmap bmp = System.Drawing.Image.FromHbitmap(hBitmap);
                GDIStuff.DeleteObject(hBitmap);
                GC.Collect();
                return bmp;
            }
            return null;
      
        }

        private static IntPtr LastCursor = IntPtr.Zero;

        public static bool IsCursorDifferent()
        {
            if (!frmMain.Instance.InitCursor) return true;

            var cursorInfo = new Win32Stuff.CURSORINFO();

            cursorInfo.cbSize = Marshal.SizeOf(cursorInfo);

            if (!Win32Stuff.GetCursorInfo(out cursorInfo))
                return true;

            if (cursorInfo.hCursor.ToString() != LastCursor.ToString())
            {
                LastCursor = cursorInfo.hCursor;

                return true;
            }
            else
            {
                return false;
            }

            /*
            IntPtr curCursor = Win32Stuff.GetCursor();

            if (curCursor == null || curCursor == IntPtr.Zero) return true;

            if (curCursor.ToString() != LastCursor.ToString())
            {
                LastCursor = curCursor;

                return true;
            }
            else
            {
                return false;
            }*/
        }

        public static Bitmap CaptureImageCursor(ref Point point, double scale)
        {
            try
            {
                var cursorInfo = new Win32Stuff.CURSORINFO();
                
                cursorInfo.cbSize = Marshal.SizeOf(cursorInfo);

                if (!Win32Stuff.GetCursorInfo(out cursorInfo))
                    return null;

                if (cursorInfo.flags != Win32Stuff.CURSOR_SHOWING)
                    return null;

                var hicon = Win32Stuff.CopyIcon(cursorInfo.hCursor);
                if (hicon == IntPtr.Zero)
                    return null;

                Win32Stuff.ICONINFO iconInfo;
                if (!Win32Stuff.GetIconInfo(hicon, out iconInfo))
                {
                    GDIStuff.DeleteObject(hicon);
                    return null;
                }

                point.X = cursorInfo.ptScreenPos.x - iconInfo.xHotspot;
                point.Y = cursorInfo.ptScreenPos.y - iconInfo.yHotspot;

                using (var maskBitmap = Image.FromHbitmap(iconInfo.hbmMask))
                {
                    //Is this a monochrome cursor?  
                    if (maskBitmap.Height == maskBitmap.Width * 2 && iconInfo.hbmColor == IntPtr.Zero)
                    {
                        var final = new Bitmap(maskBitmap.Width, maskBitmap.Width);
                        var hDesktop = Win32Stuff.GetDesktopWindow();
                        var dcDesktop = Win32Stuff.GetWindowDC(hDesktop);

                        using (var resultGraphics = Graphics.FromImage(final))
                        {
                            var resultHdc = resultGraphics.GetHdc();
                            var offsetX = (int)((point.X + 3) * scale);
                            var offsetY = (int)((point.Y + 3) * scale);

                            GDIStuff.BitBlt(resultHdc, 0, 0, final.Width, final.Height, dcDesktop, offsetX, offsetY, (int)CopyPixelOperation.SourceCopy);
                            Win32Stuff.DrawIconEx(resultHdc, 0, 0, cursorInfo.hCursor, 0, 0, 0, IntPtr.Zero, 0x0003);

                            //TODO: I have to try removing the background of this cursor capture.
                            //Native.BitBlt(resultHdc, 0, 0, final.Width, final.Height, dcDesktop, (int)point.X + 3, (int)point.Y + 3, Native.CopyPixelOperation.SourceErase);

                            //Original, ignores the screen as background.
                            //Native.BitBlt(resultHdc, 0, 0, resultBitmap.Width, resultBitmap.Height, maskHdc, 0, resultBitmap.Height, Native.CopyPixelOperation.SourceCopy); //SourceCopy
                            //Native.BitBlt(resultHdc, 0, 0, resultBitmap.Width, resultBitmap.Height, maskHdc, 0, 0, Native.CopyPixelOperation.PatInvert); //SourceInvert

                            resultGraphics.ReleaseHdc(resultHdc);
                            Win32Stuff.ReleaseDC(hDesktop, dcDesktop);
                        }

                        GDIStuff.DeleteObject(iconInfo.hbmMask);
                        GDIStuff.DeleteDC(dcDesktop);

                        return final;
                    }

                    GDIStuff.DeleteObject(iconInfo.hbmColor);
                    GDIStuff.DeleteObject(iconInfo.hbmMask);
                    GDIStuff.DeleteObject(hicon);
                }

                var icon = Icon.FromHandle(hicon);
                return icon.ToBitmap();
            }
            catch (Exception ex)
            {
                //LogWriter.Log(ex, "Impossible to get the cursor.");
            }

            return null;
        }

        public static Bitmap CaptureImageCursor2(ref Point point, double scale)
        {
            try
            {                
                var cursorInfo = new Win32Stuff.CURSORINFO();
                
                cursorInfo.cbSize = Marshal.SizeOf(cursorInfo);
                
                if (!Win32Stuff.GetCursorInfo(out cursorInfo))
                    return null;

                if (cursorInfo.flags != Win32Stuff.CURSOR_SHOWING)
                    return null;

                var hicon = Win32Stuff.CopyIcon(cursorInfo.hCursor);

                if (hicon == IntPtr.Zero)
                {
                    //7
                    Marshal.ReleaseComObject(cursorInfo);
                    return null;

                }

                Win32Stuff.ICONINFO iconInfo;

                if (!Win32Stuff.GetIconInfo(hicon, out iconInfo))
                {
                    //7

                    Marshal.ReleaseComObject(iconInfo);

                    GDIStuff.DeleteObject(hicon);

                    return null;
                }

                point.X = cursorInfo.ptScreenPos.x - iconInfo.xHotspot;
                point.Y = cursorInfo.ptScreenPos.y - iconInfo.yHotspot;

                bool retFinal = false;

                using (var maskBitmap = Image.FromHbitmap(iconInfo.hbmMask))
                {
                    //Is this a monochrome cursor?  
                    if (maskBitmap.Height == maskBitmap.Width * 2 && iconInfo.hbmColor == IntPtr.Zero)
                    {
                        using (var final = new Bitmap(maskBitmap.Width, maskBitmap.Width))
                        {
                            var hDesktop = Win32Stuff.GetDesktopWindow();
                            var dcDesktop = Win32Stuff.GetWindowDC(hDesktop);

                            using (var resultGraphics = Graphics.FromImage(final))
                            {
                                var resultHdc = resultGraphics.GetHdc();
                                var offsetX = (int)((point.X + 3) * scale);
                                var offsetY = (int)((point.Y + 3) * scale);

                                GDIStuff.BitBlt(resultHdc, 0, 0, final.Width, final.Height, dcDesktop, offsetX, offsetY, (int)CopyPixelOperation.SourceCopy);
                                Win32Stuff.DrawIconEx(resultHdc, 0, 0, cursorInfo.hCursor, 0, 0, 0, IntPtr.Zero, 0x0003);

                                //TODO: I have to try removing the background of this cursor capture.
                                //Native.BitBlt(resultHdc, 0, 0, final.Width, final.Height, dcDesktop, (int)point.X + 3, (int)point.Y + 3, Native.CopyPixelOperation.SourceErase);

                                //Original, ignores the screen as background.
                                //Native.BitBlt(resultHdc, 0, 0, resultBitmap.Width, resultBitmap.Height, maskHdc, 0, resultBitmap.Height, Native.CopyPixelOperation.SourceCopy); //SourceCopy
                                //Native.BitBlt(resultHdc, 0, 0, resultBitmap.Width, resultBitmap.Height, maskHdc, 0, 0, Native.CopyPixelOperation.PatInvert); //SourceInvert

                                //7
                                GDIStuff.DeleteObject(resultHdc);

                                //7resultGraphics.ReleaseHdc(resultHdc);
                                //7Win32Stuff.ReleaseDC(hDesktop, dcDesktop);                                
                            }

                            retFinal = true;

                            GDIStuff.DeleteObject(iconInfo.hbmMask);
                            //7
                            GDIStuff.DeleteObject(iconInfo.hbmColor);
                            GDIStuff.DeleteObject(dcDesktop);
                            GDIStuff.DeleteObject(hDesktop);

                            //7
                            Marshal.ReleaseComObject(iconInfo);
                            Marshal.ReleaseComObject(cursorInfo);

                            GDIStuff.DeleteObject(hicon);
                            //7GDIStuff.DeleteDC(dcDesktop);

                            return final;
                        }
                    }                    
                }                               

                using (var icon = Icon.FromHandle(hicon))
                {
                    GDIStuff.DeleteObject(iconInfo.hbmColor);
                    GDIStuff.DeleteObject(iconInfo.hbmMask);
                    GDIStuff.DeleteObject(hicon);

                    //7
                    Marshal.ReleaseComObject(cursorInfo);
                    Marshal.ReleaseComObject(iconInfo);

                    return icon.ToBitmap();
                }
            }
            catch (Exception ex)
            {
                //LogWriter.Log(ex, "Impossible to get the cursor.");
            }

            return null;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CURSORINFO
        {
            public Int32 cbSize;
            public Int32 flags;
            public IntPtr hCursor;
            public POINTAPI ptScreenPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINTAPI
        {
            public int x;
            public int y;
        }

        [DllImport("user32.dll")]
        private static extern bool GetCursorInfo(out CURSORINFO pci);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool DrawIconEx(IntPtr hdc, int xLeft, int yTop, IntPtr hIcon, int cxWidth, int cyHeight, int istepIfAniCur, IntPtr hbrFlickerFreeDraw, int diFlags);

        private const Int32 CURSOR_SHOWING = 0x0001;
        private const Int32 DI_NORMAL = 0x0003;

        public static Bitmap CaptureCursor3(ref Point position)
        {
            CURSORINFO cursorInfo = new CURSORINFO();
            cursorInfo.cbSize = Marshal.SizeOf(cursorInfo);

            if (!GetCursorInfo(out cursorInfo))
            {
                Marshal.ReleaseComObject(cursorInfo);

                return null;
            }

            if (cursorInfo.flags != CURSOR_SHOWING)
            {
                Marshal.ReleaseComObject(cursorInfo);

                return null;
            }

            IntPtr hicon = Win32Stuff.CopyIcon(cursorInfo.hCursor);

            if (hicon == IntPtr.Zero)
            {
                Marshal.ReleaseComObject(hicon);

                Marshal.ReleaseComObject(cursorInfo);

                return null;

            }

            Win32Stuff.ICONINFO iconInfo;

            if (!Win32Stuff.GetIconInfo(hicon, out iconInfo))
            {
                Marshal.ReleaseComObject(hicon);

                Marshal.ReleaseComObject(cursorInfo);

                Marshal.ReleaseComObject(iconInfo);

                return null;
            }

            position.X = cursorInfo.ptScreenPos.x - iconInfo.xHotspot;
            position.Y = cursorInfo.ptScreenPos.y - iconInfo.yHotspot;

            using (Bitmap maskBitmap = Bitmap.FromHbitmap(iconInfo.hbmMask))
            {
                // check for monochrome cursor
                if (maskBitmap.Height == maskBitmap.Width * 2)
                {
                    Bitmap cursor = new Bitmap(32, 32, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    Color BLACK = Color.FromArgb(255, 0, 0, 0); //cannot compare Color.Black because of different names
                    Color WHITE = Color.FromArgb(255, 255, 255, 255); //cannot compare Color.White because of different names

                    for (int y = 0; y < 32; y++)
                    {
                        for (int x = 0; x < 32; x++)
                        {
                            Color maskPixel = maskBitmap.GetPixel(x, y);
                            Color cursorPixel = maskBitmap.GetPixel(x, y + 32);
                            if (maskPixel == WHITE && cursorPixel == BLACK)
                            {
                                cursor.SetPixel(x, y, Color.Transparent);
                            }
                            else if (maskPixel == BLACK)
                            {
                                cursor.SetPixel(x, y, cursorPixel);
                            }
                            else
                            {
                                cursor.SetPixel(x, y, cursorPixel == BLACK ? WHITE : BLACK);
                            }
                        }
                    }

                    return cursor;
                }
            }

            Icon icon = Icon.FromHandle(hicon);

            Bitmap bmp1= icon.ToBitmap();

            icon.Dispose();
            icon = null;

            Marshal.ReleaseComObject(hicon);

            Marshal.ReleaseComObject(cursorInfo);

            Marshal.ReleaseComObject(iconInfo);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return bmp1;

        
        }

        public static Bitmap CaptureImageCursor2(ref int x, ref int y)
        {
            try
            {
                var cursorInfo = new Win32Stuff.CURSORINFO();
                cursorInfo.cbSize = Marshal.SizeOf(cursorInfo);

                if (!Win32Stuff.GetCursorInfo(out cursorInfo))
                    return null;

                if (cursorInfo.flags != Win32Stuff.CURSOR_SHOWING)
                    return null;

                var hicon = Win32Stuff.CopyIcon(cursorInfo.hCursor);
                if (hicon == IntPtr.Zero)
                    return null;

                Win32Stuff.ICONINFO iconInfo;
                if (!Win32Stuff.GetIconInfo(hicon, out iconInfo))
                {
                    Win32Stuff.DestroyIcon(hicon);
                    return null;
                }

                Point point = new Point();

                point.X = cursorInfo.ptScreenPos.x - iconInfo.xHotspot;
                point.Y = cursorInfo.ptScreenPos.y - iconInfo.yHotspot;

                x = point.X;
                y = point.Y;

                Bitmap final = null;

                using (var maskBitmap = Image.FromHbitmap(iconInfo.hbmMask))
                {
                    //Is this a monochrome cursor?  
                    if (maskBitmap.Height == maskBitmap.Width * 2 && iconInfo.hbmColor == IntPtr.Zero)
                    {
                        final = new Bitmap(maskBitmap.Width, maskBitmap.Width);
                        var hDesktop = Win32Stuff.GetDesktopWindow();
                        var dcDesktop = Win32Stuff.GetWindowDC(hDesktop);

                        using (var resultGraphics = Graphics.FromImage(final))
                        {
                            var resultHdc = resultGraphics.GetHdc();

                            //3GDIStuff.BitBlt(resultHdc, 0, 0, final.Width, final.Height, dcDesktop, (int)point.X + 3, (int)point.Y + 3, CopyPixelOperation.SourceCopy);

                            GDIStuff.BitBlt(resultHdc, 0, 0, final.Width, final.Height, dcDesktop, (int)point.X + 3, (int)point.Y + 3, (int)CopyPixelOperation.SourceCopy);

                            Win32Stuff.DrawIconEx(resultHdc, 0, 0, cursorInfo.hCursor, 0, 0, 0, IntPtr.Zero, 0x0003);

                            //TODO: I have to try removing the background of this cursor capture.
                            //Native.BitBlt(resultHdc, 0, 0, final.Width, final.Height, dcDesktop, (int)point.X + 3, (int)point.Y + 3, Native.CopyPixelOperation.SourceErase);

                            //3resultGraphics.ReleaseHdc(resultHdc);
                            //3Win32Stuff.ReleaseDC(hDesktop, dcDesktop);
                        }

                        //3GDIStuff.DeleteObject(iconInfo.hbmMask);
                        //3GDIStuff.DeleteDC(dcDesktop);
                        //3Win32Stuff.DestroyIcon(hicon);

                        //return final;
                    }

                    //3GDIStuff.DeleteObject(iconInfo.hbmColor);
                    //3GDIStuff.DeleteObject(iconInfo.hbmMask);
                    
                }

                var icon = Icon.FromHandle(hicon);
                Bitmap bmp2=icon.ToBitmap();

                Win32Stuff.DestroyIcon(hicon);
            }
            catch (Exception ex)
            {
                //You should catch exception with your method here.
                //LogWriter.Log(ex, "Impossible to get the cursor.");
            }

            return null;
        }

        public static Bitmap CaptureCursor(ref int x, ref int y)
        {
            Bitmap bmp;
            IntPtr hicon;
            Win32Stuff.CURSORINFO ci = new Win32Stuff.CURSORINFO();
            Win32Stuff.ICONINFO icInfo;
            ci.cbSize = Marshal.SizeOf(ci);

            if (Win32Stuff.GetCursorInfo(out ci))
            {
                if (ci.flags == Win32Stuff.CURSOR_SHOWING)
                {
                    hicon = Win32Stuff.CopyIcon(ci.hCursor);
                    if (Win32Stuff.GetIconInfo(hicon, out icInfo))
                    {
                        x = ci.ptScreenPos.x - ((int)icInfo.xHotspot);
                        y = ci.ptScreenPos.y - ((int)icInfo.yHotspot);

                       
                        Icon ic = Icon.FromHandle(hicon);

                        //bmp = Bitmap.FromHicon(hicon);

                        bmp = ic.ToBitmap();

                        bmp = Bitmap.FromHicon(bmp.GetHicon());

                        return bmp;
                    }
                }
            }

            return null;
        }

        public static Bitmap CaptureDesktopWithCursor()
        {
            int cursorX = 0;
            int cursorY = 0;
            Bitmap desktopBMP;
            Bitmap cursorBMP;
            Bitmap finalBMP;
            Graphics g;
            Rectangle r;

            desktopBMP = CaptureDesktop();
            cursorBMP = CaptureCursor(ref cursorX, ref cursorY);
            if (desktopBMP != null)
            {
                if (cursorBMP != null)
                { 
                    r = new Rectangle(cursorX, cursorY, cursorBMP.Width, cursorBMP.Height);
                    g = Graphics.FromImage(desktopBMP);
                    g.DrawImage(cursorBMP, r);
                    g.Flush();

                    return desktopBMP;
                }
                else
                    return desktopBMP;
            }

            return null;

        }


    }
}
