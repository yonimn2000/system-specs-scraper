using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YonatanMankovich.SystemSpecsScraper
{
    public partial class MainForm : Form
    {
        IList<WMI.Namespace> WMI_Namespaces;
        readonly string DOMAIN = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
        const string SPECS_PATH = "Specs.csv"; // Output file path
        const string FAILED_PATH = "FailedHosts.txt";
        // HACK: Separate Core and UI
        public MainForm()
        {
            InitializeComponent();
        }

        private void ScrapeDomainHostsBTN_Click(object sender, EventArgs e)
        {
            string text = ScrapeDomainHostsBTN.Text;
            ScrapeDomainHostsBTN.Text = "Reading. Please wait...";
            ScrapeDomainHostsBTN.Enabled = false;
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
                WMI_Namespaces = WMI_NamespacesLoader.Load();
                if (File.Exists(SPECS_PATH) && !File.ReadLines(SPECS_PATH).First().Equals(GetTableHeadersAsCSV())) // If CVS headers deffer...
                {
                    string renameTo = File.GetLastWriteTime(SPECS_PATH).ToString("yyyyMMdd-HHmmss") + " " + SPECS_PATH;
                    File.Move(SPECS_PATH, renameTo);
                    MessageBox.Show("Looks like table headers were added or removed since scraping specs last time. " +
                        "The program will rename the old CSV file to '" + renameTo + "' and create a new one.");
                }
                if (!File.Exists(SPECS_PATH))
                    File.WriteAllText(SPECS_PATH, GetTableHeadersAsCSV() + '\n');
                HostsTB.ReadOnly = true;
                ScrapeBTN.Enabled = ScrapeFailedBTN.Enabled = ScrapeDomainHostsBTN.Enabled = false;
                CleanupHostsTB();
                MainPB.Value = 0;
                MainPB.Maximum = HostsTB.Lines.Count();
                ScrapeBW.RunWorkerAsync();
            }
        }

        private void ScrapeBW_DoWork(object sender, System.ComponentModel.DoWorkEventArgs doWorkEventArgs)
        {
            string[] computerNames = HostsTB.Lines;
            object fileWriteLock = new object();
            List<Task> TaskList = new List<Task>();
            foreach (string computerName in computerNames)
            {
                Task newTask = new Task(() =>
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
                                    {
                                        row += "\",\"";
                                        try { row += queryObj[property.Name].ToString(); }
                                        catch (NullReferenceException) { } // Some properties return null value.
                                    }
                            }
                        lock (fileWriteLock)
                            File.AppendAllText(SPECS_PATH, row + "\",\"" + DateTime.Now.ToString("s").Replace('T', ' ') + "\"\n");
                    }
                    catch (Exception e) when (e is System.Runtime.InteropServices.COMException  // Host offline
                                           || e is UnauthorizedAccessException                  // Access denied
                                           || e is ManagementException)                         // Access denied
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
                newTask.Start();
                TaskList.Add(newTask);
            }
            Task.WaitAll(TaskList.ToArray());
            Invoke(new MethodInvoker(() =>
            {
                HostsTB.ReadOnly = false;
                ScrapeBTN.Enabled = ScrapeFailedBTN.Enabled = true;
                ScrapeDomainHostsBTN.Enabled = !DOMAIN.Equals("");
            }));
            if (File.ReadAllLines(SPECS_PATH).Length == 1)
            {
                File.Delete(SPECS_PATH);
                MessageBox.Show("Scraping failed.");
            }
            else
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