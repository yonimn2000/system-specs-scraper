using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SystemSpecsScraper
{
    public partial class Form1 : Form
    {
        List<WMI_Class> WMI_Classes = new List<WMI_Class>();
        string domain = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;

        struct WMI_Class
        {
            public string name;
            public IDictionary<string, string> properties;
            public WMI_Class(string newName)
            {
                name = newName;
                properties = new Dictionary<string, string>();
            }
        }

        private void InitializeWMI_Classes()
        {
            WMI_Class CPU = new WMI_Class("Win32_Processor");
            CPU.properties.Add("CPU", "Name");
            WMI_Classes.Add(CPU);

            WMI_Class OS = new WMI_Class("Win32_OperatingSystem");
            OS.properties.Add("RAM", "TotalVisibleMemorySize");
            OS.properties.Add("OS", "Caption");
            OS.properties.Add("OS Version", "Version");
            WMI_Classes.Add(OS);

            WMI_Class Device = new WMI_Class("Win32_ComputerSystemProduct");
            Device.properties.Add("Serial Number", "IdentifyingNumber");
            Device.properties.Add("Model Number", "Name");
            Device.properties.Add("Vendor", "Vendor");
            Device.properties.Add("Model Name", "Version");
            WMI_Classes.Add(Device);
        }

        private void InitializeOutputTableColumns()
        {
            OutputTable.Columns.Add("Date", "Date");
            OutputTable.Columns.Add("Host", "Host");
            foreach (WMI_Class wmiOBJ in WMI_Classes)
                foreach (KeyValuePair<string, string> property in wmiOBJ.properties)
                    OutputTable.Columns.Add(property.Key, property.Key);
        }

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            InitializeWMI_Classes();
            InitializeOutputTableColumns();
        }

        private void ReadHostsBTN_Click(object sender, EventArgs e)
        {
            HostNamesList.Text = string.Join("\n", GetDomainComputers(domain));
        }

        public static List<string> GetDomainComputers(string domain)
        {
            List<string> ComputerNames = new List<string>();
            DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain);
            DirectorySearcher mySearcher = new DirectorySearcher(entry);
            mySearcher.Filter = ("(objectClass=computer)");
            mySearcher.SizeLimit = int.MaxValue;
            mySearcher.PageSize = int.MaxValue;
            foreach (SearchResult resEnt in mySearcher.FindAll())
            {
                string ComputerName = resEnt.GetDirectoryEntry().Name;
                if (ComputerName.StartsWith("CN="))
                    ComputerName = ComputerName.Remove(0, "CN=".Length);
                if (!ComputerName.Contains("MediaAdmin"))
                    ComputerNames.Add(ComputerName);
            }
            mySearcher.Dispose();
            entry.Dispose();
            return ComputerNames;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReadHostsBTN.Enabled = !domain.Equals("");
            ReadHostsBTN.Text = !domain.Equals("") ? "Read Host Names From " + domain + " Domain" : "Not Connected to a Domain";
        }

        private void GetSpecsBTN_Click(object sender, EventArgs e)
        {
            if (!HostNamesList.Text.Equals(""))
            {
                GetSpecsBTN.Enabled = HostNamesList.Enabled = FailedComputersList.Enabled = ClearTableBTN.Enabled = CopyTableBTN.Enabled = ReadHostsBTN.Enabled = ExportTablesBTN.Enabled = false;
                GetSpecs();
                MessageBox.Show("Done reading specs!");
                GetSpecsBTN.Enabled = HostNamesList.Enabled = FailedComputersList.Enabled = ClearTableBTN.Enabled = CopyTableBTN.Enabled = ExportTablesBTN.Enabled = true;
                ReadHostsBTN.Enabled = !domain.Equals("");
            }
            else
                MessageBox.Show("Please enter hostnames before reading specs...");
        }

        private void GetSpecs()
        {
            progressBar1.Value = 0;
            HostNamesList.Lines = Regex.Replace(HostNamesList.Text, @"^\s+$[\r\n]*", "", RegexOptions.Multiline).Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);//Remove empty lines
            HostNamesList.Text = HostNamesList.Text.Replace(" ", "");
            string[] computerNames = HostNamesList.Lines;
            progressBar1.Maximum = computerNames.Count();
            foreach (string computerName in computerNames)
            {
                progressBar1.PerformStep();
                DataGridViewRow row = OutputTable.Rows[OutputTable.Rows.Add()];
                try
                {
                    foreach (WMI_Class wmiClass in WMI_Classes)
                    {
                        ManagementObjectSearcher searcher = new ManagementObjectSearcher("\\\\" + computerName + "\\root\\CIMV2", "SELECT * FROM " + wmiClass.name);
                        foreach (ManagementObject queryObj in searcher.Get())
                            foreach (KeyValuePair<string, string> property in wmiClass.properties)
                            {
                                if (property.Key.Equals("RAM"))
                                    row.Cells[property.Key].Value = Math.Round(int.Parse(queryObj[property.Value].ToString()) / 1048576.0, 2).ToString();
                                else
                                    row.Cells[property.Key].Value = queryObj[property.Value].ToString();
                            }
                    }
                    row.Cells["Date"].Value = DateTime.Now;
                    row.Cells["Host"].Value = computerName;
                }
                catch (Exception)
                {
                    if (!FailedComputersList.Text.Contains(computerName))
                        FailedComputersList.AppendText(computerName + Environment.NewLine);
                }
                if (row.Cells[0].Value == null)
                    OutputTable.Rows.RemoveAt(OutputTable.Rows.Count - 1);//Remove empty rows from the table
                if (computerName.Length != 0)
                    HostNamesList.Text = HostNamesList.Text.Replace(computerName, "");//Remove the computer name from hosts list
                HostNamesList.Lines = Regex.Replace(HostNamesList.Text, @"^\s+$[\r\n]*", "", RegexOptions.Multiline).Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);//Remove empty lines
            }
        }

        private void ExportTableBTN_Click(object sender, EventArgs e)
        {
            try
            {
                OutputTable.SelectAll();
                Clipboard.SetDataObject(OutputTable.GetClipboardContent());
                MessageBox.Show("Copied to clipboard");
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot copy the table because it is empty...");
            }
        }

        private void ClearTableBTN_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to clear the table?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
                OutputTable.Rows.Clear();
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
            if (!FailedComputersList.Text.Equals(""))
                File.WriteAllLines("Failed Computers.txt", FailedComputersList.Lines);
            try
            {
                OutputTable.SelectAll();
                File.WriteAllText("Specs.txt", OutputTable.GetClipboardContent().GetText());
            }
            catch (Exception)
            {
                MessageBox.Show("The table is empty...");
            }
            MessageBox.Show("System specs table and/or failed computers list were saved to " + Environment.CurrentDirectory, "Export Successful");
        }
    }
}