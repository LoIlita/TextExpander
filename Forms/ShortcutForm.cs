using System;
using System.Windows.Forms;

namespace TextExpander.Forms
{
    /// <summary>
    /// Formularz do dodawania i edycji skrótów.
    /// </summary>
    public partial class ShortcutForm : Form
    {
        private TextBox txtShortcut = null!;
        private TextBox txtValue = null!;
        private Button btnOK = null!;
        private Button btnCancel = null!;

        /// <summary>
        /// Pobiera wprowadzony klucz skrótu.
        /// </summary>
        public string ShortcutKey => txtShortcut.Text.Trim();

        /// <summary>
        /// Pobiera wprowadzone rozwinięcie skrótu.
        /// </summary>
        public string ShortcutValue => txtValue.Text.Trim();

        /// <summary>
        /// Inicjalizuje nową instancję formularza ShortcutForm.
        /// </summary>
        /// <param name="shortcutKey">Klucz skrótu do edycji. Jeśli pusty, formularz działa w trybie dodawania.</param>
        /// <param name="shortcutValue">Wartość skrótu do edycji.</param>
        public ShortcutForm(string? shortcutKey = "", string? shortcutValue = "")
        {
            InitializeComponent();
            txtShortcut.Text = shortcutKey ?? string.Empty;
            txtValue.Text = shortcutValue ?? string.Empty;

            if (!string.IsNullOrEmpty(shortcutKey))
            {
                Text = "Edytuj skrót";
            }
            else
            {
                Text = "Dodaj skrót";
            }
        }

        private void InitializeComponent()
        {
            this.Size = new System.Drawing.Size(400, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            var lblShortcut = new Label
            {
                Text = "Skrót:",
                Location = new System.Drawing.Point(12, 15),
                Size = new System.Drawing.Size(60, 20)
            };

            txtShortcut = new TextBox
            {
                Location = new System.Drawing.Point(82, 12),
                Size = new System.Drawing.Size(290, 20)
            };

            var lblValue = new Label
            {
                Text = "Rozwinięcie:",
                Location = new System.Drawing.Point(12, 45),
                Size = new System.Drawing.Size(60, 20)
            };

            txtValue = new TextBox
            {
                Location = new System.Drawing.Point(82, 42),
                Size = new System.Drawing.Size(290, 20),
                Multiline = true,
                Height = 60
            };

            btnOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new System.Drawing.Point(216, 120),
                Size = new System.Drawing.Size(75, 23)
            };
            btnOK.Click += BtnOK_Click;

            btnCancel = new Button
            {
                Text = "Anuluj",
                DialogResult = DialogResult.Cancel,
                Location = new System.Drawing.Point(297, 120),
                Size = new System.Drawing.Size(75, 23)
            };

            Controls.AddRange(new Control[] {
                lblShortcut,
                txtShortcut,
                lblValue,
                txtValue,
                btnOK,
                btnCancel
            });

            AcceptButton = btnOK;
            CancelButton = btnCancel;
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku OK.
        /// Sprawdza poprawność wprowadzonych danych przed zamknięciem formularza.
        /// </summary>
        private void BtnOK_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtShortcut.Text))
            {
                MessageBox.Show(
                    "Skrót nie może być pusty!", 
                    "Błąd walidacji",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
                DialogResult = DialogResult.None;
                return;
            }

            if (string.IsNullOrWhiteSpace(txtValue.Text))
            {
                MessageBox.Show(
                    "Rozwinięcie nie może być puste!", 
                    "Błąd walidacji",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
                DialogResult = DialogResult.None;
                return;
            }
        }
    }
} 