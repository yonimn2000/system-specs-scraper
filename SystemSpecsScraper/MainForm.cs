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
        readonly IList<WMI_Class> WMI_Classes;
        readonly string DOMAIN = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;

        private void InitializeOutputTableColumns()
        {
            OutputTable.Columns.Add("Date", "Date");
            OutputTable.Columns.Add("Host", "Host");
            foreach (WMI_Class wmiOBJ in WMI_Classes)
                foreach (WMI_Property property in wmiOBJ.Properties)
                    OutputTable.Columns.Add(property.DisplayName, property.DisplayName);
        }

        public MainForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            WMI_Classes = WMI_ClassesLoader.Load();
            InitializeOutputTableColumns();
        }

        private void ReadHostsBTN_Click(object sender, EventArgs e)
        {
            HostNamesList.Text = string.Join("\n", GetDomainComputers(DOMAIN));
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
            ReadHostsBTN.Enabled = !DOMAIN.Equals("");
            ReadHostsBTN.Text = !DOMAIN.Equals("") ? "Read Host Names From " + DOMAIN + " Domain" : "Not Connected to a Domain";
        }

        private void GetSpecsBTN_Click(object sender, EventArgs e)
        {
            if (!HostNamesList.Text.Equals(""))
            {
                GetSpecsBTN.Enabled = HostNamesList.Enabled = FailedComputersList.Enabled = ClearTableBTN.Enabled = CopyTableBTN.Enabled = ReadHostsBTN.Enabled = ExportTablesBTN.Enabled = false;
                GetSpecs();
                MessageBox.Show("Done reading specs!");
                GetSpecsBTN.Enabled = HostNamesList.Enabled = FailedComputersList.Enabled = ClearTableBTN.Enabled = CopyTableBTN.Enabled = ExportTablesBTN.Enabled = true;
                ReadHostsBTN.Enabled = !DOMAIN.Equals("");
            }
            else
                MessageBox.Show("Please enter host names before reading specs...");
        }

        private void GetSpecs()
        {
            MainPB.Value = 0;
            CleanupHostNamesRTB();
            string[] computerNames = HostNamesList.Lines;
            MainPB.Maximum = computerNames.Count();
            foreach (string computerName in computerNames)
            {
                MainPB.PerformStep();
                DataGridViewRow row = OutputTable.Rows[OutputTable.Rows.Add()];
                try
                {
                    foreach (WMI_Class wmiClass in WMI_Classes)
                    {
                        ManagementObjectSearcher searcher = new ManagementObjectSearcher("\\\\" + computerName + "\\root\\CIMV2", "SELECT * FROM " + wmiClass.Name);
                        foreach (ManagementObject queryObj in searcher.Get())
                            foreach (WMI_Property property in wmiClass.Properties)
                                row.Cells[property.DisplayName].Value = queryObj[property.Name].ToString();
                    }
                    row.Cells["Date"].Value = DateTime.Now;
                    row.Cells["Host"].Value = computerName;
                }
                catch (Exception e)
                {
                    File.AppendAllText("ScrapingLog.log", $"[{computerName}] {e.Message}");
                    if (!FailedComputersList.Text.Contains(computerName))
                        FailedComputersList.AppendText(computerName + Environment.NewLine);
                }
                if (row.Cells[0].Value == null)
                    OutputTable.Rows.RemoveAt(OutputTable.Rows.Count - 1); // Remove empty rows from the table.
                if (computerName.Length != 0)
                    HostNamesList.Text = HostNamesList.Text.Replace(computerName, ""); // Remove the computer name from hosts list.
                HostNamesList.Lines = Regex.Replace(HostNamesList.Text, @"^\s+$[\r\n]*", "", RegexOptions.Multiline)
                    .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None); // Remove empty lines
            }
        }

        private void CleanupHostNamesRTB()
        {
            HostNamesList.Lines = Regex.Replace(HostNamesList.Text, @"^\s+$[\r\n]*", "", RegexOptions.Multiline)
                .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None); // Remove empty lines.
            HostNamesList.Text = HostNamesList.Text.Replace(" ", ""); // Remove unnecessary spaces.
            HostNamesList.Lines = HostNamesList.Lines.Distinct().ToArray(); // Remove duplicates.
        }

        private void ExportTableBTN_Click(object sender, EventArgs e)
        {
            OutputTable.SelectAll();
            Clipboard.SetDataObject(OutputTable.GetClipboardContent());
            MessageBox.Show("Copied to clipboard");
        }

        private void ClearTableBTN_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to clear the table?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
                OutputTable.Rows.Clear();
            ClearTableBTN.Enabled = CopyTableBTN.Enabled = ExportTablesBTN.Enabled = false;
        }

        private void ExportTablesBTN_Click(object sender, EventArgs e)
        {
            if (File.Exists("Specs.txt") || File.Exists("Failed Computers.txt"))
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to overwrite these files?", "These files already exist...", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                    ExportTables();
            }
            else
                ExportTables();
        }

        private void ExportTables()
        {
            if (FailedComputersList.Text.Length > 0)
                File.WriteAllLines("Failed Computers.txt", FailedComputersList.Lines);
            OutputTable.SelectAll();
            File.WriteAllText("Specs.csv", OutputTable.GetClipboardContent().GetText().Replace('\t', ','));
            MessageBox.Show("System specs table and/or failed computers list were saved to " + Environment.CurrentDirectory, "Export Successful");
        }

        private void OutputTable_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (OutputTable.Rows.Count > 0)
                ClearTableBTN.Enabled = CopyTableBTN.Enabled = ExportTablesBTN.Enabled = true;
        }
    }
}