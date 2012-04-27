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
using DNSManagement.RR;

namespace DNSManagement
{
    /// <summary>
    /// Describes the RootHints stored in a cache file on a DNS Server. 
    /// <para>MicrosoftDNS_RootHints</para>
    /// </summary>
    public class RootHints : Domain
    {
        private ManagementObject m_mo;

        internal RootHints(ManagementObject mo) : base(mo)
        {
            m_mo = mo;
        }

        /// <summary>
        /// Writes the RootHints back to the DNS Cache file. 
        /// </summary>
        /// <param name="server">server object to execute this method on</param>
        public void WriteBackRootHintDatafile(Server server)
        {
            m_mo.InvokeMethod("WriteBackRootHintDatafile", null, null);
        }

    }
}
