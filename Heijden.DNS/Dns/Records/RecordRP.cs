/*
 RP Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.Globalization;
/* http://tools.ietf.org/rfc/rfc1183.txt

2.2. The Responsible Person RR

   The method uses a new RR type with mnemonic RP and type code of 17
   (decimal).

   RP has the following format:

   <owner> <ttl> <class> RP <mbox-dname> <txt-dname>

   Both RDATA fields are required in all RP RRs.

   The first field, <mbox-dname>, is a domain name that specifies the
   mailbox for the responsible person.  Its format in master files uses
   the DNS convention for mailbox encoding, identical to that used for
   the RNAME mailbox field in the SOA RR.  The root domain name (just
   ".") may be specified for <mbox-dname> to indicate that no mailbox is
   available.

   The second field, <txt-dname>, is a domain name for which TXT RR's
   exist.  A subsequent query can be performed to retrieve the
   associated TXT resource records at <txt-dname>.  This provides a
   level of indirection so that the entity can be referred to from
   multiple places in the DNS.  The root domain name (just ".") may be
   specified for <txt-dname> to indicate that the TXT_DNAME is absent,
   and no associated TXT RR exists.

 */

namespace Heijden.DNS
{
	public class RecordRP : Record
	{
        public string MBOXDNAME { get; set; }
        public string TXTDNAME { get; set; }

		public RecordRP(RecordReader rr)
		{
			//MBOXDNAME = rr.ReadString();
			MBOXDNAME = rr.ReadDomainName();
			TXTDNAME = rr.ReadDomainName();
		}

		public override string ToString()
		{
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}",
				MBOXDNAME,
				TXTDNAME);
		}

	}
}
