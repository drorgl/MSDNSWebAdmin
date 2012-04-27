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
2012-04-09 - Modify is acting up, can't predict when it changes yet, but it does change all properties.
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
    /// Represents a Start Of Authority (SOA) RR.
    /// </summary>
    public class SOAType : ResourceRecord
    {
        private ManagementObject m_mo;

        internal SOAType(ManagementObject mo)
            : base(mo)
        {
            m_mo = mo;
        }

        /// <summary>
        /// Time, in seconds, before an unresponsive zone is no longer authoritative.
        /// </summary>
        public TimeSpan ExpireLimit
        {
            get
            {
                return TimeSpan.FromSeconds(Convert.ToUInt32(m_mo["ExpireLimit"]));
            }
        }


        /// <summary>
        /// Lower limit on the time, in seconds, that a DNS Server or Caching resolver are allowed to cache any resource record from the zone to which this record belongs.
        /// </summary>
        public TimeSpan MinimumTTL
        {
            get
            {
                return TimeSpan.FromSeconds(Convert.ToUInt32(m_mo["MinimumTTL"]));
            }
        }

        /// <summary>
        /// Authoritative DNS Server for the zone to which the record belongs.
        /// </summary>
        public string PrimaryServer
        {
            get
            {
                return Convert.ToString(m_mo["PrimaryServer"]);
            }
        }

        /// <summary>
        /// Time, in seconds, before the zone containing this record should be refreshed.
        /// </summary>
        public TimeSpan RefreshInterval
        {
            get
            {
                return TimeSpan.FromSeconds(Convert.ToUInt32(m_mo["RefreshInterval"]));
            }
        }

        /// <summary>
        /// Name of the responsible party for the zone to which the record belongs.
        /// </summary>
        public string ResponsibleParty
        {
            get
            {
                return Convert.ToString(m_mo["ResponsibleParty"]);
            }
        }

        /// <summary>
        /// Time, in seconds, before retrying a failed refresh of the zone to which this record belongs.
        /// </summary>
        public TimeSpan RetryDelay
        {
            get
            {
                return TimeSpan.FromSeconds(Convert.ToUInt32(m_mo["RetryDelay"]));
            }
        }

        /// <summary>
        /// Serial number of the SOA record.
        /// </summary>
        public UInt32 SerialNumber
        {
            get
            {
                return Convert.ToUInt32(m_mo["SerialNumber"]);
            }
        }

        
        /// <summary>
        /// Updates the TTL, SOA Serial Number, Primary Server, Responsible Party, Refresh Interval, Retry Delay, Expire Limit and Minimum TTL (for the zone) to the values specified as the input parameters of this method. If a new value for a parameter is not specified, then the current value for the parameter is not changed. The method returns a reference to the modified object as an output parameter. 
        /// <para>NOTE: returned record is before the change!!!</para>
        /// </summary>
        /// <param name="ttl">Optional - Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="serialNumber">Optional - SOA serial number representing the number of times the zone has been updated, used by secondary servers to determine whether zone transfer is necessary.</param>
        /// <param name="primaryServer">Optional - Name of the primary server.</param>
        /// <param name="responsibleParty">Optional - Mailbox address of the responsible party, in the form of alias.domain, such as xyz.microsoft.com. Note the use of a period rather than an at symbol (@).</param>
        /// <param name="refreshInterval">Optional - Time, in seconds, before the zone containing this record should be refreshed.</param>
        /// <param name="retryDelay">Optional - Time, in seconds, the DNS Server should delay between name resolution attempts.</param>
        /// <param name="expireLimit">Optional - Time, in seconds, that secondary servers should wait for a response from the primary server before discarding their copies of the zone file as invalid.</param>
        /// <param name="minimumTTL">Optional - TTL time, in seconds, applied to resource records in the zone that do not specify their own TTL.</param>
        /// <returns>Modified Record</returns>
        [Obsolete("Returned record is before the change!!!")]
        public SOAType Modify(TimeSpan? ttl,
             UInt32? serialNumber,
             string primaryServer,
             string responsibleParty,
             TimeSpan? refreshInterval,
             TimeSpan? retryDelay,
             TimeSpan? expireLimit,
             TimeSpan? minimumTTL
            )
        {
            ManagementBaseObject inParams = m_mo.GetMethodParameters("Modify");
            if ((ttl != null) && (ttl != this.TTL))
                inParams["TTL"] = ttl.Value.TotalSeconds;

            if ((serialNumber != null) && (serialNumber != this.SerialNumber))
                inParams["SerialNumber"] = serialNumber;
            if ((!string.IsNullOrEmpty(primaryServer)) && (primaryServer != this.PrimaryServer))
                inParams["PrimaryServer"] = IPHelper.FixHostnames(primaryServer);
            if ((!string.IsNullOrEmpty(responsibleParty)) && (responsibleParty != this.ResponsibleParty))
                inParams["ResponsibleParty"] = responsibleParty;
            if ((refreshInterval != null) && (refreshInterval != this.RefreshInterval))
                inParams["RefreshInterval"] = refreshInterval.Value.TotalSeconds;
            if ((retryDelay != null) && (retryDelay != this.RetryDelay))
                inParams["RetryDelay"] = retryDelay.Value.TotalSeconds;
            if ((expireLimit != null) && (expireLimit != this.ExpireLimit))
                inParams["ExpireLimit"] = expireLimit.Value.TotalSeconds;
            if ((minimumTTL != null) && (minimumTTL != this.MinimumTTL))
                inParams["MinimumTTL"] = minimumTTL.Value.TotalSeconds;

            //return new SOAType((ManagementObject)m_mo.InvokeMethod("Modify", inParams, null));
            return new SOAType(new ManagementObject(m_mo.Scope, new ManagementPath(m_mo.InvokeMethod("Modify", inParams, null)["RR"].ToString()), null));
        }



        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLineFormat("ExpireLimit={0}", ExpireLimit);
            sb.AppendLineFormat("MinimumTTL={0}", MinimumTTL);
            sb.AppendLineFormat("PrimaryServer={0}", PrimaryServer);
            sb.AppendLineFormat("RefreshInterval={0}", RefreshInterval);
            sb.AppendLineFormat("ResponsibleParty={0}", ResponsibleParty);
            sb.AppendLineFormat("RetryDelay={0}", RetryDelay);
            sb.AppendLineFormat("SerialNumber={0}", SerialNumber);


            //RR
            base.ToString(sb);

            return sb.ToString();
        }


    }
}
