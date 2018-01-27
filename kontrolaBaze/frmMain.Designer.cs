namespace kontrolaBaze
{
    partial class frmMain
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pb1 = new System.Windows.Forms.ProgressBar();
            this.lbl_porukaKonekcija = new System.Windows.Forms.Label();
            this.btn_konekcija = new System.Windows.Forms.Button();
            this.txt_password = new System.Windows.Forms.TextBox();
            this.txt_userName = new System.Windows.Forms.TextBox();
            this.txt_DatabaseName = new System.Windows.Forms.TextBox();
            this.txt_sreverName = new System.Windows.Forms.TextBox();
            this.txtRezultatKontrole = new System.Windows.Forms.TextBox();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.kontrolaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.izvestajUTxtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sf_diag = new System.Windows.Forms.SaveFileDialog();
            this.jEUFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtRezultatKontrole);
            this.splitContainer1.Size = new System.Drawing.Size(981, 674);
            this.splitContainer1.SplitterDistance = 327;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pb1);
            this.panel1.Controls.Add(this.lbl_porukaKonekcija);
            this.panel1.Controls.Add(this.btn_konekcija);
            this.panel1.Controls.Add(this.txt_password);
            this.panel1.Controls.Add(this.txt_userName);
            this.panel1.Controls.Add(this.txt_DatabaseName);
            this.panel1.Controls.Add(this.txt_sreverName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(327, 464);
            this.panel1.TabIndex = 0;
            // 
            // pb1
            // 
            this.pb1.Location = new System.Drawing.Point(3, 438);
            this.pb1.Name = "pb1";
            this.pb1.Size = new System.Drawing.Size(321, 23);
            this.pb1.TabIndex = 6;
            // 
            // lbl_porukaKonekcija
            // 
            this.lbl_porukaKonekcija.AutoSize = true;
            this.lbl_porukaKonekcija.Location = new System.Drawing.Point(14, 173);
            this.lbl_porukaKonekcija.Name = "lbl_porukaKonekcija";
            this.lbl_porukaKonekcija.Size = new System.Drawing.Size(16, 13);
            this.lbl_porukaKonekcija.TabIndex = 5;
            this.lbl_porukaKonekcija.Text = "...";
            // 
            // btn_konekcija
            // 
            this.btn_konekcija.Location = new System.Drawing.Point(14, 119);
            this.btn_konekcija.Name = "btn_konekcija";
            this.btn_konekcija.Size = new System.Drawing.Size(185, 23);
            this.btn_konekcija.TabIndex = 4;
            this.btn_konekcija.Text = "Konektuj se..";
            this.btn_konekcija.UseVisualStyleBackColor = true;
            this.btn_konekcija.Click += new System.EventHandler(this.btn_konekcija_Click);
            // 
            // txt_password
            // 
            this.txt_password.Location = new System.Drawing.Point(14, 93);
            this.txt_password.Name = "txt_password";
            this.txt_password.Size = new System.Drawing.Size(185, 20);
            this.txt_password.TabIndex = 3;
            this.txt_password.Text = "azra220";
            // 
            // txt_userName
            // 
            this.txt_userName.Location = new System.Drawing.Point(14, 66);
            this.txt_userName.Name = "txt_userName";
            this.txt_userName.Size = new System.Drawing.Size(185, 20);
            this.txt_userName.TabIndex = 2;
            this.txt_userName.Text = "root";
            // 
            // txt_DatabaseName
            // 
            this.txt_DatabaseName.Location = new System.Drawing.Point(14, 39);
            this.txt_DatabaseName.Name = "txt_DatabaseName";
            this.txt_DatabaseName.Size = new System.Drawing.Size(185, 20);
            this.txt_DatabaseName.TabIndex = 1;
            this.txt_DatabaseName.Text = "zaprivremenatestiranja";
            // 
            // txt_sreverName
            // 
            this.txt_sreverName.Location = new System.Drawing.Point(14, 12);
            this.txt_sreverName.Name = "txt_sreverName";
            this.txt_sreverName.Size = new System.Drawing.Size(185, 20);
            this.txt_sreverName.TabIndex = 0;
            this.txt_sreverName.Text = "192.168.14.244";
            // 
            // txtRezultatKontrole
            // 
            this.txtRezultatKontrole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRezultatKontrole.Location = new System.Drawing.Point(0, 0);
            this.txtRezultatKontrole.Multiline = true;
            this.txtRezultatKontrole.Name = "txtRezultatKontrole";
            this.txtRezultatKontrole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRezultatKontrole.Size = new System.Drawing.Size(650, 674);
            this.txtRezultatKontrole.TabIndex = 0;
            // 
            // mainMenu
            // 
            this.mainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kontrolaToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(981, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // kontrolaToolStripMenuItem
            // 
            this.kontrolaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jEToolStripMenuItem,
            this.jEUFileToolStripMenuItem});
            this.kontrolaToolStripMenuItem.Enabled = false;
            this.kontrolaToolStripMenuItem.Name = "kontrolaToolStripMenuItem";
            this.kontrolaToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.kontrolaToolStripMenuItem.Text = "Kontrola";
            // 
            // jEToolStripMenuItem
            // 
            this.jEToolStripMenuItem.Name = "jEToolStripMenuItem";
            this.jEToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.jEToolStripMenuItem.Text = "JE";
            this.jEToolStripMenuItem.Click += new System.EventHandler(this.jEToolStripMenuItem_Click_1);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.izvestajUTxtToolStripMenuItem});
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.saveToolStripMenuItem.Text = "Sacuvaj";
            // 
            // izvestajUTxtToolStripMenuItem
            // 
            this.izvestajUTxtToolStripMenuItem.Name = "izvestajUTxtToolStripMenuItem";
            this.izvestajUTxtToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.izvestajUTxtToolStripMenuItem.Text = "Izvestaj u txt";
            this.izvestajUTxtToolStripMenuItem.Click += new System.EventHandler(this.izvestajUTxtToolStripMenuItem_Click);
            // 
            // jEUFileToolStripMenuItem
            // 
            this.jEUFileToolStripMenuItem.Name = "jEUFileToolStripMenuItem";
            this.jEUFileToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.jEUFileToolStripMenuItem.Text = "JE - u file";
            this.jEUFileToolStripMenuItem.Click += new System.EventHandler(this.jEUFileToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 698);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kontola baze";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_konekcija;
        private System.Windows.Forms.TextBox txt_password;
        private System.Windows.Forms.TextBox txt_userName;
        private System.Windows.Forms.TextBox txt_DatabaseName;
        private System.Windows.Forms.TextBox txt_sreverName;
        private System.Windows.Forms.Label lbl_porukaKonekcija;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem kontrolaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jEToolStripMenuItem;
        private System.Windows.Forms.TextBox txtRezultatKontrole;
        private System.Windows.Forms.ProgressBar pb1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem izvestajUTxtToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog sf_diag;
        private System.Windows.Forms.ToolStripMenuItem jEUFileToolStripMenuItem;
    }
}

