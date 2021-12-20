using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace Magnifier
{
    class InterceptKeys
    {
        [DllImport("user32.dll")]
        static extern bool SetKeyboardState(byte[] lpKeyState);
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags,
        UIntPtr dwExtraInfo);
        public const int KEYEVENTF_EXTENDEDKEY = 0x1;
        public const int KEYEVENTF_KEYUP = 0x2;        
        public const int NUMLOCK = 144;
        public const int CAPITAL = 0x14;
        public const int SCROLL = 0x91;
        public const int INSERT = 45;
        
        private const int WH_KEYBOARD_LL = 13;

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;

        private static LowLevelKeyboardProc _proc = HookCallback;

        public static bool InAddKeyMode = false;

        public static IntPtr _hookID = IntPtr.Zero;        

        /*
        private static bool bShift = false;
        private static bool bCtrl = false;
        private static bool bAlt = false;
        private static bool bAltGr = false;
        private static bool bLWin = false;
        private static bool bRWin = false;
        */

        private static int bShift = 0;
        private static int bCtrl = 0;
        private static int bAlt = 0;
        private static int bAltGr = 0;
        private static int bLWin = 0;
        private static int bRWin = 0;

        public static bool Hooked = false;
        public static bool HookedForAddKey = false;

        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

         [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
             public Keys key;
             public int scanCode;
             public int flags;
             public int time;
             public IntPtr extra;         
         }

        public static void DisableKeys()
        {
            if (!Hooked)
            {
                /*
                bwHookCallbak=new System.ComponentModel.BackgroundWorker();

                bwHookCallbak.DoWork += bwHookCallbak_DoWork;
                bwHookCallbak.RunWorkerAsync();
                */

                bShift = 0;
                bCtrl = 0;
                bAlt = 0;
                bAltGr = 0;
                bLWin = 0;
                bRWin = 0;

                _hookID = SetHook(_proc);
                Hooked = true;                
            }            
        }        

        public static void UnHookAll()
        {
            UnhookWindowsHookEx(_hookID);
            Hooked = false;            
        }        

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())

            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        //1private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam);

        private static bool ShouldSendKeys = true;

        public static List<HookItem> HookItems = new List<HookItem>();

        public static System.ComponentModel.BackgroundWorker bwHookCallbak=null;

        public static void SendNumLock()
        {
            keybd_event(NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            keybd_event(NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
        }
        /*
        private static IntPtr NewHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN || wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP)
            {
                //Module.ShowMessage("WM_KEYDOWN OR WM_KEYUP");

                KBDLLHOOKSTRUCT objKeyInfo = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

                int vkCode = (int)objKeyInfo.key;

                vkCode = Marshal.ReadInt32(lParam);

                //3HookItem hki = new HookItem(nCode, Marshal.ReadInt32(wParam), Marshal.ReadInt32(lParam));

                HookItem hki = new HookItem(nCode, (int)wParam, vkCode);

                lock (HookItems)
                {
                    HookItems.Add(hki);
                }

                return new IntPtr(1);
            }
            else
            {
                return CallNextHookEx(_hookID, nCode, wParam, lParam);
            }            
        }
        */

        public static bool CheckSetHookTimer = false;

        //private static bool ShowedMsg = false;

        //1private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        private static IntPtr HookCallback(int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParamRef)
        {            
            int vkCode = (int)lParamRef.key;
            int vkCode2 = vkCode;
            int flags = lParamRef.flags;            

            try
            {
                ShouldSendKeys = false;                                

                if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
                {                                                            
                    if (vkCode == (int)Keys.LWin || vkCode == (int)Keys.RWin)
                    {
                        bLWin++;
                        vkCode = -1;
                    }

                    if (vkCode == (int)Keys.Apps)
                    {
                        bRWin++;
                        vkCode = -1;
                    }

                    if (vkCode == (int)Keys.LShiftKey || vkCode == (int)Keys.RShiftKey || vkCode == (int)Keys.Shift || vkCode == (int)Keys.ShiftKey)
                    {
                        bShift++;
                        vkCode = -1;
                    }

                    if (vkCode == (int)Keys.LControlKey || vkCode == (int)Keys.RControlKey || vkCode == (int)Keys.Control || vkCode == (int)Keys.ControlKey)
                    {
                        bCtrl++;
                        vkCode = -1;
                    }

                    if (vkCode == (int)Keys.LMenu)
                    {
                        bAlt++;
                        vkCode = -1;
                    }

                    if (vkCode == (int)Keys.RMenu)
                    {
                        bAltGr++;
                        vkCode = -1;
                    }
                    
                    if (bCtrl != 0 && bAlt == 0 && bShift == 0 && vkCode == (int)Keys.F9)
                    {
                        frmMain.Instance.SpecifyAreas();
                        
                        return new IntPtr(1);
                    }

                    if (bCtrl == 0 && bAlt == 0 && bShift == 0 && vkCode == (int)Keys.Escape)
                    {
                        Environment.Exit(0);

                        return new IntPtr(1);
                    }                    
                }

                if (nCode >= 0 && (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP))
                {                                       
                    int vkCode3 = vkCode;

                    if (vkCode == (int)Keys.LWin || vkCode == (int)Keys.RWin)
                    {
                        vkCode3 = -1;
                    }

                    if (vkCode == (int)Keys.Apps)
                    {
                        vkCode3 = -1;
                    }

                    if (vkCode == (int)Keys.LShiftKey || vkCode == (int)Keys.RShiftKey || vkCode == (int)Keys.Shift || vkCode == (int)Keys.ShiftKey)
                    {
                        vkCode3 = -1;
                    }

                    if (vkCode == (int)Keys.LControlKey || vkCode == (int)Keys.RControlKey || vkCode == (int)Keys.Control || vkCode == (int)Keys.ControlKey)
                    {
                        vkCode3 = -1;
                    }

                    if (vkCode == (int)Keys.LMenu)
                    {
                        vkCode3 = -1;
                    }

                    if (vkCode == (int)Keys.RMenu)
                    {
                        vkCode3 = -1;
                    }

                    // ===================================

                    if (vkCode == (int)Keys.LWin || vkCode == (int)Keys.RWin)
                    {
                        bLWin = 0;
                    }

                    if (vkCode == (int)Keys.Apps)
                    {
                        bRWin = 0;
                    }

                    if (vkCode == (int)Keys.LShiftKey || vkCode == (int)Keys.RShiftKey || vkCode == (int)Keys.Shift || vkCode == (int)Keys.ShiftKey)
                    {
                        bShift = 0;
                    }

                    if (vkCode == (int)Keys.LControlKey || vkCode == (int)Keys.RControlKey || vkCode == (int)Keys.Control || vkCode == (int)Keys.ControlKey)
                    {
                        bCtrl = 0;
                    }

                    if (vkCode == (int)Keys.LMenu)
                    {
                        bAlt = 0;
                    }

                    if (vkCode == (int)Keys.RMenu)
                    {
                        bAltGr = 0;
                    }
                }

                /*
                if (nCode >= 0 && wParam == (IntPtr)WM_SYSKEYDOWN)
                {
                    int vkCode = Marshal.ReadInt32(lParam);
                    int vkCode2 = vkCode;

                    if (vkCode == (int)Keys.LWin)
                    {
                        bLWin = false;
                    }

                    if (vkCode == (int)Keys.Apps)
                    {
                        bRWin = false;
                    }

                    if (vkCode == (int)Keys.LShiftKey || vkCode == (int)Keys.RShiftKey)
                    {
                        bShift = true;
                        vkCode = -1;
                    }

                    if (vkCode == (int)Keys.LControlKey || vkCode == (int)Keys.RControlKey)
                    {
                        bCtrl = true;
                        vkCode = -1;
                    }

                    if (vkCode == (int)Keys.LMenu)
                    {
                        bAlt = true;
                        vkCode = -1;
                    }

                    if (vkCode == (int)Keys.RMenu)
                    {
                        bAltGr = true;
                        vkCode = -1;
                    }

                    //Console.WriteLine((Keys)vkCode);                   
                    
                    if (InAddKeyMode)
                    {
                        frmMain.Instance.DisplayHotKeyPressed(vkCode, bShift, bCtrl, bAlt, bAltGr, bLWin, bRWin);
                    }

                    if (frmMain.Instance.IsHotKey(vkCode, vkCode2,bShift, bCtrl, bAlt, bAltGr, bLWin, bRWin))
                    {
                        return new IntPtr(1);

                    }
                }
                */                
                  
                return CallNextHookEx(_hookID, nCode, wParam, ref lParamRef);
                
            }
            finally
            {
                ShouldSendKeys = true;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern IntPtr SetWindowsHookEx(int idHook,

            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        [return: MarshalAs(UnmanagedType.Bool)]

        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,

            //1IntPtr wParam, IntPtr lParam);

            IntPtr wParam, ref KBDLLHOOKSTRUCT lParamRef);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern IntPtr GetModuleHandle(string lpModuleName);

    }

    public class HookItem
    {
        public int nCode;
        public int wParam; 
        public int lParam;

        public HookItem(int _nCode, int _wParam, int _lParam)
        {
            nCode = _nCode;
            wParam = _wParam;
            lParam = _lParam;
        }
    }

}