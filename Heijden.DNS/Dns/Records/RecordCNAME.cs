/*
 CNAME Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
/*
 * 
3.3.1. CNAME RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                     CNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

CNAME           A <domain-name> which specifies the canonical or primary
                name for the owner.  The owner name is an alias.

CNAME RRs cause no additional section processing, but name servers may
choose to restart the query at the canonical name in certain cases.  See
the description of name server logic in [RFC-1034] for details.

 * 
 */
namespace Heijden.DNS
{
    /// <summary>
    /// CNAME RDATA format
    /// <para>Alias of one name to another: the DNS lookup will continue by retrying the lookup with the new name.</para>
    /// <para>3.3.1</para>
    /// </summary>
	public class RecordCNAME : Record
	{
        /// <summary>
        /// A domain-name which specifies the canonical or primary
        /// name for the owner.  The owner name is an alias.
        /// </summary>
        public string CNAME { get; set; }

		public RecordCNAME(RecordReader rr)
		{
			CNAME = rr.ReadDomainName();
		}

		public override string ToString()
		{
			return CNAME;
		}

	}
}
