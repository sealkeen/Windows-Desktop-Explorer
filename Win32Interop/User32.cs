using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Controls;
using NWindowsKits.windef.structs;
using NWindowsKits;

namespace Win32Interop
{
    public class User32
    {
        public static NWindowsKits.HWND defHWND = default(HWND);
        public static NWindowsKits.RECT defRECT = default(RECT);
        public const UInt32 SWP_NOSIZE = 0x0001;
        public const UInt32 SWP_NOMOVE = 0x0002;
        public static IntPtr nullptr = IntPtr.Zero;

        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        static public void SwitchToThisWindow(IntPtr window)
        {
            var hwnd = ХВНД.Криейт(window);
            NWindowsKits.user32.SwitchToThisWindow(hwnd, 1);
        }

        static public void SendWpfWindowBack(Window window)
        {
            HWND hWnd = ХВНД.Криейт(new WindowInteropHelper(window).Handle);
            HWND HWNDBottom = ХВНД.Криейт(new IntPtr(1));
            NWindowsKits.user32.SetWindowPos(hWnd, HWNDBottom, 0, 0, 0, 0, C.SWP_NOSIZE | C.SWP_NOMOVE);
        }

        static public void TileWindows()
        {
            user32.TileWindows(defHWND, C.MDITILE_HORIZONTAL, ref defRECT, 0, ref defHWND); // "Show windows stacked"
        }

        static public void CascadeWindows()
        {
            try
            {
                //user32.SetWindowPos
                //user32.SetWindowPos();
                //NWindowsKits.NWindowsKits.CascadeWindows();
                
                Rectangle rect = new Rectangle(0, 0, 1740, 1010);
                var arrayRange = Process.GetProcesses()
                    .Where(x => 
                        !string.IsNullOrEmpty(x.MainWindowTitle) &&
                        !x.MainWindowTitle.Contains("MainWindow")
                    );
                    //.Select(x => x.Handle).ToArray();

                int X = 25; int Y = 25;
                if (arrayRange.Count() > 0)
                {
                    var lastProc = arrayRange.First();
                    foreach (var proc in arrayRange)
                    {
                        var хандле = ХВНД.Криейт(proc.MainWindowHandle);
                        user32.MoveWindow(хандле, X, Y, 735, 635, 1);
                        X += 18; Y += 18;
                        lastProc = proc;
                        user32.ShowWindow(хандле, C.SW_RESTORE); // SW_RESTORE = 9,
                        user32.SetForegroundWindow(хандле);
                    }
                    user32.MoveWindow(ХВНД.Криейт(lastProc.MainWindowHandle), X, Y, 1260, 840, 1);
                }

                //user32.CascadeWindows(nullptr, 0, ref rect, 0, arrayRange); // "Cascade windows"
            } catch (Exception ex) {
            }
        }
        public static void IncreaseForegroundWindow()
        {
            var handle = user32.GetForegroundWindow();
            user32.MoveWindow(handle, 180, 0, 1640, 1080, 1);
        }
    }
}
