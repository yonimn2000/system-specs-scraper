namespace YonatanMankovich.SystemSpecsScraper
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ScrapeBTN = new System.Windows.Forms.Button();
            this.MainPB = new System.Windows.Forms.ProgressBar();
            this.ScrapeBW = new System.ComponentModel.BackgroundWorker();
            this.ScrapedDGV = new System.Windows.Forms.DataGridView();
            this.ScrapeRBs_GB = new System.Windows.Forms.GroupBox();
            this.DomainHostsRB = new System.Windows.Forms.RadioButton();
            this.FailedHostsRB = new System.Windows.Forms.RadioButton();
            this.ScrapeFileRB = new System.Windows.Forms.RadioButton();
            this.EditNamespacesBTN = new System.Windows.Forms.Button();
            this.Host = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProgressLBL = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ScrapedDGV)).BeginInit();
            this.ScrapeRBs_GB.SuspendLayout();
            this.SuspendLayout();
            // 
            // ScrapeBTN
            // 
            this.ScrapeBTN.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ScrapeBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScrapeBTN.Location = new System.Drawing.Point(178, 61);
            this.ScrapeBTN.Name = "ScrapeBTN";
            this.ScrapeBTN.Size = new System.Drawing.Size(95, 25);
            this.ScrapeBTN.TabIndex = 2;
            this.ScrapeBTN.Text = "Scrape!";
            this.ScrapeBTN.UseVisualStyleBackColor = false;
            this.ScrapeBTN.Click += new System.EventHandler(this.ScrapeBTN_Click);
            // 
            // MainPB
            // 
            this.MainPB.Location = new System.Drawing.Point(13, 91);
            this.MainPB.Name = "MainPB";
            this.MainPB.Size = new System.Drawing.Size(260, 23);
            this.MainPB.Step = 1;
            this.MainPB.TabIndex = 8;
            // 
            // ScrapeBW
            // 
            this.ScrapeBW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ScrapeBW_DoWork);
            this.ScrapeBW.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ScrapeBW_RunWorkerCompleted);
            // 
            // ScrapedDGV
            // 
            this.ScrapedDGV.AllowUserToAddRows = false;
            this.ScrapedDGV.AllowUserToDeleteRows = false;
            this.ScrapedDGV.AllowUserToResizeRows = false;
            this.ScrapedDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScrapedDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ScrapedDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Host,
            this.Status});
            this.ScrapedDGV.Location = new System.Drawing.Point(12, 133);
            this.ScrapedDGV.Name = "ScrapedDGV";
            this.ScrapedDGV.ReadOnly = true;
            this.ScrapedDGV.RowHeadersVisible = false;
            this.ScrapedDGV.Size = new System.Drawing.Size(261, 216);
            this.ScrapedDGV.TabIndex = 11;
            // 
            // ScrapeRBs_GB
            // 
            this.ScrapeRBs_GB.Controls.Add(this.DomainHostsRB);
            this.ScrapeRBs_GB.Controls.Add(this.FailedHostsRB);
            this.ScrapeRBs_GB.Controls.Add(this.ScrapeFileRB);
            this.ScrapeRBs_GB.Location = new System.Drawing.Point(12, 12);
            this.ScrapeRBs_GB.Name = "ScrapeRBs_GB";
            this.ScrapeRBs_GB.Size = new System.Drawing.Size(261, 43);
            this.ScrapeRBs_GB.TabIndex = 12;
            this.ScrapeRBs_GB.TabStop = false;
            this.ScrapeRBs_GB.Text = "What to scrape";
            // 
            // DomainHostsRB
            // 
            this.DomainHostsRB.AutoSize = true;
            this.DomainHostsRB.Enabled = false;
            this.DomainHostsRB.Location = new System.Drawing.Point(166, 20);
            this.DomainHostsRB.Name = "DomainHostsRB";
            this.DomainHostsRB.Size = new System.Drawing.Size(89, 17);
            this.DomainHostsRB.TabIndex = 2;
            this.DomainHostsRB.Text = "Domain hosts";
            this.DomainHostsRB.UseVisualStyleBackColor = true;
            // 
            // FailedHostsRB
            // 
            this.FailedHostsRB.AutoSize = true;
            this.FailedHostsRB.Location = new System.Drawing.Point(79, 20);
            this.FailedHostsRB.Name = "FailedHostsRB";
            this.FailedHostsRB.Size = new System.Drawing.Size(81, 17);
            this.FailedHostsRB.TabIndex = 1;
            this.FailedHostsRB.Text = "Failed hosts";
            this.FailedHostsRB.UseVisualStyleBackColor = true;
            // 
            // ScrapeFileRB
            // 
            this.ScrapeFileRB.AutoSize = true;
            this.ScrapeFileRB.Checked = true;
            this.ScrapeFileRB.Location = new System.Drawing.Point(7, 20);
            this.ScrapeFileRB.Name = "ScrapeFileRB";
            this.ScrapeFileRB.Size = new System.Drawing.Size(66, 17);
            this.ScrapeFileRB.TabIndex = 0;
            this.ScrapeFileRB.TabStop = true;
            this.ScrapeFileRB.Text = "Hosts.txt";
            this.ScrapeFileRB.UseVisualStyleBackColor = true;
            // 
            // EditNamespacesBTN
            // 
            this.EditNamespacesBTN.Location = new System.Drawing.Point(13, 62);
            this.EditNamespacesBTN.Name = "EditNamespacesBTN";
            this.EditNamespacesBTN.Size = new System.Drawing.Size(159, 23);
            this.EditNamespacesBTN.TabIndex = 13;
            this.EditNamespacesBTN.Text = "Open WMI properties XML";
            this.EditNamespacesBTN.UseVisualStyleBackColor = true;
            this.EditNamespacesBTN.Click += new System.EventHandler(this.EditNamespacesBTN_Click);
            // 
            // Host
            // 
            this.Host.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Host.HeaderText = "Host";
            this.Host.Name = "Host";
            this.Host.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 62;
            // 
            // ProgressLBL
            // 
            this.ProgressLBL.AutoSize = true;
            this.ProgressLBL.BackColor = System.Drawing.SystemColors.Control;
            this.ProgressLBL.Location = new System.Drawing.Point(12, 117);
            this.ProgressLBL.Name = "ProgressLBL";
            this.ProgressLBL.Size = new System.Drawing.Size(92, 13);
            this.ProgressLBL.TabIndex = 14;
            this.ProgressLBL.Text = "Finished: 0/0 (0%)";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 361);
            this.Controls.Add(this.ProgressLBL);
            this.Controls.Add(this.EditNamespacesBTN);
            this.Controls.Add(this.ScrapeRBs_GB);
            this.Controls.Add(this.ScrapedDGV);
            this.Controls.Add(this.MainPB);
            this.Controls.Add(this.ScrapeBTN);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(296, 200);
            this.Name = "MainForm";
            this.Text = "System Specs Scraper";
            ((System.ComponentModel.ISupportInitialize)(this.ScrapedDGV)).EndInit();
            this.ScrapeRBs_GB.ResumeLayout(false);
            this.ScrapeRBs_GB.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ScrapeBTN;
        private System.Windows.Forms.ProgressBar MainPB;
        private System.ComponentModel.BackgroundWorker ScrapeBW;
        private System.Windows.Forms.DataGridView ScrapedDGV;
        private System.Windows.Forms.GroupBox ScrapeRBs_GB;
        private System.Windows.Forms.RadioButton DomainHostsRB;
        private System.Windows.Forms.RadioButton FailedHostsRB;
        private System.Windows.Forms.RadioButton ScrapeFileRB;
        private System.Windows.Forms.Button EditNamespacesBTN;
        private System.Windows.Forms.DataGridViewTextBoxColumn Host;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.Label ProgressLBL;
    }
}

