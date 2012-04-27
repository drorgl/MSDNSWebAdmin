/*
 MR Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
/*
3.3.8. MR RDATA format (EXPERIMENTAL)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   NEWNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

NEWNAME         A <domain-name> which specifies a mailbox which is the
                proper rename of the specified mailbox.

MR records cause no additional section processing.  The main use for MR
is as a forwarding entry for a user who has moved to a different
mailbox.
*/
namespace Heijden.DNS
{
    /// <summary>
    /// MR (Renamed Mailbox) RDATA format (EXPERIMENTAL)
    /// 3.3.8
    /// </summary>
	public class RecordMR : Record
	{
        /// <summary>
        /// A domain-name which specifies a mailbox which is the
        /// proper rename of the specified mailbox.
        /// </summary>
        public string NEWNAME { get; set; }

		public RecordMR(RecordReader rr)
		{
			NEWNAME = rr.ReadDomainName();
		}

		public override string ToString()
		{
			return NEWNAME;
		}

	}
}
