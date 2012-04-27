/*
 TSIG Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.Globalization;
/*
 * http://www.ietf.org/rfc/rfc2845.txt
 * 
 * Field Name       Data Type      Notes
      --------------------------------------------------------------
      Algorithm Name   domain-name    Name of the algorithm
                                      in domain name syntax.
      Time Signed      u_int48_t      seconds since 1-Jan-70 UTC.
      Fudge            u_int16_t      seconds of error permitted
                                      in Time Signed.
      MAC Size         u_int16_t      number of octets in MAC.
      MAC              octet stream   defined by Algorithm Name.
      Original ID      u_int16_t      original message ID
      Error            u_int16_t      expanded RCODE covering
                                      TSIG processing.
      Other Len        u_int16_t      length, in octets, of
                                      Other Data.
      Other Data       octet stream   empty unless Error == BADTIME

 */

namespace Heijden.DNS
{
    /// <summary>
    /// Secret Key Transaction Authentication for DNS (TSIG)
    /// </summary>
	public class RecordTSIG : Record
	{
        /// <summary>
        /// Name of the algorithm in domain name syntax.
        /// </summary>
        public string ALGORITHMNAME { get; set; }
        /// <summary>
        /// seconds since 1-Jan-70 UTC.
        /// </summary>
        public long TIMESIGNED { get; set; }
        /// <summary>
        /// seconds of error permitted in Time Signed.
        /// </summary>
        public UInt16 FUDGE { get; set; }
        /// <summary>
        /// number of octets in MAC.
        /// </summary>
        public UInt16 MACSIZE { get; set; }
        /// <summary>
        /// defined by Algorithm Name.
        /// </summary>
        public byte[] MAC { get; set; }
        /// <summary>
        /// original message ID
        /// </summary>
        public UInt16 ORIGINALID { get; set; }
        /// <summary>
        /// expanded RCODE covering TSIG processing.
        /// </summary>
        public UInt16 ERROR { get; set; }
        /// <summary>
        /// length, in octets, of Other Data.
        /// </summary>
        public UInt16 OTHERLEN { get; set; }
        /// <summary>
        /// empty unless Error == BADTIME
        /// </summary>
        public byte[] OTHERDATA { get; set; }

		public RecordTSIG(RecordReader rr)
		{
			ALGORITHMNAME = rr.ReadDomainName();
			TIMESIGNED = rr.ReadUInt32() << 32 | rr.ReadUInt32();
			FUDGE = rr.ReadUInt16();
			MACSIZE = rr.ReadUInt16();
			MAC = rr.ReadBytes(MACSIZE);
			ORIGINALID = rr.ReadUInt16();
			ERROR = rr.ReadUInt16();
			OTHERLEN = rr.ReadUInt16();
			OTHERDATA = rr.ReadBytes(OTHERLEN);
		}

		public override string ToString()
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			dateTime = dateTime.AddSeconds(TIMESIGNED);
			string printDate = dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
            return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4}",
				ALGORITHMNAME,
				printDate,
				FUDGE,
				ORIGINALID,
				ERROR);
		}

	}
}
