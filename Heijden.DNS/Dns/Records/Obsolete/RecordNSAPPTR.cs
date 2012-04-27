/*
 NSAPPTR Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.Globalization;
/*
 * http://tools.ietf.org/rfc/rfc1348.txt   

 * The NSAP-PTR RR

   The NSAP-PTR RR is defined with mnemonic NSAP-PTR and a type code 23
   (decimal).

   Its function is analogous to the PTR record used for IP addresses [4,7].

   NSAP-PTR has the following format:

   <NSAP-suffix> <ttl> <class> NSAP-PTR <owner>

   All fields are required.

   <NSAP-suffix> enumerates the actual octet values assigned by the
   assigning authority for the LOCAL network.  Its format in master
   files is a <character-string> syntactically identical to that used in
   TXT and HINFO.

   The format of NSAP-PTR is class insensitive.  NSAP-PTR RR causes no
   additional section processing.

   For example:

   In net ff08000574.nsap-in-addr.arpa:

   444433332222111199990123000000ff    NSAP-PTR   foo.bar.com.

   Or in net 11110031f67293.nsap-in-addr.arpa:

   67894444333322220000  NSAP-PTR        host.school.de.

   The RR data is the ASCII representation of the digits.  It is encoded
   as a <character-string>.

 */

namespace Heijden.DNS
{
	public class RecordNSAPPTR : Record
	{
        public string OWNER { get; set; }

		public RecordNSAPPTR(RecordReader rr)
		{
			OWNER = rr.ReadString();
		}

		public override string ToString()
		{
            return string.Format(CultureInfo.InvariantCulture, "{0}", OWNER);
		}
	}
}
