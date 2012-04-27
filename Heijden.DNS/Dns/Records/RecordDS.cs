/*
 DS Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.Text;
using System.Globalization;
/*
 * http://tools.ietf.org/rfc/rfc3658.txt
 * 
2.4.  Wire Format of the DS record

   The DS (type=43) record contains these fields: key tag, algorithm,
   digest type, and the digest of a public key KEY record that is
   allowed and/or used to sign the child's apex KEY RRset.  Other keys
   MAY sign the child's apex KEY RRset.

                        1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 3 3
    0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |           key tag             |  algorithm    |  Digest type  |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |                digest  (length depends on type)               |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |                (SHA-1 digest is 20 bytes)                     |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |                                                               |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-|
   |                                                               |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-|
   |                                                               |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+

 */

namespace Heijden.DNS
{
    /// <summary>
    /// DS - Delegation signer
    /// <para>The record used to identify the DNSSEC signing key of a delegated zone</para>
    /// <para>http://tools.ietf.org/html/rfc4034</para>
    /// </summary>
	public class RecordDS : Record
	{
        /// <summary>
        /// lists the key tag of the DNSKEY RR referred to by the DS record, in network byte order.
        /// </summary>
        public UInt16 KEYTAG { get; set; }
        /// <summary>
        /// lists the algorithm number of the DNSKEY RR referred to by the DS record.
        /// <para>
        /// The algorithm number used by the DS RR is identical to the algorithm
        /// number used by RRSIG and DNSKEY RRs.
        /// </para>
        /// </summary>
        public byte ALGORITHM { get; set; }
        /// <summary>
        /// The Digest Type field identifies the algorithm used to construct the digest.
        /// </summary>
        public byte DIGESTTYPE { get; set; }
        /// <summary>
        ///  The digest is calculated by concatenating the canonical form of the
        /// fully qualified owner name of the DNSKEY RR with the DNSKEY RDATA,
        /// and then applying the digest algorithm.
        /// <para>digest = digest_algorithm( DNSKEY owner name | DNSKEY RDATA)</para>
        /// </summary>
        public byte[] DIGEST { get; set; }

		public RecordDS(RecordReader rr)
		{
			ushort length = rr.ReadUInt16(-2);
			KEYTAG = rr.ReadUInt16();
			ALGORITHM = rr.ReadByte();
			DIGESTTYPE = rr.ReadByte();
			length -= 4;
			DIGEST = new byte[length];
			DIGEST = rr.ReadBytes(length);
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			for (int intI = 0; intI < DIGEST.Length; intI++)
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0:x2}", DIGEST[intI]);
            return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3}",
				KEYTAG,
				ALGORITHM,
				DIGESTTYPE,
				sb.ToString());
		}

	}
}
