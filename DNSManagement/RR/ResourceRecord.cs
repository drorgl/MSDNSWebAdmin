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
2012-03-09 - Fixed all saving methods to return connected records
2012-03-17 - Added Dump for easy debugging.
*/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using DNSManagement.Extensions;

namespace DNSManagement.RR
{
    /// <summary>
    /// Represents the general properties of a DNS RR.
    /// </summary>
    public class ResourceRecord : Backupable
    {
        /// <summary>
        /// Class of Record
        /// </summary>
        public enum RecordClassEnum
        {
            /// <summary>
            /// Internet
            /// </summary>
            IN = 1,
            /// <summary>
            /// CSNET
            /// </summary>
            CS = 2,
            /// <summary>
            /// CHAOS
            /// </summary>
            CH = 3,
            /// <summary>
            /// Hesiod
            /// </summary>
            HS = 4
        }

        /// <summary>
        /// Resource Record Types
        /// </summary>
        public enum ResourceRecordEnum
        {
            /// <summary>
            /// IPv6 Address
            /// </summary>
            AAAA,
            /// <summary>
            /// Andrew File System Database Server
            /// </summary>
            AFSDB,
            /// <summary>
            /// ATM Address-to-Name
            /// </summary>
            //ATMA,
            /// <summary>
            /// IP Address
            /// </summary>
            A,
            /// <summary>
            /// Canonical Name
            /// </summary>
            CNAME,
            /// <summary>
            /// Host Information
            /// </summary>
            HINFO,
            /// <summary>
            /// ISDN
            /// </summary>
            ISDN,
            /// <summary>
            /// KEY
            /// </summary>
            //KEY,
            /// <summary>
            /// Mailbox
            /// </summary>
            MB,
            /// <summary>
            /// Mail Agent for Domain
            /// </summary>
            MD,
            /// <summary>
            /// Mail Forwarding Agent for Domain
            /// </summary>
            MF,
            /// <summary>
            /// Mail Group
            /// </summary>
            MG,
            /// <summary>
            /// Mail Information
            /// </summary>
            MINFO,
            /// <summary>
            /// Mailbox Rename
            /// </summary>
            MR,
            /// <summary>
            /// Mail Exchanger
            /// </summary>
            MX,
            /// <summary>
            /// Name Server
            /// </summary>
            NS,
            /// <summary>
            /// Next
            /// </summary>
            //NXT,
            /// <summary>
            /// Pointer
            /// </summary>
            PTR,
            /// <summary>
            /// Responsible Person
            /// </summary>
            RP,
            /// <summary>
            /// Route Through
            /// </summary>
            RT,
            /// <summary>
            /// Signature
            /// </summary>
            SIG,
            /// <summary>
            /// Start Of Authority
            /// </summary>
            SOA,
            /// <summary>
            /// Service
            /// </summary>
            SRV,
            /// <summary>
            /// Text
            /// </summary>
            TXT,
            /// <summary>
            /// WINS-Reverse
            /// </summary>
            WINSR,
            /// <summary>
            /// WINS
            /// </summary>
            WINS,
            /// <summary>
            /// Well-Known Service
            /// </summary>
            WKS,
            /// <summary>
            /// X.25
            /// </summary>
            X25
        }

        private ManagementObject m_mo;

        internal ResourceRecord(ManagementObject mo)
        {
            m_mo = mo;
        }

        /// <summary>
        /// Indicates the name of the Container for the Zone, Cache, or RootHints 
        /// instance that contains the RR.
        /// </summary>
        public string ContainerName
        {
            get
            {
                return Convert.ToString(m_mo["ContainerName"]);
            }
        }

        /// <summary>
        /// Indicates the FQDN or IP address of the DNS Server that contains the RR.
        /// </summary>
        public string DnsServerName
        {
            get
            {
                return Convert.ToString(m_mo["DnsServerName"]);
            }
        }

