/*
 Structs - Structures for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */




/*
 * http://www.iana.org/assignments/dns-parameters
 */


namespace Heijden.DNS
{
    /// <summary>
    /// TYPE fields are used in resource records.
    /// <para>Note that these types are a subset of QTYPEs.</para>
    /// </summary>
	public enum Type : ushort
	{
        /// <summary>
        /// a IPV4 host address
        /// </summary>
		A = 1,		
        /// <summary>
        /// an authoritative name server
        /// </summary>
		NS = 2,
        /// <summary>
        /// a mail destination (Obsolete - use MX)
        /// </summary>
		MD = 3,	
        /// <summary>
        /// a mail forwarder (Obsolete - use MX)
        /// </summary>
		MF = 4,	
        /// <summary>
        /// the canonical name for an alias
        /// </summary>
		CNAME = 5,
        /// <summary>
        /// marks the start of a zone of authority
        /// </summary>
		SOA = 6,
        /// <summary>
        /// a mailbox domain name (EXPERIMENTAL)
        /// </summary>
		MB = 7,	
        /// <summary>
        /// a mail group member (EXPERIMENTAL)
        /// </summary>
		MG = 8,	
        /// <summary>
        /// a mail rename domain name (EXPERIMENTAL)
        /// </summary>
		MR = 9,
        /// <summary>
        /// a null RR (EXPERIMENTAL)
        /// </summary>
		NULL = 10,
        /// <summary>
        /// a well known service description
        /// </summary>
		WKS = 11,
        /// <summary>
        /// a domain name pointer
        /// </summary>
		PTR = 12,
        /// <summary>
        /// host information
        /// </summary>
		HINFO = 13,	
        /// <summary>
        /// mailbox or mail list information
        /// </summary>
		MINFO = 14,	
        /// <summary>
        /// mail exchange
        /// </summary>
		MX = 15,
        /// <summary>
        /// text strings
        /// </summary>
		TXT = 16,

        /// <summary>
        /// The Responsible Person rfc1183
        /// </summary>
		RP = 17,
        /// <summary>
        /// AFS Data Base location
        /// </summary>
		AFSDB = 18,	
        /// <summary>
        /// X.25 address rfc1183
        /// </summary>
		X25 = 19,
        /// <summary>
        /// ISDN address rfc1183 
        /// </summary>
		ISDN = 20,
        /// <summary>
        /// The Route Through rfc1183
        /// </summary>
		RT = 21,
        
        /// <summary>
        /// Network service access point address rfc1706
        /// </summary>
		NSAP = 22,
        /// <summary>
        /// Obsolete, rfc1348
        /// </summary>
		NSAPPTR = 23, 

        /// <summary>
        /// Cryptographic public key signature rfc2931 / rfc2535
        /// </summary>
		SIG = 24,
        /// <summary>
        /// Public key as used in DNSSEC rfc2535
        /// </summary>
		KEY = 25,

        /// <summary>
        /// Pointer to X.400/RFC822 mail mapping information rfc2163
        /// </summary>
		PX = 26,

        /// <summary>
        /// Geographical position rfc1712 (obsolete)
        /// </summary>
		GPOS = 27,

        /// <summary>
        /// a IPV6 host address, rfc3596
        /// </summary>
		AAAA = 28,

        /// <summary>
        /// Location information rfc1876
        /// </summary>
		LOC = 29,

        /// <summary>
        /// Next Domain, Obsolete rfc2065 / rfc2535
        /// </summary>
		NXT = 30,

        /// <summary>
        /// *** Endpoint Identifier (Patton)
        /// </summary>
		EID = 31,
        /// <summary>
        /// *** Nimrod Locator (Patton)
        /// </summary>
		NIMLOC = 32,

        /// <summary>
        /// Location of services rfc2782
        /// </summary>
		SRV = 33,

        /// <summary>
        /// *** ATM Address (Dobrowski)
        /// </summary>
		ATMA = 34,

        /// <summary>
        /// The Naming Authority Pointer rfc3403
        /// </summary>
		NAPTR = 35,	

        /// <summary>
        /// Key Exchange Delegation Record rfc2230
        /// </summary>
		KX = 36,

        /// <summary>
        /// *** CERT RFC2538
        /// </summary>
		CERT = 37,

