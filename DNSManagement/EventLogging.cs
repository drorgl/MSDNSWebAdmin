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
using System.Diagnostics;
using System.Management;

namespace DNSManagement
{
    /// <summary>
    /// EventLog helper
    /// </summary>
    public class EventLogging 
    {

        internal ManagementScope m_scope;

        /// <summary>
        /// .ctor 
        /// </summary>
        /// <param name="logName">log name e.g. System/Application/DNS Server</param>
        /// <param name="server">server name</param>
        public EventLogging(string host, string username, string password)
        {
            ConnectionOptions connoptions = new ConnectionOptions();
            connoptions.Username = username;
            connoptions.Password = password;
            connoptions.Impersonation = ImpersonationLevel.Impersonate;

            m_scope = new ManagementScope(@"\\" + host + @"\root\cimv2", connoptions);
            m_scope.Connect();
        }

        /// <summary>
        /// Number of Entries in Event Log
        /// </summary>
        /// <returns></returns>
        public int Count(string logName)
        {
            string query = String.Format("SELECT * FROM Win32_NTLogEvent Where LogFile='{0}'", logName);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(m_scope, new ObjectQuery(query));

            ManagementObjectCollection collection = searcher.Get();
            return collection.Count;
        }

        /// <summary>
        /// Get All EventLog Entries as a list
        /// </summary>
        /// <returns></returns>
        public IList<EventLogEntry> GetAll(string logName)
        {
            string query = String.Format("SELECT * FROM Win32_NTLogEvent Where LogFile='{0}'", logName);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(m_scope, new ObjectQuery(query));

            ManagementObjectCollection collection = searcher.Get();
            
            List<EventLogEntry> records = new List<EventLogEntry>();
            foreach (ManagementObject p in collection)
            {
                records.Add(new EventLogEntry(p));
            }

            return records.ToArray();
        }

        /// <summary>
        /// Get a subset of EventLog entries
        /// </summary>
        /// <param name="startIndex">zero-based start</param>
        /// <param name="length"></param>
        /// <returns></returns>
        public IList<EventLogEntry> Get(string logName,int startIndex, int length)
        {
            string query = String.Format("SELECT * FROM Win32_NTLogEvent Where LogFile='{0}'", logName);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(m_scope, new ObjectQuery(query));

            ManagementObjectCollection collection = searcher.Get();

            int counter = -1;

            List<EventLogEntry> records = new List<EventLogEntry>();
            foreach (ManagementObject p in collection)
            {
                counter++;
                if (startIndex > counter)
                    continue;
                if (counter >= length + startIndex)
                    break;
                records.Add(new EventLogEntry(p));
            }

            return records.ToArray();
        }


    }
}
