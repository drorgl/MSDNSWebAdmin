/*
 HIP Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
/*
5. HIP RR Storage Format


   The RDATA for a HIP RR consists of a public key algorithm type, the
   HIT length, a HIT, a public key, and optionally one or more
   rendezvous server(s).

    0                   1                   2                   3
    0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |  HIT length   | PK algorithm  |          PK length            |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |                                                               |
   ~                           HIT                                 ~
   |                                                               |
   +                     +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |                     |                                         |
   +-+-+-+-+-+-+-+-+-+-+-+                                         +
   |                           Public Key                          |
   ~                                                               ~
   |                                                               |
   +                               +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |                               |                               |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+                               +
   |                                                               |
   ~                       Rendezvous Servers                      ~
   |                                                               |
   +             +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |             |
   +-+-+-+-+-+-+-+

 */

namespace Heijden.DNS
{
    /// <summary>
    /// HIP - Host Identity Protocol
    /// <para>Method of separating the end-point identifier and locator roles of IP addresses.</para>
    /// <para>http://tools.ietf.org/html/rfc5205</para>
    /// </summary>
    //[Obsolete("not-used")]
	public class RecordHIP : Record
	{
        public byte[] RDATA { get; set; }

		public RecordHIP(RecordReader rr)
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