        /// <summary>
        /// IPv6 address rfc3363 (rfc2874 rfc3226)
        /// </summary>
		A6 = 38,
        /// <summary>
        /// A way to provide aliases for a whole domain, not just a single domain name as with CNAME. rfc2672
        /// </summary>
		DNAME = 39,	

        /// <summary>
        /// *** SINK Eastlake
        /// </summary>
		SINK = 40,
        /// <summary>
        /// *** OPT RFC2671
        /// </summary>
		OPT = 41,

        /// <summary>
        /// *** APL [RFC3123]
        /// </summary>
		APL = 42,

        /// <summary>
        /// Delegation Signer rfc3658
        /// </summary>
		DS = 43,

        /// <summary>
        /// SSH Key Fingerprint rfc4255
        /// </summary>
		SSHFP = 44,	
        /// <summary>
        /// IPSECKEY rfc4025
        /// </summary>
		IPSECKEY = 45,
        /// <summary>
        /// RRSIG rfc3755
        /// </summary>
		RRSIG = 46,	
        /// <summary>
        /// NSEC rfc3755
        /// </summary>
		NSEC = 47,	
        /// <summary>
        /// DNSKEY 3755
        /// </summary>
		DNSKEY = 48,
        /// <summary>
        /// DHCID rfc4701
        /// </summary>
		DHCID = 49,	

        /// <summary>
        /// NSEC3 rfc5155
        /// </summary>
		NSEC3 = 50,	
        /// <summary>
        /// NSEC3PARAM rfc5155
        /// </summary>
		NSEC3PARAM = 51,

        /// <summary>
        /// Host Identity Protocol  [RFC-ietf-hip-dns-09.txt]
        /// </summary>
		HIP = 55,

        /// <summary>
        /// SPF rfc4408
        /// </summary>
		SPF = 99,

        /// <summary>
        /// *** IANA-Reserved
        /// </summary>
		UINFO = 100,
        /// <summary>
        /// *** IANA-Reserved
        /// </summary>
		UID = 101,
        /// <summary>
        /// *** IANA-Reserved
        /// </summary>
		GID = 102,
        /// <summary>
        /// *** IANA-Reserved
        /// </summary>
		UNSPEC = 103,

        /// <summary>
        /// Transaction key rfc2930
        /// </summary>
		TKEY = 249,	
        /// <summary>
        /// Transaction signature rfc2845
        /// </summary>
		TSIG = 250,	

        /// <summary>
        /// DNSSEC Trust Authorities          [Weiler]  13 December 2005
        /// </summary>
		TA=32768,
        /// <summary>
        /// DNSSEC Lookaside Validation       [RFC4431]
        /// </summary>
		DLV=32769	
	}

    /// <summary>
    /// QTYPE fields appear in the question part of a query.
    /// <para>QTYPES are a superset of TYPEs, hence all TYPEs are valid QTYPEs.</para>
    /// <remarks>3.2.3 QType values</remarks>
    /// </summary>
	public enum QType : ushort
	{
        /// <summary>
        /// a IPV4 host address
        /// </summary>
		A = Type.A,	
        /// <summary>
        /// an authoritative name server
        /// </summary>
		NS = Type.NS,
        /// <summary>
        /// a mail destination (Obsolete - use MX)
        /// </summary>
		MD = Type.MD,
        /// <summary>
        /// a mail forwarder (Obsolete - use MX)
        /// </summary>
		MF = Type.MF,
        /// <summary>
        /// the canonical name for an alias
        /// </summary>
		CNAME = Type.CNAME,	
        /// <summary>
        /// marks the start of a zone of authority
        /// </summary>
		SOA = Type.SOA,
        /// <summary>
        /// a mailbox domain name (EXPERIMENTAL)
        /// </summary>
		MB = Type.MB,
        /// <summary>
        /// a mail group member (EXPERIMENTAL)
        /// </summary>
		MG = Type.MG,
        /// <summary>
        /// a mail rename domain name (EXPERIMENTAL)
        /// </summary>
		MR = Type.MR,
        /// <summary>
        /// a null RR (EXPERIMENTAL)
        /// </summary>
		NULL = Type.NULL,
        /// <summary>
        /// a well known service description
        /// </summary>
		WKS = Type.WKS,
        /// <summary>
        /// a domain name pointer
        /// </summary>
		PTR = Type.PTR,	
        /// <summary>
        /// host information
        /// </summary>
		HINFO = Type.HINFO,	
        /// <summary>
        /// mailbox or mail list information
        /// </summary>
		MINFO = Type.MINFO,	
        /// <summary>
        /// mail exchange
        /// </summary>
		MX = Type.MX,	
        /// <summary>
        /// text strings
        /// </summary>
		TXT = Type.TXT,		


