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
    /// Represents a Route Through (RT) RR.
    /// </summary>
    public class RTType : ResourceRecord
    {
        private ManagementObject m_mo;

        internal RTType(ManagementObject mo)
            : base(mo)
        {
            m_mo = mo;
        }

        /// <summary>
        /// FQDN specifying a host to serve as an intermediate in reaching the host specified by owner.
        /// </summary>
        public string IntermediateHost
        {
            get
            {
                return Convert.ToString(m_mo["IntermediateHost"]);
            }
        }

        /// <summary>
        /// Preference given to this RR among others at the same owner. Lower values are preferred.
        /// </summary>
        public UInt16 Preference
        {
            get
            {
                return Convert.ToUInt16(m_mo["Preference"]);
            }
        }

        
        /// <summary>
        /// Instantiates an RT Type of RR based on the data in the method's input parameters: the record's DNS Server Name, Container Name, Owner/host Name, class (default = IN), time-to-live value, record preference and intermediate host name. It returns a reference to the new object as an output parameter. 
        /// </summary>
        /// <param name="server">Server object</param>
        /// <param name="dnsServerName">Fully Qualified Domain Name (FQDN) or IP address of the DNS Server that contains this RR.</param>
        /// <param name="containerName">Name of the Container for the Zone, Cache, or RootHints instance which contains this RR.</param>
        /// <param name="ownerName">Owner FQDN for the RR.</param>
        /// <param name="recordClass">Class of the RR.</param>
        /// <param name="ttl">Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="intermediateHost">FQDN specifying a host to serve as an intermediate in reaching the host specified by owner.</param>
        /// <param name="preference">Preference given to this RR among others at the same owner. Lower values are preferred.</param>
        /// <returns>the new object.</returns>
        public static RTType CreateInstanceFromPropertyData(
            Server server,
            string dnsServerName,
            string containerName,
            string ownerName,
            RecordClassEnum? recordClass,
            TimeSpan? ttl,
            string intermediateHost, UInt16 preference)
        {
            if (server == null)
                throw new ArgumentNullException("server is required");

            ManagementClass dnsClass = new ManagementClass(server.m_scope, new ManagementPath("MicrosoftDNS_RTType"), null);
            ManagementBaseObject inParams = dnsClass.GetMethodParameters("CreateInstanceFromPropertyData");
            inParams["DnsServerName"] = dnsServerName;
            inParams["ContainerName"] = containerName;
            inParams["OwnerName"] = ownerName;
            if (recordClass != null)
                inParams["RecordClass"] = (UInt32)recordClass.Value;
            if (ttl != null)
                inParams["TTL"] = ttl.Value.TotalSeconds;
            inParams["IntermediateHost"] = intermediateHost;
            inParams["Preference"] = preference;


            //return new RTType((ManagementObject)dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null));
            try
            {
                return new RTType(new ManagementObject(server.m_scope, new ManagementPath(dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null)["RR"].ToString()), null));
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }

       
        /// <summary>
        /// Updates the TTL, Preference, and Intermediate Host to the values specified as the input parameters of this method. If a new value for a parameter is not specified, then the current value for the parameter is not changed. The method returns a reference to the modified object as an output parameter. 
        /// </summary>
        /// <param name="ttl">Optional - Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <returns>the modified object.</returns>
        public RTType Modify(TimeSpan? ttl, string intermediateHost, UInt16? preference)
        {
            ManagementBaseObject inParams = m_mo.GetMethodParameters("Modify");
            if ((ttl != null) && (ttl != this.TTL))
                inParams["TTL"] = ttl.Value.TotalSeconds;

            if ((preference != null) && (preference != this.Preference))
                inParams["Preference"] = (UInt16)preference;

            if ((!string.IsNullOrEmpty(intermediateHost)) && (intermediateHost != this.IntermediateHost))
                inParams["IntermediateHost"] = IPHelper.FixHostnames(intermediateHost);

            //return new RTType((ManagementObject)m_mo.InvokeMethod("Modify", inParams, null));
            return new RTType(new ManagementObject(m_mo.Scope, new ManagementPath(m_mo.InvokeMethod("Modify", inParams, null)["RR"].ToString()), null));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLineFormat("IntermediateHost={0}", IntermediateHost);
            sb.AppendLineFormat("Preference={0}", Preference);

            //RR
            base.ToString(sb);

            return sb.ToString();
        }
    }
}
