using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TextExpander.Interfaces;

namespace TextExpander.Services
{
    /// <summary>
    /// Klasa implementująca globalny hook klawiatury.
    /// Umożliwia przechwytywanie i obsługę zdarzeń klawiatury na poziomie systemu.
    /// </summary>
    public class GlobalKeyboardHook : IKeyboardHook
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        private readonly LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;
        private readonly object _lockObject = new object();

        /// <summary>
        /// Zdarzenie wywoływane, gdy klawisz zostaje wciśnięty.
        /// </summary>
        public event KeyEventHandler? KeyDown;

        /// <summary>
        /// Inicjalizuje nową instancję klasy GlobalKeyboardHook.
        /// </summary>
        public GlobalKeyboardHook()
        {
            _proc = HookCallback;
        }

        /// <summary>
        /// Rozpoczyna nasłuchiwanie zdarzeń klawiatury.
        /// </summary>
        public void StartListening()
        {
            lock (_lockObject)
            {
                if (_hookID == IntPtr.Zero)
                {
                    using (Process curProcess = Process.GetCurrentProcess())
                    using (ProcessModule? curModule = curProcess.MainModule)
                    {
                        _hookID = SetWindowsHookEx(WH_KEYBOARD_LL, _proc,
                            GetModuleHandle(curModule?.ModuleName), 0);
                    }
                }
            }
        }

        /// <summary>
        /// Zatrzymuje nasłuchiwanie zdarzeń klawiatury.
        /// </summary>
        public void StopListening()
        {
            lock (_lockObject)
            {
                if (_hookID != IntPtr.Zero)
                {
                    UnhookWindowsHookEx(_hookID);
                    _hookID = IntPtr.Zero;
                }
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            lock (_lockObject)
            {
                if (nCode >= 0)
                {
                    var vkCode = Marshal.ReadInt32(lParam);
                    var key = (Keys)vkCode;
                    var ctrl = (Control.ModifierKeys & Keys.Control) != 0;
                    var alt = (Control.ModifierKeys & Keys.Alt) != 0;
                    var shift = (Control.ModifierKeys & Keys.Shift) != 0;

                    if (wParam == (IntPtr)WM_KEYDOWN)
                    {
                        var e = new KeyEventArgs(key | (ctrl ? Keys.Control : Keys.None) |
                            (alt ? Keys.Alt : Keys.None) | (shift ? Keys.Shift : Keys.None));
                        
                        // Bezpieczne wywoływanie zdarzenia
                        var handler = KeyDown;
                        if (handler != null)
                        {
                            handler.Invoke(this, e);
                            if (e.Handled)
                                return (IntPtr)1;
                        }
                    }
                }

                return CallNextHookEx(_hookID, nCode, wParam, lParam);
            }
        }

        #region Native Methods
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn,
            IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string? lpModuleName);
        #endregion
    }
} 