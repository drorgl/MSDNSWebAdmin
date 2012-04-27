/*
 KEY Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.Globalization;

#region Rfc info
/* http://www.ietf.org/rfc/rfc2535.txt
 * 
3.1 KEY RDATA format

   The RDATA for a KEY RR consists of flags, a protocol octet, the
   algorithm number octet, and the public key itself.  The format is as
   follows:
                        1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 3 3
    0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |             flags             |    protocol   |   algorithm   |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |                                                               /
   /                          public key                           /
   /                                                               /
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-|

   The KEY RR is not intended for storage of certificates and a separate
   certificate RR has been developed for that purpose, defined in [RFC
   2538].

   The meaning of the KEY RR owner name, flags, and protocol octet are
   described in Sections 3.1.1 through 3.1.5 below.  The flags and
   algorithm must be examined before any data following the algorithm
   octet as they control the existence and format of any following data.
   The algorithm and public key fields are described in Section 3.2.
   The format of the public key is algorithm dependent.

   KEY RRs do not specify their validity period but their authenticating
   SIG RR(s) do as described in Section 4 below.

*/
#endregion

namespace Heijden.DNS
{
    /// <summary>
    /// KEY - key record
    /// <para>Used only for SIG(0) (RFC 2931) and TKEY (RFC 2930).[5] RFC 3445 
    /// eliminated their use for application keys and limited their use to 
    /// DNSSEC.[6] RFC 3755 designates DNSKEY as the replacement within DNSSEC.[7]
    /// </para>
    /// <para>http://tools.ietf.org/html/rfc2535</para>
    /// <para>http://tools.ietf.org/html/rfc2930</para>
    /// </summary>
	public class RecordKEY : Record
	{
        public UInt16 FLAGS { get; set; }
        public byte PROTOCOL { get; set; }
        public byte ALGORITHM { get; set; }
        public string PUBLICKEY { get; set; }

		public RecordKEY(RecordReader rr)
		{
			FLAGS = rr.ReadUInt16();
			PROTOCOL = rr.ReadByte();
			ALGORITHM = rr.ReadByte();
			PUBLICKEY = rr.ReadString();
		}

		public override string ToString()
		{
            return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} \"{3}\"",
				FLAGS,
				PROTOCOL,
				ALGORITHM,
				PUBLICKEY);
		}

	}
}
