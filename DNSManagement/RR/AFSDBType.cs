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
    /// Represents an Andrew File System Database Server (AFSDB) RR.
    /// </summary>
    public class AFSDBType : ResourceRecord
    {
        /// <summary>
        /// Subtype of the host AFS server.
        /// </summary>
        public enum SubtypeEnum
        {
            /// <summary>
            /// host has an AFS version 3.0 Volume Location Server for the named AFS cell
            /// </summary>
            Ver3 = 1,
            /// <summary>
            /// host has an authenticated name server holding the cell-root directory node for the named DCE/NCA cell.
            /// </summary>
            AuthNS = 2
        }


        private ManagementObject m_mo;

        internal AFSDBType(ManagementObject mo) : base(mo)
        {
            m_mo = mo;
        }

        /// <summary>
        /// FQDN specifying a host that has a server for the AFS cell specified in the owner name.
        /// </summary>
        public string ServerName
        {
            get
            {
                //Workaround for MS bug.
                return IPHelper.FixHostnames(Convert.ToString(m_mo["ServerName"]));
            }
        }

        /// <summary>
        /// Subtype of the host AFS server.
        /// </summary>
        public SubtypeEnum Subtype
        {
            get
            {
                return (SubtypeEnum)Convert.ToUInt16(m_mo["Subtype"]);
            }
        }


        /// <summary>
        /// Instantiates an AFSDB Resource Record based on the data in the method's
        /// input parameters: the record's DNS Server Name, Container Name, Owner/Cell
        /// Name, class (default = IN), time-to-live value, and host AFS server subtype
        /// and name. Returns a reference to the new object as an output parameter. 
        /// </summary>
        /// <param name="server">Server object</param>
        /// <param name="dnsServerName">FQDN or IP address of the DNS Server that contains this RR.</param>
        /// <param name="containerName">Name of the Container for the Zone, Cache, or RootHints instance which contains this RR.</param>
        /// <param name="ownerName">Owner name for the RR.</param>
        /// <param name="recordClass">Class of the RR.</param>
        /// <param name="ttl">Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="subtype">Subtype of the host AFS server.</param>
        /// <param name="serverName">FQDN specifying a host that has a server for the AFS cell specified in the owner name.</param>
        /// <returns>the new object.</returns>
        public static AFSDBType CreateInstanceFromPropertyData(
            Server server,
            string dnsServerName,
            string containerName,
            string ownerName,
            RecordClassEnum? recordClass,
            TimeSpan? ttl,
            SubtypeEnum subtype,
            string serverName )
        {
            if (server == null)
                throw new ArgumentNullException("server is required");

            ManagementClass dnsClass = new ManagementClass(server.m_scope, new ManagementPath("MicrosoftDNS_AFSDBType"), null);
            ManagementBaseObject inParams = dnsClass.GetMethodParameters("CreateInstanceFromPropertyData");
            inParams["DnsServerName"] = dnsServerName;
            inParams["ContainerName"] = containerName;
            inParams["OwnerName"] = ownerName;
            if (recordClass != null)
                inParams["RecordClass"] = (UInt32)recordClass.Value;
            if (ttl != null)
                inParams["TTL"] = ttl.Value.TotalSeconds;
            inParams["Subtype"] = (UInt16)subtype;
            inParams["ServerName"] = IPHelper.FixHostnames(serverName);

            try
            {
                return new AFSDBType(new ManagementObject(server.m_scope, new ManagementPath(dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null)["RR"].ToString()), null));
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }

        /// <summary>
        /// updates the TTL, Subtype and Server Name to the values specified
        /// as the input parameters of this method. If a new value for a 
        /// parameter is not specified, then the current value for the 
        /// parameter is not changed. The method returns a reference to
        /// the modified object as an output parameter. 
        /// </summary>
        /// <param name="ttl">Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="subType">Subtype of the host AFS server. </param>
        /// <param name="serverName">FQDN specifying a host that has a server for the AFS cell specified in the owner name.</param>
        /// <returns>the modified object.</returns>
        public AFSDBType Modify(TimeSpan? ttl, SubtypeEnum? subType, string serverName)
        {
            ManagementBaseObject inParams = m_mo.GetMethodParameters("Modify");
            if ((ttl != null) && (ttl != this.TTL))
                inParams["TTL"] = ttl.Value.TotalSeconds;
            if ((subType != null) && (subType != this.Subtype))
                inParams["Subtype"] = (UInt16)subType;
            if ((!string.IsNullOrEmpty(serverName)) && (serverName != this.ServerName))
                //workaround for MS bug.
                inParams["ServerName"] = IPHelper.FixHostnames( serverName);

            
            return new AFSDBType(new ManagementObject(m_mo.Scope, new ManagementPath(m_mo.InvokeMethod("Modify", inParams, null)["RR"].ToString()), null));
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLineFormat("ServerName={0}", ServerName);
            sb.AppendLineFormat("Subtype={0}", Subtype);

            //RR
            base.ToString(sb);

            return sb.ToString();
        }

    }
}