        /// <summary>
        /// Represents the FQDN of the Domain that contains the RR. This 
        /// property may contain the strings \..Cache\ or \..RootHints\ if 
        /// the internal cache or RootHints contain the RR, respectively.
        /// </summary>
        public string DomainName
        {
            get
            {
                return Convert.ToString(m_mo["DomainName"]);
            }
        }

        /// <summary>
        /// Owner name for the RR.
        /// </summary>
        public string OwnerName
        {
            get
            {
                return Convert.ToString(m_mo["OwnerName"]);
            }
        }

        /// <summary>
        /// Resource record data.
        /// </summary>
        public RecordClassEnum RecordClass
        {
            get
            {
                return (RecordClassEnum)Convert.ToUInt16(m_mo["RecordClass"]);
            }
        }

        /// <summary>
        /// Text representation of the entire RR.
        /// </summary>
        public string TextRepresentation
        {
            get
            {
                return Convert.ToString(m_mo["TextRepresentation"]);
            }
        }

        /// <summary>
        /// Time at which the RR was last refreshed, expressed in elapsed
        /// hours since January 1, 1701, Greenwich Mean Time (GMT).
        /// </summary>
        public UnixDateTime TimeStamp
        {
            get
            {
                return new UnixDateTime( Convert.ToUInt32(m_mo["TimeStamp"]));
            }
        }

        /// <summary>
        /// Time, in seconds, that the RR can be cached by a DNS resolver.
        /// </summary>
        public TimeSpan TTL
        {
            get
            {
                return TimeSpan.FromSeconds( Convert.ToUInt32(m_mo["TTL"]));
            }
        }

        /// <summary>
        /// Returns type of record A/NS/MX etc'
        /// </summary>
        public string RecordTypeText
        {
            get
            {
                //b.root-servers.net. IN A 192.228.79.201
                var textparts = this.TextRepresentation.Split(' ');
                if (textparts.Length > 2)
                    return textparts[2];
                return null;
            }
        }

        /// <summary>
        /// Return type of record
        /// </summary>
        public ResourceRecordEnum ResourceRecordType
        {
            get
            {
                return (ResourceRecordEnum)Enum.Parse(typeof(ResourceRecordEnum), RecordTypeText);
            }
        }

        /// <summary>
        /// Returns Record type
        /// </summary>
        public Type RecordType
        {
            get
            {
                switch (this.ResourceRecordType)
                {
                    case ResourceRecordEnum.AAAA:
                        return typeof(AAAAType);
                    case ResourceRecordEnum.AFSDB:
                        return typeof(AFSDBType);
                    //case ResourceRecordEnum.ATMA:
                    //    return typeof(ATMAType);
                    case ResourceRecordEnum.A:
                        return typeof(AType);
                    case ResourceRecordEnum.CNAME:
                        return typeof(CNAMEType);
                    case ResourceRecordEnum.HINFO:
                        return typeof(HINFOType);
                    case ResourceRecordEnum.ISDN:
                        return typeof(ISDNType);
                    //case ResourceRecordEnum.KEY:
                    //    return typeof(KEYType);
                    case ResourceRecordEnum.MB:
                        return typeof(MBType);
                    case ResourceRecordEnum.MD:
                        return typeof(MDType);
                    case ResourceRecordEnum.MF:
                        return typeof(MFType);
                    case ResourceRecordEnum.MG:
                        return typeof(MGType);
                    case ResourceRecordEnum.MINFO:
                        return typeof(MINFOType);
                    case ResourceRecordEnum.MR:
                        return typeof(MRType);
                    case ResourceRecordEnum.MX:
                        return typeof(MXType);
                    case ResourceRecordEnum.NS:
                        return typeof(NSType);
                    //case ResourceRecordEnum.NXT:
                    //    return typeof(NXTType);
                    case ResourceRecordEnum.PTR:
                        return typeof(PTRType);
                    case ResourceRecordEnum.RP:
                        return typeof(RPType);
                    case ResourceRecordEnum.RT:
                        return typeof(RTType);
                    case ResourceRecordEnum.SIG:
                        return typeof(SIGType);
                    case ResourceRecordEnum.SOA:
                        return typeof(SOAType);
                    case ResourceRecordEnum.SRV:
                        return typeof(SRVType);
                    case ResourceRecordEnum.TXT:
                        return typeof(TXTType);
                    case ResourceRecordEnum.WINSR:
                        return typeof(WINSRType);
                    case ResourceRecordEnum.WINS:
                        return typeof(WINSType);
                    case ResourceRecordEnum.WKS:
                        return typeof(WKSType);
                    case ResourceRecordEnum.X25:
                        return typeof(X25Type);
                }

                throw new NotImplementedException("No implementation for record type " + this.RecordTypeText);
            }
        }

