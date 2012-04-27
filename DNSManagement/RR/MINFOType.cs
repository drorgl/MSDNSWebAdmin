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
    /// Represents an Mail Information (MINFO) RR. 
    /// </summary>
    public class MINFOType : ResourceRecord
    {
        private ManagementObject m_mo;

        internal MINFOType(ManagementObject mo)
            : base(mo)
        {
            m_mo = mo;
        }

        /// <summary>
        /// FQDN specifying a mailbox to receive error messages related to either the mailing list, or to the mailbox specified by the owner name of the MINFO record.
        /// </summary>
        public string ErrorMailbox
        {
            get
            {
                return Convert.ToString(m_mo["ErrorMailbox"]);
            }
        }

        /// <summary>
        /// FQDN specifying a mailbox responsible for the mailing list or mailbox specified in the record's Owner Name.
        /// </summary>
        public string ResponsibleMailbox
        {
            get
            {
                return Convert.ToString(m_mo["ResponsibleMailbox"]);
            }
        }

        
        /// <summary>
        /// Instantiates an MINFO Type of RR based on the data in the method's input parameters: the record's DNS Server Name, Container Name, Owner Name of the mail list/box, class (default = IN), time-to-live value and the responsible and error mailboxes. It returns a reference to the new object as an output parameter. 
        /// <para>Note: Must save zone to get real record back, non-standard implementation</para>
        /// </summary>
        /// <param name="server">Server object</param>
        /// <param name="dnsServerName">Fully Qualified Domain Name (FQDN) or IP address of the DNS Server that contains this RR.</param>
        /// <param name="containerName">Name of the Container for the Zone, Cache, or RootHints instance which contains this RR.</param>
        /// <param name="ownerName">Owner FQDN for the RR.</param>
        /// <param name="recordClass">Class of the RR.</param>
        /// <param name="ttl">Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="errorMailbox">FQDN specifying a mailbox responsible for the mailing list or mailbox specified in the record's Owner Name.</param>
        /// <param name="responsibleMailbox">FQDN specifying a mailbox to receive error messages related to either the mailing list, or to the mailbox specified by the owner name of the MINFO record.</param>
        /// <returns>the new object.</returns>
        [Obsolete("Non standard method implementation, read notes")]
        public static MINFOType CreateInstanceFromPropertyData(
            Server server,
            string dnsServerName,
            string containerName,
            string ownerName,
            RecordClassEnum? recordClass,
            TimeSpan? ttl,
            string responsibleMailbox, string errorMailbox)
        {
            if (server == null)
                throw new ArgumentNullException("server is required");

            ManagementClass dnsClass = new ManagementClass(server.m_scope, new ManagementPath("MicrosoftDNS_MINFOType"), null);
            ManagementBaseObject inParams = dnsClass.GetMethodParameters("CreateInstanceFromPropertyData");
            inParams["DnsServerName"] = dnsServerName;
            inParams["ContainerName"] = containerName;
            inParams["OwnerName"] = ownerName;
            if (recordClass != null)
                inParams["RecordClass"] = (UInt32)recordClass.Value;
            if (ttl != null)
                inParams["TTL"] = ttl.Value.TotalSeconds;
            inParams["ResponsibleMailbox"] = responsibleMailbox;
            inParams["ErrorMailbox"] = errorMailbox;

            //return new MINFOType((ManagementObject)dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null));
            return new MINFOType(new ManagementObject(server.m_scope, new ManagementPath(dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null)["RR"].ToString()), null));
        }

       
        /// <summary>
        /// This method updates the TTL, Responsible Mailbox, and Error Mailbox to the values specified as the input parameters of this method. If a new value for a parameter is not specified, then the current value for the parameter is not changed. The method returns a reference to the modified object as an output parameter. 
        /// </summary>
        /// <param name="ttl">Optional - Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="errorMailbox">Optional - FQDN specifying a mailbox to receive error messages related to either the mailing list, or to the mailbox specified by the owner name of the MINFO record.</param>
        /// <param name="responsibleMailbox">Optional - FQDN specifying a mailbox responsible for the mailing list or mailbox specified in the record's Owner Name.</param>
        /// <returns>the modified object.</returns>
        public MINFOType Modify(TimeSpan? ttl, string responsibleMailbox, string errorMailbox)
        {
            ManagementBaseObject inParams = m_mo.GetMethodParameters("Modify");
            if ((ttl != null) && (ttl != this.TTL))
                inParams["TTL"] = ttl.Value.TotalSeconds;
            if ((!string.IsNullOrEmpty(responsibleMailbox)) && (responsibleMailbox != this.ResponsibleMailbox))
                inParams["ResponsibleMailbox"] = IPHelper.FixHostnames(responsibleMailbox);
            if ((!string.IsNullOrEmpty(errorMailbox)) && (errorMailbox != this.ErrorMailbox))
                inParams["ErrorMailbox"] = IPHelper.FixHostnames(errorMailbox);

            try
            {
                return new MINFOType(new ManagementObject(m_mo.Scope, new ManagementPath(m_mo.InvokeMethod("Modify", inParams, null)["RR"].ToString()), null));
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLineFormat("ErrorMailbox={0}", ErrorMailbox);
            sb.AppendLineFormat("ResponsibleMailbox={0}", ResponsibleMailbox);

            //RR
            base.ToString(sb);

            return sb.ToString();
        }
    }
}
