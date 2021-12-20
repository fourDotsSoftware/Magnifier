using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Magnifier
{
    public class InterceptMouse
    {
        private static LowLevelMouseProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        /*
        public static void Main()
        {
            _hookID = SetHook(_proc);
            Application.Run();
            UnhookWindowsHookEx(_hookID);
        }
        */

        public static bool DisableMouse()
        {
            _hookID = SetHook(_proc);

            return true;
        }

        public static bool UnHookAll()
        {
            return UnhookWindowsHookEx(_hookID);
        }

        public static IntPtr SetHook(LowLevelMouseProc proc)
        {
            return SetWindowsHookEx(WH_MOUSE_LL, proc,
                    IntPtr.Zero, 0);

            /*
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }*/
        }

        public delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        public static int PreviousX = -1;
        public static int PreviousY = -1;

        public static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {

            if (frmMain.Instance.Visible)
            {
                int x = System.Windows.Forms.Control.MousePosition.X;
                int y = System.Windows.Forms.Control.MousePosition.Y;

                if (PreviousX != -1)
                {
                    int difx = x - PreviousX;
                    int dify = y - PreviousY;

                    if (Properties.Settings.Default.FollowMouseCursor)
                    {
                        frmMain.Instance.Left += difx;
                        frmMain.Instance.Top += dify;
                    }
                }

                PreviousX = x;
                PreviousY = y;
            }

            /*
            if (nCode >= 0)                
            {
                
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                //Console.WriteLine(hookStruct.pt.x + ", " + hookStruct.pt.y);

                int x = hookStruct.pt.x;
                int y = hookStruct.pt.y;

                bool x1 = ((long)(ExtensionMethods.HighWord(hookStruct.mouseData)) == (long)MouseMessages.XBUTTON1);
                bool x2 = ((long)(ExtensionMethods.HighWord(hookStruct.mouseData)) == (long)MouseMessages.XBUTTON2);
                
                if (frmMain.Instance.IsMouseHotButton((MouseMessages)wParam,x,y,x1,x2))
                {
                    List<IRemapDetailsItem> lst = frmMain.Instance.IsRemapMouseButton((MouseMessages)wParam);

                        if (lst != null)
                        {
                            RemapDetails.ExecuteRemap(lst);

                            return new IntPtr(1);
                        }
                        else
                        {
                            return new IntPtr(1);
                        }
                    
                }
            }            
            */

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private const int WH_MOUSE_LL = 14;

        public enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_MOUSEHWHEEL=0x020E,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_MBUTTONDOWN= 0x0207,        
            WM_MBUTTONUP=0x0208,
            WM_MBUTTONDBLCLK=0x0209,            
            WM_XBUTTONDOWN=0x020B, 
            WM_XBUTTONUP=0x020C,
            XBUTTON1=0x0001,            
            XBUTTON2=0x0002,
            WM_LBUTTONDBLCLK=0x0203,
            WM_RBUTTONDBLCLK=0x0206,
            WM_XBUTTONDBLCLK=0x020D                              
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]

        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

    }

    public static class ExtensionMethods
    {
        public static long LowWord(long number)
        { return number & 0x0000FFFF; }
        public static long LowWord(long number, long newValue)
        { return (number & 0xFFFF0000) + (newValue & 0x0000FFFF); }
        public static long HighWord(long number)
        { return number & 0xFFFF0000; }
        public static long HighWord(long number, long newValue)
        { return (number & 0x0000FFFF) + (newValue << 16); }
    }
}
