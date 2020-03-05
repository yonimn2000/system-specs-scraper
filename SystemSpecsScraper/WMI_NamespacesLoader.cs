using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;

namespace YonatanMankovich.SystemSpecsScraper
{
    static class WMI_NamespacesLoader
    {
        public static string Path { get; set; } = "WMI_Namespaces.xml";

        public static IList<WMI.Namespace> Load()
        {
            RebuildDefaultBoardOptionsFileIfDoesNotExist();

            XmlDocument XML_Doc = new XmlDocument();
            // Read from the embedded XML Schema resource file.
            Stream schemaStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(typeof(WMI_NamespacesLoader).Namespace + ".WMI_Namespaces.xsd");
            XML_Doc.Schemas.Add(XmlSchema.Read(schemaStream, null));
            XML_Doc.Load(Path);
            // Validate according to the schema file.
            XML_Doc.Validate(null);

            IList<WMI.Namespace> WMI_Namespaces = new List<WMI.Namespace>();
            foreach (XmlNode WMI_NamespaceNode in XML_Doc.DocumentElement) // Iterate namespaces
            {
                WMI.Namespace WMI_Namespace = new WMI.Namespace(WMI_NamespaceNode.Attributes["name"].Value);
                foreach (XmlNode WMI_ClassNode in WMI_NamespaceNode.ChildNodes) // Iterate classes
                {
                    WMI.Class WMI_Class = new WMI.Class(WMI_ClassNode.Attributes["name"].Value);
                    foreach (XmlNode WMI_PropertyNode in WMI_ClassNode.ChildNodes) // Iterate properties
                        WMI_Class.Properties.Add(new WMI.Property(WMI_PropertyNode.Attributes["name"].Value,
                            WMI_PropertyNode.Attributes["displayName"].Value));
                    WMI_Namespace.Classes.Add(WMI_Class); 
                }
                WMI_Namespaces.Add(WMI_Namespace);
            }
            return WMI_Namespaces;
        }

        public static void RebuildDefaultBoardOptionsFileIfDoesNotExist()
        {
            if (!File.Exists(Path))
            {
                // Read from the embedded resource file.
                Assembly.GetExecutingAssembly().GetManifestResourceNames();
                Stream stream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream(typeof(WMI_NamespacesLoader).Namespace + ".WMI_Namespaces.xml");
                FileStream fileStream = File.Create(Path);
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
                fileStream.Close();
            }
        }
    }
}