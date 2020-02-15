using System.Collections.Generic;

namespace YonatanMankovich.SystemSpecsScraper
{
    internal class WMI_Class
    {
        public string Name { get; set; }
        public IList<WMI_Property> Properties { get; } = new List<WMI_Property>();

        public WMI_Class(string newName)
        {
            Name = newName;
        }
    }
}