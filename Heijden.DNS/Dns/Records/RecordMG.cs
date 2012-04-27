/*
 MG Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
/*
3.3.6. MG RDATA format (EXPERIMENTAL)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   MGMNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

MGMNAME         A <domain-name> which specifies a mailbox which is a
                member of the mail group specified by the domain name.

MG records cause no additional section processing.
*/
namespace Heijden.DNS
{
    /// <summary>
    /// MG RDATA format (EXPERIMENTAL)
    /// <para>3.3.6</para>
    /// </summary>
	public class RecordMG : Record
	{
        /// <summary>
        /// A domain-name which specifies a mailbox which is a
        /// member of the mail group specified by the domain name.
        /// </summary>
        public string MGMNAME { get; set; }

		public RecordMG(RecordReader rr)
		{
			MGMNAME = rr.ReadDomainName();
		}

		public override string ToString()
		{
			return MGMNAME;
		}

	}
}