        /// <summary>
        /// The Responsible Person rfc1183
        /// </summary>
		RP = Type.RP,
        /// <summary>
        /// AFS Data Base location
        /// </summary>
		AFSDB = Type.AFSDB,	
        /// <summary>
        /// X.25 address rfc1183
        /// </summary>
		X25 = Type.X25,	
        /// <summary>
        /// ISDN address rfc1183
        /// </summary>
		ISDN = Type.ISDN,
        /// <summary>
        /// The Route Through rfc1183
        /// </summary>
		RT = Type.RT,

        /// <summary>
        /// Network service access point address rfc1706
        /// </summary>
		NSAP = Type.NSAP,
        /// <summary>
        /// 
        /// Obsolete, rfc1348
        /// </summary>
		NSAP_PTR = Type.NSAPPTR, 

        /// <summary>
        /// Cryptographic public key signature rfc2931 / rfc2535
        /// </summary>
		SIG = Type.SIG,	
        /// <summary>
        /// Public key as used in DNSSEC rfc2535
        /// </summary>
		KEY = Type.KEY,	

        /// <summary>
        /// Pointer to X.400/RFC822 mail mapping information rfc2163
        /// </summary>
		PX = Type.PX,

        /// <summary>
        /// Geographical position rfc1712 (obsolete)
        /// </summary>
		GPOS = Type.GPOS,	

        /// <summary>
        /// a IPV6 host address
        /// </summary>
		AAAA = Type.AAAA,

        /// <summary>
        /// Location information rfc1876
        /// </summary>
		LOC = Type.LOC,	

        /// <summary>
        /// Obsolete rfc2065 / rfc2535
        /// </summary>
		NXT = Type.NXT,		

        /// <summary>
        /// *** Endpoint Identifier (Patton)
        /// </summary>
		EID = Type.EID,	
        /// <summary>
        /// *** Nimrod Locator (Patton)
        /// </summary>
		NIMLOC = Type.NIMLOC,

        /// <summary>
        /// Location of services rfc2782
        /// </summary>
		SRV = Type.SRV,		

        /// <summary>
        /// *** ATM Address (Dobrowski)
        /// </summary>
		ATMA = Type.ATMA,	

        /// <summary>
        /// The Naming Authority Pointer rfc3403
        /// </summary>
		NAPTR = Type.NAPTR,	

        /// <summary>
        /// Key Exchange Delegation Record rfc2230
        /// </summary>
		KX = Type.KX,

        /// <summary>
        /// *** CERT RFC2538
        /// </summary>
		CERT = Type.CERT,

        /// <summary>
        /// IPv6 address rfc3363
        /// </summary>
		A6 = Type.A6,	
        /// <summary>
        /// A way to provide aliases for a whole domain, not just a single domain name as with CNAME. rfc2672
        /// </summary>
		DNAME = Type.DNAME,	

        /// <summary>
        /// *** SINK Eastlake
        /// </summary>
		SINK = Type.SINK,
        /// <summary>
        /// *** OPT RFC2671
        /// </summary>
		OPT = Type.OPT,		

        /// <summary>
        /// *** APL [RFC3123]
        /// </summary>
		APL = Type.APL,	

        /// <summary>
        /// Delegation Signer rfc3658
        /// </summary>
		DS = Type.DS,

