using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace YonatanMankovich.SystemSpecsScraper
{
    public partial class MainForm : Form
    {
        IList<WMI.Namespace> WMI_Namespaces;
        readonly string DOMAIN = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
        const string SPECS_PATH = "Specs.csv"; // Output file path
        const string FAILED_PATH = "FailedHosts.txt";

        public MainForm()
        {
            InitializeComponent();
        }

        private void ScrapeDomainHostsBTN_Click(object sender, EventArgs e)
        {
            string text = ScrapeDomainHostsBTN.Text;
            ScrapeDomainHostsBTN.Enabled = false;
            ScrapeDomainHostsBTN.Text = "Reading. Please wait...";
            HostsTB.Lines = GetDomainComputers(DOMAIN).ToArray();
            ScrapeDomainHostsBTN.Enabled = true;
            ScrapeDomainHostsBTN.Text = text;
            ScrapeBTN.PerformClick();
        }

        public static IList<string> GetDomainComputers(string domain)
        {
            List<string> computerNames = new List<string>();
            DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain);
            DirectorySearcher mySearcher = new DirectorySearcher(entry)
            {
                Filter = ("(objectClass=computer)"),
                SizeLimit = int.MaxValue,
                PageSize = int.MaxValue
            };
            foreach (SearchResult resEnt in mySearcher.FindAll())
            {
                string ComputerName = resEnt.GetDirectoryEntry().Name;
                if (ComputerName.StartsWith("CN="))
                    computerNames.Add(ComputerName.Remove(0, 3));
            }
            mySearcher.Dispose();
            entry.Dispose();
            //computerNames.Remove("MediaAdmin");
            computerNames.Sort();
            return computerNames;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (DOMAIN.Equals("")) // If not on a domain...
            {
                ScrapeDomainHostsBTN.Enabled = false;
                ScrapeDomainHostsBTN.Text = "Not connected to a domain";
            }
            else
            {
                ScrapeDomainHostsBTN.Enabled = true;
                ScrapeDomainHostsBTN.Text = "Scrape " + DOMAIN.ToUpper() + " hosts";
            }
        }

        private void ScrapeBTN_Click(object sender, EventArgs e)
        {
            if (HostsTB.Text.Equals(""))
                MessageBox.Show("Please enter host names before reading specs...");
            else
            {
                ScrapeBTN.Enabled = ScrapeFailedBTN.Enabled = HostsTB.Enabled = ScrapeDomainHostsBTN.Enabled = false;
                CleanupHostsTB();
                MainPB.Value = 0;
                MainPB.Maximum = HostsTB.Lines.Count();
                ScrapeBW.RunWorkerAsync();
            }
        }

        private void ScrapeBW_DoWork(object sender, System.ComponentModel.DoWorkEventArgs doWorkEventArgs)
        {
            WMI_Namespaces = WMI_NamespacesLoader.Load();
            if (!File.Exists(SPECS_PATH))
                File.WriteAllText(SPECS_PATH, GetTableHeadersAsCSV() + '\n');
            string[] computerNames = HostsTB.Lines;
            object fileWriteLock = new object();
            System.Threading.Tasks.Parallel.ForEach(computerNames, (computerName) =>
            {
                try
                {
                    string row = "\"" + computerName;
                    foreach (WMI.Namespace WMI_Namespace in WMI_Namespaces)
                        foreach (WMI.Class WMI_Class in WMI_Namespace.Classes)
                        {
                            ManagementObjectSearcher searcher = new ManagementObjectSearcher("\\\\" + computerName + "\\root\\"
                                + WMI_Namespace.Name, "SELECT * FROM " + WMI_Class.Name);
                            foreach (ManagementObject queryObj in searcher.Get())
                                foreach (WMI.Property property in WMI_Class.Properties)
                                    row += "\",\"" + queryObj[property.Name].ToString();
                        }
                    lock (fileWriteLock)
                        File.AppendAllText(SPECS_PATH, row + "\",\"" + DateTime.Now.ToString("s").Replace('T', ' ') + "\"\n");
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    lock (fileWriteLock)
                        File.AppendAllText(FAILED_PATH, computerName + "\n");
                }
                Invoke(new MethodInvoker(() =>
                {
                    MainPB.PerformStep();
                    HostsTB.Text = RemoveFirst(HostsTB.Text, computerName); // Remove the computer name from hosts list.
                    CleanupHostsTB();
                    HostsLBL.Text = $"Hosts (one per line) [{computerNames.Length - HostsTB.Lines.Length}/{computerNames.Length}]";
                }));
            });
            Invoke(new MethodInvoker(() =>
            {
                ScrapeBTN.Enabled = ScrapeFailedBTN.Enabled = HostsTB.Enabled = true;
                ScrapeDomainHostsBTN.Enabled = !DOMAIN.Equals("");
            }));
            System.Diagnostics.Process.Start(SPECS_PATH);
        }

        private string GetTableHeadersAsCSV()
        {
            string output = "\"Host";
            foreach (WMI.Namespace WMI_Namespace in WMI_Namespaces)
                foreach (WMI.Class WMI_Class in WMI_Namespace.Classes)
                    foreach (WMI.Property property in WMI_Class.Properties)
                        output += "\",\"" + property.DisplayName;
            return output + "\",\"DateTime\"";
        }

        public string RemoveFirst(string text, string search)
        {
            int pos = text.IndexOf(search);
            return pos < 0 ? text : text.Substring(0, pos) + text.Substring(pos + search.Length);
        }

        private void CleanupHostsTB()
        {
            HostsTB.Lines = Regex.Replace(HostsTB.Text, @"^\s+$[\r\n]*", "", RegexOptions.Multiline)
                .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None); // Remove empty lines.
            HostsTB.Text = HostsTB.Text.Replace(" ", ""); // Remove unnecessary spaces.
            HostsTB.Lines = HostsTB.Lines.Distinct().ToArray(); // Remove duplicates.
        }

        private void ScrapeFailedBTN_Click(object sender, EventArgs e)
        {
            if (File.Exists(FAILED_PATH))
            {
                HostsTB.Text = File.ReadAllText(FAILED_PATH).Trim('\n');
                File.Delete(FAILED_PATH);
                ScrapeBTN.PerformClick();
            }
            else
                MessageBox.Show(FAILED_PATH + " file was not found...");
        }
    }
}