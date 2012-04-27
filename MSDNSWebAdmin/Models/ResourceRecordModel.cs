using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DNSManagement.RR;
using DNSManagement.Extensions;

namespace MSDNSWebAdmin.Models
{
    public class ResourceRecordModel
    {
        private ResourceRecord _record;

        public ResourceRecordModel()
        {
            this._record = null;
            this.NewRecord = true;
        }

        public ResourceRecordModel(ResourceRecord resourceRecord)
        {
            this._record = resourceRecord;
            this.RecordClass = _record.RecordClass;
            this.ResourceRecordType = _record.ResourceRecordType;
            this.ContainerName = _record.ContainerName;
            this.DnsServerName = _record.DnsServerName;
            this.NewRecord = false;
        }

        public bool NewRecord { get; set; }

        public string OwnerName
        {
            get
            {
                if (_record != null)
                    return _record.OwnerName;

                return string.Empty;
            }
        }

        public string ContainerName {get;set;}


        public string DnsServerName {get;set;}
        

        public DNSManagement.RR.ResourceRecord.RecordClassEnum RecordClass {get;set;}

        public string RecordTypeText
        {
            get
            {
                if (_record != null)
                    return _record.RecordTypeText;

                return string.Empty;
            }
        }

        public DNSManagement.RR.ResourceRecord.ResourceRecordEnum ResourceRecordType {get;set;}

        public string TextRepresentation
        {
            get
            {
                if (_record != null)
                    return _record.TextRepresentation;

                return string.Empty;
            }
        }

        public UnixDateTime TimeStamp
        {
            get
            {
                if (_record != null)
                    return _record.TimeStamp;

                return new UnixDateTime(DateTime.UtcNow);
            }
        }

        public TimeSpan TTL
        {
            get
            {
                if (_record != null)
                    return _record.TTL;

                return TimeSpan.FromHours(1);
            }
        }

        public string AAAA_IPv6Address
        {
            get{
                if (_record == null)
                    return string.Empty;

                return ((AAAAType)_record.UnderlyingRecord).IPv6Address;
            }
        }

