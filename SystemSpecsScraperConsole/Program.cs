using System;
using System.Diagnostics;
using System.IO;
using YonatanMankovich.SimpleConsoleMenus;
using YonatanMankovich.SystemSpecsScraper;

namespace YonatanMankovich.SystemSpecsScraperConsole
{
    class Program
    {
        const string HOSTS_PATH = "Hosts.txt";
        const string FAILED_PATH = "FailedHosts.txt";
        const string DOMAIN_HOSTS_PATH = "DomainHosts.txt";
        const string SPECS_PATH = "Specs.csv"; // Output file path

        static readonly Scraper scraper = new Scraper(SPECS_PATH, FAILED_PATH);

        static void Main(string[] args)
        {
            scraper.UpdateHostStatusAction = OnUpdateHostStatus;
            if (args.Length > 0)
                scraper.Scrape(File.ReadAllLines(args[0]));
            else
            {
                scraper.FinishedScrapingAction = OnFinishedScraping;
                while (true)
                    ShowMenu();
            }
        }

        private static void ShowMenu()
        {
            SimpleActionConsoleMenu menu = new SimpleActionConsoleMenu("Choose an option:");
            if (File.Exists(HOSTS_PATH))
                menu.AddOption($"Scrape '{HOSTS_PATH}'", () => { scraper.Scrape(File.ReadAllLines(HOSTS_PATH)); });
            else
                menu.AddOption($"Create and open '{HOSTS_PATH}'", () => { File.Create(HOSTS_PATH).Close(); Process.Start(HOSTS_PATH); });
            if (File.Exists(FAILED_PATH))
                menu.AddOption($"Scrape '{FAILED_PATH}'", () => { scraper.Scrape(File.ReadAllLines(FAILED_PATH)); });
            if (DomainMethods.IsOnDomain())
            {
                menu.AddOption("Scrape domain hosts", () => { scraper.ScrapeDomainComputers(); });
                menu.AddOption("Save domain host names to " + DOMAIN_HOSTS_PATH, () => { File.WriteAllLines(DOMAIN_HOSTS_PATH, DomainMethods.GetComputersFromCurrentDomain()); });
                if (File.Exists(DOMAIN_HOSTS_PATH))
                    menu.AddOption($"Open '{DOMAIN_HOSTS_PATH}'", () => { Process.Start(DOMAIN_HOSTS_PATH); });
            }
            menu.AddOption("Open WMI properties file", () => { Process.Start(WMI_NamespacesLoader.NamespacesPath); });
            if (File.Exists(SPECS_PATH))
                menu.AddOption($"Open '{SPECS_PATH}'", () => { Process.Start(SPECS_PATH); });
            if (File.Exists(HOSTS_PATH))
                menu.AddOption($"Open '{HOSTS_PATH}'", () => { Process.Start(HOSTS_PATH); });
            if (File.Exists(FAILED_PATH))
                menu.AddOption($"Open '{FAILED_PATH}'", () => { Process.Start(FAILED_PATH); });
            menu.AddOption("Exit", () => { Environment.Exit(0); });
            menu.Show();
            Console.CursorVisible = false;
            Console.Clear();
            menu.DoAction();
            Console.CursorVisible = true;
        }

        static readonly object consoleWriteLock = new object();
        static void OnUpdateHostStatus(string host, string status)
        {
            lock (consoleWriteLock)
            {
                int finishedCount = scraper.GetFinishedHostsCount();
                int totalCount = scraper.GetTotalHostsCount();
                int percent = (int)Math.Round(100 * (double)finishedCount / totalCount);
                Console.CursorTop = 0;
                Console.WriteLine($"     Queued: {scraper.QueuedHostsCount}         \n" + // Spaces to overwrite old writes.
                                  $" Working on: {scraper.WorkingOnHostsCount}     \n" +
                                  $"  Succeeded: {scraper.SucceededHostsCount}\n" +
                                  $"     Failed: {scraper.FailedHostsCount}\n" +
                                  $"   Finished: {percent}% ({finishedCount}/{totalCount})");
            }
        }

        static void OnFinishedScraping()
        {
            Console.WriteLine("\nDone!\n");
            Console.ReadLine();
            Console.Clear();
        }
    }
}