        /// <summary>
        /// *** SSH Key Fingerprint RFC-ietf-secsh-dns
        /// </summary>
		SSHFP = Type.SSHFP,	
        /// <summary>
        /// rfc4025
        /// </summary>
		IPSECKEY = Type.IPSECKEY, 
        /// <summary>
        /// *** RRSIG RFC-ietf-dnsext-dnssec-2535
        /// </summary>
		RRSIG = Type.RRSIG,	
        /// <summary>
        /// *** NSEC RFC-ietf-dnsext-dnssec-2535
        /// </summary>
		NSEC = Type.NSEC,	
        /// <summary>
        /// *** DNSKEY RFC-ietf-dnsext-dnssec-2535
        /// </summary>
		DNSKEY = Type.DNSKEY,
        /// <summary>
        /// rfc4701
        /// </summary>
		DHCID = Type.DHCID,	

        /// <summary>
        /// RFC5155
        /// </summary>
		NSEC3 = Type.NSEC3,	
        /// <summary>
        /// RFC5155
        /// </summary>
		NSEC3PARAM = Type.NSEC3PARAM, 

        /// <summary>
        /// RFC-ietf-hip-dns-09.txt
        /// </summary>
		HIP = Type.HIP,	

        /// <summary>
        /// RFC4408
        /// </summary>
		SPF = Type.SPF,		
        /// <summary>
        /// *** IANA-Reserved
        /// </summary>
		UINFO = Type.UINFO,	
        /// <summary>
        /// *** IANA-Reserved
        /// </summary>
		UID = Type.UID,	
        /// <summary>
        /// *** IANA-Reserved
        /// </summary>
		GID = Type.GID,	
        /// <summary>
        /// *** IANA-Reserved
        /// </summary>
		UNSPEC = Type.UNSPEC,

        /// <summary>
        /// Transaction key rfc2930
        /// </summary>
		TKEY = Type.TKEY,	
        /// <summary>
        /// Transaction signature rfc2845
        /// </summary>
		TSIG = Type.TSIG,	

        /// <summary>
        /// incremental transfer                  [RFC1995]
        /// </summary>
		IXFR = 251,
        /// <summary>
        /// transfer of an entire zone            [RFC1035]
        /// </summary>
		AXFR = 252,
        /// <summary>
        /// mailbox-related RRs (MB, MG or MR)    [RFC1035]
        /// </summary>
		MAILB = 253,
        /// <summary>
        /// mail agent RRs (Obsolete - see MX)    [RFC1035]
        /// </summary>
		MAILA = 254,
        /// <summary>
        /// A request for all records             [RFC1035]
        /// </summary>
		ANY = 255,	

        /// <summary>
        /// DNSSEC Trust Authorities    [Weiler]  13 December 2005
        /// </summary>
		TA = Type.TA,		
        /// <summary>
        /// DNSSEC Lookaside Validation [RFC4431]
        /// </summary>
		DLV = Type.DLV
	}
    
    /// <summary>
    /// CLASS fields appear in resource records. 
    /// <remarks>3.2.4. CLASS values</remarks>
    /// </summary>
	public enum Class : ushort
	{
        /// <summary>
        /// the Internet
        /// </summary>
		IN = 1,	
        /// <summary>
        /// the CSNET class (Obsolete - used only for examples in some obsolete RFCs)
        /// </summary>
		CS = 2,	
        /// <summary>
        /// the CHAOS class
        /// </summary>
		CH = 3,	
        /// <summary>
        /// Hesiod [Dyer 87]
        /// </summary>
		HS = 4	
	}

    /// <summary>
    /// QCLASS values are a superset of CLASS values
    /// <para>every CLASS is a valid QCLASS.</para>
    /// <remarks>3.2.5. QCLASS values</remarks>
    /// </summary>
	public enum QClass : ushort
	{
        /// <summary>
        /// the Internet
        /// </summary>
		IN = Class.IN,
        /// <summary>
        /// the CSNET class (Obsolete - used only for examples in some obsolete RFCs)
        /// </summary>
		CS = Class.CS,
        /// <summary>
        /// the CHAOS class
        /// </summary>
		CH = Class.CH,
        /// <summary>
        /// Hesiod [Dyer 87]
        /// </summary>
		HS = Class.HS,

        /// <summary>
        /// any class
        /// </summary>
		ANY = 255
	}

