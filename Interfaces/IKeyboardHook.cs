using System;
using System.Windows.Forms;

namespace TextExpander.Interfaces
{
    public interface IKeyboardHook : IDisposable
    {
        event KeyEventHandler? KeyDown;
        event KeyEventHandler? KeyUp;
        void StartHook();
        void StopHook();
    }
} 