        /// <summary>
        /// Returns the underlying record
        /// </summary>
        public ResourceRecord UnderlyingRecord
        {
            get
            {
                switch (this.ResourceRecordType)
                {
                    case ResourceRecordEnum.AAAA:
                        return new AAAAType(this.m_mo);
                    case ResourceRecordEnum.AFSDB:
                        return new AFSDBType(this.m_mo);
                    //case ResourceRecordEnum.ATMA:
                    //    return new ATMAType(this.m_mo);
                    case ResourceRecordEnum.A:
                        return new AType(this.m_mo);
                    case ResourceRecordEnum.CNAME:
                        return new CNAMEType(this.m_mo);
                    case ResourceRecordEnum.HINFO:
                        return new HINFOType(this.m_mo);
                    case ResourceRecordEnum.ISDN:
                        return new ISDNType(this.m_mo);
                    //case ResourceRecordEnum.KEY:
                    //    return new KEYType(this.m_mo);
                    case ResourceRecordEnum.MB:
                        return new MBType(this.m_mo);
                    case ResourceRecordEnum.MD:
                        return new MDType(this.m_mo);
                    case ResourceRecordEnum.MF:
                        return new MFType(this.m_mo);
                    case ResourceRecordEnum.MG:
                        return new MGType(this.m_mo);
                    case ResourceRecordEnum.MINFO:
                        return new MINFOType(this.m_mo);
                    case ResourceRecordEnum.MR:
                        return new MRType(this.m_mo);
                    case ResourceRecordEnum.MX:
                        return new MXType(this.m_mo);
                    case ResourceRecordEnum.NS:
                        return new NSType(this.m_mo);
                    //case ResourceRecordEnum.NXT:
                    //    return new NXTType(this.m_mo);
                    case ResourceRecordEnum.PTR:
                        return new PTRType(this.m_mo);
                    case ResourceRecordEnum.RP:
                        return new RPType(this.m_mo);
                    case ResourceRecordEnum.RT:
                        return new RTType(this.m_mo);
                    case ResourceRecordEnum.SIG:
                        return new SIGType(this.m_mo);
                    case ResourceRecordEnum.SOA:
                        return new SOAType(this.m_mo);
                    case ResourceRecordEnum.SRV:
                        return new SRVType(this.m_mo);
                    case ResourceRecordEnum.TXT:
                        return new TXTType(this.m_mo);
                    case ResourceRecordEnum.WINSR:
                        return new WINSRType(this.m_mo);
                    case ResourceRecordEnum.WINS:
                        return new WINSType(this.m_mo);
                    case ResourceRecordEnum.WKS:
                        return new WKSType(this.m_mo);
                    case ResourceRecordEnum.X25:
                        return new X25Type(this.m_mo);
                }

                throw new NotImplementedException("No implementation for record type " + this.RecordTypeText);
            }
        }