    /// <summary>
    /// Response code - this 4 bit field is set as part of responses.
    /// </summary>
	public enum RCode
	{
        /// <summary>
        /// No Error                           [RFC1035]
        /// </summary>
		NoError = 0,
        /// <summary>
        /// Format Error                       [RFC1035]
        /// </summary>
		FormErr = 1,
        /// <summary>
        /// Server Failure                     [RFC1035]
        /// </summary>
		ServFail = 2,
        /// <summary>
        /// Non-Existent Domain                [RFC1035]
        /// </summary>
		NXDomain = 3,
        /// <summary>
        /// Not Implemented                    [RFC1035]
        /// </summary>
		NotImp = 4,	
        /// <summary>
        /// Query Refused                      [RFC1035]
        /// </summary>
		Refused = 5,
        /// <summary>
        /// Name Exists when it should not     [RFC2136]
        /// </summary>
		YXDomain = 6,
        /// <summary>
        /// RR Set Exists when it should not   [RFC2136]
        /// </summary>
		YXRRSet = 7,
        /// <summary>
        /// RR Set that should exist does not  [RFC2136]
        /// </summary>
		NXRRSet = 8,
        /// <summary>
        /// Server Not Authoritative for zone  [RFC2136]
        /// </summary>
		NotAuth = 9,
        /// <summary>
        /// Name not contained in zone         [RFC2136]
        /// </summary>
		NotZone = 10,

        /// <summary>
        /// Reserved
        /// </summary>
		RESERVED11 = 11,
        /// <summary>
        /// Reserved
        /// </summary>
		RESERVED12 = 12,
        /// <summary>
        /// Reserved
        /// </summary>
		RESERVED13 = 13,
        /// <summary>
        /// Reserved
        /// </summary>
		RESERVED14 = 14,
        /// <summary>
        /// Reserved
        /// </summary>
		RESERVED15 = 15,

        /// <summary>
        /// Bad OPT Version                    [RFC2671]
        /// <para>TSIG Signature Failure             [RFC2845]</para>
        /// </summary>
		BADVERSSIG = 16,
		
        /// <summary>
        /// Key not recognized                 [RFC2845]
        /// </summary>
		BADKEY = 17,
        /// <summary>
        /// Signature out of time window       [RFC2845]
        /// </summary>
		BADTIME = 18,
        /// <summary>
        /// Bad TKEY Mode                      [RFC2930]
        /// </summary>
		BADMODE = 19,
        /// <summary>
        /// Duplicate key name                 [RFC2930]
        /// </summary>
		BADNAME = 20,
        /// <summary>
        /// Algorithm not supported            [RFC2930]
        /// </summary>
		BADALG = 21,
        /// <summary>
        /// Bad Truncation                     [RFC4635]
        /// </summary>
		BADTRUNC = 22
		/*
			23-3840              available for assignment
				0x0016-0x0F00
			3841-4095            Private Use
				0x0F01-0x0FFF
			4096-65535           available for assignment
				0x1000-0xFFFF
		*/

	}

    /// <summary>
    /// A four bit field that specifies kind of query in this message.
    /// <para>This value is set by the originator of a query and copied into the response.</para>
    /// </summary>
	public enum OPCode
	{
        /// <summary>
        /// a standard query (QUERY)
        /// </summary>
		Query = 0,
        /// <summary>
        /// OpCode Retired (previously IQUERY (inverse query))
        /// <para>No further [RFC3425] assignment of this code available</para>
        /// </summary>
		IQUERY = 1,
        /// <summary>
        /// a server status request (STATUS) RFC1035
        /// </summary>
		Status = 2,
        /// <summary>
        /// IANA
        /// </summary>
		RESERVED3 = 3,

        /// <summary>
        /// RFC1996
        /// </summary>
		Notify = 4,	
        /// <summary>
        /// RFC2136
        /// </summary>
		Update = 5,	

		RESERVED6 = 6,
		RESERVED7 = 7,
		RESERVED8 = 8,
		RESERVED9 = 9,
		RESERVED10 = 10,
		RESERVED11 = 11,
		RESERVED12 = 12,
		RESERVED13 = 13,
		RESERVED14 = 14,
		RESERVED15 = 15,
	}

    /// <summary>
    /// Transport type
    /// </summary>
	public enum TransportType
	{
        /// <summary>
        /// UDP
        /// </summary>
		Udp,
        /// <summary>
        /// TCP
        /// </summary>
		Tcp
	}
}
