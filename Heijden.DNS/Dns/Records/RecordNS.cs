/*
 NS Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
/*
 3.3.11. NS RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   NSDNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

NSDNAME         A <domain-name> which specifies a host which should be
                authoritative for the specified class and domain.

NS records cause both the usual additional section processing to locate
a type A record, and, when used in a referral, a special search of the
zone in which they reside for glue information.

The NS RR states that the named host should be expected to have a zone
starting at owner name of the specified class.  Note that the class may
not indicate the protocol family which should be used to communicate
with the host, although it is typically a strong hint.  For example,
hosts which are name servers for either Internet (IN) or Hesiod (HS)
class information are normally queried using IN class protocols.
 */
namespace Heijden.DNS
{
    /// <summary>
    /// NS (Authoritative Name Server) RDATA format
    /// <para>3.3.11</para>
    /// </summary>
	public class RecordNS : Record
	{
        /// <summary>
        /// A domain-name which specifies a host which should be
        /// authoritative for the specified class and domain.
        /// </summary>
        public string NSDNAME { get; set; }

		public RecordNS(RecordReader rr)
		{
			NSDNAME = rr.ReadDomainName();
		}

		public override string ToString()
		{
			return NSDNAME;
		}

	}
}
