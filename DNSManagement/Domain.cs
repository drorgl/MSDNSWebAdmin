/*
DNSManagement - Wrapper for WMI Management of MS DNS
Copyright (C) 2011 Dror Gluska
	
This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public License
(LGPL) as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.
The terms of redistributing and/or modifying this software also
include exceptions to the LGPL that facilitate static linking.
 	
This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
Lesser General Public License for more details.
 	
You should have received a copy of the GNU Lesser General Public License
along with this library; if not, write to Free Software Foundation, Inc.,
51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA


Change log:
2011-05-17 - Initial version

*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using DNSManagement.Extensions;
using DNSManagement.RR;

namespace DNSManagement
{
    /// <summary>
    /// Represents a domain in a DNS hierarchy tree.
    /// </summary>
    public class Domain
    {
        /// <summary>
        /// WMI Unsupported Record types.
        /// <para>Microsoft decided to partially support a few record types, partially supported record types will be removed from this library.</para>
        /// </summary>
        public string[] UnsupportedRecordTypes = new string[] { "ATMA" };

        private ManagementObject m_mo;

        internal Domain(ManagementObject mo)
        {
            m_mo = mo;
        }

        /// <summary>
        /// Name of the container of the domain, which could be a Zone, Cache, or RootHints. 
        /// <para>
        /// In cases where the Container is a Zone (an instance of the MicrosoftDNS_Zone class), this property contains the FQDN of the Zone.
        /// </para>
        /// <para>When the Container is the root zone, the string \.\ should be used.</para>
        /// <para>In cases where the Container is the DNS cache of resource records (an instance of the MicrosoftDNS_Cache class), this property is set to the string \..Cache\.</para>
        /// <para>If the Container is RootHints (an instance of the MicrosoftDNS_RootHints class), this property should be set to \..RootHints\.</para>
        /// </summary>
        public string ContainerName
        {
            get
            {
                return Convert.ToString(m_mo["ContainerName"]);
            }
        }

        /// <summary>
        /// FQDN or IP address of the DNS Server that contains the domain.
        /// </summary>
        public string DnsServerName
        {
            get
            {
                return Convert.ToString(m_mo["DnsServerName"]);
            }
        }

        /// <summary>
        /// FQDN of the domain. 
        /// <para>For instances of DNS Cache or RootHints, the strings \..Cache\ and \..RootHints\ should be used, respectively.</para>
        /// </summary>
        public string Name
        {
            get
            {
                return Convert.ToString(m_mo["Name"]);
            }
        }

        /// <summary>
        /// Obtains DS distinguished Name for the zone. 
        /// </summary>
        /// <returns></returns>
        public string GetDistinguishedName()
        {
            return Convert.ToString(m_mo.InvokeMethod("GetDistinguishedName", null));
        }


        /// <summary>
        /// Gets all records for Root Hints
        /// </summary>
        /// <returns></returns>
        public DNSManagement.RR.ResourceRecord[] GetRecords()
        {
            string query = String.Format("SELECT * FROM MicrosoftDNS_ResourceRecord WHERE ContainerName='{0}'", this.ContainerName);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(m_mo.Scope, new ObjectQuery(query));

            ManagementObjectCollection collection = searcher.Get();

            List<ResourceRecord> records = new List<ResourceRecord>();
            try
            {
                foreach (ManagementObject p in collection)
                {
                    var rr = new ResourceRecord(p);
                    if (!UnsupportedRecordTypes.Contains( rr.RecordTypeText))
                        records.Add(rr.UnderlyingRecord);
                }
            }catch (System.Management.ManagementException me)
            {
                throw new Exception(me.ErrorInformation.GetText(System.Management.TextFormat.Mof), me);
            }

            return records.ToArray();

        }

        /// <summary>
        /// Deletes the object
        /// </summary>
        public void Delete()
        {
            m_mo.Delete();
        }

        internal void ToString(StringBuilder sb)
        {
            sb.AppendLineFormat("ContainerName={0}", ContainerName);
            sb.AppendLineFormat("DnsServerName={0}", DnsServerName);
            sb.AppendLineFormat("Name={0}", Name);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            ToString(sb);
            
            return sb.ToString();
        }
    }
}
