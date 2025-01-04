using System;
using System.Windows.Forms;

namespace TextExpander.Interfaces
{
    /// <summary>
    /// Interfejs definiujący operacje przechwytywania zdarzeń klawiatury.
    /// Umożliwia nasłuchiwanie i reagowanie na zdarzenia klawiatury w systemie.
    /// </summary>
    public interface IKeyboardHook
    {
        /// <summary>
        /// Zdarzenie wywoływane, gdy klawisz zostaje wciśnięty.
        /// </summary>
        event KeyEventHandler KeyDown;

        /// <summary>
        /// Rozpoczyna nasłuchiwanie zdarzeń klawiatury.
        /// </summary>
        void StartListening();

        /// <summary>
        /// Zatrzymuje nasłuchiwanie zdarzeń klawiatury.
        /// </summary>
        void StopListening();
    }
} 