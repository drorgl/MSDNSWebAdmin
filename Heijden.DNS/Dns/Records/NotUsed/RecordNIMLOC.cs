/*
 NIMLOC Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
/*
1.2. The NIMLOC (Nimrod Locator) RR

   The NIMLOC (Nimrod Locator) RR is defined with mnemonic "NIMLOC"
   and TYPE code 32 (decimal) and is used to map from domain
   names to Nimrod Locators.  Nimrod Locators are possibly variable
   length strings of octets whose content is only meaningful to the
   Nimrod routing system.  Since the top level RR format and semantics
   as defined in Section 3.2.1 of RFC 1035 include a length indicator,
   the Domain Name System is not required to understand any internal
   structure.

   A Nimrod system may have any number of Locators associated with it.
   They are in this sense like A and AAAA RRs for IPv4 and IPv6
   addresses.  Multiple NIMLOC RRs with the same NAME, CLASS and RDATA
   are the same and can be merged in a cache, retaining only the
   highest TTL.

   The format of a Nimrod Locator (NIMLOC) RR is:

                                            1  1  1  1  1  1
              0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
           +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
           |                                               |
           /                                               /
           /                        NAME                   /
           |                                               |
           +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
           |                    TYPE = NIMLOC              |
           +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
           |                    CLASS = IN                 |
           +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
           |                        TTL                    |
           |                                               |
           +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
           |                      RDLENGTH                 |
           +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
           /                       RDATA                   /
           /                                               /
           +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

   where:

   *  NAME: an owner name, i.e., the name of the DNS node to which this
      resource record pertains.

   *  TYPE: two octets containing the NIMLOC RR TYPE code of 32 (decimal).

   *  CLASS: two octets containing the RR IN CLASS code of 1.

   *  TTL: a 32 bit signed integer that specifies the time interval in
      seconds that the resource record may be cached before the source
      of the information should again be consulted.

   *  RDLENGTH: an unsigned 16 bit integer that specifies the length in
      octets of the RDATA field.

   *  RDATA: a variable length string of octets containing the Nimrod
      Locator.  The value is the binary encoding of the Locator
      specified in the Nimrod protocol[[[ref to be supplied]]].


 */

namespace Heijden.DNS
{
    /// <summary>
    /// The NIMLOC (Nimrod Locator) RR
    /// </summary>
    //[Obsolete("not-used")]
	public class RecordNIMLOC : Record
	{
        public byte[] RDATA { get; set; }

		public RecordNIMLOC(RecordReader rr)
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
