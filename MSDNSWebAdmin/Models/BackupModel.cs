using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DNSManagement;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;

namespace MSDNSWebAdmin.Models
{
    [Serializable]
    public class BackupModel
    {
        public Server Server { get; set; }

        public List<Zone> Zones { get; set; }
        public Dictionary<string, List<DNSManagement.RR.ResourceRecord>> ResourceRecords { get; set; }
        //private List<DNSManagement.RR.ResourceRecord> ResourceRecords { get; set; }

        public void PopulateFromServer(DNSManagement.Server msserver)
        {
            //load server
            this.Server = msserver;

            //initialize zones collection
            this.Zones = new List<Zone>();

            //initialize resource records dictionary
            this.ResourceRecords = new Dictionary<string,List<DNSManagement.RR.ResourceRecord>>();
            //this.ResourceRecords = new List<DNSManagement.RR.ResourceRecord>();

            //load all zones.
            foreach (var zone in msserver.GetZones())
            {
                this.Zones.Add(zone);

                if (!this.ResourceRecords.ContainsKey(zone.Name))
                    this.ResourceRecords[zone.Name] = new List<DNSManagement.RR.ResourceRecord>();

                //in each zone, load all resource records.
                foreach (var rr in zone.GetRecords())
                {
                    this.ResourceRecords[zone.Name].Add(rr);
                    //this.ResourceRecords.Add(rr);
                }
    
            }
        }

        public string ToXml()
        {
            //SoapFormatter sf = new SoapFormatter();
            //MemoryStream ms = new MemoryStream();
            //sf.Serialize(ms, this);
            //return System.Text.UTF8Encoding.UTF8.GetString(ms.ToArray());

            XmlSerializer xmlser = new XmlSerializer(this.GetType());

            StringWriter sw = new StringWriter();
            xmlser.Serialize(sw, this);
            return sw.ToString();
        }

        public List<KeyValuePair<string, string>> ToConfigurationFiles()
        {
            List<KeyValuePair<string, string>> retval = new List<KeyValuePair<string, string>>();
            retval.Add(new KeyValuePair<string, string>("DnsSettings.txt", this.Server.ToConfigurationFile()));

            foreach (var zone in this.Zones)
            {
                var filename = zone.DataFile;
                StringBuilder sbText = new StringBuilder();
                foreach (var rr in this.ResourceRecords[zone.Name])
                {
                    sbText.AppendLine(rr.TextRepresentation);
                }

                retval.Add(new KeyValuePair<string, string>(filename, sbText.ToString()));
            }

            return retval;
        }

        public string ToConfigurationFile()
        {
            return this.Server.ToConfigurationFile();
        }
    }
}