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

namespace MSDNSWebAdmin.Models
{
    public class DNSServerModel
    {
        public string ServerName { get; set; }
        public string[] ServerIPAddresses { get; set; }
        public string[] ListenAddresses { get; set; }
        public ResolvedServer[] Forwarders { get; set; }
        public bool IsSlave { get; set; }
        public string Version { get; set; }
        public bool NoRecursion { get; set; }
        public bool BindSecondaries { get; set; }
        public bool StrictFileParsing { get; set; }
        public bool RoundRobin { get; set; }
        public bool LocalNetPriority { get; set; }
        public bool SecureResponses { get; set; }
        public DNSManagement.Server.NameCheckFlagEnum NameCheckFlag { get; set; }
        public DNSManagement.Server.BootMethodEnum BootMethod { get; set; }
        public TimeSpan ScavengingInterval { get; set; }
        public ResolvedServer[] RootHints { get; set; }
        public DNSManagement.Server.LogLevelEnum LogLevel { get; set; }
        public string[] LogIPFilterList { get; set; }
        public string LogFilePath { get; set; }
        public uint LogFileMaxSize { get; set; }
        public DNSManagement.Server.EventLogLevelEnum EventLogLevel { get; set; }


        public static DNSServerModel FromServer(DNSManagement.Server server)
        {
            var model = new DNSServerModel();
            model.ServerName = server.Name;
            model.ServerIPAddresses = server.ServerAddresses;
            model.ListenAddresses = server.ListenAddresses;
            model.Forwarders = ResolvedServer.FromIPs(server.Forwarders);
            model.IsSlave = server.IsSlave;
            model.Version = server.Version.ToString();
            model.NoRecursion = server.NoRecursion;
            model.BindSecondaries = server.BindSecondaries;
            model.StrictFileParsing = server.StrictFileParsing;
            model.RoundRobin = server.RoundRobin;
            model.LocalNetPriority = server.LocalNetPriority;
            model.SecureResponses = server.SecureResponses;
            model.NameCheckFlag = server.NameCheckFlag;
            model.BootMethod = server.BootMethod;
            model.ScavengingInterval = server.ScavengingInterval;
            model.LogLevel = server.LogLevel;
            model.LogIPFilterList = server.LogIPFilterList;
            model.LogFilePath = server.LogFilePath;
            model.LogFileMaxSize = server.LogFileMaxSize;
            model.EventLogLevel = server.EventLogLevel;

            //convert root hints to list for display
            List<ResolvedServer> roothintslist = new List<ResolvedServer>();
            var rhints = server.GetRootHints();
            foreach (var rh in rhints)
            {
                var records = rh.GetRecords();
                foreach (var r in records.Where(i => i.DomainName == "..RootHints"))
                {
                    if (r.ResourceRecordType != DNSManagement.RR.ResourceRecord.ResourceRecordEnum.NS)
                        continue;
                    var ns = ((DNSManagement.RR.NSType)r.UnderlyingRecord);
                    var nsa = (records.Where(i=>i.RecordType == typeof(DNSManagement.RR.AType) && ((DNSManagement.RR.AType)i.UnderlyingRecord).OwnerName == ns.NSHost)).Select(i=>((DNSManagement.RR.AType)i).IPAddress).ToList();
                    nsa.AddRange((records.Where(i => i.RecordType == typeof(DNSManagement.RR.AAAAType) && ((DNSManagement.RR.AAAAType)i.UnderlyingRecord).OwnerName == ns.NSHost)).Select(i=>((DNSManagement.RR.AAAAType)i).IPv6Address).ToArray());


                    roothintslist.Add(new ResolvedServer
                    {
                        ServerName = ns.NSHost,
                        IPAddress = string.Join(", ", nsa.ToArray())
                    });
                }
            }
            model.RootHints = roothintslist.ToArray();

            return model;
        }
    }
}