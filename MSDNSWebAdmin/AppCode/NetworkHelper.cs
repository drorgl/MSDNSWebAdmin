/*
DNS Web Admin - MS DNS Web Administration
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



Change log:
2011-05-17 - Initial version
2012-03-10 - Added ClientIPAddress
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace System.Web.Mvc.Html
{
    public class NetworkHelper
    {
        public static string GetFirstIp(string computerName)
        {
            var address = Dns.GetHostEntry(computerName);

            var first = address.AddressList.FirstOrDefault();

            if (first == null)
                return string.Empty;

            return first.ToString();
        }


        //http://www.chapleau.info/blog/2008/09/09/reverse-dns-lookup-with-timeout-in-c.html
        private delegate IPHostEntry GetHostEntryHandler(string ip);

        /// <summary>
        /// Reverse DNS Lookup
        /// </summary>
        /// <param name="ip">ip address</param>
        /// <param name="timeout">timeout in milliseconds</param>
        /// <returns></returns>
        public static string GetReverseDNS(string ip, int timeout)
        {
            try
            {
                GetHostEntryHandler callback = new GetHostEntryHandler(Dns.GetHostEntry);
                IAsyncResult result = callback.BeginInvoke(ip, null, null);
                if (result.AsyncWaitHandle.WaitOne(timeout, false))
                {
                    return callback.EndInvoke(result).HostName;
                }
                else
                {
                    return ip;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves the Client IP or IPs if the proxy messed things up
        /// <remarks>Taken from http://stackoverflow.com/questions/2577496/how-can-i-get-the-clients-ip-address-in-asp-net-mvc </remarks>
        /// </summary>
        /// <param name="context">HttpContext.Current</param>
        /// <returns>IP or IPs the client connects from</returns>
        public static string ClientIPAddress(HttpContextBase context)
        {
            string remoteAddr = context.Request.ServerVariables["REMOTE_ADDR"];
            string xForwardedFor = context.Request.ServerVariables["X_FORWARDED_FOR"];


            if (xForwardedFor == null)
            {
                return remoteAddr;
            }
            else
            {
                var ipslist = xForwardedFor.Split(',').ToList();
                ipslist.Add(remoteAddr);
                ipslist = ipslist.Distinct().ToList();
                return string.Join(",", ipslist.ToArray());
            }


        }
    }
}