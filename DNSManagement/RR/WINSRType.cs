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
    /// Represents a WINS-Reverse (WINSR) RR.
    /// </summary>
    public class WINSRType : ResourceRecord
    {
        /// <summary>
        /// WINSR mapping flag that specifies whether the record must be included into the zone replication. 
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

        internal WINSRType(ManagementObject mo)
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
        /// Time out, in seconds, for a DNS Server using WINS Reverse Look up.
        /// </summary>
        public TimeSpan LookupTimeout
        {
            get
            {
                return TimeSpan.FromSeconds(Convert.ToUInt32(m_mo["LookupTimeout"]));
            }
        }

        /// <summary>
        /// WINSR mapping flag that specifies whether the record must be included into the zone replication. 
        /// </summary>
        public MappingFlagEnum MappingFlag
        {
            get
            {
                return (MappingFlagEnum)Convert.ToUInt32(m_mo["MappingFlag"]);
            }
        }

        /// <summary>
        /// Domain name to append to returned NetBIOS names.
        /// </summary>
        public string ResultDomain
        {
            get
            {
                return Convert.ToString(m_mo["ResultDomain"]);
            }
        }


        /// <summary>
        /// Instantiates a WINSR Type of RR based on the data in the method's input parameters: the record's DNS Server Name, Container Name, Owner Name, class (default = IN), time-to-live value, and WINS mapping flag, reverse look-up time out, WINS cache time out and domain name to append. It returns a reference to the new object as an output parameter. 
        /// </summary>
        /// <param name="server">Server object</param>
        /// <param name="dnsServerName">Fully Qualified Domain Name (FQDN) or IP address of the DNS Server that contains this RR.</param>
        /// <param name="containerName">Name of the Container for the Zone, Cache, or RootHints instance which contains this RR.</param>
        /// <param name="ownerName">Owner FQDN for the RR.</param>
        /// <param name="recordClass">Class of the RR.</param>
        /// <param name="ttl">Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="cacheTimeout">Time, in seconds, a DNS Server using WINS Look up may cache the WINS server's response.</param>
        /// <param name="lookupTimeout">Time out, in seconds, for a DNS Server using WINS Reverse Look up.</param>
        /// <param name="mappingFlag">WINSR mapping flag that specifies whether the record must be included into the zone replication.</param>
        /// <param name="resultDomain">Domain name to append to returned NetBIOS names.</param>
        /// <returns>the new object.</returns>
        [Obsolete("Not supported by WMI, always returns Generic Error")]
        public static WINSRType CreateInstanceFromPropertyData(
            Server server,
            string dnsServerName,
            string containerName,
            string ownerName,
            RecordClassEnum? recordClass,
            TimeSpan? ttl,
            MappingFlagEnum mappingFlag,
            TimeSpan lookupTimeout,
            TimeSpan cacheTimeout,
            string resultDomain
            )
        {
            if (server == null)
                throw new ArgumentNullException("server is required");

            ManagementClass dnsClass = new ManagementClass(server.m_scope, new ManagementPath("MicrosoftDNS_WINSRType"), null);
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
            inParams["ResultDomain"] = resultDomain;


            //return new WINSRType((ManagementObject)dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null));
            try
            {

                return new WINSRType(new ManagementObject(server.m_scope, new ManagementPath(dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null)["RR"].ToString()), null));
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }


        /// <summary>
        /// Updates the TTL, Mapping Flag, Look-up Time out, Cache Time out and Result Domain to the values specified as the input parameters of this method. If a new value for a parameter is not specified, then the current value for the parameter is not changed. The method returns a reference to the modified object as an output parameter. 
        /// </summary>
        /// <param name="ttl">Optional - Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="cacheTimeout">Optional - Time, in seconds, a DNS Server using WINS Look up may cache the WINS server's response.</param>
        /// <param name="lookupTimeout">Optional - Time out, in seconds, for a DNS Server using WINS Reverse Look up.</param>
        /// <param name="mappingFlag">Optional - WINSR mapping flag that specifies whether the record must be included into the zone replication. </param>
        /// <param name="resultDomain">Optional - Domain name to append to returned NetBIOS names.</param>
        /// <returns>the modified object.</returns>
        [Obsolete("Not supported by WMI, always returns Generic Error")]
        public WINSRType Modify(TimeSpan? ttl, MappingFlagEnum? mappingFlag,
            TimeSpan? lookupTimeout,
            TimeSpan? cacheTimeout,
            string resultDomain)
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

            if ((!string.IsNullOrEmpty(resultDomain)) && (resultDomain != this.ResultDomain))
                inParams["ResultDomain"] = resultDomain;

            //return new WINSType((ManagementObject)m_mo.InvokeMethod("Modify", inParams, null));
            try
            {
                return new WINSRType(new ManagementObject(m_mo.Scope, new ManagementPath(m_mo.InvokeMethod("Modify", inParams, null)["RR"].ToString()), null));
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
            sb.AppendLineFormat("ResultDomain={0}", ResultDomain);

            //RR
            base.ToString(sb);

            return sb.ToString();
        }

    }
}
