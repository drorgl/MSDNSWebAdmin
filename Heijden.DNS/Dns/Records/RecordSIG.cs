/*
 SIG Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.Globalization;

#region Rfc info
/*
 * http://www.ietf.org/rfc/rfc2535.txt
 * 4.1 SIG RDATA Format

   The RDATA portion of a SIG RR is as shown below.  The integrity of
   the RDATA information is protected by the signature field.

                           1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 3 3
       0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |        type covered           |  algorithm    |     labels    |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |                         original TTL                          |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |                      signature expiration                     |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |                      signature inception                      |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |            key  tag           |                               |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+         signer's name         +
      |                                                               /
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-/
      /                                                               /
      /                            signature                          /
      /                                                               /
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+


*/
#endregion

namespace Heijden.DNS
{
	public class RecordSIG : Record
	{
        public UInt16 TYPECOVERED { get; set; }
        public byte ALGORITHM { get; set; }
        public byte LABELS { get; set; }
        public UInt32 ORIGINALTTL { get; set; }
        public UInt32 SIGNATUREEXPIRATION { get; set; }
        public UInt32 SIGNATUREINCEPTION { get; set; }
        public UInt16 KEYTAG { get; set; }
        public string SIGNERSNAME { get; set; }
        public string SIGNATURE { get; set; }

		public RecordSIG(RecordReader rr)
		{
			TYPECOVERED = rr.ReadUInt16();
			ALGORITHM = rr.ReadByte();
			LABELS = rr.ReadByte();
			ORIGINALTTL = rr.ReadUInt32();
			SIGNATUREEXPIRATION = rr.ReadUInt32();
			SIGNATUREINCEPTION = rr.ReadUInt32();
			KEYTAG = rr.ReadUInt16();
			SIGNERSNAME = rr.ReadDomainName();
			SIGNATURE = rr.ReadString();
		}

		public override string ToString()
		{
            return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4} {5} {6} {7} \"{8}\"",
				TYPECOVERED,
				ALGORITHM,
				LABELS,
				ORIGINALTTL,
				SIGNATUREEXPIRATION,
				SIGNATUREINCEPTION,
				KEYTAG,
				SIGNERSNAME,
				SIGNATURE);
		}

	}
}
