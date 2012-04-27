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

*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using DNSManagement.Extensions;

namespace DNSManagement
{
    /// <summary>
    /// Represents a single DNS Server statistic.
    /// </summary>
    public class Statistic
    {
        private ManagementObject m_mo;

        internal Statistic(ManagementObject mo)
        {
            m_mo = mo;
        }

        /// <summary>
        /// Numeric representation of CollectionName.
        /// </summary>
        public UInt32 CollectionId
        {
            get
            {
                return Convert.ToUInt32(m_mo["CollectionId"]);
            }
        }

        /// <summary>
        /// Name of the collection to which the statistic belongs.
        /// </summary>
        public string CollectionName
        {
            get
            {
                return Convert.ToString(m_mo["CollectionName"]);
            }
        }


        /// <summary>
        /// Indicates the FQDN or IP address of the DNS Server that contains the statistic.
        /// </summary>
        public string DnsServerName
        {
            get
            {
                return Convert.ToString(m_mo["DnsServerName"]);
            }
        }

        /// <summary>
        /// Name of the statistic.
        /// </summary>
        public string Name
        {
            get
            {
                return Convert.ToString(m_mo["Name"]);
            }
        }

        /// <summary>
        /// String value of the statistic, used only when the statistic value is not represented as a DWORD.
        /// </summary>
        public string StringValue
        {
            get
            {
                return Convert.ToString(m_mo["StringValue"]);
            }
        }

        /// <summary>
        /// Numeric value of the statistic, represented as a DWORD.
        /// </summary>
        public UInt32 Value
        {
            get
            {
                return Convert.ToUInt32(m_mo["Value"]);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLineFormat("CollectionId={0}", CollectionId);
            sb.AppendLineFormat("CollectionName={0}", CollectionName);
            sb.AppendLineFormat("DnsServerName={0}", DnsServerName);
            sb.AppendLineFormat("Name={0}", Name);
            sb.AppendLineFormat("StringValue={0}", StringValue);
            sb.AppendLineFormat("Value={0}", Value);

            return sb.ToString();
        }
    }
}
