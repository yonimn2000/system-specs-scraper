using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace YonatanMankovich.SystemSpecsScraper
{
    public partial class MainForm : Form
    {
        readonly IList<WMI.Namespace> WMI_Namespaces;
        readonly string DOMAIN = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
        readonly DataTable specsTable = new DataTable();

        private void InitializespecsTableColumns()
        {
            specsTable.Columns.Add("Host", typeof(string));
            foreach (WMI.Namespace WMI_Namespace in WMI_Namespaces)
                foreach (WMI.Class WMI_Class in WMI_Namespace.Classes)
                    foreach (WMI.Property property in WMI_Class.Properties)
                        specsTable.Columns.Add(property.DisplayName, typeof(string));
        }

        public MainForm()
        {
            InitializeComponent();
            WMI_Namespaces = WMI_NamespacesLoader.Load();
            InitializespecsTableColumns();
        }

        private void ReadHostsBTN_Click(object sender, EventArgs e)
        {
            string text = ReadHostsBTN.Text;
            ReadHostsBTN.Enabled = false;
            ReadHostsBTN.Text = "Reading. Please wait...";
            HostsTB.Lines = GetDomainComputers(DOMAIN).ToArray();
            ReadHostsBTN.Enabled = true;
            ReadHostsBTN.Text = text;
        }

        public static List<string> GetDomainComputers(string domain)
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
            return computerNames;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (DOMAIN.Equals("")) // If not on a domain...
            {
                ReadHostsBTN.Enabled = false;
                ReadHostsBTN.Text = "Not connected to a domain";
            }
            else
            {
                ReadHostsBTN.Enabled = true;
                ReadHostsBTN.Text = "Read hosts from " + DOMAIN.ToUpper();
            }
        }

        private void ScrapeBTN_Click(object sender, EventArgs e)
        {
            if (HostsTB.Text.Equals(""))
                MessageBox.Show("Please enter host names before reading specs...");
            else
            {
                ScrapeBTN.Enabled = HostsTB.Enabled = ReadHostsBTN.Enabled = false;
                CleanupHostsTB();
                MainPB.Value = 0;
                MainPB.Maximum = HostsTB.Lines.Count();
                ScrapeBW.RunWorkerAsync();
            }
        }

        private void ScrapeBW_DoWork(object sender, System.ComponentModel.DoWorkEventArgs doWorkEventArgs)
        {
            string[] computerNames = HostsTB.Lines;
            string log = "";
            System.Threading.Tasks.Parallel.ForEach(computerNames, (computerName) =>
            {
                DataRow row = specsTable.NewRow();
                try
                {
                    foreach (WMI.Namespace WMI_Namespace in WMI_Namespaces)
                        foreach (WMI.Class WMI_Class in WMI_Namespace.Classes)
                        {
                            ManagementObjectSearcher searcher = new ManagementObjectSearcher("\\\\" + computerName + "\\root\\"
                                + WMI_Namespace.Name, "SELECT * FROM " + WMI_Class.Name);
                            foreach (ManagementObject queryObj in searcher.Get())
                                foreach (WMI.Property property in WMI_Class.Properties)
                                    row[property.DisplayName] = queryObj[property.Name].ToString();
                        }
                    row["Host"] = computerName;
                    specsTable.Rows.Add(row);
                }
                catch (System.Runtime.InteropServices.COMException e)
                {
                    log += $"[{DateTime.Now}] {computerName} | {e.Message}";
                    File.WriteAllText("Failed Hosts.txt", computerName + "\n");
                }
                Invoke(new MethodInvoker(() =>
                {
                    MainPB.PerformStep();
                    HostsTB.Text = HostsTB.Text.Replace(computerName, ""); // Remove the computer name from hosts list.
                    CleanupHostsTB();
                }));
            });
            File.AppendAllText("ScrapingLog.log", log + $"[{DateTime.Now}] Finished\n\n");
            ExportTables();
            Invoke(new MethodInvoker(() =>
            {
                ScrapeBTN.Enabled = HostsTB.Enabled = true;
                ReadHostsBTN.Enabled = !DOMAIN.Equals("");
            }));
        }

        private void CleanupHostsTB()
        {
            HostsTB.Lines = Regex.Replace(HostsTB.Text, @"^\s+$[\r\n]*", "", RegexOptions.Multiline)
                .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None); // Remove empty lines.
            HostsTB.Text = HostsTB.Text.Replace(" ", ""); // Remove unnecessary spaces.
            HostsTB.Lines = HostsTB.Lines.Distinct().ToArray(); // Remove duplicates.
        }

        private void ExportTables()
        {
            StringBuilder stringBuilder = new StringBuilder();
            IEnumerable<string> columnNames = specsTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            stringBuilder.AppendLine(string.Join(",", columnNames));
            foreach (DataRow row in specsTable.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                stringBuilder.AppendLine(string.Join(",", fields));
            }
            string path = DateTime.Now.ToString("s").Replace(':', '-') + " Specs.csv";
            File.WriteAllText(path, stringBuilder.ToString());
            System.Diagnostics.Process.Start(path);
        }
    }
}