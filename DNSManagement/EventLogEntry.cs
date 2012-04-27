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
    /// EventLogEntry for EventLogging.
    /// <para>Win32_NTLogEvent</para>
    /// </summary>
    public class EventLogEntry
    {
        /// <summary>
        /// Type of event enum
        /// </summary>
        public enum EventTypeEnum
        {
            Error = 1,
            Warning = 2,
            Information = 3,
            SecurityAuditSuccess = 4,
            SecurityAuditFailure = 5
        }


        private ManagementObject m_mo;

        /// <summary>
        /// internal .ctor
        /// </summary>
        /// <param name="mo"></param>
        internal EventLogEntry(ManagementObject mo)
        {
            m_mo = mo;
        }

        /// <summary>
        /// Subcategory for this event. This subcategory is source-specific.
        /// </summary>
        public UInt16 Category
        {
            get
            {
                return Convert.ToUInt16(m_mo["Category"]);
            }
        }

        /// <summary>
        /// Translation of the subcategory. The translation is source-specific.
        /// </summary>
        public string CategoryString
        {
            get
            {
                return Convert.ToString(m_mo["CategoryString"]);
            }
        }

        /// <summary>
        /// Name of the computer that generated this event.
        /// </summary>
        public string ComputerName
        {
            get
            {
                return Convert.ToString(m_mo["ComputerName"]);
            }
        }

        /// <summary>
        /// List of the binary data that accompanied the report of the Windows NT event.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return m_mo["Data"] as byte[];
            }
        }

        /// <summary>
        /// Value of the lower 16-bits of the EventIdentifier property. It is present to match the value displayed in the Windows NT Event Viewer. 
        /// <para>Note: Two events from the same source may have the same value for this property but may have different severity and EventIdentifier values.</para>
        /// </summary>
        public UInt16 EventCode
        {
            get
            {
                return Convert.ToUInt16(m_mo["EventCode"]);
            }
        }

        /// <summary>
        /// Identifier of the event. This is specific to the source that generated the event log entry and is used, together with SourceName, to uniquely identify a Windows NT event type.
        /// </summary>
        public UInt32 EventIdentifier
        {
            get
            {
                return Convert.ToUInt32(m_mo["EventIdentifier"]);
            }
        }

        /// <summary>
        /// Type of event
        /// </summary>
        public EventTypeEnum EventType
        {
            get
            {
                return (EventTypeEnum)Convert.ToUInt16(m_mo["EventType"]);
            }
        }

        /// <summary>
        /// List of the insertion strings that accompanied the report of the Windows NT event.
        /// </summary>
        public string[] InsertionStrings
        {
            get
            {
                return m_mo["InsertionStrings"] as string[];
            }
        }

        /// <summary>
        /// Name of Windows NT event log file. Together with RecordNumber, this is used to uniquely identify an instance of this class.
        /// </summary>
        public string Logfile
        {
            get
            {
                return Convert.ToString(m_mo["Logfile"]);
            }
        }

        /// <summary>
        /// Event message as it appears in the Windows NT event log. This is a standard message with zero or more insertion 
        /// strings supplied by the source of the Windows NT event. The insertion strings are inserted into the standard message
        /// in a predefined format. If there are no insertion strings or there is a problem inserting the insertion strings, only
        /// the standard message will be present in this field.
        /// </summary>
        public string Message
        {
            get
            {
                return Convert.ToString(m_mo["Message"]);
            }
        }

        /// <summary>
        /// Identifies the event within the Windows NT event log file. This is specific to the log file and is used together with the log file name to uniquely identify an instance of this class.
        /// </summary>
        public UInt32 RecordNumber
        {
            get
            {
                return Convert.ToUInt32(m_mo["RecordNumber"]);
            }
        }

        /// <summary>
        /// Name of the source (application, service, driver, or subsystem) that generated the entry. It is used, together with EventIdentifier to uniquely identify a Windows NT event type.
        /// </summary>
        public string SourceName
        {
            get
            {
                return Convert.ToString(m_mo["SourceName"]);
            }
        }

        /// <summary>
        /// The source that generated the event.
        /// </summary>
        public DateTime TimeGenerated
        {
            get
            {
                return ManagementDateTimeConverter.ToDateTime(m_mo["TimeGenerated"] as string);
            }
        }

        /// <summary>
        /// Event was written to the log file.
        /// </summary>
        public DateTime TimeWritten
        {
            get
            {
                return ManagementDateTimeConverter.ToDateTime(m_mo["TimeWritten"] as string);
            }
        }

        /// <summary>
        /// Type of event. This is an enumerated string. It is preferable to use the EventType property rather than the Type property.
        /// </summary>
        public string Type
        {
            get
            {
                return Convert.ToString(m_mo["Type"]);
            }
        }

        /// <summary>
        /// User name of the logged-on user when the event occurred. If the user name cannot be determined, this will be NULL.
        /// </summary>
        public string User
        {
            get
            {
                return Convert.ToString(m_mo["User"]);
            }
        }


    }
}
