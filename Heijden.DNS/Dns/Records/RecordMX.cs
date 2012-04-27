/*
 MX Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.Globalization;

namespace Heijden.DNS
{
	/*
	3.3.9. MX RDATA format

		+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
		|                  PREFERENCE                   |
		+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
		/                   EXCHANGE                    /
		/                                               /
		+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

	where:

	PREFERENCE      A 16 bit integer which specifies the preference given to
					this RR among others at the same owner.  Lower values
					are preferred.

	EXCHANGE        A <domain-name> which specifies a host willing to act as
					a mail exchange for the owner name.

	MX records cause type A additional section processing for the host
	specified by EXCHANGE.  The use of MX RRs is explained in detail in
	[RFC-974].
	*/

    /// <summary>
    /// MX (Mail Exchanger) RDATA format
    /// <para>3.3.9</para>
    /// </summary>
	public class RecordMX : Record, IComparable
	{
        /// <summary>
        /// A 16 bit integer which specifies the preference given to
        /// this RR among others at the same owner.  Lower values
        /// are preferred.
        /// </summary>
        public ushort PREFERENCE { get; set; }

        /// <summary>
        /// A domain-name which specifies a host willing to act as
        /// a mail exchange for the owner name.
        /// </summary>
        public string EXCHANGE { get; set; }

		public RecordMX(RecordReader rr)
		{
			PREFERENCE = rr.ReadUInt16();
			EXCHANGE = rr.ReadDomainName();
		}

		public override string ToString()
		{
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}", PREFERENCE, EXCHANGE);
		}

		public int CompareTo(object objA)
		{
			RecordMX recordMX = objA as RecordMX;
			if (recordMX == null)
				return -1;
			else if (this.PREFERENCE > recordMX.PREFERENCE)
				return 1;
			else if (this.PREFERENCE < recordMX.PREFERENCE)
				return -1;
			else // they are the same, now compare case insensitive names
				return string.Compare(this.EXCHANGE, recordMX.EXCHANGE, true);
		}

	}
}
