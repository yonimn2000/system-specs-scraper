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
            this.ScrapeDomainHostsBTN = new System.Windows.Forms.Button();
            this.ScrapeBTN = new System.Windows.Forms.Button();
            this.HostsLBL = new System.Windows.Forms.Label();
            this.MainPB = new System.Windows.Forms.ProgressBar();
            this.HostsTB = new System.Windows.Forms.TextBox();
            this.ScrapeBW = new System.ComponentModel.BackgroundWorker();
            this.ScrapeFailedBTN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ScrapeDomainHostsBTN
            // 
            this.ScrapeDomainHostsBTN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScrapeDomainHostsBTN.Location = new System.Drawing.Point(9, 231);
            this.ScrapeDomainHostsBTN.Name = "ScrapeDomainHostsBTN";
            this.ScrapeDomainHostsBTN.Size = new System.Drawing.Size(263, 25);
            this.ScrapeDomainHostsBTN.TabIndex = 1;
            this.ScrapeDomainHostsBTN.Text = "Scrape {DOMAIN} hosts";
            this.ScrapeDomainHostsBTN.UseVisualStyleBackColor = true;
            this.ScrapeDomainHostsBTN.Click += new System.EventHandler(this.ScrapeDomainHostsBTN_Click);
            // 
            // ScrapeBTN
            // 
            this.ScrapeBTN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScrapeBTN.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ScrapeBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScrapeBTN.Location = new System.Drawing.Point(170, 262);
            this.ScrapeBTN.Name = "ScrapeBTN";
            this.ScrapeBTN.Size = new System.Drawing.Size(102, 25);
            this.ScrapeBTN.TabIndex = 2;
            this.ScrapeBTN.Text = "Scrape!";
            this.ScrapeBTN.UseVisualStyleBackColor = false;
            this.ScrapeBTN.Click += new System.EventHandler(this.ScrapeBTN_Click);
            // 
            // HostsLBL
            // 
            this.HostsLBL.AutoSize = true;
            this.HostsLBL.Location = new System.Drawing.Point(12, 9);
            this.HostsLBL.Name = "HostsLBL";
            this.HostsLBL.Size = new System.Drawing.Size(98, 13);
            this.HostsLBL.TabIndex = 6;
            this.HostsLBL.Text = "Hosts (one per line)";
            // 
            // MainPB
            // 
            this.MainPB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainPB.Location = new System.Drawing.Point(9, 293);
            this.MainPB.Name = "MainPB";
            this.MainPB.Size = new System.Drawing.Size(263, 20);
            this.MainPB.Step = 1;
            this.MainPB.TabIndex = 8;
            // 
            // HostsTB
            // 
            this.HostsTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HostsTB.Location = new System.Drawing.Point(9, 25);
            this.HostsTB.Multiline = true;
            this.HostsTB.Name = "HostsTB";
            this.HostsTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.HostsTB.Size = new System.Drawing.Size(263, 200);
            this.HostsTB.TabIndex = 9;
            // 
            // ScrapeBW
            // 
            this.ScrapeBW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ScrapeBW_DoWork);
            // 
            // ScrapeFailedBTN
            // 
            this.ScrapeFailedBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ScrapeFailedBTN.Location = new System.Drawing.Point(9, 262);
            this.ScrapeFailedBTN.Name = "ScrapeFailedBTN";
            this.ScrapeFailedBTN.Size = new System.Drawing.Size(155, 25);
            this.ScrapeFailedBTN.TabIndex = 10;
            this.ScrapeFailedBTN.Text = "Scrape failed hosts";
            this.ScrapeFailedBTN.UseVisualStyleBackColor = true;
            this.ScrapeFailedBTN.Click += new System.EventHandler(this.ScrapeFailedBTN_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 324);
            this.Controls.Add(this.ScrapeFailedBTN);
            this.Controls.Add(this.HostsTB);
            this.Controls.Add(this.MainPB);
            this.Controls.Add(this.HostsLBL);
            this.Controls.Add(this.ScrapeBTN);
            this.Controls.Add(this.ScrapeDomainHostsBTN);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "MainForm";
            this.Text = "System Specs Scraper";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ScrapeDomainHostsBTN;
        private System.Windows.Forms.Button ScrapeBTN;
        private System.Windows.Forms.Label HostsLBL;
        private System.Windows.Forms.ProgressBar MainPB;
        private System.Windows.Forms.TextBox HostsTB;
        private System.ComponentModel.BackgroundWorker ScrapeBW;
        private System.Windows.Forms.Button ScrapeFailedBTN;
    }
}

