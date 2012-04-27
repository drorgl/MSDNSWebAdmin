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
2012-03-17 - Discovered WMI doesn't fully support ATMA, possible workaround, if important, read http://www.broadband-forum.org/technical/atmtechspec.php

*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using DNSManagement.Extensions;

/*
namespace DNSManagement.RR
{
    /// <summary>
    /// Represents an ATM Address-to-Name (ATMA) RR. 
    /// </summary>
    public class ATMAType : ResourceRecord
    {
        /// <summary>
        /// ATM address format.
        /// </summary>
        public enum AddressFormatEnum
        {
            /// <summary>
            /// ATM End System Address (AESA) format
            /// </summary>
            AESA = 0,
            /// <summary>
            /// E.164 format.
            /// </summary>
            E164 = 1
        }

        private ManagementObject m_mo;

        internal ATMAType(ManagementObject mo)
            : base(mo)
        {
            m_mo = mo;
        }

        /// <summary>
        /// Variable length string of octets containing the ATM address 
        /// of the node/owner to which this RR pertains. The first 4
        /// bytes of the array are used to store the size of the octet
        /// string. The most significant byte is stored in byte 0.
        /// </summary>
        public string ATMAddress
        {
            get
            {
                return Convert.ToString(m_mo["ATMAddress"]);
            }
        }

        /// <summary>
        /// ATM address format.
        /// </summary>
        public AddressFormatEnum Format
        {
            get
            {
                return (AddressFormatEnum)Convert.ToUInt16(m_mo["Format"]);
            }
        }


        /// <summary>
        /// Instantiates an ATMA Resource Record based on the data in the method's input
        /// parameters: the record's DNS Server Name, Container Name, Owner/Node Name, 
        /// class (default = IN), time-to-live value, and ATM format and address. 
        /// Returns a reference to the new object as an output parameter. 
        /// </summary>
        /// <param name="server">Server object</param>
        /// <param name="dnsServerName">FQDN or IP address of the DNS Server that contains this RR.</param>
        /// <param name="containerName">Name of the Container for the Zone, Cache, or RootHints instance which contains this RR.</param>
        /// <param name="ownerName">Owner name for the RR.</param>
        /// <param name="recordClass">Class of the RR. </param>
        /// <param name="ttl">Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="format">ATM address format. </param>
        /// <param name="atmaaddress">Variable length string of octets containing the ATM address of the node/owner to which this RR pertains. The first four bytes of the array are used to store the size of the octet string. The most significant byte is stored in byte 0.</param>
        /// <returns>the new object.</returns>
        public static ATMAType CreateInstanceFromPropertyData(
             Server server,
             string dnsServerName,
             string containerName,
             string ownerName,
             RecordClassEnum? recordClass,
             TimeSpan? ttl,
             AddressFormatEnum format,
             string atmaaddress)
        {
            if (server == null)
                throw new ArgumentNullException("server is required");

            ManagementClass dnsClass = new ManagementClass(server.m_scope, new ManagementPath("MicrosoftDNS_ATMAType"), null);
            ManagementBaseObject inParams = dnsClass.GetMethodParameters("CreateInstanceFromPropertyData");
            inParams["DnsServerName"] = dnsServerName;
            inParams["ContainerName"] = containerName;
            inParams["OwnerName"] = ownerName;
            if (recordClass != null)
                inParams["RecordClass"] = (UInt32)recordClass.Value;
            if (ttl != null)
                inParams["TTL"] = ttl.Value.TotalSeconds;
            inParams["Format"] = (UInt16)format;
            inParams["ATMAddress"] = atmaaddress;

            //return new ATMAType((ManagementObject)dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null));
            return new ATMAType(new ManagementObject(server.m_scope, new ManagementPath(dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null)["RR"].ToString()), null));
        }

        /// <summary>
        /// Updates the TTL, Format and ATMA Address to the values specified as the
        /// input parameters of this method. If a new value for a parameter is not
        /// specified, the current value for the parameter is not changed. The method 
        /// returns a reference to the modified object as an output parameter. 
        /// </summary>
        /// <param name="ttl">Optional - Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="format">Optional - ATM address format.</param>
        /// <param name="atmaaddress">Optional - Variable length string of octets containing the ATM address of the node/owner to which this RR pertains. The first four bytes of the array are used to store the size of the octet string. The most significant byte is stored in byte 0.</param>
        /// <returns>the new object.</returns>
        public ATMAType Modify(TimeSpan? ttl, AddressFormatEnum? format, string atmaaddress)
        {
            ManagementBaseObject inParams = m_mo.GetMethodParameters("Modify");
            if ((ttl != null) && (ttl != this.TTL))
                inParams["TTL"] = ttl.Value.TotalSeconds;
            if ((format != null) && (format != this.Format))
                inParams["Format"] = (UInt16)format;
            if ((!string.IsNullOrEmpty(atmaaddress)) && (atmaaddress != this.ATMAddress))
                inParams["ATMAddress"] = atmaaddress;

            //return new ATMAType((ManagementObject)m_mo.InvokeMethod("Modify", inParams, null));
            return new ATMAType(new ManagementObject(m_mo.Scope, new ManagementPath(m_mo.InvokeMethod("Modify", inParams, null)["RR"].ToString()), null));
        }



        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLineFormat("ATMAddress={0}", ATMAddress);
            sb.AppendLineFormat("Format={0}", Format);

            //RR
            base.ToString(sb);

            return sb.ToString();
        }

    }
}
*/