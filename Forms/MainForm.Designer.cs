using System.Windows.Forms;

namespace TextExpander.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.listViewShortcuts = new TextExpander.Controls.DoubleBufferedListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.btnToggleListening = new System.Windows.Forms.Button();
            this.btnAddShortcut = new System.Windows.Forms.Button();
            this.btnEditShortcut = new System.Windows.Forms.Button();
            this.btnDeleteShortcut = new System.Windows.Forms.Button();
            this.btnChangeHotkey = new System.Windows.Forms.Button();
            this.lblHotkey = new System.Windows.Forms.Label();
            this.btnChangeTheme = new System.Windows.Forms.Button();

            // listViewShortcuts
            this.listViewShortcuts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewShortcuts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.columnHeader1,
                this.columnHeader2});
            this.listViewShortcuts.FullRowSelect = true;
            this.listViewShortcuts.GridLines = true;
            this.listViewShortcuts.Location = new System.Drawing.Point(12, 80);
            this.listViewShortcuts.Name = "listViewShortcuts";
            this.listViewShortcuts.Size = new System.Drawing.Size(560, 269);
            this.listViewShortcuts.TabIndex = 0;
            this.listViewShortcuts.UseCompatibleStateImageBehavior = false;
            this.listViewShortcuts.View = System.Windows.Forms.View.Details;
            this.listViewShortcuts.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.listViewShortcuts.ScrollBars = ScrollBars.Vertical;
            this.listViewShortcuts.AllowColumnReorder = false;

            // columnHeader1
            this.columnHeader1.Text = "Skrót";
            this.columnHeader1.Width = 100;

            // columnHeader2
            this.columnHeader2.Text = "Rozwinięcie";
            this.columnHeader2.Width = 456;

            // btnToggleListening
            this.btnToggleListening.Location = new System.Drawing.Point(12, 12);
            this.btnToggleListening.Name = "btnToggleListening";
            this.btnToggleListening.Size = new System.Drawing.Size(120, 30);
            this.btnToggleListening.Text = "Start Listening";
            this.btnToggleListening.Click += new System.EventHandler(this.BtnToggleListening_Click);

            // btnAddShortcut
            this.btnAddShortcut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddShortcut.Location = new System.Drawing.Point(12, 359);
            this.btnAddShortcut.Name = "btnAddShortcut";
            this.btnAddShortcut.Size = new System.Drawing.Size(120, 30);
            this.btnAddShortcut.Text = "Add Shortcut";
            this.btnAddShortcut.Click += new System.EventHandler(this.BtnAddShortcut_Click);

            // btnEditShortcut
            this.btnEditShortcut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditShortcut.Location = new System.Drawing.Point(142, 359);
            this.btnEditShortcut.Name = "btnEditShortcut";
            this.btnEditShortcut.Size = new System.Drawing.Size(120, 30);
            this.btnEditShortcut.Text = "Edit Shortcut";
            this.btnEditShortcut.Click += new System.EventHandler(this.BtnEditShortcut_Click);

            // btnDeleteShortcut
            this.btnDeleteShortcut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteShortcut.Location = new System.Drawing.Point(272, 359);
            this.btnDeleteShortcut.Name = "btnDeleteShortcut";
            this.btnDeleteShortcut.Size = new System.Drawing.Size(120, 30);
            this.btnDeleteShortcut.Text = "Delete Shortcut";
            this.btnDeleteShortcut.Click += new System.EventHandler(this.BtnDeleteShortcut_Click);

            // btnChangeHotkey
            this.btnChangeHotkey.Location = new System.Drawing.Point(142, 12);
            this.btnChangeHotkey.Name = "btnChangeHotkey";
            this.btnChangeHotkey.Size = new System.Drawing.Size(120, 30);
            this.btnChangeHotkey.Text = "Change Hotkey";
            this.btnChangeHotkey.Click += new System.EventHandler(this.BtnChangeHotkey_Click);

            // btnChangeTheme
            this.btnChangeTheme.Location = new System.Drawing.Point(272, 12);
            this.btnChangeTheme.Name = "btnChangeTheme";
            this.btnChangeTheme.Size = new System.Drawing.Size(120, 30);
            this.btnChangeTheme.Text = "Zmień motyw";
            this.btnChangeTheme.Click += new System.EventHandler(this.BtnChangeTheme_Click);

            // lblHotkey
            this.lblHotkey.AutoSize = true;
            this.lblHotkey.Location = new System.Drawing.Point(12, 54);
            this.lblHotkey.Name = "lblHotkey";
            this.lblHotkey.Size = new System.Drawing.Size(100, 15);
            this.lblHotkey.Text = "Hotkey: Ctrl+M";

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 401);
            this.Controls.Add(this.btnToggleListening);
            this.Controls.Add(this.btnChangeHotkey);
            this.Controls.Add(this.btnChangeTheme);
            this.Controls.Add(this.lblHotkey);
            this.Controls.Add(this.listViewShortcuts);
            this.Controls.Add(this.btnAddShortcut);
            this.Controls.Add(this.btnEditShortcut);
            this.Controls.Add(this.btnDeleteShortcut);
            this.MinimumSize = new System.Drawing.Size(600, 440);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Name = "MainForm";
            this.Text = "TextExpander";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private TextExpander.Controls.DoubleBufferedListView listViewShortcuts;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnToggleListening;
        private System.Windows.Forms.Button btnAddShortcut;
        private System.Windows.Forms.Button btnEditShortcut;
        private System.Windows.Forms.Button btnDeleteShortcut;
        private System.Windows.Forms.Button btnChangeHotkey;
        private System.Windows.Forms.Button btnChangeTheme;
        private System.Windows.Forms.Label lblHotkey;

        #endregion
    }
} 