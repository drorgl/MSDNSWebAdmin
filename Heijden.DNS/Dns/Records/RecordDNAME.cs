/*
 DNAME Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
/*
 * http://tools.ietf.org/rfc/rfc2672.txt
 * 
3. The DNAME Resource Record

   The DNAME RR has mnemonic DNAME and type code 39 (decimal).
   DNAME has the following format:

      <owner> <ttl> <class> DNAME <target>

   The format is not class-sensitive.  All fields are required.  The
   RDATA field <target> is a <domain-name> [DNSIS].

 * 
 */
namespace Heijden.DNS
{
    /// <summary>
    /// DNAME creates an alias for a name and all its subnames, unlike CNAME, which 
    /// aliases only the exact name in its label. Like the CNAME record, the DNS 
    /// lookup will continue by retrying the lookup with the new name.
    /// <para>http://tools.ietf.org/html/rfc2672</para>
    /// </summary>
	public class RecordDNAME : Record
	{
        /// <summary>
        /// DNAME record is the substitution of the record's
        /// target for its owner as a suffix of a domain name.
        /// </summary>
        public string TARGET { get; set; }

		public RecordDNAME(RecordReader rr)
		{
			TARGET = rr.ReadDomainName();
		}

		public override string ToString()
		{
			return TARGET;
		}

	}
}
