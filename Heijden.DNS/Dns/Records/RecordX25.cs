/*
 X25 RR for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.Globalization;
/* http://tools.ietf.org/rfc/rfc1183.txt

3.1. The X25 RR

   The X25 RR is defined with mnemonic X25 and type code 19 (decimal).

   X25 has the following format:

   <owner> <ttl> <class> X25 <PSDN-address>

   <PSDN-address> is required in all X25 RRs.

   <PSDN-address> identifies the PSDN (Public Switched Data Network)
   address in the X.121 [10] numbering plan associated with <owner>.
   Its format in master files is a <character-string> syntactically
   identical to that used in TXT and HINFO.

   The format of X25 is class insensitive.  X25 RRs cause no additional
   section processing.

   The <PSDN-address> is a string of decimal digits, beginning with the
   4 digit DNIC (Data Network Identification Code), as specified in
   X.121. National prefixes (such as a 0) MUST NOT be used.

   For example:

   Relay.Prime.COM.  X25       311061700956


 */

namespace Heijden.DNS
{
    /// <summary>
    /// X25 RR
    /// </summary>
	public class RecordX25 : Record
	{
        /// <summary>
        /// identifies the PSDN (Public Switched Data Network)
        /// address in the X.121 [10] numbering plan associated with &lt;owner&gt;.
        /// Its format in master files is a &lt;character-string&gt; syntactically
        /// identical to that used in TXT and HINFO.
        /// </summary>
        public string PSDNADDRESS { get; set; }

		public RecordX25(RecordReader rr)
		{
			PSDNADDRESS = rr.ReadString();
		}

		public override string ToString()
		{
            return string.Format(CultureInfo.InvariantCulture, "{0}",
				PSDNADDRESS);
		}

	}
}
