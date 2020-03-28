using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
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
        static readonly System.Timers.Timer statusRefreshTimer = new System.Timers.Timer(50);

        static void Main(string[] args)
        {
            statusRefreshTimer.Elapsed += StatusRefreshTimer_Elapsed;
            statusRefreshTimer.AutoReset = true;
            statusRefreshTimer.Enabled = false;

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
                menu.AddOption($"Scrape '{HOSTS_PATH}'", () => { StartScraping(() => { scraper.Scrape(File.ReadAllLines(HOSTS_PATH)); }); });
            else
                menu.AddOption($"Create and open '{HOSTS_PATH}'", () =>
                {
                    File.Create(HOSTS_PATH).Close();
                    Process.Start(HOSTS_PATH);
                });
            if (File.Exists(FAILED_PATH))
                menu.AddOption($"Scrape '{FAILED_PATH}'", () => { StartScraping(() => { scraper.Scrape(File.ReadAllLines(FAILED_PATH)); }); });
            if (DomainMethods.IsOnDomain())
            {
                menu.AddOption("Scrape domain hosts", () =>
                {
                    WriteGettingDomainHosts();
                    StartScraping(() => { scraper.ScrapeDomainComputers(); });
                });
                menu.AddOption($"Save domain hostnames to '{DOMAIN_HOSTS_PATH}'", () =>
                {
                    WriteGettingDomainHosts();
                    File.WriteAllLines(DOMAIN_HOSTS_PATH, DomainMethods.GetComputersFromCurrentDomain());
                    Console.Clear();
                });
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

        private static void StartScraping(Action scrapeAction)
        {
            statusRefreshTimer.Enabled = true;
            scrapeAction();
        }

        private static void WriteGettingDomainHosts()
        {
            Console.WriteLine("Getting domain hosts.\nPlease wait...");
            Thread.Sleep(10); // Needed to show the text above
        }

        private static void StatusRefreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            statusRefreshTimer.Interval = 1000;
            Console.CursorTop = 0;
            Console.WriteLine("Scraping. Please wait...");
            int finishedCount = scraper.GetFinishedHostsCount();
            int totalCount = scraper.GetTotalHostsCount();
            int percent = Math.Max(0, (int)Math.Round(100 * (double)finishedCount / totalCount));
            Console.WriteLine($"     Queued: {scraper.QueuedHostsCount}        \n" + // Spaces to overwrite old writes.
                              $" Working on: {scraper.WorkingOnHostsCount}     \n" +
                              $"  Succeeded: {scraper.SucceededHostsCount}\n" +
                              $"     Failed: {scraper.FailedHostsCount}\n" +
                              $"   Finished: {percent}% ({finishedCount}/{totalCount})\n" +
                              $"    Elapsed: {scraper.GetElapsedTime():hh\\:mm\\:ss}\n" +
                              $"  Remaining: {scraper.GetEstimatedTimeRemaining():hh\\:mm\\:ss}");
        }

        static void OnFinishedScraping()
        {
            statusRefreshTimer.Enabled = false;
            Console.Clear();
            Console.WriteLine("Done!");
            Console.WriteLine($" Succeeded: {scraper.SucceededHostsCount}\n" +
                              $"    Failed: {scraper.FailedHostsCount}\n" +
                              $"   Elapsed: {scraper.GetElapsedTime():hh\\:mm\\:ss}");
            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
            Console.Clear();
        }
    }
}