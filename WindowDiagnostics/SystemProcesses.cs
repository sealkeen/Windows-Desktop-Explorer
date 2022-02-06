using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WindowDiagnostics
{
    public class SystemProcesses
    {
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

        const int WM_COMMAND = 0x111;
        const int MIN_ALL = 419;
        const int MIN_ALL_UNDO = 416;

        public static void MinimizeAll()
        {
            IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
            SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL, IntPtr.Zero);
            System.Threading.Thread.Sleep(2000);
            SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL_UNDO, IntPtr.Zero);
        }

        public static Process[] GetPocesses()
        {
            return Process.GetProcesses();
        }

        public static void listProcesses()
        {
            foreach (Process process in GetPocesses())
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
                }
            }
        }
    }
}
