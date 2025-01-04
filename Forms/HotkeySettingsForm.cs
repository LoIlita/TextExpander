using System;
using System.Windows.Forms;
using TextExpander.Settings;

namespace TextExpander.Forms
{
    public partial class HotkeySettingsForm : Form
    {
        private readonly AppSettings _settings;
        private CheckBox chkControl = null!;
        private CheckBox chkAlt = null!;
        private CheckBox chkShift = null!;
        private ComboBox cmbKey = null!;
        private Button btnOK = null!;
        private Button btnCancel = null!;
        private Label lblCurrentHotkey = null!;

        public HotkeySettingsForm(AppSettings settings)
        {
            _settings = settings;
            InitializeComponent();
            LoadCurrentSettings();
        }

        private void InitializeComponent()
        {
            this.Text = "Hotkey Settings";
            this.Size = new System.Drawing.Size(300, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            lblCurrentHotkey = new Label
            {
                Text = $"Current Hotkey: {_settings.GetKeyDescription()}",
                Location = new System.Drawing.Point(12, 15),
                Size = new System.Drawing.Size(260, 20)
            };

            // Modyfikatory
            chkControl = new CheckBox
            {
                Text = "Ctrl",
                Location = new System.Drawing.Point(12, 45),
                Size = new System.Drawing.Size(60, 20)
            };

            chkAlt = new CheckBox
            {
                Text = "Alt",
                Location = new System.Drawing.Point(82, 45),
                Size = new System.Drawing.Size(60, 20)
            };

            chkShift = new CheckBox
            {
                Text = "Shift",
                Location = new System.Drawing.Point(152, 45),
                Size = new System.Drawing.Size(60, 20)
            };

            // Lista klawiszy
            cmbKey = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new System.Drawing.Point(12, 75),
                Size = new System.Drawing.Size(260, 20)
            };
            PopulateKeysList();

            // Przyciski
            btnOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new System.Drawing.Point(116, 120),
                Size = new System.Drawing.Size(75, 23)
            };
            btnOK.Click += BtnOK_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new System.Drawing.Point(197, 120),
                Size = new System.Drawing.Size(75, 23)
            };

            Controls.AddRange(new Control[] {
                lblCurrentHotkey,
                chkControl,
                chkAlt,
                chkShift,
                cmbKey,
                btnOK,
                btnCancel
            });

            AcceptButton = btnOK;
            CancelButton = btnCancel;
        }

        private void PopulateKeysList()
        {
            var letterKeys = new Keys[] {
                Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J,
                Keys.K, Keys.L, Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T,
                Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z
            };

            foreach (Keys key in letterKeys)
            {
                cmbKey.Items.Add(key);
            }
        }

        private void LoadCurrentSettings()
        {
            chkControl.Checked = _settings.RequireControl;
            chkAlt.Checked = _settings.RequireAlt;
            chkShift.Checked = _settings.RequireShift;
            cmbKey.SelectedItem = _settings.TriggerKey;
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            if (cmbKey.SelectedItem == null)
            {
                MessageBox.Show("Please select a key!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
                return;
            }

            if (!chkControl.Checked && !chkAlt.Checked && !chkShift.Checked)
            {
                MessageBox.Show("Please select at least one modifier key (Ctrl, Alt, or Shift)!",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
                return;
            }

            _settings.RequireControl = chkControl.Checked;
            _settings.RequireAlt = chkAlt.Checked;
            _settings.RequireShift = chkShift.Checked;
            _settings.TriggerKey = (Keys)cmbKey.SelectedItem;
        }
    }
} 