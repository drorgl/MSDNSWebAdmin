/*
DNS Web Admin - MS DNS Web Administration - Audit class
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
This class adds a line to the audit trail every time a user performs an action in the application

Change log:
2012-03-10 - Initial version

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Collections.Concurrent;
using System.Data.SqlServerCe;
using System.Threading;

namespace MSDNSWebAdmin.AppCode
{
    /// <summary>
    /// Auditing class
    /// <para>Responsible for adding an audit log and retrieving the log</para>
    /// </summary>
    public class Audit
    {
        /// <summary>
        /// Audit log item types
        /// </summary>
        [Flags]
        public enum AuditTypeEnum
        {
            /// <summary>
            /// No items will be logged
            /// </summary>
            None = 0,
            /// <summary>
            /// Unauthorized attempted access will be logged
            /// </summary>
            UnauthorizedAccess = 1,
            Login = 2,
            View =4,
            Change = 8,
            ViewEventLog = 16,
            NSLookup = 32
        }

        /// <summary>
        /// Audit log media types
        /// </summary>
        [Flags]
        public enum AuditMediaEnum
        {
            None = 0,
            DB = 1,
            File = 2
        }

        /// <summary>
        /// Configured AuditTypes
        /// </summary>
        public static AuditTypeEnum AuditTypes = (AuditTypeEnum)Enum.Parse(typeof(AuditTypeEnum),ConfigurationManager.AppSettings["AuditType"]);

        /// <summary>
        /// Configured Media Types
        /// </summary>
        public static AuditMediaEnum AuditMedia = (AuditMediaEnum)Enum.Parse(typeof(AuditMediaEnum),ConfigurationManager.AppSettings["AuditMedia"]);

        /// <summary>
        /// Configured Maximum records in database
        /// </summary>
        public static int AuditMediaDBMaximumRecords = Convert.ToInt32(ConfigurationManager.AppSettings["AuditMediaDBMaximumRecords"]);

        /// <summary>
        /// log4net formatting
        /// </summary>
        private static string LogLineFormat = "{0} {1} {2} {3} {4} {5} {6}";

        /// <summary>
        /// Audit records queue
        /// </summary>
        private static ConcurrentQueue<AuditRecord> auditrecords = new ConcurrentQueue<AuditRecord>();

        /// <summary>
        /// Background database handler thread
        /// </summary>
        private static Thread tDatabaseHandler = null;

        /// <summary>
        /// Database saving handler for thread
        /// </summary>
        private static void DatabaseHandler()
        {
            DatabaseHandler(true);
        }

        /// <summary>
        /// Database saving handler
        /// </summary>
        /// <param name="repeatedly">should it go indefinatly? just for background thread</param>
        private static void DatabaseHandler(bool repeatedly)
        {
            DateTime lastcheck = DateTime.MinValue;
            while (true)
            {
                //store all audit records
                using (DNSAdminEntities db = new DNSAdminEntities())
                {
                    AuditRecord auditrec = null;
                    while (auditrecords.TryDequeue(out auditrec))
                    {
                        db.AuditRecords.AddObject(auditrec);
                    }
                    db.SaveChanges();
                    db.AcceptAllChanges();
                }


                //check 10 minutes passed
                if ((DateTime.UtcNow - lastcheck).TotalMinutes > 10)
                {
                    lastcheck = DateTime.UtcNow;
                    //remove old records
                    int removeitems = 0;
                    using (DNSAdminEntities db = new DNSAdminEntities())
                    {
                        var deletelist = db.AuditRecords.OrderByDescending(i => i.TimestampUTC).ToList().Skip(AuditMediaDBMaximumRecords).ToList();
                        removeitems = deletelist.Count();
                        foreach (var item in deletelist)
                            db.DeleteObject(item);
                        db.SaveChanges();
                        db.AcceptAllChanges();
                    }
                    //if any records removed, compact database
                    if (removeitems > 0)
                    {
                        using (SqlCeEngine engine = new SqlCeEngine("Data Source = |DataDirectory|\\DNSAdmin.sdf"))
                        {
                            engine.Shrink();
                        }
                    }
                }

                if (repeatedly)
                    Thread.Sleep(5000);
                else
                    break;
            }
        }

        /// <summary>
        /// Log audit/messages
        /// </summary>
        /// <param name="type">type of event</param>
        /// <param name="server">server name</param>
        /// <param name="username">username</param>
        /// <param name="clientips">connected ip</param>
        /// <param name="text">log text</param>
        public static void Log(AuditTypeEnum type, string server, string username, string clientips, string text, string zoneName, string recordName)
        {
            if (!AuditTypes.HasFlag(type))
                return;

            if (AuditMedia.HasFlag(AuditMediaEnum.DB))
            {
                if (auditrecords.Count > 100)
                    throw new Exception("Audit records are not being save quick enough to the database, check server load or check if the database is locked");

                auditrecords.Enqueue(new AuditRecord
                {
                    Id = Guid.NewGuid(),
                    Server = (server != null ) ? server.ToLower() : string.Empty,
                    TimestampUTC = DateTime.UtcNow,
                    Type = type.ToString(),
                    Username = username.ToLower(),
                    ClientIP = clientips,
                    Text = text,
                    Zone = zoneName,
                    RecordName = recordName
                });
             
                //first execution always access the database, easy to see startup errors
                if (tDatabaseHandler == null)
                {
                    DatabaseHandler(false);

                    tDatabaseHandler = new Thread(new ThreadStart(DatabaseHandler));
                    tDatabaseHandler.Start();
                }

            }

            if (AuditMedia.HasFlag(AuditMediaEnum.File))
            {
                var log = log4net.LogManager.GetLogger("MSDNSAdmin");
                log.InfoFormat(LogLineFormat, type.ToString(), (server != null) ? server.ToLower() : string.Empty, username.ToLower(), clientips, text, zoneName, recordName);
            }
        }

        /// <summary>
        /// Retrieves log messages from database
        /// </summary>
        /// <returns></returns>
        public static List<AuditRecord> GetAuditRecords()
        {
            using (DNSAdminEntities db = new DNSAdminEntities())
            {
                return db.AuditRecords.OrderByDescending(i => i.TimestampUTC).ToList();
            }
        }
    }
}