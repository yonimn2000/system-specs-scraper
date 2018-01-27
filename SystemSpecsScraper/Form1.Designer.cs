namespace SystemSpecsScraper
{
    partial class Form1
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
            this.OutputTable = new System.Windows.Forms.DataGridView();
            this.HostNamesList = new System.Windows.Forms.RichTextBox();
            this.ReadHostsBTN = new System.Windows.Forms.Button();
            this.GetSpecsBTN = new System.Windows.Forms.Button();
            this.FailedComputersList = new System.Windows.Forms.RichTextBox();
            this.CopyTableBTN = new System.Windows.Forms.Button();
            this.DomainLBL = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ClearTableBTN = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.OutputTable)).BeginInit();
            this.SuspendLayout();
            // 
            // OutputTable
            // 
            this.OutputTable.AllowUserToAddRows = false;
            this.OutputTable.AllowUserToDeleteRows = false;
            this.OutputTable.AllowUserToOrderColumns = true;
            this.OutputTable.AllowUserToResizeRows = false;
            this.OutputTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.OutputTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.OutputTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OutputTable.Location = new System.Drawing.Point(324, 27);
            this.OutputTable.Name = "OutputTable";
            this.OutputTable.ReadOnly = true;
            this.OutputTable.RowHeadersVisible = false;
            this.OutputTable.Size = new System.Drawing.Size(648, 222);
            this.OutputTable.TabIndex = 0;
            // 
            // HostNamesList
            // 
            this.HostNamesList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.HostNamesList.Location = new System.Drawing.Point(12, 27);
            this.HostNamesList.Name = "HostNamesList";
            this.HostNamesList.Size = new System.Drawing.Size(150, 164);
            this.HostNamesList.TabIndex = 0;
            this.HostNamesList.Text = "";
            // 
            // ReadHostsBTN
            // 
            this.ReadHostsBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ReadHostsBTN.Location = new System.Drawing.Point(12, 197);
            this.ReadHostsBTN.Name = "ReadHostsBTN";
            this.ReadHostsBTN.Size = new System.Drawing.Size(306, 23);
            this.ReadHostsBTN.TabIndex = 1;
            this.ReadHostsBTN.Text = "Read Host Names From ";
            this.ReadHostsBTN.UseVisualStyleBackColor = true;
            this.ReadHostsBTN.Click += new System.EventHandler(this.ReadHostsBTN_Click);
            // 
            // GetSpecsBTN
            // 
            this.GetSpecsBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GetSpecsBTN.Location = new System.Drawing.Point(12, 226);
            this.GetSpecsBTN.Name = "GetSpecsBTN";
            this.GetSpecsBTN.Size = new System.Drawing.Size(150, 23);
            this.GetSpecsBTN.TabIndex = 2;
            this.GetSpecsBTN.Text = "Get Specs";
            this.GetSpecsBTN.UseVisualStyleBackColor = true;
            this.GetSpecsBTN.Click += new System.EventHandler(this.GetSpecsBTN_Click);
            // 
            // FailedComputersList
            // 
            this.FailedComputersList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.FailedComputersList.Location = new System.Drawing.Point(168, 27);
            this.FailedComputersList.Name = "FailedComputersList";
            this.FailedComputersList.Size = new System.Drawing.Size(150, 164);
            this.FailedComputersList.TabIndex = 5;
            this.FailedComputersList.Text = "";
            // 
            // CopyTableBTN
            // 
            this.CopyTableBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CopyTableBTN.Location = new System.Drawing.Point(247, 226);
            this.CopyTableBTN.Name = "CopyTableBTN";
            this.CopyTableBTN.Size = new System.Drawing.Size(71, 23);
            this.CopyTableBTN.TabIndex = 3;
            this.CopyTableBTN.Text = "Copy Table";
            this.CopyTableBTN.UseVisualStyleBackColor = true;
            this.CopyTableBTN.Click += new System.EventHandler(this.ExportTableBTN_Click);
            // 
            // DomainLBL
            // 
            this.DomainLBL.AutoSize = true;
            this.DomainLBL.Location = new System.Drawing.Point(12, 9);
            this.DomainLBL.Name = "DomainLBL";
            this.DomainLBL.Size = new System.Drawing.Size(34, 13);
            this.DomainLBL.TabIndex = 6;
            this.DomainLBL.Text = "Hosts";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(165, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Failed Computers";
            // 
            // ClearTableBTN
            // 
            this.ClearTableBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ClearTableBTN.Location = new System.Drawing.Point(168, 226);
            this.ClearTableBTN.Name = "ClearTableBTN";
            this.ClearTableBTN.Size = new System.Drawing.Size(73, 23);
            this.ClearTableBTN.TabIndex = 4;
            this.ClearTableBTN.Text = "Clear Table";
            this.ClearTableBTN.UseVisualStyleBackColor = true;
            this.ClearTableBTN.Click += new System.EventHandler(this.ClearTableBTN_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(324, 9);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(648, 12);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 261);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.ClearTableBTN);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DomainLBL);
            this.Controls.Add(this.CopyTableBTN);
            this.Controls.Add(this.FailedComputersList);
            this.Controls.Add(this.GetSpecsBTN);
            this.Controls.Add(this.ReadHostsBTN);
            this.Controls.Add(this.HostNamesList);
            this.Controls.Add(this.OutputTable);
            this.Name = "Form1";
            this.Text = "System Specs Scraper";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.OutputTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView OutputTable;
        private System.Windows.Forms.RichTextBox HostNamesList;
        private System.Windows.Forms.Button ReadHostsBTN;
        private System.Windows.Forms.Button GetSpecsBTN;
        private System.Windows.Forms.RichTextBox FailedComputersList;
        private System.Windows.Forms.Button CopyTableBTN;
        private System.Windows.Forms.Label DomainLBL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ClearTableBTN;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

