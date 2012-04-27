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
    /// Represents a Host Information (HINFO) RR. 
    /// </summary>
    public class HINFOType : ResourceRecord
    {
        private ManagementObject m_mo;

        internal HINFOType(ManagementObject mo)
            : base(mo)
        {
            m_mo = mo;
        }

        /// <summary>
        /// CPU type of the record owner.
        /// </summary>
        public string CPU
        {
            get
            {
                return Convert.ToString(m_mo["CPU"]);
            }
        }

        /// <summary>
        /// Operating system of the record owner.
        /// </summary>
        public string OS
        {
            get
            {
                return Convert.ToString(m_mo["OS"]);
            }
        }


        /// <summary>
        /// Instantiates an HINFO of RR based on the data in the method's input parameters: the record's DNS Server Name, Container Name, Owner Name, class (default = IN), time-to-live value, and the host's CPU and operating system types. It returns a reference to the new object as an output parameter. 
        /// </summary>
        /// <param name="server">Server object</param>
        /// <param name="dnsServerName">FQDN or IP address of the DNS Server that contains this RR.</param>
        /// <param name="containerName">Name of the Container for the Zone, Cache, or RootHints instance which contains this RR.</param>
        /// <param name="ownerName">Owner name for the RR.</param>
        /// <param name="recordClass">Optional - Class of the RR</param>
        /// <param name="ttl">Optional - Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="cpu">CPU type of the record owner.</param>
        /// <param name="os">Operating system of the record owner.</param>
        /// <returns>the new object.</returns>
        public static HINFOType CreateInstanceFromPropertyData(
          Server server,
          string dnsServerName,
          string containerName,
          string ownerName,
          RecordClassEnum? recordClass,
          TimeSpan? ttl,
          string cpu,
          string os)
        {
            if (server == null)
                throw new ArgumentNullException("server is required");

            ManagementClass dnsClass = new ManagementClass(server.m_scope, new ManagementPath("MicrosoftDNS_HINFOType"), null);
            ManagementBaseObject inParams = dnsClass.GetMethodParameters("CreateInstanceFromPropertyData");
            inParams["DnsServerName"] = dnsServerName;
            inParams["ContainerName"] = containerName;
            inParams["OwnerName"] = ownerName;
            if (recordClass != null)
                inParams["RecordClass"] = (UInt32)recordClass.Value;
            if (ttl != null)
                inParams["TTL"] = ttl.Value.TotalSeconds;
            inParams["CPU"] = cpu;
            inParams["OS"] = os;

            try
            {
                return new HINFOType(new ManagementObject(server.m_scope, new ManagementPath(dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null)["RR"].ToString()), null));
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }

        /// <summary>
        /// Updates the TTL, CPU, and operating system to the values specified as the input parameters of this method. If a new value for a parameter is not specified, then the current value for the parameter is not changed. The method returns a reference to the modified object as an output parameter. 
        /// </summary>
        /// <param name="ttl">Optional - Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="cpu">Optional - CPU type of the record owner.</param>
        /// <param name="os">Optional - Operating system of the record owner.</param>
        /// <returns>the new object.</returns>
        public HINFOType Modify(TimeSpan? ttl, string cpu, string os)
        {
            ManagementBaseObject inParams = m_mo.GetMethodParameters("Modify");
            if ((ttl != null) && (ttl != this.TTL))
                inParams["TTL"] = ttl.Value.TotalSeconds;
            if ((!string.IsNullOrEmpty(cpu)) && (cpu != this.CPU))
                inParams["CPU"] = cpu;
            if ((!string.IsNullOrEmpty(os)) && (os != this.OS))
                inParams["OS"] = os;
           

            //return new HINFOType((ManagementObject)m_mo.InvokeMethod("Modify", inParams, null));
            return new HINFOType(new ManagementObject(m_mo.Scope, new ManagementPath(m_mo.InvokeMethod("Modify", inParams, null)["RR"].ToString()), null));
        }



        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLineFormat("CPU={0}", CPU);
            sb.AppendLineFormat("OS={0}", OS);

            //RR
            base.ToString(sb);

            return sb.ToString();
        }
    }
}
