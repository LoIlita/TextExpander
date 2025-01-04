using System.Windows.Forms;
using System.Reflection;

namespace TextExpander.Controls
{
    /// <summary>
    /// ListView z włączonym podwójnym buforowaniem dla zredukowania migotania.
    /// </summary>
    public class DoubleBufferedListView : ListView
    {
        private ScrollBars scrollBars = ScrollBars.Both;

        public new ScrollBars ScrollBars
        {
            get { return scrollBars; }
            set 
            { 
                scrollBars = value;
                if (IsHandleCreated)
                {
                    RecreateHandle();
                }
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                
                // Usuń flagi dla pasków przewijania
                cp.Style &= ~0x200000; // WS_VSCROLL
                cp.Style &= ~0x100000; // WS_HSCROLL

                // Dodaj odpowiednie paski przewijania
                if ((scrollBars & ScrollBars.Vertical) != 0)
                    cp.Style |= 0x200000;
                if ((scrollBars & ScrollBars.Horizontal) != 0)
                    cp.Style |= 0x100000;
                
                return cp;
            }
        }

        protected override void WndProc(ref Message m)
        {
            // Blokuj zmianę rozmiaru kolumn
            if (m.Msg == 0x0114) // WM_HSCROLL
                return;
            if (m.Msg == 0x0115) // WM_VSCROLL
                if (m.WParam.ToInt32() == 0)
                    return;
            base.WndProc(ref m);
        }

        public DoubleBufferedListView()
        {
            // Włącz podwójne buforowanie za pomocą refleksji
            var pi = typeof(ListView).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
            pi?.SetValue(this, true);

            // Podstawowa konfiguracja
            View = View.Details;
            FullRowSelect = true;
            GridLines = true;

            // Domyślnie tylko pionowy pasek przewijania
            ScrollBars = ScrollBars.Vertical;

            // Dodaj obsługę zdarzeń
            this.HandleCreated += (s, e) => 
            {
                // Wymuś pełne odświeżenie po utworzeniu kontrolki
                this.Invalidate(true);
            };
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            // Wymuś odświeżenie wszystkich elementów
            if (this.Items.Count > 0)
            {
                this.RedrawItems(0, this.Items.Count - 1, true);
            }
            base.OnInvalidated(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            // Wymuś odświeżenie po zmianie zaznaczenia
            this.Invalidate();
        }
    }
} 