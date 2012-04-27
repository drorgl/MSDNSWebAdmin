/*
 NULL Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */


using System;
using System.Globalization;
/*
3.3.10. NULL RDATA format (EXPERIMENTAL)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                  <anything>                   /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

Anything at all may be in the RDATA field so long as it is 65535 octets
or less.

NULL records cause no additional section processing.  NULL RRs are not
allowed in master files.  NULLs are used as placeholders in some
experimental extensions of the DNS.
*/
namespace Heijden.DNS
{
    /// <summary>
    /// NULL RDATA format (EXPERIMENTAL)
    /// <para>3.3.10</para>
    /// </summary>
	public class RecordNULL : Record
	{
        /// <summary>
        /// Anything at all may be in the RDATA field so long as it is 65535 octets
        /// or less.
        /// </summary>
        public byte[] ANYTHING { get; set; }

		public RecordNULL(RecordReader rr)
		{
			rr.Position -= 2;
			// re-read length
			ushort RDLENGTH = rr.ReadUInt16();
			ANYTHING = new byte[RDLENGTH];
			ANYTHING = rr.ReadBytes(RDLENGTH);
		}

		public override string ToString()
		{
            return string.Format(CultureInfo.InvariantCulture, "...binary data... ({0}) bytes", ANYTHING.Length);
		}

	}
}
