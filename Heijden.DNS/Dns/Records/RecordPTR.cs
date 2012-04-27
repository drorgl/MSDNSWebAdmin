/*
 PTR Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
/*
 3.3.12. PTR RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   PTRDNAME                    /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

PTRDNAME        A <domain-name> which points to some location in the
                domain name space.

PTR records cause no additional section processing.  These RRs are used
in special domains to point to some other location in the domain space.
These records are simple data, and don't imply any special processing
similar to that performed by CNAME, which identifies aliases.  See the
description of the IN-ADDR.ARPA domain for an example.
 */

namespace Heijden.DNS
{
    /// <summary>
    /// PTR (Domain Name Pointer) RDATA format
    /// <para>3.3.12</para>
    /// </summary>
	public class RecordPTR : Record
	{
        /// <summary>
        /// A domain-name which points to some location in the
        /// domain name space.
        /// </summary>
        public string PTRDNAME { get; set; }

		public RecordPTR(RecordReader rr)
		{
			PTRDNAME = rr.ReadDomainName();
		}

		public override string ToString()
		{
			return PTRDNAME;
		}

	}
}
