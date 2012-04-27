/*
 SOA Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.Globalization;

/*
3.3.13. SOA RDATA format

	+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
	/                     MNAME                     /
	/                                               /
	+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
	/                     RNAME                     /
	+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
	|                    SERIAL                     |
	|                                               |
	+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
	|                    REFRESH                    |
	|                                               |
	+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
	|                     RETRY                     |
	|                                               |
	+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
	|                    EXPIRE                     |
	|                                               |
	+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
	|                    MINIMUM                    |
	|                                               |
	+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

MNAME           The <domain-name> of the name server that was the
				original or primary source of data for this zone.

RNAME           A <domain-name> which specifies the mailbox of the
				person responsible for this zone.

SERIAL          The unsigned 32 bit version number of the original copy
				of the zone.  Zone transfers preserve this value.  This
				value wraps and should be compared using sequence space
				arithmetic.

REFRESH         A 32 bit time interval before the zone should be
				refreshed.

RETRY           A 32 bit time interval that should elapse before a
				failed refresh should be retried.

EXPIRE          A 32 bit time value that specifies the upper limit on
				the time interval that can elapse before the zone is no
				longer authoritative.

MINIMUM         The unsigned 32 bit minimum TTL field that should be
				exported with any RR from this zone.

SOA records cause no additional section processing.

All times are in units of seconds.

Most of these fields are pertinent only for name server maintenance
operations.  However, MINIMUM is used in all query operations that
retrieve RRs from a zone.  Whenever a RR is sent in a response to a
query, the TTL field is set to the maximum of the TTL field from the RR
and the MINIMUM field in the appropriate SOA.  Thus MINIMUM is a lower
bound on the TTL field for all RRs in a zone.  Note that this use of
MINIMUM should occur when the RRs are copied into the response and not
when the zone is loaded from a master file or via a zone transfer.  The
reason for this provison is to allow future dynamic update facilities to
change the SOA RR with known semantics.
*/

namespace Heijden.DNS
{
    /// <summary>
    /// SOA (Start of Authority) RDATA format
    /// <para>3.3.13</para>
    /// </summary>
	public class RecordSOA : Record
	{
        /// <summary>
        /// The domain-name of the name server that was the
        /// original or primary source of data for this zone.
        /// </summary>
        public string MNAME { get; set; }
        /// <summary>
        /// A domain-name which specifies the mailbox of the
        /// person responsible for this zone.
        /// </summary>
        public string RNAME { get; set; }
        /// <summary>
        /// The unsigned 32 bit version number of the original copy
        /// of the zone.  Zone transfers preserve this value.  This
        /// value wraps and should be compared using sequence space
        /// arithmetic.
        /// </summary>
        public uint SERIAL { get; set; }
        /// <summary>
        /// A 32 bit time interval before the zone should be
        /// refreshed.
        /// </summary>
        public uint REFRESH { get; set; }

        /// <summary>
        /// A 32 bit time interval that should elapse before a
        /// failed refresh should be retried.
        /// </summary>
        public uint RETRY { get; set; }

        /// <summary>
        /// A 32 bit time value that specifies the upper limit on
        /// the time interval that can elapse before the zone is no
        /// longer authoritative.
        /// </summary>
        public uint EXPIRE { get; set; }

        /// <summary>
        /// The unsigned 32 bit minimum TTL field that should be
        /// exported with any RR from this zone.
        /// </summary>
        public uint MINIMUM { get; set; }

		public RecordSOA(RecordReader rr)
		{
			MNAME = rr.ReadDomainName();
			RNAME = rr.ReadDomainName();
			SERIAL = rr.ReadUInt32();
			REFRESH = rr.ReadUInt32();
			RETRY = rr.ReadUInt32();
			EXPIRE = rr.ReadUInt32();
			MINIMUM = rr.ReadUInt32();
		}

		public override string ToString()
		{
            return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4} {5} {6}",
				MNAME,
				RNAME,
				SERIAL,
				REFRESH,
				RETRY,
				EXPIRE,
				MINIMUM);
		}
	}
}
