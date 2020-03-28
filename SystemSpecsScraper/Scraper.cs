using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace YonatanMankovich.SystemSpecsScraper
{
    public class Scraper
    {
        public string SpecsPath { get; set; }
        public string FailedPath { get; set; }

        public int QueuedHostsCount { get; set; }
        public int WorkingOnHostsCount { get; set; }
        public int FailedHostsCount { get; set; }
        public int SucceededHostsCount { get; set; }

        public Action<string, string> UpdateHostStatusAction { get; set; }
        public Action FinishedScrapingAction { get; set; }

        private IList<WMI.Namespace> WMI_Namespaces { get; set; }
        private Stopwatch StopWatch { get; } = new Stopwatch();

        public Scraper(string specsPath, string failedPath)
        {
            SpecsPath = specsPath;
            FailedPath = failedPath;
            WMI_NamespacesLoader.RebuildDefaultNamespacesFileIfDoesNotExist();
        }

        public void Scrape(string[] computerNames)
        {
            StopWatch.Restart();
            QueuedHostsCount = WorkingOnHostsCount = FailedHostsCount = SucceededHostsCount = 0;
            computerNames = computerNames.Distinct().ToArray(); // Remove duplicates.
            WMI_Namespaces = WMI_NamespacesLoader.Load();
            if (File.Exists(SpecsPath) && !File.ReadLines(SpecsPath).First().Equals(GetTableHeadersAsCSV())) // If CVS headers deffer...
                File.Move(SpecsPath, File.GetLastWriteTime(SpecsPath).ToString("yyyyMMdd-HHmmss") + " " + SpecsPath);
            if (!File.Exists(SpecsPath))
                File.WriteAllText(SpecsPath, GetTableHeadersAsCSV() + '\n');
            object fileWriteLock = new object();
            List<Task> TaskList = new List<Task>();
            for (int currentComputerIndex = 0; currentComputerIndex < computerNames.Length; currentComputerIndex++)
            {
                QueuedHostsCount++;
                string computerName = computerNames[currentComputerIndex];
                UpdateHostStatusAction?.Invoke(computerName, "Queued");
                Task newTask = new Task(() =>
                {
                    WorkingOnHostsCount++;
                    QueuedHostsCount--;
                    UpdateHostStatusAction?.Invoke(computerName, "Started");
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
                            File.AppendAllText(SpecsPath, row + "\",\"" + DateTime.Now.ToString("s").Replace('T', ' ') + "\"\n");
                        SucceededHostsCount++;
                        WorkingOnHostsCount--;
                        UpdateHostStatusAction?.Invoke(computerName, "Success");
                    }
                    catch (Exception e) when (e is System.Runtime.InteropServices.COMException  // Host offline or does not exist on current network.
                                           || e is UnauthorizedAccessException                  // Access denied
                                           || e is ManagementException)                         // Access denied
                    {
                        lock (fileWriteLock)
                            File.AppendAllText(FailedPath, computerName + "\n");
                        FailedHostsCount++;
                        WorkingOnHostsCount--;
                        UpdateHostStatusAction?.Invoke(computerName, e.Message);
                    }
                });
                newTask.Start();
                TaskList.Add(newTask);
            }
            Task.WaitAll(TaskList.ToArray());
            if (File.Exists(FailedPath))
                File.WriteAllLines(FailedPath, File.ReadAllLines(FailedPath).Distinct());
            FinishedScrapingAction?.Invoke();
            StopWatch.Stop();
        }

        public void ScrapeDomainComputers()
        {
            Scrape(DomainMethods.GetComputersFromCurrentDomain());
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

        public int GetTotalHostsCount()
        {
            return QueuedHostsCount + WorkingOnHostsCount + SucceededHostsCount + FailedHostsCount;
        }

        public int GetFinishedHostsCount()
        {
            return SucceededHostsCount + FailedHostsCount;
        }

        public TimeSpan GetElapsedTime()
        {
            return StopWatch.Elapsed;
        }

        public TimeSpan GetEstimatedTimeRemaining()
        {
            int finishedCount = GetFinishedHostsCount();
            TimeSpan elapsedTime = GetElapsedTime();
            return finishedCount == 0 ? TimeSpan.Zero : TimeSpan.FromTicks(elapsedTime.Ticks * GetTotalHostsCount() / finishedCount - elapsedTime.Ticks);
        }
    }
}