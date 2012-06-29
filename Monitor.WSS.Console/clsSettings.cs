using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Monitor.WSS.Console
{
    class clsSettings
    {
        static string settingsFileName = "Settings.xml";

        public static void GetSettings()
        {
            FileInfo settingsFile = new FileInfo(settingsFileName);

            if (!settingsFile.Exists)
                CreateDefault();

            XDocument data = XDocument.Load(settingsFileName);


        }

        private static void CreateDefault()
        {
            using (XmlWriter writer = XmlWriter.Create(settingsFileName))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Settings");

                writer.WriteElementString("MailServer", "192.168.0.246");
                writer.WriteElementString("Port", "27");
                writer.WriteElementString("To", "phill@xperan.co.uk");

                writer.WriteStartElement("Servers");
                writer.WriteElementString("Address", "192.168.0.1");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

        }
    }

    public class Settings
    {

    }
}
