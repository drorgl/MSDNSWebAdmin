/*
DNS Web Admin - MS DNS Web Administration - Authorization class
Copyright (C) 2011 Dror Gluska
	
This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.

Description:
This class makes sure you have access to the server requested

Change log:
2012-03-10 - Initial version

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace MSDNSWebAdmin.AppCode
{
    /// <summary>
    /// Checks user is allowed to access requested server
    /// </summary>
    public class Authorization
    {
        
       
       
        /// <summary>
        /// Checks if access should be allowed to this server
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="username"></param>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public static bool IsAllowedServer(string domain, string username, string serverName)
        {
            //Check server is in config or config is empty
            if ((AppSettings.LimitToServers.Length > 0) && (!AppSettings.LimitToServers.Contains(serverName.ToLower())))
                return false;

            //Check server is in allowed list
            using (DNSAdminEntities db = new DNSAdminEntities())
            {
                string user = domain + "\\" + username;
                var servers = db.DNSServers.Where(i => i.Username == user && i.ServerName == serverName);
                if (servers.FirstOrDefault() != null)
                    return true;
            }

            return false;
        }

        
    }
}