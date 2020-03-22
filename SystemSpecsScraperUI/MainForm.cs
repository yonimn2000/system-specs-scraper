using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace YonatanMankovich.SystemSpecsScraper
{
    public partial class MainForm : Form
    {
        const string HOSTS_PATH = "Hosts.txt";
        const string SPECS_PATH = "Specs.csv"; // Output file path
        const string FAILED_PATH = "FailedHosts.txt";

        private Scraper Scraper { get; } = new Scraper(SPECS_PATH, FAILED_PATH);
        private Dictionary<string, int> HostRowIndexes { get; } = new Dictionary<string, int>();

        public MainForm()
        {
            InitializeComponent();
            DomainHostsRB.Enabled = DomainMethods.IsOnDomain();
            FailedHostsRB.Enabled = File.Exists(FAILED_PATH);

            Scraper.StartedScrapingHostAction = OnStartedScrapingHost;
            Scraper.UpdateHostStatusAction = OnUpdateHostStatus;
        }

        private void OnStartedScrapingHost(string host)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                int currentRowIndex = ScrapedDGV.Rows.Add(host, "Pending");
                HostRowIndexes.Add(host, currentRowIndex);
                ScrapedDGV.Rows[currentRowIndex].Cells["Status"].Style.BackColor = Color.Yellow;
            }));
        }

        private void OnUpdateHostStatus(int totalItems, string host, string status)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                MainPB.Maximum = totalItems;
                MainPB.PerformStep();
                ProgressLBL.Text = $"Finished: {MainPB.Value}/{totalItems} ({Math.Round(100 * (double)MainPB.Value / totalItems)}%)";
                DataGridViewRow hostRow = ScrapedDGV.Rows[HostRowIndexes[host]];
                hostRow.Cells["Status"].Style.BackColor = status.Equals("Success") ? Color.LimeGreen : Color.Red;
                hostRow.Cells["Status"].Value = status;
            }));
        }

        private void ScrapeBTN_Click(object sender, EventArgs e)
        {
            ScrapeBTN.Enabled = false;
            MainPB.Value = 0;
            ScrapedDGV.Rows.Clear();
            ScrapeBW.RunWorkerAsync();
        }

        private void ScrapeBW_DoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            if (ScrapeFileRB.Checked)
            {
                if (!File.Exists(HOSTS_PATH))
                {
                    MessageBox.Show(HOSTS_PATH + " was not found. Please add at least one host name to the file to start scraping specs.");
                    File.Create(HOSTS_PATH).Close();
                    Process.Start(HOSTS_PATH);
                }
                else if (File.ReadAllText(HOSTS_PATH).Length == 0)
                {
                    MessageBox.Show(HOSTS_PATH + " is empty. Please add at least one host name to the file to start scraping specs.");
                    Process.Start(HOSTS_PATH);
                }
                else
                    Scraper.Scrape(File.ReadAllLines(HOSTS_PATH));
            }
            else if (FailedHostsRB.Checked)
            {
                Scraper.Scrape(File.ReadAllLines(FAILED_PATH));
            }
            else if (DomainHostsRB.Checked)
                Scraper.ScrapeDomainComputers();
        }

        private void EditNamespacesBTN_Click(object sender, EventArgs e)
        {
            Process.Start(WMI_NamespacesLoader.NamespacesPath);
        }

        private void ScrapeBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                ScrapeBTN.Enabled = true;
            }));
            Process.Start(SPECS_PATH);
        }
    }
}