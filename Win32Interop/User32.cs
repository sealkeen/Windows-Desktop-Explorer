using System;
using System.Collections.Generic;
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
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;

        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

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
            
            //user32.SetWindowPos
            //user32.SetWindowPos();
            //NWindowsKits.NWindowsKits.CascadeWindows();
            //CascadeWindows(NULL, MDITILE_ZORDER, NULL, 0, NULL); // "Cascade windows"
        }
    }
}
