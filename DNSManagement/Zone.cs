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
2012-03-17 - Added Dump for easy debugging.
 
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
    /// The MicrosoftDNS_Zone class describes a DNS Zone. Every instance of the MicrosoftDNS_Zone class must be assigned to exactly one DNS Server. Zones may be associated with multiple instances of MicrosoftDNS_Domain or MicrosoftDNS_ResourceRecord classes.
    /// </summary>
    public class Zone : Domain
    {
        /// <summary>
        /// Indicates whether the Zone accepts dynamic update requests.
        /// </summary>
        public enum ZoneAllowUpdateEnum
        {
            /// <summary>
            /// No updates allowed.
            /// </summary>
            NoUpdates = 0,
            /// <summary>
            /// Zone accepts both secure and nonsecure updates.
            /// </summary>
            SecureAndNonSecureUpdates = 1,
            /// <summary>
            /// Zone accepts secure updates only.
            /// </summary>
            SecureOnlyUpdates = 2
        }

        /// <summary>
        /// Indicates whether zone transfer is allowed to designated secondaries only. Designated secondaries are DNS Servers whose IP addresses are listed in SecondariesIPAddressesArray.
        /// </summary>
        public enum SecureSecondariesEnum
        {
            /// <summary>
            /// Send zone transfers to all secondary servers that request them.
            /// </summary>
            All = 0,
            /// <summary>
            /// Send zone transfers only to name servers that are authoritative for the zone.
            /// </summary>
            Authoritative = 1,
            /// <summary>
            /// Send zone transfers only to servers specified in SecondaryServers.
            /// </summary>
            Specified = 2,
            /// <summary>
            /// Do not send zone transfers.
            /// </summary>
            DoNotSend = 3
        }

        public enum ZoneTypeCreate
        {
            Primary = 0,
            Secondary = 1,
            Stub = 2,
            Forwarder = 3
        }

        public enum ZoneTypeEnum
        {
            ADIntegrated = 0,
            Primary = 1,
            Secondary = 2,
            Stub = 3,
            Forwarder = 4
        }

        /// <summary>
        /// Types of security settings that are enforced by the master DNS server to honor zone transfer requests for this zone.
        /// <remarks>DNS_ZONE_SECONDARY_SECURITY</remarks>
        /// </summary>
        public enum SecondarySecurityEnum
        {
            /// <summary>
            /// No security enforcement for secondaries, that is, any zone transfer request will be honored.
            /// <remarks>ZONE_SECSECURE_NO_SECURITY</remarks>
            /// </summary>
            NoSecurity = 0,
            //
            /// <summary>
            /// Zone transfer request will be honored from the remote servers, which are in the list of name servers for this zone.
            /// <remarks>ZONE_SECSECURE_NS_ONLY</remarks>
            /// </summary>
            NSOnly = 1,
            /// <summary>
            /// Zone transfer request will be honored from the remote servers, which are explicitly configured by IP addresses in the aipSecondaries field in the DNS_RPC_ZONE_INFO structure (section 2.2.5.2.4).
            /// <remarks>ZONE_SECSECURE_LIST_ONLY</remarks>
            /// </summary>
            ListOnly = 2,
            /// <summary>
            /// No zone transfer requests will be honored.
            /// <remarks>ZONE_SECSECURE_NO_XFR</remarks>
            /// </summary>
            NoXFR = 3
        }

        /// <summary>
        ///  levels of notification settings that can be configured on a master DNS server to send out notifications to secondaries about any changes to this zone, so that they can initiate a zone transfer to get updated zone information.
        ///  <remarks>DNS_ZONE_NOTIFY_LEVEL</remarks>
        /// </summary>
        public enum NotifyLevelEnum
        {
            
            /// <summary>
            /// The Master DNS server does not send any zone notifications.
            /// <remarks>ZONE_NOTIFY_OFF</remarks>
            /// </summary>
            Off = 0,
            /// <summary>
            /// The Master DNS server sends zone notifications to all secondary servers for this zone, either they are listed as name-servers for this zone or they are present explicitly zone notify list for this zone.
            /// <remarks>ZONE_NOTIFY_ALL_SECONDARIES</remarks>
            /// </summary>
            AllSecondaries = 1,
            /// <summary>
            /// The Master DNS server sends zone notifications only to those remote servers which are explicitly configured by IP addresses in the aipNotify field in the DNS_RPC_ZONE_INFO structure (section 2.2.5.2.4).
            /// <remarks>ZONE_NOTIFY_LIST_ONLY</remarks>
            /// </summary>
            ListOnly = 2
        }

        private ManagementObject m_mo;

        internal Zone(ManagementObject mo) : base(mo)
        {
            m_mo = mo;
        }

        private Zone(): base(null) { }

        /// <summary>
        /// Specifies the aging and scavenging behavior of the zone. Zero indicates
        /// scavenging is disabled. When scavenging is disabled, the timestamps of 
        /// records in the zone are not refreshed, and the records are not subjected
        /// to scavenging. When set to one, records are subjected to scavenging and 
        /// their timestamps are refreshed when the server receives the dynamic update
        /// request for the records. For Active Directory-integrated zones, this value
        /// is set to the DefaultAgingState property of the DNS Server where the zone
        /// is created. For standard primary zones, the default value is zero. 
        /// </summary>
        public bool Aging
        {
            get
            {
                return Convert.ToBoolean(m_mo["Aging"]);
            }
            set
            {
                m_mo["Aging"] = (value) ? 1 : 0;
            }
        }

        /// <summary>
        /// Indicates whether the Zone accepts dynamic update requests.
        /// </summary>
        public ZoneAllowUpdateEnum AllowUpdate
        {
            get
            {
                return (ZoneAllowUpdateEnum)Convert.ToUInt32(m_mo["AllowUpdate"]);
            }
            set
            {
                m_mo["AllowUpdate"] = (UInt32)value;
            }
        }

        /// <summary>
        /// Indicates whether the Zone is autocreated.
        /// </summary>
        public bool AutoCreated
        {
            get
            {
                return Convert.ToBoolean(m_mo["AutoCreated"]);
            }
        }

        /// <summary>
        /// Specifies the time when the server may attempt scavenging the zone. Even 
        /// if the zone is configured to enable scavenging the DNS server will not 
        /// attempt scavenging this zone until after this moment. This value is set 
        /// to the current time plus the Refresh Interval of the zone whenever the 
        /// zone is loaded. This parameter is stored locally, and is not replicated to 
        /// other copies of the zone.
        /// </summary>
        public UInt32 AvailForScavengeTime
        {
            get
            {
                return Convert.ToUInt32(m_mo["AvailForScavengeTime"]);
            }
        }

        /// <summary>
        /// Indicates the name of the zone file.
        /// </summary>
        public string DataFile
        {
            get
            {
                return Convert.ToString(m_mo["DataFile"]);
            }
            set
            {
                m_mo["DataFile"] = value;
            }
        }

        /// <summary>
        /// Indicates whether the WINS record is replicated. If set to TRUE, WINS record
        /// replication is disabled.
        /// </summary>
        public bool DisableWINSRecordReplication
        {
            get
            {
                return Convert.ToBoolean(m_mo["DisableWINSRecordReplication"]);
            }
        }

        /// <summary>
        /// Specifies whether the zone is DS integrated.
        /// </summary>
        public bool DsIntegrated
        {
            get
            {
                return Convert.ToBoolean(m_mo["DsIntegrated"]);
            }
        }

        /// <summary>
        /// Indicates whether the DNS uses recursion when resolving the names for the 
        /// specified forward zone. Applicable to Conditional Forwarding zones only.
        /// </summary>
        public bool ForwarderSlave
        {
            get
            {
                return Convert.ToBoolean(m_mo["ForwarderSlave"]);
            }
            set
            {
                m_mo["ForwarderSlave"] = (value) ? 1 : 0;
            }
        }

        /// <summary>
        /// Indicates the time, in seconds, a DNS server forwarding a query for the name
        /// under the forward zone waits for resolution from the forwarder before 
        /// attempting to resolve the query itself. This parameter is applicable to 
        /// the Forward zones only.
        /// </summary>
        public UInt32 ForwarderTimeout
        {
            get
            {
                return Convert.ToUInt32(m_mo["ForwarderTimeout"]);
            }
        }

        /// <summary>
        /// Number of seconds since the beginning of January 1, 1970 GMT, since the 
        /// SOA serial number for the zone was last checked.
        /// </summary>
        public UnixDateTime LastSuccessfulSoaCheck
        {
            get
            {
                return new UnixDateTime((double) Convert.ToUInt32(m_mo["LastSuccessfulSoaCheck"]));
            }
        }

        /// <summary>
        /// Number of seconds since the beginning of January 1, 1970 GMT, since the 
        /// zone was last transferred from a master server.
        /// </summary>
        public UnixDateTime LastSuccessfulXfr
        {
            get
            {
                return new UnixDateTime((double) Convert.ToUInt32(m_mo["LastSuccessfulXfr"]));
            }
        }

        /// <summary>
        /// Local IP addresses of the master DNS servers for this zone. If set, these 
        /// masters over-ride the MasterServers found in Active Directory.
        /// </summary>
        public string[] LocalMasterServers
        {
            get
            {
                return m_mo["LocalMasterServers"] as string[];
            }
            set
            {
                m_mo["LocalMasterServers"] = value;
            }
        }

        /// <summary>
        /// IP addresses of the master DNS servers for this zone.
        /// </summary>
        public string[] MasterServers
        {
            get
            {
                return m_mo["MasterServers"] as string[];
            }
            set
            {
                m_mo["MasterServers"] = value;
            }
        }

        /// <summary>
        /// Specifies the time interval between the last update of a record's timestamp 
        /// and the earliest moment when the timestamp can be refreshed.
        /// <para>Hours</para>
        /// </summary>
        public TimeSpan NoRefreshInterval
        {
            get
            {
                return TimeSpan.FromHours( Convert.ToUInt32(m_mo["NoRefreshInterval"]));
            }
            set
            {
                m_mo["NoRefreshInterval"] = Convert.ToUInt32(value.TotalHours);
            }
        }

        /// <summary>
        /// Indicates whether the master Zone notifies secondaries of any changes in its 
        /// RRs database. Set to 1 to notify secondaries.
        /// </summary>
        public bool Notify
        {
            //TODO: change to bool
            get
            {
                return Convert.ToBoolean(m_mo["Notify"]);
            }
        }

        /// <summary>
        /// Array of strings enumerating IP addresses of DNS servers to be notified of
        /// changes in this zone.
        /// </summary>
        public string[] NotifyServers
        {
            get
            {
                return m_mo["NotifyServers"] as string[];
            }
        }

        /// <summary>
        /// Indicates whether the Zone is paused.
        /// </summary>
        public bool Paused
        {
            get
            {
                return Convert.ToBoolean(m_mo["Paused"]);
            }
        }

        /// <summary>
        /// Specifies the refresh interval, during which the records with nonzero
        /// timestamp are expected to be refreshed to remain in the zone. Records 
        /// that have not been refreshed by expiration of the Refresh interval could
        /// be removed by the next scavenging performed by a server. This value should 
        /// never be less than the longest refresh period of the records registered 
        /// in the zone. Values that are too small may lead to removal of valid DNS 
        /// records. values that are too large prolong the lifetime of stale records. 
        /// This value is set to the DefaultRefreshInterval property of the DNS server
        /// where the zone is created.
        /// <para>Hours</para>
        /// </summary>
        public TimeSpan RefreshInterval
        {
            get
            {
                return TimeSpan.FromHours( Convert.ToUInt32(m_mo["RefreshInterval"]));
            }
            set
            {
                m_mo["RefreshInterval"] = Convert.ToUInt32(value.TotalHours);
            }
        }

        /// <summary>
        /// Indicates whether the Zone is reverseca (TRUE) or forward (FALSE).
        /// </summary>
        public bool Reverse
        {
            get
            {
                return Convert.ToBoolean(m_mo["Reverse"]);
            }
        }

        /// <summary>
        /// Array of strings that enumerates the list of IP addresses of DNS servers 
        /// that are allowed to perform scavenging of stale records of this zone. 
        /// If the list is not specified, any primary DNS server authoritative for
        /// the zone is allowed to scavenge the zone when other prerequisites are met.
        /// </summary>
        public string[] ScavengeServers
        {
            get
            {
                return m_mo["ScavengeServers"] as string[];
            }
            set
            {
                m_mo["ScavengeServers"] = value;
            }
        }

        /// <summary>
        /// Array of strings enumerating IP addresses of DNS servers allowed to receive 
        /// this zone through zone replication.
        /// </summary>
        public string[] SecondaryServers
        {
            get
            {
                return m_mo["SecondaryServers"] as string[];
            }
            set
            {
                m_mo["SecondaryServers"] = value;
            }
        }

        /// <summary>
        /// Indicates whether zone transfer is allowed to designated secondaries
        /// only. Designated secondaries are DNS Servers whose IP addresses are 
        /// listed in SecondariesIPAddressesArray.
        /// </summary>
        public SecureSecondariesEnum SecureSecondaries
        {
            get
            {
                return (SecureSecondariesEnum)Convert.ToUInt32(m_mo["SecureSecondaries"]);
            }
            //set
            //{
            //    m_mo["SecureSecondaries"] = (UInt32)value;
            //}
        }

        /// <summary>
        /// Indicates whether the copy of the Zone is expired. If TRUE, the Zone is 
        /// expired, or shut down.
        /// </summary>
        public bool Shutdown
        {
            get
            {
                return Convert.ToBoolean(m_mo["Shutdown"]);
            }
        }

        /// <summary>
        /// Indicates whether the Zone uses WINS look up.
        /// </summary>
        public bool UseWins
        {
            get
            {
                return Convert.ToBoolean(m_mo["UseWins"]);
            }
            //set
            //{
            //    m_mo["UseWins"] = value;
            //}
        }

        /// <summary>
        /// This Boolean indicates whether the Zone uses NBStat reverse lookup.
        /// </summary>
        public bool UseNBStat
        {
            get
            {
                return Convert.ToBoolean(m_mo["UseNBStat"]);
            }
        }

        /// <summary>
        /// indicates the type of the Zone. Valid values are:
        /// DS integrated
        /// Primary
        /// Secondary
        /// </summary>
        public ZoneTypeEnum ZoneType
        {
            get
            {
                return (ZoneTypeEnum)Convert.ToUInt32(m_mo["ZoneType"]);
            }
            set
            {
                m_mo["ZoneType"] = (uint)value;
            }
        }

        /// <summary>
        /// enables aging for some or all non-NS and non-SOA records in a zone.
        /// </summary>
        /// <param name="nodeName">Optional - Name of the node to age.</param>
        /// <param name="applyToSubtree">Optional - Specifies whether aging should apply to all records 
        /// in the subtree. Set to TRUE to apply aging to all records in the subtree,
        /// beginning with NodeName.</param>
        /// <returns></returns>
        public UInt32 AgeAllRecords(string nodeName, bool? applyToSubtree)
        {
            try
            {

                return Convert.ToUInt32(m_mo.InvokeMethod("AgeAllRecords", new object[] { nodeName, applyToSubtree }));
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }

        /// <summary>
        /// changes the type of a zone.
        /// </summary>
        /// <param name="zoneType">Type of zone</param>
        /// <param name="dataFileName">Optional - Name of the data file associated with the zone.</param>
        /// <param name="ipAddr">Optional - IP address of the mater DNS Server for the zone.</param>
        /// <param name="adminEmailName">Optional - Email address of the administrator responsible for the zone.</param>
        public Zone ChangeZoneType(ZoneTypeCreate zoneType, string dataFileName, string[] ipAddr, 
                                    string adminEmailName)
        {

            ManagementBaseObject inParams = m_mo.GetMethodParameters("ChangeZoneType");
            inParams["ZoneType"] = (UInt32)zoneType ;

            if (!string.IsNullOrEmpty(dataFileName))
                inParams["DataFileName"] = dataFileName;

            if (ipAddr != null)
                inParams["IpAddr"] = ipAddr;
            else if (((ipAddr == null) || (ipAddr.Length == 0)) && (zoneType != ZoneTypeCreate.Primary))
                throw new ArgumentException("ipAddr must contain at least one address");

            if ((this.ZoneType == ZoneTypeEnum.Stub) && (zoneType == ZoneTypeCreate.Primary))
                throw new ArgumentException("Unable to change from Stub to Primary, change to secondary first");

            if ((zoneType == ZoneTypeCreate.Primary) && (string.IsNullOrEmpty(this.DataFile)) && (string.IsNullOrEmpty(dataFileName)))
                throw new ArgumentException("Primary zone needs a filename");

            if (!string.IsNullOrEmpty(adminEmailName))
                inParams["AdminEmailName"] = adminEmailName;

            //ChangeZoneType not acting as documented in 
            //http://msdn.microsoft.com/en-us/library/windows/desktop/ms682759(v=vs.85).aspx
            try
            {
                m_mo.InvokeMethod("ChangeZoneType", inParams, null);//["RR"].ToString()

                return new Zone(new ManagementObject(m_mo.Scope, new ManagementPath(m_mo.ToString()), null));
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }


        }


        /// <summary>
        /// creates a DNS zone. 
        /// </summary>
        /// <param name="server">a Server instance</param>
        /// <param name="zoneName">String representing the name of the zone.</param>
        /// <param name="zoneType">Type of zone.</param>
        /// <param name="dsIntegrated">Indicates whether zone data is stored in the Active Directory or in files. If TRUE, the data is stored in the Active Directory; if FALSE, the data is stored in files.</param>
        /// <param name="dataFileName">Optional - Name of the data file associated with the zone.</param>
        /// <param name="ipAddr">Optional - IP address of the master DNS Server for the zone.</param>
        /// <param name="adminEmail">Optional - Email address of the administrator responsible for the zone.</param>
        /// <returns>the new Zone created</returns>
        public static Zone CreateZone(Server server,
                                string zoneName,
                                ZoneTypeCreate zoneType,
                                bool dsIntegrated,
                                string dataFileName,
                                string[] ipAddr,
                                string adminEmail)
        {
            if (server == null)
                throw new ArgumentNullException("server is required");

            try
            {
                ManagementObject mc = new ManagementClass(server.m_scope, new ManagementPath("MicrosoftDNS_Zone"), null);
                mc.Get();
                ManagementBaseObject parameters = mc.GetMethodParameters("CreateZone");
                parameters["ZoneName"] = zoneName;
                parameters["ZoneType"] = (UInt32)zoneType;
                parameters["DsIntegrated"] = dsIntegrated;
                if (!string.IsNullOrEmpty(dataFileName))
                    parameters["DataFileName"] = dataFileName;
                //if (!string.IsNullOrEmpty(ipAddr))
                parameters["IpAddr"] = ipAddr;
                if (!string.IsNullOrEmpty(adminEmail))
                    parameters["AdminEmailName"] = adminEmail;

                return new Zone(new ManagementObject(server.m_scope, new ManagementPath(mc.InvokeMethod("CreateZone", parameters, null)["RR"].ToString()), null));
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
          

        }

        /// <summary>
        /// Forces an update of the secondary from the Master DNS Server. 
        /// </summary>
        public void ForceRefresh()
        {
            try
            {
                m_mo.InvokeMethod("ForceRefresh", null);
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }


        /// <summary>
        /// Pauses the Zone. 
        /// </summary>
        public void PauseZone()
        {
            try
            {
                m_mo.InvokeMethod("PauseZone", null);
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }

        /// <summary>
        /// reloads the DNS Zone from its database.
        /// </summary>
        public void ReloadZone()
        {
            try
            {
                m_mo.InvokeMethod("ReloadZone", null);
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }

        /// <summary>
        /// resets the IP addresses for secondary DNS Servers in the zone.
        /// </summary>
        /// <param name="secondaryServers">Array of IP addresses for secondary DNS Servers.</param>
        /// <param name="secureSecondaries">Specifies the security to be applied </param>
        /// <param name="notifyServers">IP address of DNS Servers to be notified when the zone changes.</param>
        /// <param name="notify">Notification setting and must be one of the following: </param>
        /// <returns>Zone</returns>
        public Zone ResetSecondaries(string[] secondaryServers, SecondarySecurityEnum secureSecondaries,
                                     string[] notifyServers,
                                     NotifyLevelEnum notify)
        {

            ManagementBaseObject inParams = m_mo.GetMethodParameters("ResetSecondaries");
            inParams["SecondaryServers"] = secondaryServers;
            inParams["SecureSecondaries"] = (UInt32)secureSecondaries;
            inParams["NotifyServers"] = notifyServers;
            inParams["Notify"] = (UInt32)notify;

            //return new Zone(new ManagementObject(m_mo.Scope, new ManagementPath(m_mo.InvokeMethod("ResetSecondaries", inParams, null)["RR"].ToString()), null));


            //ResetSecondaries not acting as documented in 
            //http://msdn.microsoft.com/en-us/library/windows/desktop/ms682765(v=vs.85).aspx
            try
            {
                m_mo.InvokeMethod("ResetSecondaries", inParams, null);//["RR"].ToString()

                return new Zone(new ManagementObject(m_mo.Scope, new ManagementPath(m_mo.ToString()), null));
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }

        }

        /// <summary>
        /// Resumes the Zone
        /// </summary>
        public void ResumeZone()
        {
            m_mo.InvokeMethod("ResumeZone", null);
        }

        /// <summary>
        /// Forces an update of the Zone from the Directory Service (DS). 
        /// For this method to be valid, the ZoneType must be 0—the Zone must
        /// indeed be stored in the DS. 
        /// </summary>
        public void UpdateFromDS()
        {
            try
            {
                m_mo.InvokeMethod("UpdateFromDS", null);
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }

        /// <summary>
        /// Saves Zone data to its zone file. 
        /// </summary>
        public void WriteBackZone()
        {
            try
            {
                m_mo.InvokeMethod("WriteBackZone", null);
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }

        /// <summary>
        /// Saves a modified Zone
        /// </summary>
        public void Save()
        {
            m_mo.Put();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLineFormat("Aging={0}", Aging);
            sb.AppendLineFormat("AllowUpdate={0}", AllowUpdate);
            sb.AppendLineFormat("AutoCreated={0}", AutoCreated);
            sb.AppendLineFormat("AvailForScavengeTime={0}", AvailForScavengeTime);
            sb.AppendLineFormat("DataFile={0}", DataFile);
            sb.AppendLineFormat("DisableWINSRecordReplication={0}", DisableWINSRecordReplication);
            sb.AppendLineFormat("DsIntegrated={0}", DsIntegrated);
            sb.AppendLineFormat("ForwarderSlave={0}", ForwarderSlave);
            sb.AppendLineFormat("ForwarderTimeout={0}", ForwarderTimeout);
            sb.AppendLineFormat("LastSuccessfulSoaCheck={0}", LastSuccessfulSoaCheck);
            sb.AppendLineFormat("LastSuccessfulXfr={0}", LastSuccessfulXfr);
            sb.AppendLineFormat("LocalMasterServers={0}", LocalMasterServers);
            sb.AppendLineFormat("MasterServers={0}", MasterServers);
            sb.AppendLineFormat("NoRefreshInterval={0}", NoRefreshInterval);
            sb.AppendLineFormat("Notify={0}", Notify);
            sb.AppendLineFormat("NotifyServers={0}", NotifyServers);
            sb.AppendLineFormat("Paused={0}", Paused);
            sb.AppendLineFormat("RefreshInterval={0}", RefreshInterval);
            sb.AppendLineFormat("Reverse={0}", Reverse);
            sb.AppendLineFormat("ScavengeServers={0}", ScavengeServers);
            sb.AppendLineFormat("SecondaryServers={0}", SecondaryServers);
            sb.AppendLineFormat("SecureSecondaries={0}", SecureSecondaries);
            sb.AppendLineFormat("Shutdown={0}", Shutdown);
            sb.AppendLineFormat("UseWins={0}", UseWins);
            sb.AppendLineFormat("ZoneType={0}", ZoneType);

            //from Domain class
            base.ToString(sb);

            return sb.ToString();
        }

        public string ToConfigurationFile()
        {
            StringBuilder sb = new StringBuilder();
            var type = this.GetType();
            var props = type.GetProperties();

            foreach (var prop in props)
            {
                sb.AppendLineFormat("{0}={1}", prop.Name, prop.GetValue(this, null));
            }

            var records = this.GetRecords().OrderBy(i=>i.RecordTypeText);

            var soarecord = records.FirstOrDefault(i => i.RecordType == typeof(DNSManagement.RR.SOAType));
            if (soarecord != null)
            {
                sb.AppendLine("SOA Record");
                sb.AppendLine(soarecord.TextRepresentation);
            }

            var rectype = "";

            foreach (var rec in records)
            {
                if (rec == soarecord)
                    continue;

                if (rectype != rec.RecordTypeText)
                {
                    rectype = rec.RecordTypeText;
                    sb.AppendLineFormat("{0} Records:", rec.RecordTypeText);
                }

                sb.AppendLineFormat(rec.TextRepresentation);
            }

            return sb.ToString();

        }

        internal List<KeyValuePair<string, object>> Dump()
        {
            List<KeyValuePair<string, object>> results = new List<KeyValuePair<string, object>>();
            foreach (var p in this.m_mo.Properties)
            {
                results.Add(new KeyValuePair<string, object>(p.Name, p.Value));
            }
            return results;
        }
    }
}
