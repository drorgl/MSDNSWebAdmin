/*
 DNSKEY Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
/*
DNSKEY RDATA Wire Format


   The RDATA for a DNSKEY RR consists of a 2 octet Flags Field, a 1
   octet Protocol Field, a 1 octet Algorithm Field, and the Public Key
   Field.

                        1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 3 3
    0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |              Flags            |    Protocol   |   Algorithm   |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   /                                                               /
   /                            Public Key                         /
   /                                                               /
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+

 */

namespace Heijden.DNS
{
    /// <summary>
    /// DNSKEY - DNS Key record
    /// <para>The key record used in DNSSEC. Uses the same format as the KEY record.</para>
    /// <para>http://tools.ietf.org/html/rfc4034</para>
    /// </summary>
    //[Obsolete("not-used")]
	public class RecordDNSKEY : Record
	{
        public byte[] RDATA { get; set; }

		public RecordDNSKEY(RecordReader rr)
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
