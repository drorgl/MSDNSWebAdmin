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
2012-04-27 - FixHostnames added to modify, inconsistency in results without it.
 
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNSManagement.Extensions;
using System.Management;

namespace DNSManagement.RR
{
    /// <summary>
    /// Represents a Service (SRV) RR.
    /// </summary>
    public class SRVType : ResourceRecord
    {
        private ManagementObject m_mo;

        internal SRVType(ManagementObject mo)
            : base(mo)
        {
            m_mo = mo;
        }

        /// <summary>
        /// Priority of the target host specified in the owner name. Lower numbers imply higher priorities.
        /// </summary>
        public UInt16 Priority
        {
            get
            {
                return Convert.ToUInt16(m_mo["Priority"]);
            }
        }

        /// <summary>
        /// Weight of the target host. This is useful when selecting among hosts that have the same priority. The chances of using this host should be proportional to its weight.
        /// </summary>
        public UInt16 Weight
        {
            get
            {
                return Convert.ToUInt16(m_mo["Weight"]);
            }
        }


        /// <summary>
        /// Port used on the target host of a protocol service.
        /// </summary>
        public UInt16 Port
        {
            get
            {
                return Convert.ToUInt16(m_mo["Port"]);
            }
        }

        /// <summary>
        /// FQDN of the target host. A target of \.\ means that the service is decidedly not available at this domain.
        /// </summary>
        public string SRVDomainName
        {
            get
            {
                return Convert.ToString(m_mo["SRVDomainName"]);
            }
        }



        /// <summary>
        /// Instantiates an SRV Type of RR based on the data in the method's input parameters: the record's DNS Server Name, Container Name, Owner/target Name, class (default = IN), time-to-live value, and target host's priority, weight, port, and domain name. It returns a reference to the new object as an output parameter. 
        /// </summary>
        /// <param name="server">Server object</param>
        /// <param name="dnsServerName">Fully Qualified Domain Name (FQDN) or IP address of the DNS Server that contains this RR.</param>
        /// <param name="containerName">Name of the Container for the Zone, Cache, or RootHints instance which contains this RR.</param>
        /// <param name="ownerName">Owner FQDN for the RR.</param>
        /// <param name="recordClass">Class of the RR.</param>
        /// <param name="ttl">Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="domainName">FQDN of the target host. A target of \.\ means that the service is decidedly not available at this domain.</param>
        /// <param name="port">Port used on the target host of a protocol service.</param>
        /// <param name="priority">Priority of the target host specified in the owner name. Lower numbers imply higher priorities.</param>
        /// <param name="weight">Weight of the target host. This is useful when selecting among hosts that have the same priority. The chances of using this host should be proportional to its weight.</param>
        /// <returns>the new object.</returns>
        public static SRVType CreateInstanceFromPropertyData(
            Server server,
            string dnsServerName,
            string containerName,
            string ownerName,
            RecordClassEnum? recordClass,
            TimeSpan? ttl,
            UInt16 priority, UInt16 weight, UInt16 port,
            string domainName)
        {
            if (server == null)
                throw new ArgumentNullException("server is required");

            ManagementClass dnsClass = new ManagementClass(server.m_scope, new ManagementPath("MicrosoftDNS_SRVType"), null);
            ManagementBaseObject inParams = dnsClass.GetMethodParameters("CreateInstanceFromPropertyData");
            inParams["DnsServerName"] = dnsServerName;
            inParams["ContainerName"] = containerName;
            inParams["OwnerName"] = ownerName;
            if (recordClass != null)
                inParams["RecordClass"] = (UInt32)recordClass.Value;
            if (ttl != null)
                inParams["TTL"] = ttl.Value.TotalSeconds;

            inParams["Priority"] = priority;
            inParams["Weight"] = weight;
            inParams["Port"] = port;
            inParams["SRVDomainName"] = domainName;

            //return new SRVType((ManagementObject)dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null));
            return new SRVType(new ManagementObject(server.m_scope, new ManagementPath(dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null)["RR"].ToString()), null));
        }


        /// <summary>
        /// Updates the TTL, Priority, Weight, Port, and Domain Name to the values specified as the input parameters of this method. If a new value for a parameter is not specified, then the current value for the parameter is not changed. The method returns a reference to the modified object as an output parameter. 
        /// </summary>
        /// <param name="ttl">Optional - Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="domainName">Optional - FQDN of the target host. A target of \.\ means that the service is decidedly not available at this domain.</param>
        /// <param name="port">Optional - Port used on the target host of a protocol service.</param>
        /// <param name="priority">Optional - Priority of the target host specified in the owner name. Lower numbers imply higher priorities.</param>
        /// <param name="weight">Optional - Weight of the target host. This is useful when selecting among hosts that have the same priority. The chances of using this host should be proportional to its weight.</param>
        /// <returns>the modified object.</returns>
        public SRVType Modify(TimeSpan? ttl, UInt16? priority, UInt16? weight, UInt16? port,
            string srvDomainName)
        {
            ManagementBaseObject inParams = m_mo.GetMethodParameters("Modify");
            if ((ttl != null) && (ttl != this.TTL))
                inParams["TTL"] = ttl.Value.TotalSeconds;

            if ((priority != null) && (priority != this.Priority))
                inParams["Priority"] = priority;
            if ((weight != null) && (weight != this.Weight))
                inParams["Weight"] = weight;
            if ((port != null) && (port != this.Port))
                inParams["Port"] = port;
            if ((!string.IsNullOrEmpty(srvDomainName)) && (srvDomainName != this.SRVDomainName))
                inParams["SRVDomainName"] = IPHelper.FixHostnames(srvDomainName);

            try
            {
                return new SRVType(new ManagementObject(m_mo.Scope, new ManagementPath(m_mo.InvokeMethod("Modify", inParams, null)["RR"].ToString()), null));
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLineFormat("DomainName={0}", DomainName);
            sb.AppendLineFormat("Port={0}", Port);
            sb.AppendLineFormat("Priority={0}", Priority);
            sb.AppendLineFormat("Weight={0}", Weight);

            //RR
            base.ToString(sb);

            return sb.ToString();
        }
        
    }
}
