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

namespace DNSManagement
{
    /// <summary>
    /// Describes a cache existing on a DNS Server 
    /// </summary>
    public class Cache : Domain
    {
        private ManagementObject m_mo;

        internal Cache(ManagementObject mo) : base(mo)
        {
            m_mo = mo;
        }

        /// <summary>
        /// Clears the DNS Server cache of resource records. 
        /// </summary>
        /// <param name="server"></param>
        public static void ClearCache(Server server)
        {
            var cache = new ManagementClass(server.m_scope, new ManagementPath("MicrosoftDNS_Cache"), null);
            cache.Get();

            var coll = cache.GetInstances();

            List<Cache> cachelist = new List<Cache>();
            foreach (ManagementObject cacheobj in coll)
                cacheobj.InvokeMethod("ClearCache", null);


        }
    }
}
