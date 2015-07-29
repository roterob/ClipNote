using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eClipx
{
    public class KeyboardHook
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static User32.LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private static bool ctrlRequired;
        private static bool shiftRequired;
        private static Keys keyRequired;
        private static Action _callback;

        static KeyboardHook()
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                _hookID = User32.SetWindowsHookEx(WH_KEYBOARD_LL, _proc,
                    User32.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public static void CallWhen(bool ctrl, bool shift, Keys key, Action callback)
        {
            ctrlRequired = ctrl;
            shiftRequired = shift;
            keyRequired = key;
            _callback = callback;
        }

        public static void Unbind()
        {
            User32.UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            bool ctrlIsPressed = ((Control.ModifierKeys & Keys.Control) == Keys.Control);
            bool shiftIsPressed = ((Control.ModifierKeys & Keys.Shift) == Keys.Shift);

            if (nCode >= 0 && (!ctrlRequired || ctrlIsPressed ) && (!shiftRequired || shiftIsPressed) && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if ((Keys)vkCode == keyRequired)
                {
                    _callback();
                    return (System.IntPtr)1; 
                }
            }
            return User32.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