        public string AFSDB_ServerName
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((AFSDBType)_record.UnderlyingRecord).ServerName;
            }
        }

        public AFSDBType.SubtypeEnum AFSDB_Subtype
        {
            get
            {
                if (_record == null)
                    return AFSDBType.SubtypeEnum.AuthNS;

                return ((AFSDBType)_record.UnderlyingRecord).Subtype;
            }
        }


        //public string ATMA_ATMAddress
        //{
        //    get
        //    {
        //        if (_record == null)
        //            return string.Empty;

        //        return ((ATMAType)_record.UnderlyingRecord).ATMAddress;
        //    }
        //}

        //public ATMAType.AddressFormatEnum ATMA_Format
        //{
        //    get
        //    {
        //        if (_record == null)
        //            return ATMAType.AddressFormatEnum.AESA;

        //        return ((ATMAType)_record.UnderlyingRecord).Format;
        //    }
        //}

        public string A_IPAddress
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((AType)_record.UnderlyingRecord).IPAddress;
            }
        }

        public string CNAME_PrimaryName
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((CNAMEType)_record.UnderlyingRecord).PrimaryName;
            }
        }

        public string HINFO_CPU
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((HINFOType)_record.UnderlyingRecord).CPU;
            }
        }

        public string HINFO_OS
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((HINFOType)_record.UnderlyingRecord).OS;
            }
        }

        public string ISDN_SubAddress
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((ISDNType)_record.UnderlyingRecord).SubAddress;
            }
        }

        public string ISDN_ISDNNumber
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((ISDNType)_record.UnderlyingRecord).ISDNNumber;
            }
        }

        //public DNSManagement.RR.KEYType.AlgorithmEnum KEY_Algorithm
        //{
        //    get
        //    {
        //        if (_record == null)
        //            return KEYType.AlgorithmEnum.Diffie_Hellman;

        //        return ((KEYType)_record.UnderlyingRecord).Algorithm;
        //    }
        //}

        //public ushort KEY_Flags
        //{
        //    get
        //    {
        //        if (_record == null)
        //            return 0;

        //        return ((KEYType)_record.UnderlyingRecord).Flags;
        //    }
        //}

        //public DNSManagement.RR.KEYType.ProtocolEnum KEY_Protocol
        //{
        //    get
        //    {
        //        if (_record == null)
        //            return KEYType.ProtocolEnum.All;

        //        return ((KEYType)_record.UnderlyingRecord).Protocol;
        //    }
        //}

        //public string KEY_PublicKey
        //{
        //    get
        //    {
        //        if (_record == null)
        //            return string.Empty;

        //        return ((KEYType)_record.UnderlyingRecord).PublicKey;
        //    }
        //}

        public string MB_MBHost
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((MBType)_record.UnderlyingRecord).MBHost;
            }
        }

        public string MD_MDHost
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((MDType)_record.UnderlyingRecord).MDHost;
            }
        }

        public string MF_MFHost
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((MFType)_record.UnderlyingRecord).MFHost;
            }
        }

        public string MG_MGMailbox
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((MGType)_record.UnderlyingRecord).MGMailbox;
            }
        }

        public string MINFO_ErrorMailbox
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((MINFOType)_record.UnderlyingRecord).ErrorMailbox;
            }
        }

        public string MINFO_ResponsibleMailbox
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((MINFOType)_record.UnderlyingRecord).ResponsibleMailbox;
            }
        }

        public string MR_MRMailbox
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((MRType)_record.UnderlyingRecord).MRMailbox;
            }
        }

        public ushort MX_Preference
        {
            get
            {
                if (_record == null)
                    return 0;

                return ((MXType)_record.UnderlyingRecord).Preference;
            }
        }

        public string MX_MailExchange
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((MXType)_record.UnderlyingRecord).MailExchange;
            }
        }

        public string NS_NSHost
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((NSType)_record.UnderlyingRecord).NSHost;
            }
        }

        //public string NXT_NextDomainName
        //{
        //    get
        //    {
        //        if (_record == null)
        //            return string.Empty;

        //        return ((NXTType)_record.UnderlyingRecord).NextDomainName;
        //    }
        //}

        //public string NXT_Types
        //{
        //    get
        //    {
        //        if (_record == null)
        //            return string.Empty;

        //        return ((NXTType)_record.UnderlyingRecord).Types;
        //    }
        //}

        public string PTR_PTRDomainName
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((PTRType)_record.UnderlyingRecord).PTRDomainName;
            }
        }

        public string RP_RPMailbox
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((RPType)_record.UnderlyingRecord).RPMailbox;
            }
        }

        public string RP_TXTDomainName
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((RPType)_record.UnderlyingRecord).TXTDomainName;
            }
        }

        public string RT_IntermediateHost
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((RTType)_record.UnderlyingRecord).IntermediateHost;
            }
        }

        public ushort RT_Preference
        {
            get
            {
                if (_record == null)
                    return 0;

                return ((RTType)_record.UnderlyingRecord).Preference;
            }
        }

        public DNSManagement.RR.SIGType.AlgorithmEnum SIG_Algorithm
        {
            get
            {
                if (_record == null)
                    return SIGType.AlgorithmEnum.Diffie_Hellman;

                return ((SIGType)_record.UnderlyingRecord).Algorithm;
            }
        }

        public ushort SIG_KeyTag
        {
            get
            {
                if (_record == null)
                    return 0;

                return ((SIGType)_record.UnderlyingRecord).KeyTag;
            }
        }

        public ushort SIG_Labels
        {
            get
            {
                if (_record == null)
                    return 0;

                return ((SIGType)_record.UnderlyingRecord).Labels;
            }
        }

        public TimeSpan SIG_OriginalTTL
        {
            get
            {
                if (_record == null)
                    return TimeSpan.FromTicks(0);

                return ((SIGType)_record.UnderlyingRecord).OriginalTTL;
            }
        }

        public string SIG_Signature
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((SIGType)_record.UnderlyingRecord).Signature;
            }
        }

        public UnixDateTime SIG_SignatureExpiration
        {
            get
            {
                if (_record == null)
                    return new UnixDateTime(0);

                return ((SIGType)_record.UnderlyingRecord).SignatureExpiration;
            }
        }

        public UnixDateTime SIG_SignatureInception
        {
            get
            {
                if (_record == null)
                    return new UnixDateTime(0);

                return ((SIGType)_record.UnderlyingRecord).SignatureInception;
            }
        }

        public string SIG_SignerName
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((SIGType)_record.UnderlyingRecord).SignerName;
            }
        }


        public ushort SIG_TypeCovered
        {
            get
            {
                if (_record == null)
                    return 0;

                return ((SIGType)_record.UnderlyingRecord).TypeCovered;
            }
        }

        public TimeSpan SOA_ExpireLimit
        {
            get
            {
                if (_record == null)
                    return TimeSpan.FromMinutes(0);

                return ((SOAType)_record.UnderlyingRecord).ExpireLimit;
            }
        }

        public TimeSpan SOA_MinimumTTL
        {
            get
            {
                if (_record == null)
                    return TimeSpan.FromMinutes(0);

                return ((SOAType)_record.UnderlyingRecord).MinimumTTL;
            }
        }

        public string SOA_PrimaryServer
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((SOAType)_record.UnderlyingRecord).PrimaryServer;
            }
        }

        public TimeSpan SOA_RefreshInterval
        {
            get
            {
                if (_record == null)
                    return TimeSpan.FromMinutes(0);

                return ((SOAType)_record.UnderlyingRecord).RefreshInterval;
            }
        }

        public string SOA_ResponsibleParty
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((SOAType)_record.UnderlyingRecord).ResponsibleParty;
            }
        }

        public TimeSpan SOA_RetryDelay
        {
            get
            {
                if (_record == null)
                    return TimeSpan.FromMinutes(0);

                return ((SOAType)_record.UnderlyingRecord).RetryDelay;
            }
        }

        public uint SOA_SerialNumber
        {
            get
            {
                if (_record == null)
                    return 0;

                return ((SOAType)_record.UnderlyingRecord).SerialNumber;
            }
        }

        public uint SRV_Priority
        {
            get
            {
                if (_record == null)
                    return 0;

                return ((SRVType)_record.UnderlyingRecord).Priority;
            }
        }

        public uint SRV_Weight
        {
            get
            {
                if (_record == null)
                    return 0;

                return ((SRVType)_record.UnderlyingRecord).Weight;
            }
        }

        public uint SRV_Port
        {
            get
            {
                if (_record == null)
                    return 0;

                return ((SRVType)_record.UnderlyingRecord).Port;
            }
        }

        public string SRV_SRVDomainName
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((SRVType)_record.UnderlyingRecord).SRVDomainName;
            }
        }

        public string TXT_DescriptiveText
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((TXTType)_record.UnderlyingRecord).DescriptiveText;
            }
        }

        public TimeSpan WINSR_CacheTimeout
        {
            get
            {
                if (_record == null)
                    return TimeSpan.FromSeconds(0);

                return ((WINSRType)_record.UnderlyingRecord).CacheTimeout;
            }
        }

        public TimeSpan WINSR_LookupTimeout
        {
            get
            {
                if (_record == null)
                    return TimeSpan.FromSeconds(0);

                return ((WINSRType)_record.UnderlyingRecord).LookupTimeout;
            }
        }

        public DNSManagement.RR.WINSRType.MappingFlagEnum WINSR_MappingFlag
        {
            get
            {
                if (_record == null)
                    return WINSRType.MappingFlagEnum.NonReplication;

                return ((WINSRType)_record.UnderlyingRecord).MappingFlag;
            }
        }

        public string WINSR_ResultDomain
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((WINSRType)_record.UnderlyingRecord).ResultDomain;
            }
        }

        public TimeSpan WINS_CacheTimeout
        {
            get
            {
                if (_record == null)
                    return TimeSpan.FromSeconds(0);

                return ((WINSType)_record.UnderlyingRecord).CacheTimeout;
            }
        }

        public TimeSpan WINS_LookupTimeout
        {
            get
            {
                if (_record == null)
                    return TimeSpan.FromSeconds(0);

                return ((WINSType)_record.UnderlyingRecord).LookupTimeout;
            }
        }

        public DNSManagement.RR.WINSType.MappingFlagEnum WINS_MappingFlag
        {
            get
            {
                if (_record == null)
                    return WINSType.MappingFlagEnum.NonReplication;

                return ((WINSType)_record.UnderlyingRecord).MappingFlag;
            }
        }

        public string WINS_WinsServers
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((WINSType)_record.UnderlyingRecord).WinsServers;
            }
        }

        public string WKS_InternetAddress
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((WKSType)_record.UnderlyingRecord).InternetAddress;
            }
        }

        public string WKS_IPProtocol
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((WKSType)_record.UnderlyingRecord).IPProtocol;
            }
        }

        public string WKS_Services
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((WKSType)_record.UnderlyingRecord).Services;
            }
        }

        public string X25_PSDNAddress
        {
            get
            {
                if (_record == null)
                    return string.Empty;

                return ((X25Type)_record.UnderlyingRecord).PSDNAddress;
            }
        }


    }
}