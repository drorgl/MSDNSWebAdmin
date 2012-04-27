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
using DNSManagement.Extensions;
using System.Management;

namespace DNSManagement.RR
{
    /// <summary>
    /// Represents a Mail Exchanger (MX) RR.
    /// </summary>
    public class MXType : ResourceRecord
    {
        private ManagementObject m_mo;

        internal MXType(ManagementObject mo)
            : base(mo)
        {
            m_mo = mo;
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
        /// FQDN specifying a host willing to act as a mail exchange for the owner name.
        /// </summary>
        public string MailExchange
        {
            get
            {
                return Convert.ToString(m_mo["MailExchange"]);
            }
        }

        /// <summary>
        /// Instantiates an MX Type of RR based on the data in the method's input parameters: the record's DNS Server Name, Container Name, Owner Name, class (default = IN), time-to-live value, record preference, and host name willing to be a mail exchange. It returns a reference to the new object as an output parameter. 
        /// <para>Note: Must save zone to get real record back, non-standard implementation</para>
        /// </summary>
        /// <param name="server">Server object</param>
        /// <param name="dnsServerName">FQDN or IP address of the DNS Server that contains this RR.</param>
        /// <param name="containerName">Name of the Container for the Zone, Cache, or RootHints instance that contains this RR.</param>
        /// <param name="ownerName">Owner name for the RR.</param>
        /// <param name="recordClass">Class of the RR.</param>
        /// <param name="ttl">Optional - Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="mailExchange">FQDN specifying a host willing to act as a mail exchange for the owner name.</param>
        /// <param name="preference">Preference given to this RR among others at the same owner. Lower values are preferred.</param>
        /// <returns>the new object.</returns>
        [Obsolete("Non standard method implementation, read notes")]
        public static MXType CreateInstanceFromPropertyData(
           Server server,
           string dnsServerName,
           string containerName,
           string ownerName,
           RecordClassEnum? recordClass,
           TimeSpan? ttl,
            UInt16 preference, string mailExchange)
        {
            if (server == null)
                throw new ArgumentNullException("server is required");

            ManagementClass dnsClass = new ManagementClass(server.m_scope, new ManagementPath("MicrosoftDNS_MXType"), null);
            ManagementBaseObject inParams = dnsClass.GetMethodParameters("CreateInstanceFromPropertyData");
            inParams["DnsServerName"] = dnsServerName;
            inParams["ContainerName"] = containerName;
            inParams["OwnerName"] = ownerName;
            if (recordClass != null)
                inParams["RecordClass"] = (UInt32)recordClass.Value;
            if (ttl != null)
                inParams["TTL"] = ttl.Value.TotalSeconds;
            inParams["Preference"] = preference;
            inParams["MailExchange"] = mailExchange;

            //return new MXType((ManagementObject)dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null));

            //non standard record implementation, workaround needed:
            return new MXType(new ManagementObject(server.m_scope, new ManagementPath(dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null)["RR"].ToString()), null));
        }

        /// <summary>
        /// Updates the TTL, Preference, and Mail Exchange to the values specified as the input parameters of this method. If a new value for a parameter is not specified, then the current value for the parameter is not changed. The method returns a reference to the modified object as an output parameter. 
        /// </summary>
        /// <param name="ttl">Optional - Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="mailExchange">Optional - FQDN specifying a host willing to act as a mail exchange for the owner name.</param>
        /// <param name="preference">Optional - Preference given to this RR among others at the same owner. Lower values are preferred.</param>
        /// <returns>the modified object.</returns>
        public MXType Modify(TimeSpan? ttl, UInt16? preference, string mailExchange)
        {
            ManagementBaseObject inParams = m_mo.GetMethodParameters("Modify");
            if ((ttl != null) && (ttl != this.TTL))
                inParams["TTL"] = ttl.Value.TotalSeconds;

            if ((preference != null) && (preference != this.Preference))
                inParams["Preference"] = (UInt16)preference;

            if ((!string.IsNullOrEmpty(mailExchange)) && (mailExchange != this.MailExchange))
                inParams["MailExchange"] = mailExchange;

            //return new MXType((ManagementObject)m_mo.InvokeMethod("Modify", inParams, null));
            return new MXType(new ManagementObject(m_mo.Scope, new ManagementPath(m_mo.InvokeMethod("Modify", inParams, null)["RR"].ToString()), null));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLineFormat("MailExchange={0}", MailExchange);
            sb.AppendLineFormat("Preference={0}", Preference);

            //RR
            base.ToString(sb);

            return sb.ToString();
        }
    }
}
