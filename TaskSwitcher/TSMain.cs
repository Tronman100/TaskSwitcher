using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TaskSwitcher
{
    class TSMain
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string className, string windowTitle);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);

        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref Windowplacement lpwndpl);

        private enum ShowWindowEnum
        {
            Hide = 0,
            ShowNormal = 1, ShowMinimized = 2, ShowMaximized = 3,
            Maximize = 3, ShowNormalNoActivate = 4, Show = 5,
            Minimize = 6, ShowMinNoActivate = 7, ShowNoActivate = 8,
            Restore = 9, ShowDefault = 10, ForceMinimized = 11
        };

        private struct Windowplacement
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        // Code adapted from
        // https://stackoverflow.com/questions/7268302/get-the-titles-of-all-open-windows
        private static void PrintWindowTitles()
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (process.MainWindowHandle != IntPtr.Zero && process.MainWindowTitle.Length != 0)
                {
                    System.Console.Out.WriteLine(process.MainWindowTitle);
                }
            }
        }

        // Code adapted from 
        // https://stackoverflow.com/questions/9099479/restore-a-minimized-window-of-another-application

        private static void BringWindowToFront(string[] args)
        {
            string windowTitle = args[0];

            IntPtr wdwIntPtr = FindWindow(null, windowTitle);

            if (wdwIntPtr == IntPtr.Zero && args.Length > 1)
            {
                // Code adapted from
                // https://stackoverflow.com/questions/240171/launching-an-application-exe-from-c
                Process processToLaunch = new Process();
                processToLaunch.StartInfo.FileName = args[1];
                string arguments = "";
                for (int nextArg = 2; nextArg < args.Length; ++nextArg)
                {
                    arguments += args[nextArg];
                }

                processToLaunch.StartInfo.Arguments = arguments;
                processToLaunch.Start();
                return;
            }

            //get the hWnd of the process
            Windowplacement placement = new Windowplacement();
            GetWindowPlacement(wdwIntPtr, ref placement);

            // Check if window is minimized
            if (placement.showCmd == 2)
            {
                //the window is hidden so we restore it
                ShowWindow(wdwIntPtr, ShowWindowEnum.Restore);
            }

            //set user's focus to the window
            SetForegroundWindow(wdwIntPtr);
        }

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                System.Console.Out.WriteLine("TaskSwitcher v1.0");
                System.Console.Out.WriteLine("Usage: TaskSwitcher list");
                System.Console.Out.WriteLine("Usage: TaskSwitcher <program name> [<start program>] [<args1>  - <argsN>]");
                return;
            }

            if (args[0].ToLower() == "list")
            {
                PrintWindowTitles();
                return;
            }

            BringWindowToFront(args);
        }
    }
}
