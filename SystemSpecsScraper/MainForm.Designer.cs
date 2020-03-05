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
            this.ReadHostsBTN = new System.Windows.Forms.Button();
            this.ScrapeBTN = new System.Windows.Forms.Button();
            this.DomainLBL = new System.Windows.Forms.Label();
            this.MainPB = new System.Windows.Forms.ProgressBar();
            this.HostsTB = new System.Windows.Forms.TextBox();
            this.ScrapeBW = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // ReadHostsBTN
            // 
            this.ReadHostsBTN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReadHostsBTN.Location = new System.Drawing.Point(9, 231);
            this.ReadHostsBTN.Name = "ReadHostsBTN";
            this.ReadHostsBTN.Size = new System.Drawing.Size(263, 25);
            this.ReadHostsBTN.TabIndex = 1;
            this.ReadHostsBTN.Text = "Read hosts from {DOMAIN}";
            this.ReadHostsBTN.UseVisualStyleBackColor = true;
            this.ReadHostsBTN.Click += new System.EventHandler(this.ReadHostsBTN_Click);
            // 
            // ScrapeBTN
            // 
            this.ScrapeBTN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScrapeBTN.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ScrapeBTN.Location = new System.Drawing.Point(9, 262);
            this.ScrapeBTN.Name = "ScrapeBTN";
            this.ScrapeBTN.Size = new System.Drawing.Size(263, 25);
            this.ScrapeBTN.TabIndex = 2;
            this.ScrapeBTN.Text = "Scrape!";
            this.ScrapeBTN.UseVisualStyleBackColor = false;
            this.ScrapeBTN.Click += new System.EventHandler(this.ScrapeBTN_Click);
            // 
            // DomainLBL
            // 
            this.DomainLBL.AutoSize = true;
            this.DomainLBL.Location = new System.Drawing.Point(12, 9);
            this.DomainLBL.Name = "DomainLBL";
            this.DomainLBL.Size = new System.Drawing.Size(98, 13);
            this.DomainLBL.TabIndex = 6;
            this.DomainLBL.Text = "Hosts (one per line)";
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 324);
            this.Controls.Add(this.HostsTB);
            this.Controls.Add(this.MainPB);
            this.Controls.Add(this.DomainLBL);
            this.Controls.Add(this.ScrapeBTN);
            this.Controls.Add(this.ReadHostsBTN);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "MainForm";
            this.Text = "System Specs Scraper";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ReadHostsBTN;
        private System.Windows.Forms.Button ScrapeBTN;
        private System.Windows.Forms.Label DomainLBL;
        private System.Windows.Forms.ProgressBar MainPB;
        private System.Windows.Forms.TextBox HostsTB;
        private System.ComponentModel.BackgroundWorker ScrapeBW;
    }
}

