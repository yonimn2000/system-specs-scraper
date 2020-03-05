using System.Collections.Generic;

namespace YonatanMankovich.SystemSpecsScraper
{
    internal static class WMI
    {
        internal class Namespace
        {
            public string Name { get; set; }
            public IList<Class> Classes { get; } = new List<Class>();

            public Namespace(string name)
            {
                Name = name;
            }
        }

        internal class Class
        {
            public string Name { get; set; }
            public IList<Property> Properties { get; } = new List<Property>();

            public Class(string name)
            {
                Name = name;
            }
        }

        internal class Property
        {
            public string Name { get; set; }

            /// <summary> Gets or sets the name that will be shown in the output. </summary>
            public string DisplayName { get; set; }

            public Property(string name, string displayName)
            {
                Name = name;
                DisplayName = displayName;
            }
        }
    }
}