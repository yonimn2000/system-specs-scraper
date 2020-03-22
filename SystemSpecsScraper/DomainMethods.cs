using System;
using System.Collections.Generic;
using System.DirectoryServices;

namespace YonatanMankovich.SystemSpecsScraper
{
    public static class DomainMethods
    {
        public static string GetDomainName()
        {
            return System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
        }

        public static bool IsOnDomain()
        {
            return GetDomainName().Length > 0;
        }

        public static string[] GetComputersFromCurrentDomain()
        {
            if (!IsOnDomain())
                throw new InvalidOperationException("The current computer is not part of a domain.");
            return GetDomainComputers(GetDomainName());
        }

        public static string[] GetDomainComputers(string domain)
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
            return computerNames.ToArray();
        }
    }
}