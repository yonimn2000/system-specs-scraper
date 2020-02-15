namespace YonatanMankovich.SystemSpecsScraper
{
    internal class WMI_Property
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public WMI_Property(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }
    }
}