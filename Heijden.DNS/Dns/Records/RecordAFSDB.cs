/*
 AFSDB Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.Globalization;
/* http://tools.ietf.org/rfc/rfc1183.txt

 * 1. AFS Data Base location

   This section defines an extension of the DNS to locate servers both
   for AFS (AFS is a registered trademark of Transarc Corporation) and
   for the Open Software Foundation's (OSF) Distributed Computing
   Environment (DCE) authenticated naming system using HP/Apollo's NCA,
   both to be components of the OSF DCE.  The discussion assumes that
   the reader is familiar with AFS [5] and NCA [6].

   The AFS (originally the Andrew File System) system uses the DNS to
   map from a domain name to the name of an AFS cell database server.
   The DCE Naming service uses the DNS for a similar function: mapping
   from the domain name of a cell to authenticated name servers for that
   cell.  The method uses a new RR type with mnemonic AFSDB and type
   code of 18 (decimal).

   AFSDB has the following format:

   <owner> <ttl> <class> AFSDB <subtype> <hostname>

   Both RDATA fields are required in all AFSDB RRs.  The <subtype> field
   is a 16 bit integer.  The <hostname> field is a domain name of a host
   that has a server for the cell named by the owner name of the RR.

 */

namespace Heijden.DNS
{
    /// <summary>
    /// AFS (Andrew File System) Data Base location
    /// <para>http://tools.ietf.org/rfc/rfc1183.txt</para>
    /// </summary>
	public class RecordAFSDB : Record
	{
        /// <summary>
        /// sub-type indicates server type
        /// <para>
        /// 1 = AFS version 3.0 volume location server for the named AFS cell.
        /// 2 = DCE authenticated server. 
        /// </para>
        /// </summary>
        public ushort SUBTYPE { get; set; }
        /// <summary>
        /// A-record for the database server
        /// </summary>
        public string HOSTNAME { get; set; }

		public RecordAFSDB(RecordReader rr)
		{
			SUBTYPE = rr.ReadUInt16();
			//HOSTNAME = rr.ReadString();
			HOSTNAME = rr.ReadDomainName();
		}

		public override string ToString()
		{
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}",
				SUBTYPE,
				HOSTNAME);
		}

	}
}
