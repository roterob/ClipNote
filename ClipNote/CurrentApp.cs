using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eClipx
{
    public static class CurrentApp
    {
        private static System.Timers.Timer _timer;
        private static IntPtr previousHandler = IntPtr.Zero;
        private static string titlePreviousHandler;

        public static IntPtr Handler { get { return previousHandler; } }

        internal static void StartWatcher()
        {
            _timer = new System.Timers.Timer(500);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        internal static void StopWatcher()
        {
            _timer.Stop();
            _timer.Dispose();
        }



        static void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            IntPtr currentHandler = User32.GetForegroundWindow();
            if (currentHandler != Common.MyHandle)
            {
                StringBuilder sbTitle = new StringBuilder(1024);
                User32.GetWindowText(currentHandler, sbTitle, sbTitle.Capacity);
                titlePreviousHandler = sbTitle.ToString();
                previousHandler = currentHandler;
            }
        }

        public static void SendKeys(string text)
        {
            //TODO: Evaluar el uso de GetCurrentApp en vez del watcher
            //previousHandler = GetCurrentApp();
            if (previousHandler != IntPtr.Zero)
            {
                Clipboard.SetText(text, TextDataFormat.Text);
                User32.SetForegroundWindow(previousHandler);
                System.Threading.Thread.Sleep(500);
                System.Windows.Forms.SendKeys.Send("^v");
            }
        }


        private static int GetCurrentApp()
        {
            int res = 0;
            var windows = new List<string>();

            int nDeshWndHandle = User32.GetDesktopWindow();
            int nChildHandle = User32.GetWindow(nDeshWndHandle,
                                User32.GW_CHILD);

            int count = 0;
            while (nChildHandle != 0)
            {
                if (nChildHandle == Common.MyHandle.ToInt32())
                {
                    nChildHandle = User32.GetWindow(nChildHandle, User32.GW_HWNDNEXT);
                }

                if (User32.IsWindowVisible(nChildHandle) != 0)
                {
                    StringBuilder sbTitle = new StringBuilder(1024);
                    User32.GetWindowText(nChildHandle, sbTitle, sbTitle.Capacity);
                    String sWinTitle = sbTitle.ToString();
                    if (sWinTitle.Length > 0)
                    {
                        windows.Add(sWinTitle);

                        if (count > 0)
                        {
                            return nChildHandle;
                        }
                        else
                        {
                            count++;
                        }
                    }


                }
                nChildHandle = User32.GetWindow(nChildHandle, User32.GW_HWNDNEXT);
            }

            return res;
        }

    }
}