        /// <summary>
        /// Removes the Resource from DNS
        /// </summary>
        public void Delete()
        {
            try
            {
                m_mo.Delete();
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }


        /// <summary>
        /// Parses the RR in the TextRepresentation string, and with the input
        /// DNS Server and Container Names, defines, and instantiates a 
        /// ResourceRecord object. The method returns a reference to the 
        /// new object as an output parameter. 
        /// </summary>
        /// <param name="server">Server object</param>
        /// <param name="DnsServerName">Name of the DNS Server on which the RR should be created.</param>
        /// <param name="ContainerName">Name of the container into which the new RR should be placed.</param>
        /// <param name="TextRepresentation">Text representation of the RR instance to be created.</param>
        /// <returns></returns>
        [Obsolete("Throws invalid method")]
        public static ResourceRecord CreateInstanceFromTextRepresentation(
                Server server,
                string dnsServerName,
                string containerName,
                string textRepresentation)
        {
            if (server == null)
                throw new ArgumentNullException("server is required");

            ManagementClass dnsRRClass = new ManagementClass(server.m_scope, new ManagementPath("MicrosoftDNS_ResourceRecord"), null);
            ManagementBaseObject inParams = dnsRRClass.GetMethodParameters("CreateInstanceFromTextRepresentation");
            inParams["DnsServerName"] = dnsServerName;
            inParams["ContainerName"] = containerName;
            inParams["TextRepresentation"] = textRepresentation;

            //return new ResourceRecord((ManagementObject)dnsRRClass.InvokeMethod("CreateInstanceFromTextRepresentation", inParams, null));
            return new ResourceRecord(new ManagementObject(server.m_scope, new ManagementPath(dnsRRClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null)["RR"].ToString()), null));
        }

        /// <summary>
        /// Retrieves an existing instance of the MicrosoftDns_ResourceRecord 
        /// subclass, represented by the TextRepresentation string along 
        /// with the DNS Server and Container Name. 
        /// </summary>
        /// <param name="server">Server object</param>
        /// <param name="dnsServerName">Name of the DNS Server from which the RR should be retrieved.</param>
        /// <param name="containerName">Name of the container from which the RR should be obtained.</param>
        /// <param name="textRepresentation">Text representation of the RR to be retrieved.</param>
        /// <returns></returns>
        [Obsolete("Throws invalid method")]
        public static ResourceRecord GetObjectByTextRepresentation(
                Server server,
                string dnsServerName,
                string containerName,
                string textRepresentation)
        {
            if (server == null)
                throw new ArgumentNullException("server is required");

            ManagementClass dnsRRClass = new ManagementClass(server.m_scope, new ManagementPath("MicrosoftDNS_ResourceRecord"), null);
            ManagementBaseObject inParams = dnsRRClass.GetMethodParameters("GetObjectByTextRepresentation");
            inParams["DnsServerName"] = dnsServerName;
            inParams["ContainerName"] = containerName;
            inParams["TextRepresentation"] = textRepresentation;

            //return new ResourceRecord((ManagementObject)dnsRRClass.InvokeMethod("GetObjectByTextRepresentation", inParams, null));
            try
            {
                return new ResourceRecord(new ManagementObject(server.m_scope, new ManagementPath(dnsRRClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null)["RR"].ToString()), null));
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }




        internal void ToString(StringBuilder sb)
        {
            sb.AppendLineFormat("ContainerName={0}", ContainerName);
            sb.AppendLineFormat("DnsServerName={0}", DnsServerName);
            sb.AppendLineFormat("DomainName={0}", DomainName);
            sb.AppendLineFormat("OwnerName={0}", OwnerName);
            sb.AppendLineFormat("RecordClass={0}", RecordClass);
            sb.AppendLineFormat("TextRepresentation={0}", TextRepresentation);
            sb.AppendLineFormat("TimeStamp={0}", TimeStamp);
            sb.AppendLineFormat("TTL={0}", TTL);
            sb.AppendLineFormat("RecordTypeText={0}", RecordTypeText);
            sb.AppendLineFormat("RecordType={0}", RecordType);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            ToString(sb);

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
