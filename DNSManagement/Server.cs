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
    /// MS DNS Server
    /// </summary>
    public class Server : Backupable, IDisposable
    {
        /// <summary>
        /// Specifies whether the DNS Server accepts dynamic update requests. Valid values are as shown in the following table. 
        /// </summary>
        [Flags]
        public enum AllowUpdateEnum
        {
            /// <summary>
            /// No Restrictions.
            /// </summary>
            NoRestrictions = 0,
            /// <summary>
            /// Does not allow dynamic updates of SOA records.
            /// </summary>
            NoDynamicUpdateOfSOA = 1,
            /// <summary>
            /// Does not allow dynamic updates of NS records at the zone root.
            /// </summary>
            NoDynamicUpdateOfNSRootOnly = 2,
            /// <summary>
            /// Does not allow dynamic updates of NS records not at the zone root (delegation NS records).
            /// </summary>
            NoDynamicUpdateOfNS = 4

        }

        /// <summary>
        /// Indicates which standard primary zones that are authoritative 
        /// for the name of the DNS Server must be updated when the name 
        /// server changes. Valid values are as follows: 
        /// </summary>
        [Flags]
        public enum AutoConfigFileZonesEnum
        {
            /// <summary>
            /// None
            /// </summary>
            None = 0,
            /// <summary>
            /// Only servers that allow dynamic updates.
            /// </summary>
            OnlyServersAllowingDynamicUpdates = 1,
            /// <summary>
            /// Only servers that do not allow dynamic updates.
            /// </summary>
            OnlyServersDontAllowDynamicUpdates = 2,
            /// <summary>
            /// All servers.
            /// </summary>
            AllServers = 4
        }

        /// <summary>
        /// Initialization method for the DNS Server. Valid values are shown in the following table. 
        /// </summary>
        public enum BootMethodEnum
        {
            /// <summary>
            /// Uninitialized
            /// </summary>
            Uninitialized = 0,
            /// <summary>
            /// Boot from file.
            /// </summary>
            File = 1,
            /// <summary>
            /// Boot from registry.
            /// </summary>
            Registry = 2,
            /// <summary>
            /// Boot from directory and registry.
            /// </summary>
            DirectoryAndRegisty = 3
        }

        /// <summary>
        /// Specifies whether the DNS Server includes DNSSEC-specific RRs, KEY, SIG, and 
        /// NXT in a response
        /// </summary>
        public enum EnableDnsSecEnum
        {
            /// <summary>
            /// No DNSSEC records are included in the response unless the query 
            /// requested a resource record set of the DNSSEC record type.
            /// </summary>
            NoDNSSEC = 0,
            /// <summary>
            /// DNSSEC records are included in the response according to RFC 2535.
            /// </summary>
            IncludedRFC2535 = 1,
            /// <summary>
            /// DNSSEC records are included in a response only if the original client 
            /// query contained the OPT resource record according to RFC 2671
            /// </summary>
            IncludedRFC2671 = 2
        }

        /// <summary>
        /// Indicates which events the DNS Server records in the Event Viewer system log. 
        /// </summary>
        [Flags]
        public enum EventLogLevelEnum
        {
            /// <summary>
            /// None
            /// </summary>
            None = 0,
            /// <summary>
            /// Log only errors.
            /// </summary>
            ErrorsOnly = 1,
            /// <summary>
            /// Log only warnings and errors.
            /// </summary>
            WarningsAndErrors = 2,
            /// <summary>
            /// Log all events.
            /// </summary>
            All = 4
        }

        /// <summary>
        /// Indicates which policies are activated in the Event Viewer system log. 
        /// </summary>
        [Flags]
        public enum LogLevelEnum : uint
        {
            Query = 1,
            Notify = 16,
            Update = 32,
            NonqueryTransactions = 254,
            Questions = 256,
            Answers = 512,
            Send = 4096,
            Receive = 8192,
            UDP = 16384,
            TCP = 32768,
            AllPackets = 65535,
            NTDSWrite = 65536,
            NTDSUpdate = 131072,
            FullPackets = 16777216,
            Unmatched = 33554432,
            WriteThrough = 2147483648,
            
        }

        /// <summary>
        /// Indicates the set of eligible characters to be used in DNS names. 
        /// </summary>
        public enum NameCheckFlagEnum : uint
        {
            StrictRFCANSI = 0,
            NonRFCANSI = 1,
            MultibyteUTF8 = 2,
            AllNames = 3
        }

        /// <summary>
        /// RPC protocol or protocols over which administrative RPC runs. 
        /// </summary>
        [Flags]
        public enum RpcProtocolEnum
        {
            None = 0,
            TCP = 1,
            NamedPipes = 2,
            LPC = 4
        }

        /// <summary>
        /// Flag that restricts the type of records that can be updated via
        /// DDNS. Used in conjunction with AllowUpdate. The value of 
        /// this property can be any sum of the following values:
        /// <remarks>Source - http://fengnet.com/book/DNS.on.Windows.Server.2003/0596005628_dnswinsvr-chp-14-sect-3.html </remarks>
        /// </summary>
        [Flags]
        public enum UpdateOptionsEnum : uint
        {
            /// <summary>
            /// No restrictions.
            /// </summary>
            NoRestrictions = 0,
            /// <summary>
            /// Exclude SOA records.
            /// </summary>
            ExcludeSOARecords = 1,
            /// <summary>
            /// Exclude NS records.
            /// </summary>
            ExcludeNSRecords = 2,
            /// <summary>
            /// Exclude delegation NS records.
            /// </summary>
            ExcludeDelegationNSRecords = 4,
            /// <summary>
            /// Exclude server host records.
            /// </summary>
            ExcludeServerHostRecords = 8,
            /// <summary>
            /// Exclude SOA records for secure dynamic updates.
            /// </summary>
            ExcludeSOARecordsForSecureDynamicUpdates = 256,
            /// <summary>
            /// Exclude root NS records for secure dynamic updates.
            /// </summary>
            ExcludeRootNSRecordsForSecureDynamicUpdates = 512,
            /// <summary>
            /// On standard dynamic updates, exclude NS, SOA, and server 
            /// host records for dynamic updates, and for secure dynamic 
            /// updates, exclude root NS and SOA 
            /// records. Allows delegations and server host updates.
            /// </summary>
            ExcludeNSSOAServerHostRecords = 783,
            /// <summary>
            /// On secure dynamic updates, exclude delegation NS records.
            /// </summary>
            ExcludeDelegationNSRecordsForSecureDynamicUpdates = 1024,
            /// <summary>
            /// Exclude server host records for secure dynamic updates.
            /// </summary>
            ExcludeServerHostRecordsForSecureDynamicUpdates = 2048,
            /// <summary>
            /// Exclude DS records.
            /// </summary>
            ExcludeDSRecords = 16777216,
            /// <summary>
            /// Disable dynamic update.
            /// </summary>
            DisableDynamicUpdate = 2147483648
        }


        internal ManagementScope m_scope;

        private ManagementClass m_serverClass;
        private ManagementObject m_serverObject;

        /// <summary>
        /// Server - local
        /// </summary>
        public Server()
        {
            m_scope = new ManagementScope(@"root\microsoftdns");
            m_scope.Connect();
        }

        /// <summary>
        /// Server - host/username/password
        /// </summary>
        /// <param name="host"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public Server(string host, string username, string password)
        {
            ConnectionOptions connoptions = new ConnectionOptions();
            connoptions.Username = username;
            connoptions.Password = password;
            connoptions.Impersonation = ImpersonationLevel.Impersonate;

            m_scope = new ManagementScope(@"\\" + host + @"\root\microsoftdns",connoptions);
            m_scope.Connect();
        }

        /// <summary>
        /// Loads the server class
        /// </summary>
        private void loadServerClass()
        {
            if (m_serverClass != null)
                return;

            m_serverClass = new ManagementClass(m_scope, new ManagementPath("MicrosoftDNS_Server"), null);
            m_serverClass.Get();
            var coll = m_serverClass.GetInstances();
            foreach (ManagementObject o in coll)
            {
                m_serverObject = o;
                break;
            }
        }

        /// <summary>
        /// Maximum number of host records returned in response to an address request. Values between 5 and 28 are valid.
        /// </summary>
        public int AddressAnswerLimit
        {
            get
            {
                loadServerClass();
                return Convert.ToInt32(m_serverObject["AddressAnswerLimit"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["AddressAnswerLimit"] = value;
            }
        }

        /// <summary>
        /// Specifies whether the DNS Server accepts dynamic update requests. Valid values are as shown in the following table. 
        /// </summary>
        public AllowUpdateEnum AllowUpdate
        {
            get
            {
                loadServerClass();
                return (AllowUpdateEnum)Convert.ToInt32(m_serverObject["AllowUpdate"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["AllowUpdate"] = value;
            }
        }

        /// <summary>
        /// Indicates whether the DNS Server attempts to update its cache 
        /// entries using data from root servers. When a DNS Server 
        /// boots, it needs a list of root server 'hints'—NS and A records 
        /// for the servers—historically called the cache file. 
        /// Microsoft DNS Servers have a feature that enables them 
        /// to attempt to write back a new cache file based on the 
        /// responses from root servers.
        /// </summary>
        public bool AutoCacheUpdate
        {
            get
            {
                loadServerClass();
                return Convert.ToBoolean(m_serverObject["AutoCacheUpdate"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["AutoCacheUpdate"] = value;
            }
        }

        /// <summary>
        /// Indicates which standard primary zones that are authoritative for 
        /// the name of the DNS Server must be updated when the name server 
        /// changes. Valid values are as follows: 
        /// </summary>
        public AutoConfigFileZonesEnum AutoConfigFileZones
        {
            get
            {
                loadServerClass();
                return (AutoConfigFileZonesEnum)Convert.ToInt32(m_serverObject["AutoConfigFileZones"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["AutoConfigFileZones"] = value;
            }
        }

        /// <summary>
        /// Determines the AXFR message format when sending to non-Microsoft DNS Server 
        /// secondaries. When set to TRUE, the DNS Server sends transfers to 
        /// non-Microsoft DNS Server secondaries in the uncompressed format. 
        /// When FALSE, all transfers are sent in the fast format.
        /// </summary>
        public bool BindSecondaries
        {
            get
            {
                loadServerClass();
                return Convert.ToBoolean(m_serverObject["BindSecondaries"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["BindSecondaries"] = value;
            }
        }

        /// <summary>
        /// Initialization method for the DNS Server.
        /// </summary>
        public BootMethodEnum BootMethod
        {
            get
            {
                loadServerClass();
                return (BootMethodEnum)Convert.ToInt32(m_serverObject["BootMethod"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["BootMethod"] = value;
            }
        }

        /// <summary>
        /// Default ScavengingInterval value set for all Active Directory-integrated 
        /// zones created on this DNS Server. The default value is zero, indicating 
        /// scavenging is disabled.
        /// </summary>
        public bool DefaultAgingState
        {
            get
            {
                loadServerClass();
                return Convert.ToBoolean(m_serverObject["DefaultAgingState"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["DefaultAgingState"] = value;
            }
        }

        /// <summary>
        /// No-refresh interval, in hours, set for all Active Directory-integrated 
        /// zones created on this DNS Server. The default value is 168 hours (seven days).
        /// </summary>
        public TimeSpan DefaultNoRefreshInterval
        {
            get
            {
                loadServerClass();
                return TimeSpan.FromHours( Convert.ToInt32(m_serverObject["DefaultNoRefreshInterval"]));
            }
            set
            {
                loadServerClass();
                m_serverObject["DefaultNoRefreshInterval"] = value.TotalHours;
            }
        }

        /// <summary>
        /// Indicates whether the DNS Server automatically creates standard reverse look up zones.
        /// </summary>
        public bool DisableAutoReverseZones
        {
            get
            {
                loadServerClass();
                return Convert.ToBoolean(m_serverObject["DisableAutoReverseZones"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["DisableAutoReverseZones"] = value;
            }
        }

        /// <summary>
        /// Indicates whether the default port binding for a socket used to send 
        /// queries to remote DNS Servers can be overridden.
        /// </summary>
        public bool DisjointNets
        {
            get
            {
                loadServerClass();
                return Convert.ToBoolean(m_serverObject["DisjointNets"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["DisjointNets"] = value;
            }
        }


        /// <summary>
        /// Indicates whether there is an available DS on the DNS Server.
        /// </summary>
        public bool DsAvailable
        {
            get
            {
                loadServerClass();
                return Convert.ToBoolean(m_serverObject["DsAvailable"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["DsAvailable"] = value;
            }
        }

        /// <summary>
        /// Interval, in seconds, to poll the DS-integrated zones.
        /// </summary>
        public TimeSpan DsPollingInterval
        {
            get
            {
                loadServerClass();
                return TimeSpan.FromSeconds(Convert.ToInt32(m_serverObject["DsPollingInterval"]));
            }
            set
            {
                loadServerClass();
                m_serverObject["DsPollingInterval"] = value.TotalSeconds;
            }
        }

        /// <summary>
        /// Lifetime of tombstoned records in Directory Service integrated zones, expressed in seconds.
        /// </summary>
        public TimeSpan DsTombstoneInterval
        {
            get
            {
                loadServerClass();
                return TimeSpan.FromSeconds( Convert.ToUInt32(m_serverObject["DsTombstoneInterval"]));
            }
            set
            {
                loadServerClass();
                m_serverObject["DsTombstoneInterval"] = value.TotalSeconds ;
            }
        }

        /// <summary>
        /// Lifetime, in seconds, of the cached information describing the EDNS 
        /// version supported by other DNS Servers.
        /// </summary>
        public TimeSpan EDnsCacheTimeout
        {
            get
            {
                loadServerClass();
                return TimeSpan.FromSeconds( Convert.ToUInt32(m_serverObject["EDnsCacheTimeout"]));
            }
            set
            {
                loadServerClass();
                m_serverObject["EDnsCacheTimeout"] = value.TotalSeconds;
            }
        }

        /// <summary>
        /// Specifies whether support for application directory partitions is 
        /// enabled on the DNS Server.
        /// </summary>
        public bool EnableDirectoryPartitions
        {
            get
            {
                loadServerClass();
                return Convert.ToBoolean(m_serverObject["EnableDirectoryPartitions"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["EnableDirectoryPartitions"] = value;
            }
        }

        /// <summary>
        /// Specifies whether the DNS Server includes DNSSEC-specific RRs, KEY, SIG, and 
        /// NXT in a response
        /// </summary>
        public EnableDnsSecEnum EnableDnsSec
        {
            get
            {
                loadServerClass();
                return (EnableDnsSecEnum)Convert.ToUInt32(m_serverObject["EnableDnsSec"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["EnableDnsSec"] = value;
            }
        }

        /// <summary>
        /// Specifies the behavior of the DNS Server. When TRUE, the DNS Server 
        /// always responds with OPT resource records according to RFC 2671, unless 
        /// the remote server has indicated it does not support EDNS in a prior 
        /// exchange. If FALSE, the DNS Server responds to queries with OPTs only 
        /// if OPTs are sent in the original query.
        /// </summary>
        public bool EnableEDnsProbes
        {
            get
            {
                loadServerClass();
                return Convert.ToBoolean(m_serverObject["EnableEDnsProbes"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["EnableEDnsProbes"] = value;
            }
        }

        /// <summary>
        /// Indicates which events the DNS Server records in the Event Viewer system log.
        /// </summary>
        public EventLogLevelEnum EventLogLevel
        {
            get
            {
                loadServerClass();
                return (EventLogLevelEnum)Convert.ToUInt32(m_serverObject["EventLogLevel"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["EventLogLevel"] = value;
            }
        }

        /// <summary>
        /// Determines whether queries for data in delegated subzones are sent to forwarders or follow the delegation.
        /// <para>False - Automatically send queries referring to delegated subzones to the appropriate subzone.</para>
        /// <para>True - Forward queries referring to the delegated subzone to the existing forwarders.</para>
        /// <remarks>Source - http://fengnet.com/book/DNS.on.Windows.Server.2003/0596005628_dnswinsvr-chp-14-sect-3.html </remarks>
        /// </summary>
        public bool ForwardDelegations
        {
            get
            {
                loadServerClass();
                return Convert.ToBoolean(m_serverObject["ForwardDelegations"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["ForwardDelegations"] = value;
            }
        }

        /// <summary>
        /// Enumerates the list of IP addresses of Forwarders to which the DNS 
        /// Server forwards queries.
        /// </summary>
        public string[] Forwarders
        {
            get
            {
                loadServerClass();
                return m_serverObject["Forwarders"] as string[];
            }
            set
            {
                loadServerClass();
                m_serverObject["Forwarders"] = value;
            }
        }

        /// <summary>
        /// Time, in seconds, a DNS Server forwarding a query will wait for 
        /// resolution from the forwarder before attempting to resolve the query itself.
        /// <para>This value is meaningless if the forwarding server is not 
        /// set to use recursion. To determine this, check the IsSlave 
        /// Boolean property.</para>
        /// </summary>
        public TimeSpan ForwardingTimeout
        {
            get
            {
                loadServerClass();
                return TimeSpan.FromSeconds( Convert.ToUInt32(m_serverObject["ForwardingTimeout"]));
            }
            set
            {
                loadServerClass();
                m_serverObject["ForwardingTimeout"] = (UInt32) value.TotalSeconds;
            }

        }

        /// <summary>
        /// TRUE if the DNS server does not use recursion when name-resolution 
        /// through forwarders fails.
        /// </summary>
        public bool IsSlave
        {
            get
            {
                loadServerClass();
                return Convert.ToBoolean(m_serverObject["IsSlave"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["IsSlave"] = value;
            }
        }

        /// <summary>
        /// Enumerates the list of IP addresses on which the DNS Server can receive queries.
        /// </summary>
        public string[] ListenAddresses
        {
            get
            {
                loadServerClass();
                return m_serverObject["ListenAddresses"] as string[];
            }
            set
            {
                loadServerClass();
                m_serverObject["ListenAddresses"] = value;
            }
        }

        /// <summary>
        /// Indicates whether the DNS Server gives priority to the local net address 
        /// when returning A records.
        /// </summary>
        public bool LocalNetPriority
        {
            get
            {
                loadServerClass();
                return Convert.ToBoolean(m_serverObject["LocalNetPriority"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["LocalNetPriority"] = value;
            }
        }

        /// <summary>
        /// Size of the DNS Server debug log, in bytes.
        /// </summary>
        public UInt32 LogFileMaxSize
        {
            get
            {
                loadServerClass();
                return Convert.ToUInt32(m_serverObject["LogFileMaxSize"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["LogFileMaxSize"] = value;
            }
        }

        /// <summary>
        /// File name and path for the DNS Server debug log. Default is %system32%\dns\dns.log. 
        /// Relative paths are relative to %Systemroot%\System32. Absolute paths may be 
        /// used, but UNC paths are not supported.
        /// </summary>
        public string LogFilePath
        {
            get
            {
                loadServerClass();
                return Convert.ToString(m_serverObject["LogFilePath"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["LogFilePath"] = value;
            }
        }

        /// <summary>
        ///     List of IP addresses used to filter DNS events written to the debug log.
        /// </summary>
        public string[] LogIPFilterList
        {
            get
            {
                loadServerClass();
                return m_serverObject["LogIPFilterList"] as string[];
            }
            set
            {
                loadServerClass();
                m_serverObject["LogIPFilterList"] = value;
            }
        }

        /// <summary>
        /// Indicates which policies are activated in the Event Viewer system log. 
        /// </summary>
        public LogLevelEnum LogLevel
        {
            get
            {
                loadServerClass();
                return (LogLevelEnum)Convert.ToUInt32(m_serverObject["LogLevel"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["LogLevel"] = value;
            }
        }

        /// <summary>
        /// Indicates whether the DNS Server performs loose wildcarding. If undefined 
        /// or zero, the server follows the wildcarding behavior specified in the 
        /// DNS RFC. In this case, an administrator is advised to include MX records 
        /// for all hosts incapable of receiving mail. If nonzero, the server seeks 
        /// the closest wildcard node; in this case, an administrator should put MX 
        /// records at the zone root and in a wildcard node ('*') directly below the 
        /// zone root. Also, administrators should put self-referencing MX records 
        /// on hosts that receive their own mail.
        /// </summary>
        public bool LooseWildcarding
        {
            get
            {
                loadServerClass();
                return Convert.ToBoolean(m_serverObject["LooseWildcarding"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["LooseWildcarding"] = value;
            }
        }

        /// <summary>
        /// Maximum time, in seconds, the record of a recursive name query may remain 
        /// in the DNS Server cache. The DNS Server deletes records from the cache when 
        /// the value of this entry expires, even if the value of the TTL field in the 
        /// record is greater.        
        /// <para>The default value of this property is 86,400 seconds (1 day).</para>
        /// </summary>
        public TimeSpan MaxCacheTTL
        {
            get
            {
                loadServerClass();
                return TimeSpan.FromSeconds( Convert.ToUInt32(m_serverObject["MaxCacheTTL"]));
            }
            set
            {
                loadServerClass();
                m_serverObject["MaxCacheTTL"] = value.TotalSeconds;
            }
        }

        /// <summary>
        /// Maximum time, in seconds, a name error result from a recursive query may 
        /// remain in the DNS Server cache. DNS deletes records from the cache when 
        /// this timer expires, even if the TTL field is greater. Default value is 86,400 (one day).
        /// </summary>
        public TimeSpan MaxNegativeCacheTTL
        {
            get
            {
                loadServerClass();
                return TimeSpan.FromSeconds( Convert.ToUInt32(m_serverObject["MaxNegativeCacheTTL"]));
            }
            set
            {
                loadServerClass();
                m_serverObject["MaxNegativeCacheTTL"] = value.TotalSeconds;
            }
        }

        /// <summary>
        /// Fully qualified domain name (FQDN) or IP address of the DNS Server.
        /// </summary>
        public string Name
        {
            get
            {
                loadServerClass();
                return Convert.ToString(m_serverObject["Name"]);
            }
        }

        /// <summary>
        /// Indicates the set of eligible characters to be used in DNS names. 
        /// </summary>
        public NameCheckFlagEnum NameCheckFlag
        {
            get
            {
                loadServerClass();
                return (NameCheckFlagEnum)Convert.ToUInt32(m_serverObject["NameCheckFlag"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["NameCheckFlag"] = value;
            }
        }

        /// <summary>
        /// Indicates whether the DNS Server performs recursive look ups. TRUE indicates 
        /// recursive look ups are not performed.
        /// </summary>
        public bool NoRecursion
        {
            get
            {
                loadServerClass();
                return Convert.ToBoolean(m_serverObject["NoRecursion"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["NoRecursion"] = value;
            }
        }

        /// <summary>
        /// Elapsed seconds before retrying a recursive look up. If the property is 
        /// undefined or zero, retries are made after three seconds. Users are 
        /// discouraged from altering this property. There are certain situations 
        /// when the property should be changed; one example is when the DNS Server
        /// contacts remote servers over a slow link, and the DNS Server is retrying 
        /// before receiving response from the remote DNS. In this case, raising the 
        /// time out to be slightly longer than the observed response time from the 
        /// remote DNS would be reasonable.
        /// </summary>
        public TimeSpan RecursionRetry
        {
            get
            {
                loadServerClass();
                return TimeSpan.FromSeconds(Convert.ToUInt32(m_serverObject["RecursionRetry"]));
            }
            set
            {
                loadServerClass();
                m_serverObject["RecursionRetry"] = (UInt32)value.TotalSeconds;
            }
        }

        /// <summary>
        /// Elapsed seconds before the DNS Server gives up recursive query. If the property
        /// is undefined or zero, the DNS Server gives up after 15 seconds. In general,
        /// the 15-second time out is sufficient to allow any outstanding response to 
        /// get back to the DNS Server. 
        /// <para>Users are discouraged from altering this property. One scenario where
        /// the property should be changed is when the DNS Server contacts remote servers 
        /// over a slow link, and the DNS Server is observed rejecting queries (with 
        /// SERVER_FAILURE) before responses are received.</para>
        /// <para>Client resolvers also retry queries, so careful investigation is
        /// required to determine whether remote responses are actually associated
        /// with the query that timed out. In this case, raising the time out value
        /// to be slightly longer than the observed response time from the remote DNS
        /// would be reasonable.</para>
        /// </summary>
        public TimeSpan RecursionTimeout
        {
            get
            {
                loadServerClass();
                return TimeSpan.FromSeconds(Convert.ToUInt32(m_serverObject["RecursionTimeout"]));
            }
            set
            {
                loadServerClass();
                m_serverObject["RecursionTimeout"] = (UInt32) value.TotalSeconds;
            }
        }

        /// <summary>
        /// Indicates whether the DNS Server round robins multiple A records.
        /// </summary>
        public bool RoundRobin
        {
            get
            {
                loadServerClass();
                return Convert.ToBoolean(m_serverObject["RoundRobin"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["RoundRobin"] = value;
            }
        }

        /// <summary>
        /// RPC protocol or protocols over which administrative RPC runs.
        /// </summary>
        public RpcProtocolEnum RpcProtocol
        {
            get
            {
                loadServerClass();
                return (RpcProtocolEnum)Convert.ToInt32(m_serverObject["RpcProtocol"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["RpcProtocol"] = value;
            }
        }

        /// <summary>
        /// Interval, in hours, between two consecutive scavenging operations performed 
        /// by the DNS Server. Zero indicates scavenging is disabled. The default value
        /// is 168 hours (seven days).
        /// </summary>
        public TimeSpan ScavengingInterval
        {
            get
            {
                loadServerClass();
                return TimeSpan.FromHours( Convert.ToUInt32(m_serverObject["ScavengingInterval"]));
            }
            set
            {
                loadServerClass();
                m_serverObject["ScavengingInterval"] = value.TotalHours;
            }
        }

        /// <summary>
        /// Indicates whether the DNS Server exclusively saves records of names 
        /// in the same subtree as the server that provided them.
        /// </summary>
        public bool SecureResponses
        {
            get
            {
                loadServerClass();
                return Convert.ToBoolean(m_serverObject["SecureResponses"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["SecureResponses"] = value;
            }
        }

        /// <summary>
        /// Port on which the DNS Server sends UDP queries to other servers. By 
        /// default, the DNS Server sends queries on a socket bound to the DNS port. 
        /// <para>Under certain situations, this is not the best configuration. One 
        /// obvious case is when an administrator blocks the DNS port with a firewall 
        /// to prevent outside access to the DNS Server, but still wants the server to 
        /// be able to contact Internet DNS Servers to provide name resolution for
        /// internal clients. Another case is when the DNS Server is supporting 
        /// disjoint nets (the property DisjointNets set to TRUE identifies this 
        /// scenario). In these cases, setting the SendOnNonDnsPort property to a 
        /// nonzero value directs the DNS Server to bind to an arbitrary port for 
        /// sending to remote DNS Servers.</para>
        /// <para>If the SendOnNonDnsPort value is greater than 1024, the DNS
        /// Server binds explicitly to the port value given. This configuration
        /// option is useful when an administrator needs to fix the DNS query port
        /// for firewall purposes.</para>
        /// </summary>
        public UInt32 SendPort
        {
            get
            {
                loadServerClass();
                return Convert.ToUInt32(m_serverObject["SendPort"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["SendPort"] = value;
            }
        }

        /// <summary>
        /// Enumerates the list of IP addresses for the DNS Server.
        /// </summary>
        public string[] ServerAddresses
        {
            get
            {
                loadServerClass();
                return m_serverObject["ServerAddresses"] as string[];
            }
        }

        /// <summary>
        /// Indicates whether the DNS Server parses zone files strictly. If undefined
        /// or zero, the server will log and ignore bad data in the zone file and
        /// continue to load. If nonzero, the server will log and fail on zone file errors.
        /// </summary>
        public bool StrictFileParsing
        {
            get
            {
                loadServerClass();
                return Convert.ToBoolean(m_serverObject["StrictFileParsing"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["StrictFileParsing"] = value;
            }
        }

        /// <summary>
        /// Restricts the type of records that can be dynamically updated on the
        /// server, used in addition to the AllowUpdate settings on Server and Zone objects.
        /// </summary>
        public UpdateOptionsEnum UpdateOptions
        {
            get
            {
                loadServerClass();
                return (UpdateOptionsEnum) Convert.ToUInt32(m_serverObject["UpdateOptions"]);
            }
        }

        /// <summary>
        /// Version of the DNS Server.
        /// </summary>
        public WMIVersion Version
        {
            get
            {
                loadServerClass();
                return new WMIVersion( Convert.ToUInt32(m_serverObject["Version"]));
            }
        }


        /// <summary>
        /// Specifies whether the DNS Server writes NS and SOA records to the authority 
        /// section on successful response.
        /// </summary>
        public bool WriteAuthorityNS
        {
            get
            {
                loadServerClass();
                return Convert.ToBoolean(m_serverObject["WriteAuthorityNS"]);
            }
            set
            {
                loadServerClass();
                m_serverObject["WriteAuthorityNS"] = value;
            }
        }

        /// <summary>
        /// Time, in seconds, the DNS Server waits for a successful TCP connection 
        /// to a remote server when attempting a zone transfer.
        /// </summary>
        public TimeSpan XfrConnectTimeout
        {
            get
            {
                loadServerClass();
                return TimeSpan.FromSeconds( Convert.ToUInt32(m_serverObject["XfrConnectTimeout"]));
            }
            set
            {
                loadServerClass();
                m_serverObject["XfrConnectTimeout"] = value.TotalSeconds;
            }
        }

        /// <summary>
        /// Saves modifications to the Server
        /// </summary>
        public void Save()
        {
            loadServerClass();
            try
            {
                m_serverObject.Put();
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }

        /// <summary>
        /// Retrieves DNS distinguished name for the zone.
        /// </summary>
        /// <returns></returns>
        public string GetDistinguishedName()
        {
            loadServerClass();
            return Convert.ToString( m_serverObject.InvokeMethod("GetDistinguishedName", null));
        }

        /// <summary>
        /// Starts scavenging stale records in the zones subjected to scavenging.
        /// </summary>
        /// <returns></returns>
        public UInt32 StartScavenging()
        {
            loadServerClass();
            return Convert.ToUInt32(m_serverObject.InvokeMethod("StartScavenging", null,null));
        }

        /// <summary>
        /// Starts the DNS Server.
        /// </summary>
        /// <returns></returns>
        public UInt32 StartService()
        {
            loadServerClass();
            try
            {
                return Convert.ToUInt32(m_serverObject.InvokeMethod("StartService", null, null));
            }
            catch (System.Management.ManagementException me)
            {
                throw new WMIException(me);
            }
        }

        /// <summary>
        /// Stops the DNS Server.
        /// </summary>
        /// <returns></returns>
        public UInt32 StopService()
        {
            loadServerClass();
            try
            {
                return Convert.ToUInt32(m_serverObject.InvokeMethod("StopService", null,null));
            }
            catch (System.Management.ManagementException me)
            {
                throw new WMIException(me);
            }
        }

        /// <summary>
        /// Retrieves a list of Domains from the server
        /// </summary>
        /// <returns></returns>
        public Domain[] GetDomains()
        {
            var domains = new ManagementClass(m_scope, new ManagementPath("MicrosoftDNS_Domain"), null);
            domains.Get();
            var coll = domains.GetInstances();

            List<Domain> domainlist = new List<Domain>();

            foreach (ManagementObject domain in coll)
                domainlist.Add(new Domain(domain));

            return domainlist.ToArray() ;
        }

        /// <summary>
        /// Retrieves all Zones
        /// </summary>
        /// <returns></returns>
        public Zone[] GetZones()
        {
            try
            {
                var zones = new ManagementClass(m_scope, new ManagementPath("MicrosoftDNS_Zone"), null);
                zones.Get();
                var coll = zones.GetInstances();

                List<Zone> zonelist = new List<Zone>();
                foreach (ManagementObject zone in coll)
                    zonelist.Add(new Zone(zone));
                return zonelist.ToArray();
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }

        /// <summary>
        /// Get Cached requests
        /// </summary>
        /// <returns></returns>
        public Cache[] GetCache()
        {
            var cache = new ManagementClass(m_scope, new ManagementPath("MicrosoftDNS_Cache"), null);
            cache.Get();
            var coll = cache.GetInstances();
            List<Cache> cachelist = new List<Cache>();
            foreach (ManagementObject cacheobj in coll)
                cachelist.Add(new Cache(cacheobj));
            return cachelist.ToArray();
        }

        /// <summary>
        /// Get Root Hints
        /// </summary>
        /// <returns></returns>
        public RootHints[] GetRootHints()
        {
            var roothints = new ManagementClass(m_scope, new ManagementPath("MicrosoftDNS_RootHints"), null);
            roothints.Get();
            var coll = roothints.GetInstances();
            List<RootHints> roothintlist = new List<RootHints>();
            foreach (ManagementObject rhobj in coll)
                roothintlist.Add(new RootHints(rhobj));
            return roothintlist.ToArray();
        }

        ///// <summary>
        ///// Gets all records
        ///// </summary>
        ///// <returns></returns>
        ////records are under zones, not server
        //public DNSManagement.RR.ResourceRecord[] GetRecords()
        //{
        //    var records = new ManagementClass(m_scope, new ManagementPath("MicrosoftDNS_ResourceRecord"), null);
        //    records.Get();
        //    var coll = records.GetInstances();
        //    List<DNSManagement.RR.ResourceRecord> recordlist = new List<DNSManagement.RR.ResourceRecord>();
        //    foreach (ManagementObject rhobj in coll)
        //        recordlist.Add(new DNSManagement.RR.ResourceRecord(rhobj));
        //    return recordlist.ToArray();
        //}

        /// <summary>
        /// Gets Statistics
        /// </summary>
        /// <returns></returns>
        public Statistic[] GetStatistics()
        {
            var statistics = new ManagementClass(m_scope, new ManagementPath("MicrosoftDNS_Statistic"), null);
            statistics.Get();
            var coll = statistics.GetInstances();
            List<Statistic> statisticlist = new List<Statistic>();
            foreach (ManagementObject stobj in coll)
                statisticlist.Add(new Statistic(stobj));
            return statisticlist.ToArray();
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLineFormat("AddressAnswerLimit={0}", AddressAnswerLimit);
            sb.AppendLineFormat("AllowUpdate={0}", AllowUpdate);
            sb.AppendLineFormat("AutoCacheUpdate={0}", AutoCacheUpdate);
            sb.AppendLineFormat("AutoConfigFileZones={0}", AutoConfigFileZones);
            sb.AppendLineFormat("BindSecondaries={0}", BindSecondaries);
            sb.AppendLineFormat("BootMethod={0}", BootMethod);
            sb.AppendLineFormat("DefaultAgingState={0}", DefaultAgingState);
            sb.AppendLineFormat("DefaultNoRefreshInterval={0}", DefaultNoRefreshInterval);
            sb.AppendLineFormat("DisableAutoReverseZones={0}", DisableAutoReverseZones);
            sb.AppendLineFormat("DisjointNets={0}", DisjointNets);
            sb.AppendLineFormat("DsAvailable={0}", DsAvailable);
            sb.AppendLineFormat("DsPollingInterval={0}", DsPollingInterval);
            sb.AppendLineFormat("DsTombstoneInterval={0}", DsTombstoneInterval);
            sb.AppendLineFormat("EDnsCacheTimeout={0}", EDnsCacheTimeout);
            sb.AppendLineFormat("EnableDirectoryPartitions={0}", EnableDirectoryPartitions);
            sb.AppendLineFormat("EnableDnsSec={0}", EnableDnsSec);
            sb.AppendLineFormat("EnableEDnsProbes={0}", EnableEDnsProbes);
            sb.AppendLineFormat("EventLogLevel={0}", EventLogLevel);
            sb.AppendLineFormat("ForwardDelegations={0}", ForwardDelegations);
            sb.AppendLineFormat("Forwarders={0}", Forwarders);
            sb.AppendLineFormat("ForwardingTimeout={0}", ForwardingTimeout);
            sb.AppendLineFormat("IsSlave={0}", IsSlave);
            sb.AppendLineFormat("ListenAddresses={0}", ListenAddresses);
            sb.AppendLineFormat("LocalNetPriority={0}", LocalNetPriority);
            sb.AppendLineFormat("LogFileMaxSize={0}", LogFileMaxSize);
            sb.AppendLineFormat("LogFilePath={0}", LogFilePath);
            sb.AppendLineFormat("LogIPFilterList={0}", LogIPFilterList);
            sb.AppendLineFormat("LogLevel={0}", LogLevel);
            sb.AppendLineFormat("LooseWildcarding={0}", LooseWildcarding);
            sb.AppendLineFormat("MaxCacheTTL={0}", MaxCacheTTL);
            sb.AppendLineFormat("MaxNegativeCacheTTL={0}", MaxNegativeCacheTTL);
            sb.AppendLineFormat("Name={0}", Name);
            sb.AppendLineFormat("NameCheckFlag={0}", NameCheckFlag);
            sb.AppendLineFormat("NoRecursion={0}", NoRecursion);
            sb.AppendLineFormat("RecursionRetry={0}", RecursionRetry);
            sb.AppendLineFormat("RecursionTimeout={0}", RecursionTimeout);
            sb.AppendLineFormat("RoundRobin={0}", RoundRobin);
            sb.AppendLineFormat("RpcProtocol={0}", RpcProtocol);
            sb.AppendLineFormat("ScavengingInterval={0}", ScavengingInterval);
            sb.AppendLineFormat("SecureResponses={0}", SecureResponses);
            sb.AppendLineFormat("SendPort={0}", SendPort);
            sb.AppendLineFormat("ServerAddresses={0}", ServerAddresses);
            sb.AppendLineFormat("StrictFileParsing={0}", StrictFileParsing);
            sb.AppendLineFormat("UpdateOptions={0}", UpdateOptions);
            sb.AppendLineFormat("Version={0}", Version);
            sb.AppendLineFormat("WriteAuthorityNS={0}", WriteAuthorityNS);
            sb.AppendLineFormat("XfrConnectTimeout={0}", XfrConnectTimeout);

            return sb.ToString();
        }

        public override string ToConfigurationFile()
        {
            StringBuilder sb = new StringBuilder();
            var type = this.GetType() ;
            var props = type.GetProperties();

            sb.AppendLine("[ServerSettings]");

            base.ToConfigurationFile();
            //foreach (var prop in props)
            //{
            //    sb.AppendLineFormat("{0}={1}",prop.Name,prop.GetValue(this,null));
            //}

            var zones = this.GetZones();
            sb.AppendLine("[Zones]");
            foreach (var zone in zones)
            {
                sb.AppendLineFormat("{0}={1},{2}",zone.Name,zone.ZoneType, (zone.DsIntegrated == false) ? "File":"");
            }

            foreach (var zone in zones)
            {
                sb.AppendLineFormat("[{0}]",zone.Name);
                sb.Append(zone.ToConfigurationFile());
            }

            return sb.ToString();
        }



        #region IDisposable Members

        public void Dispose()
        {
            if (m_serverClass != null)
                m_serverClass.Dispose();
            if (m_serverObject != null)
                m_serverObject.Dispose();
        }

        #endregion
    }
}
