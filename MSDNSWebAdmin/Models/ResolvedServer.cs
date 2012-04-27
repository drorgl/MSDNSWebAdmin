/*
DNS Web Admin - MS DNS Web Administration
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
using System.Web;
using System.Collections.Concurrent;
using MSDNSWebAdmin.AppCode;

namespace MSDNSWebAdmin.Models
{
    /// <summary>
    /// Provides a simple mechanism for IP/Host conversions
    /// </summary>
    public class ResolvedServer
    {
        /// <summary>
        /// IP Address
        /// </summary>
        public string IPAddress { get; set; }
        /// <summary>
        /// FQDN
        /// </summary>
        public string ServerName { get; set; }



        /// <summary>
        /// creates an instance based on IP Address
        /// </summary>
        public static ResolvedServer FromIP(string ipaddr)
        {
            ResolvedServer rs = CacheManager.Get<ResolvedServer>("FromIP", ipaddr);
            if (rs == null)
            {
                rs = new ResolvedServer();
                rs.IPAddress = ipaddr;
                try
                {
                    var host = System.Net.Dns.GetHostEntry(rs.IPAddress);
                    rs.ServerName = host.HostName;
                }
                catch
                {
                    rs.ServerName = "<No such host>";
                }
            }

            CacheManager.Set("FromIP", ipaddr, DateTime.UtcNow.AddMinutes(5), rs);

            return rs;
        }

        /// <summary>
        /// Creates an instance based on FQDN
        /// </summary>
        public static ResolvedServer FromFQDN(string fqdn)
        {
            ResolvedServer rs = new ResolvedServer();
            rs.ServerName = fqdn;

            var host = System.Net.Dns.GetHostEntry(rs.ServerName);
            if (host.AddressList.Length > 0)
                rs.IPAddress = host.AddressList[0].ToString();

            return rs;
        }

        /// <summary>
        /// Creates a collection based on IP Adresses collection
        /// </summary>
        public static ResolvedServer[] FromIPs(string[] ipaddrs)
        {
            List<ResolvedServer> rss = new List<ResolvedServer>();
            if (ipaddrs != null)
                foreach (var ip in ipaddrs)
                    rss.Add(FromIP(ip));
            return rss.ToArray();
        }

        /// <summary>
        /// Creates a collection based on FQDN collection
        /// </summary>
        public static ResolvedServer[] FromFQDNs(string[] fqdns)
        {
            List<ResolvedServer> rsfqdn = new List<ResolvedServer>();
            if (fqdns != null)
                foreach (var fqdn in fqdns)
                    rsfqdn.Add(FromFQDN(fqdn));
            return rsfqdn.ToArray();
        }
    }
}