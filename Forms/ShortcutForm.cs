using System;
using System.Windows.Forms;

namespace TextExpander.Forms
{
    public partial class ShortcutForm : Form
    {
        private TextBox txtShortcut = null!;
        private TextBox txtValue = null!;
        private Button btnOK = null!;
        private Button btnCancel = null!;

        public string ShortcutKey => txtShortcut.Text.Trim();
        public string ShortcutValue => txtValue.Text.Trim();

        public ShortcutForm(string shortcutKey = "", string shortcutValue = "")
        {
            InitializeComponent();
            txtShortcut.Text = shortcutKey;
            txtValue.Text = shortcutValue;
            if (!string.IsNullOrEmpty(shortcutKey))
            {
                txtShortcut.ReadOnly = true;
                Text = "Edit Shortcut";
            }
            else
            {
                Text = "Add Shortcut";
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
                Text = "Shortcut:",
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
                Text = "Value:",
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
                Text = "Cancel",
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

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtShortcut.Text))
            {
                MessageBox.Show("Shortcut cannot be empty!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
                return;
            }

            if (string.IsNullOrWhiteSpace(txtValue.Text))
            {
                MessageBox.Show("Value cannot be empty!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
                return;
            }
        }
    }
} 