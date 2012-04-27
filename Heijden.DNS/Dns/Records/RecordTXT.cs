/*
 TXT Record for DNS
 
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
3.3.14. TXT RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   TXT-DATA                    /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

TXT-DATA        One or more <character-string>s.

TXT RRs are used to hold descriptive text.  The semantics of the text
depends on the domain where it is found.
 * 
*/
#endregion

namespace Heijden.DNS
{
    /// <summary>
    /// TXT RDATA format
    /// <para>
    /// TXT RRs are used to hold descriptive text.  The semantics of the text
    /// depends on the domain where it is found.
    /// </para>
    /// </summary>
	public class RecordTXT : Record
	{
        /// <summary>
        /// One or more character-strings.
        /// </summary>
        public string TXT { get; set; }

		public RecordTXT(RecordReader rr)
		{
			TXT = rr.ReadString();
		}

		public override string ToString()
		{
            return string.Format(CultureInfo.InvariantCulture, "\"{0}\"", TXT);
		}

	}
}
