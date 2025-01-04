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
            this.listViewShortcuts = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.btnToggleListening = new System.Windows.Forms.Button();
            this.btnAddShortcut = new System.Windows.Forms.Button();
            this.btnEditShortcut = new System.Windows.Forms.Button();
            this.btnDeleteShortcut = new System.Windows.Forms.Button();
            this.btnChangeHotkey = new System.Windows.Forms.Button();
            this.lblHotkey = new System.Windows.Forms.Label();

            // listViewShortcuts
            this.listViewShortcuts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.columnHeader1,
                this.columnHeader2});
            this.listViewShortcuts.FullRowSelect = true;
            this.listViewShortcuts.GridLines = true;
            this.listViewShortcuts.Location = new System.Drawing.Point(12, 41);
            this.listViewShortcuts.Name = "listViewShortcuts";
            this.listViewShortcuts.Size = new System.Drawing.Size(460, 300);
            this.listViewShortcuts.TabIndex = 0;
            this.listViewShortcuts.UseCompatibleStateImageBehavior = false;
            this.listViewShortcuts.View = System.Windows.Forms.View.Details;

            // columnHeader1
            this.columnHeader1.Text = "Skrót";
            this.columnHeader1.Width = 150;

            // columnHeader2
            this.columnHeader2.Text = "Rozwinięcie";
            this.columnHeader2.Width = 300;

            // btnToggleListening
            this.btnToggleListening.Location = new System.Drawing.Point(12, 12);
            this.btnToggleListening.Name = "btnToggleListening";
            this.btnToggleListening.Size = new System.Drawing.Size(100, 23);
            this.btnToggleListening.TabIndex = 1;
            this.btnToggleListening.Text = "Start Listening";
            this.btnToggleListening.Click += new System.EventHandler(this.BtnToggleListening_Click);

            // btnAddShortcut
            this.btnAddShortcut.Location = new System.Drawing.Point(12, 347);
            this.btnAddShortcut.Name = "btnAddShortcut";
            this.btnAddShortcut.Size = new System.Drawing.Size(75, 23);
            this.btnAddShortcut.TabIndex = 2;
            this.btnAddShortcut.Text = "Dodaj";
            this.btnAddShortcut.Click += new System.EventHandler(this.BtnAddShortcut_Click);

            // btnEditShortcut
            this.btnEditShortcut.Location = new System.Drawing.Point(93, 347);
            this.btnEditShortcut.Name = "btnEditShortcut";
            this.btnEditShortcut.Size = new System.Drawing.Size(75, 23);
            this.btnEditShortcut.TabIndex = 3;
            this.btnEditShortcut.Text = "Edytuj";
            this.btnEditShortcut.Click += new System.EventHandler(this.BtnEditShortcut_Click);

            // btnDeleteShortcut
            this.btnDeleteShortcut.Location = new System.Drawing.Point(174, 347);
            this.btnDeleteShortcut.Name = "btnDeleteShortcut";
            this.btnDeleteShortcut.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteShortcut.TabIndex = 4;
            this.btnDeleteShortcut.Text = "Usuń";
            this.btnDeleteShortcut.Click += new System.EventHandler(this.BtnDeleteShortcut_Click);

            // btnChangeHotkey
            this.btnChangeHotkey.Location = new System.Drawing.Point(372, 12);
            this.btnChangeHotkey.Name = "btnChangeHotkey";
            this.btnChangeHotkey.Size = new System.Drawing.Size(100, 23);
            this.btnChangeHotkey.TabIndex = 5;
            this.btnChangeHotkey.Text = "Zmień hotkey";
            this.btnChangeHotkey.Click += new System.EventHandler(this.BtnChangeHotkey_Click);

            // lblHotkey
            this.lblHotkey.AutoSize = true;
            this.lblHotkey.Location = new System.Drawing.Point(118, 17);
            this.lblHotkey.Name = "lblHotkey";
            this.lblHotkey.Size = new System.Drawing.Size(248, 13);
            this.lblHotkey.TabIndex = 6;
            this.lblHotkey.Text = "Hotkey: Ctrl+M";

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 382);
            this.Controls.Add(this.lblHotkey);
            this.Controls.Add(this.btnChangeHotkey);
            this.Controls.Add(this.btnDeleteShortcut);
            this.Controls.Add(this.btnEditShortcut);
            this.Controls.Add(this.btnAddShortcut);
            this.Controls.Add(this.btnToggleListening);
            this.Controls.Add(this.listViewShortcuts);
            this.Name = "MainForm";
            this.Text = "TextExpander";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ListView listViewShortcuts;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnToggleListening;
        private System.Windows.Forms.Button btnAddShortcut;
        private System.Windows.Forms.Button btnEditShortcut;
        private System.Windows.Forms.Button btnDeleteShortcut;
        private System.Windows.Forms.Button btnChangeHotkey;
        private System.Windows.Forms.Label lblHotkey;

        #endregion
    }
} 