using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;

namespace YonatanMankovich.SystemSpecsScraper
{
    static class WMI_ClassesLoader
    {
        public static string Path { get; set; } = "WMI_Classes.xml";

        public static IList<WMI_Class> Load()
        {
            RebuildDefaultBoardOptionsFileIfDoesNotExist();

            XmlDocument XML_Doc = new XmlDocument();
            // Read from the embedded XML Schema resource file.
            Stream schemaStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(typeof(WMI_ClassesLoader).Namespace + ".WMI_Classes.xsd");
            XML_Doc.Schemas.Add(XmlSchema.Read(schemaStream, null));
            XML_Doc.Load(Path);
            // Validate according to the schema file.
            XML_Doc.Validate(null);

            IList<WMI_Class> WMI_Classes = new List<WMI_Class>();
            foreach (XmlNode WMI_ClassNode in XML_Doc.DocumentElement)
            {
                WMI_Class WMI_Class = new WMI_Class(WMI_ClassNode.Attributes["name"].Value);
                foreach (XmlNode WMI_PropertyNode in WMI_ClassNode.ChildNodes)
                    WMI_Class.Properties.Add(new WMI_Property(WMI_PropertyNode.Attributes["name"].Value,
                        WMI_PropertyNode.Attributes["displayName"].Value));
                WMI_Classes.Add(WMI_Class);
            }
            return WMI_Classes;
        }

        public static void RebuildDefaultBoardOptionsFileIfDoesNotExist()
        {
            if (!File.Exists(Path))
            {
                // Read from the embedded resource file.
                Assembly.GetExecutingAssembly().GetManifestResourceNames();
                Stream stream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream(typeof(WMI_ClassesLoader).Namespace + ".WMI_Classes.xml");
                FileStream fileStream = File.Create(Path);
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
                fileStream.Close();
            }
        }
    }
}