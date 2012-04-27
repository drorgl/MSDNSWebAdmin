/*
 MF Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
/*
 * 
3.3.5. MF RDATA format (Obsolete)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   MADNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

MADNAME         A <domain-name> which specifies a host which has a mail
                agent for the domain which will accept mail for
                forwarding to the domain.

MF records cause additional section processing which looks up an A type
record corresponding to MADNAME.

MF is obsolete.  See the definition of MX and [RFC-974] for details ofw
the new scheme.  The recommended policy for dealing with MD RRs found in
a master file is to reject them, or to convert them to MX RRs with a
preference of 10. */
namespace Heijden.DNS
{
    /// <summary>
    /// MF RDATA format (Obsolete)
    /// </summary>
	public class RecordMF : Record
	{
        /// <summary>
        /// A domain-name which specifies a host which has a mail
        /// agent for the domain which will accept mail for
        /// forwarding to the domain.
        /// </summary>
        public string MADNAME { get; set; }

		public RecordMF(RecordReader rr)
		{
			MADNAME = rr.ReadDomainName();
		}

		public override string ToString()
		{
			return MADNAME;
		}

	}
}
