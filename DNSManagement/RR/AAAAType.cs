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
2012-03-09 - Fixed all saving methods to return connected records
 
*/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using DNSManagement.Extensions;

namespace DNSManagement.RR
{
    /// <summary>
    /// Represents an IPv6 Address (AAAA), often pronounced quad-A, RR.
    /// </summary>
    public class AAAAType : ResourceRecord
    {
        private ManagementObject m_mo;

        internal AAAAType(ManagementObject mo) : base(mo)
        {
            m_mo = mo;
        }

        /// <summary>
        /// IPv6 address for the host.
        /// </summary>
        public string IPv6Address
        {
            get
            {
                return Convert.ToString(m_mo["IPv6Address"]);
            }
        }

        /// <summary>
        /// Instantiates an 'AAAA' type of RR based on the data in the method's input
        /// parameters: the record's DNS Server Name, Container Name, Owner/Host
        /// Name, class (default = IN), time-to-live value, and the IPv6
        /// address. It returns a reference to the new object as an output 
        /// parameter. 
        /// </summary>
        /// <param name="server">Server object</param>
        /// <param name="dnsServerName">FQDN or IP address of the DNS Server that contains this RR.</param>
        /// <param name="containerName">Name of the Container for the Zone, Cache, or RootHints instance which contains this RR.</param>
        /// <param name="ownerName">Owner name for the RR.</param>
        /// <param name="recordClass">Optional - Class of the RR.</param>
        /// <param name="ttl">Optional - Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="ipv6Address">IPv6 address for the host.</param>
        /// <returns>new AAAA record</returns>
        public static AAAAType CreateInstanceFromPropertyData(
            Server server,
            string dnsServerName,
            string containerName,
            string ownerName,
            RecordClassEnum? recordClass,
            TimeSpan? ttl,
            string ipv6Address)
        {
            if (server == null)
                throw new ArgumentNullException("server is required");

            try
            {
                ManagementClass dnsClass = new ManagementClass(server.m_scope, new ManagementPath("MicrosoftDNS_AAAAType"), null);
                ManagementBaseObject inParams = dnsClass.GetMethodParameters("CreateInstanceFromPropertyData");
                inParams["DnsServerName"] = dnsServerName;
                inParams["ContainerName"] = containerName;
                inParams["OwnerName"] = ownerName;
                if (recordClass != null)
                    inParams["RecordClass"] = (UInt32)recordClass.Value;
                if (ttl != null)
                    inParams["TTL"] = ttl.Value.TotalSeconds;
                inParams["IPv6Address"] = ipv6Address;

                //return new AAAAType((ManagementObject)dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null));
                return new AAAAType(new ManagementObject(server.m_scope, new ManagementPath(dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null)["RR"].ToString()), null));
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }

        /// <summary>
        /// updates an IPv6 address (AAAA) Resource Record.
        /// </summary>
        /// <param name="ttl">Optional - Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="ipv6Address">Optional - IPv6 address for the host.</param>
        /// <returns>updated AAAA record</returns>
        public AAAAType Modify(TimeSpan? ttl, string ipv6Address)
        {
            ManagementBaseObject inParams = m_mo.GetMethodParameters("Modify");
            if ((ttl != null) && (ttl != this.TTL))
                inParams["TTL"] = ttl.Value.TotalSeconds;
            if ((!string.IsNullOrEmpty(ipv6Address)) && (ipv6Address != this.IPv6Address))
                inParams["IPv6Address"] = ipv6Address;

            try
            {
                return new AAAAType(new ManagementObject(m_mo.Scope, new ManagementPath(m_mo.InvokeMethod("Modify", inParams, null)["RR"].ToString()), null));
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLineFormat("IPv6Address={0}", IPv6Address);

            //RR
            base.ToString(sb);

            return sb.ToString();
        }
    }
}
