/*
 MB Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */


using System;
/*
3.3.3. MB RDATA format (EXPERIMENTAL)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   MADNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

MADNAME         A <domain-name> which specifies a host which has the
                specified mailbox.

MB records cause additional section processing which looks up an A type
RRs corresponding to MADNAME.
*/
namespace Heijden.DNS
{
    /// <summary>
    /// MB (Mailbox) RDATA format (EXPERIMENTAL)
    /// <para>3.3.3</para>
    /// </summary>
	public class RecordMB : Record
	{
        /// <summary>
        /// A domain-name which specifies a host which has the
        /// specified mailbox.
        /// </summary>
        public string MADNAME { get; set; }

		public RecordMB(RecordReader rr)
		{
			MADNAME = rr.ReadDomainName();
		}

		public override string ToString()
		{
			return MADNAME;
		}

	}
}
