/*
 IPSECKEY Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
/*
2.1. IPSECKEY RDATA Format


   The RDATA for an IPSECKEY RR consists of a precedence value, a
   gateway type, a public key, algorithm type, and an optional gateway
   address.


       0                   1                   2                   3
       0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |  precedence   | gateway type  |  algorithm  |     gateway     |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-------------+                 +
      ~                            gateway                            ~
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |                                                               /
      /                          public key                           /
      /                                                               /
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-|

 */

namespace Heijden.DNS
{
    /// <summary>
    /// IPSECKEY - 	IPSEC Key
    /// <para>Key record that can be used with IPSEC</para>
    /// <para>http://tools.ietf.org/html/rfc4025</para>
    /// </summary>
    //[Obsolete("not-used")]
	public class RecordIPSECKEY : Record
	{
        public byte[] RDATA { get; set; }

		public RecordIPSECKEY(RecordReader rr)
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
