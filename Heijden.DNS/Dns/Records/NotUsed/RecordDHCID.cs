/*
 DHCID Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
/*
3.1. DHCID RDATA Format


   The RDATA section of a DHCID RR in transmission contains RDLENGTH
   octets of binary data.  The format of this data and its
   interpretation by DHCP servers and clients are described below.

   DNS software should consider the RDATA section to be opaque.  DHCP
   clients or servers use the DHCID RR to associate a DHCP client's
   identity with a DNS name, so that multiple DHCP clients and servers
   may deterministically perform dynamic DNS updates to the same zone.
   From the updater's perspective, the DHCID resource record RDATA
   consists of a 2-octet identifier type, in network byte order,



Stapp, et al.               Standards Track                     [Page 3]

 
RFC 4701                      The DHCID RR                  October 2006


   followed by a 1-octet digest type, followed by one or more octets
   representing the actual identifier:

           < 2 octets >    Identifier type code
           < 1 octet >     Digest type code
           < n octets >    Digest (length depends on digest type)

 */

namespace Heijden.DNS
{
    /// <summary>
    /// DHCID - DHCP identifier
    /// <para>Used in conjunction with the FQDN option to DHCP</para>
    /// <para>http://tools.ietf.org/html/rfc4701</para>
    /// </summary>
    //[Obsolete("not-used")]
	public class RecordDHCID : Record
	{
        public byte[] RDATA { get; set; }

		public RecordDHCID(RecordReader rr)
		{
			// re-read length
			ushort RDLENGTH = rr.ReadUInt16(-2);
			RDATA = rr.ReadBytes(RDLENGTH);
		}

		public override string ToString()
		{
			return string.Format("not-used");
		}

	}
}
