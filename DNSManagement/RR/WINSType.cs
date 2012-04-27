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
2012-04-09 - Fix the WinsServers returning junk ("/ )
 
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
    /// Represents a WINS RR.
    /// </summary>
    public class WINSType : ResourceRecord
    {
        /// <summary>
        /// WINS mapping flag that specifies whether the record must be included into the zone replication. It may have only two values: 0x80000000 and 0x00010000 corresponding to the replication and no-replication (local record) flags, respectively.
        /// </summary>
        public enum MappingFlagEnum : uint
        {
            /// <summary>
            /// Replication flag
            /// </summary>
            Replication = 0x80000000,
            /// <summary>
            /// No-replication (local record) flag
            /// </summary>
            NonReplication = 0x00010000
        }

        private ManagementObject m_mo;

        internal WINSType(ManagementObject mo)
            : base(mo)
        {
            m_mo = mo;
        }

        /// <summary>
        /// Time, in seconds, that a DNS Server using WINS Look up may cache the WINS server's response.
        /// </summary>
        public TimeSpan CacheTimeout
        {
            get
            {
                return TimeSpan.FromSeconds(Convert.ToUInt32(m_mo["CacheTimeout"]));
            }
        }

        /// <summary>
        /// Time, in seconds, that a DNS Server attempts resolution using WINS Look up.
        /// </summary>
        public TimeSpan LookupTimeout
        {
            get
            {
                return TimeSpan.FromSeconds(Convert.ToUInt32(m_mo["LookupTimeout"]));
            }
        }

        /// <summary>
        /// WINS mapping flag that specifies whether the record must be included into the zone replication. It may have only two values: 0x80000000 and 0x00010000 corresponding to the replication and no-replication (local record) flags, respectively
        /// </summary>
        public MappingFlagEnum MappingFlag
        {
            get
            {
                return (MappingFlagEnum)Convert.ToUInt32(m_mo["MappingFlag"]);
            }
        }

        /// <summary>
        /// List of comma-separated IP addresses of WINS servers used in WINS Look ups.
        /// </summary>
        public string WinsServers
        {
            get
            {
                return Convert.ToString(m_mo["WinsServers"]).Replace("\"","").Replace(" ","");
            }
        }


        /// <summary>
        /// Instantiates a WINS Type of RR based on the data in the method's input parameters: the record's DNS Server Name, Container Name, Owner Name, class (default = IN), time-to-live value, and WINS mapping flag, look-up time out, cache time out and list of IP addresses for look up. It returns a reference to the new object as an output parameter. 
        /// </summary>
        /// <param name="server">Server object</param>
        /// <param name="dnsServerName">Fully Qualified Domain Name (FQDN) or IP address of the DNS Server that contains this RR.</param>
        /// <param name="containerName">Name of the Container for the Zone, Cache, or RootHints instance which contains this RR.</param>
        /// <param name="ownerName">Owner FQDN for the RR.</param>
        /// <param name="recordClass">Class of the RR.</param>
        /// <param name="ttl">Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="cacheTimeout">Time, in seconds, that a DNS Server using WINS Look up may cache the WINS server's response.</param>
        /// <param name="lookupTimeout">Time, in seconds, that a DNS Server attempts resolution using WINS Look up.</param>
        /// <param name="mappingFlag">WINS mapping flag that specifies whether the record must be included into the zone replication</param>
        /// <param name="winsServers">List of comma-separated IP addresses of WINS servers used in WINS Look ups.</param>
        /// <returns>the new object.</returns>
        public static WINSType CreateInstanceFromPropertyData(
            Server server,
            string dnsServerName,
            string containerName,
            string ownerName,
            RecordClassEnum? recordClass,
            TimeSpan? ttl,
            MappingFlagEnum mappingFlag,
            TimeSpan lookupTimeout,
            TimeSpan cacheTimeout,
            string winsServers
            )
        {
            if (server == null)
                throw new ArgumentNullException("server is required");

            ManagementClass dnsClass = new ManagementClass(server.m_scope, new ManagementPath("MicrosoftDNS_WINSType"), null);
            ManagementBaseObject inParams = dnsClass.GetMethodParameters("CreateInstanceFromPropertyData");
            inParams["DnsServerName"] = dnsServerName;
            inParams["ContainerName"] = containerName;
            inParams["OwnerName"] = ownerName;
            if (recordClass != null)
                inParams["RecordClass"] = (UInt32)recordClass.Value;
            if (ttl != null)
                inParams["TTL"] = ttl.Value.TotalSeconds;
            inParams["MappingFlag"] = (UInt32)mappingFlag;
            inParams["LookupTimeout"] = lookupTimeout.TotalSeconds;
            inParams["CacheTimeout"] = cacheTimeout.TotalSeconds;
            inParams["WinsServers"] = winsServers;

            try
            {
                return new WINSType(new ManagementObject(server.m_scope, new ManagementPath(dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null)["RR"].ToString()), null));
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }


        /// <summary>
        /// Updates the TTL, Mapping Flag, Look-up Time out, Cache Time out and Wins Servers to the values specified as the input parameters of this method. If a new value for a parameter is not specified, then the current value for the parameter is not changed. The method returns a reference to the modified object as an output parameter. 
        /// </summary>
        /// <param name="ttl">Optional - Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="cacheTimeout">Optional - Time, in seconds, that a DNS Server using WINS Look up may cache the WINS server's response.</param>
        /// <param name="lookupTimeout">Optional - Time, in seconds, that a DNS Server attempts resolution using WINS Look up.</param>
        /// <param name="mappingFlag">Optional - WINS mapping flag that specifies whether the record must be included into the zone replication</param>
        /// <param name="winsServers">Optional - List of comma-separated IP addresses of WINS servers used in WINS Look ups.</param>
        /// <returns>the modified object.</returns>
        [Obsolete("Method is not behaving as expected")]
        public WINSType Modify(TimeSpan? ttl, MappingFlagEnum? mappingFlag,
            TimeSpan? lookupTimeout,
            TimeSpan? cacheTimeout,
            string winsServers)
        {
            ManagementBaseObject inParams = m_mo.GetMethodParameters("Modify");
            if ((ttl != null) && (ttl != this.TTL))
                inParams["TTL"] = ttl.Value.TotalSeconds;

            if ((mappingFlag != null) && (mappingFlag != this.MappingFlag))
                inParams["MappingFlag"] = (UInt32)mappingFlag;
            if ((lookupTimeout != null) && (lookupTimeout != this.LookupTimeout))
                inParams["LookupTimeout"] = lookupTimeout.Value.TotalSeconds;
            if ((cacheTimeout != null) && (cacheTimeout != this.CacheTimeout))
                inParams["CacheTimeout"] = cacheTimeout.Value.TotalSeconds;

            if ((!string.IsNullOrEmpty(winsServers)) && (winsServers != this.WinsServers))
                inParams["WinsServers"] = winsServers;

            try
            {
                return new WINSType(new ManagementObject(m_mo.Scope, new ManagementPath(m_mo.InvokeMethod("Modify", inParams, null)["RR"].ToString()), null));
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLineFormat("CacheTimeout={0}", CacheTimeout);
            sb.AppendLineFormat("LookupTimeout={0}", LookupTimeout);
            sb.AppendLineFormat("MappingFlag={0}", MappingFlag);
            sb.AppendLineFormat("WinsServers={0}", WinsServers);

            //RR
            base.ToString(sb);

            return sb.ToString();
        }



    }
}
