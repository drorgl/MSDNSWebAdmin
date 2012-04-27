/*
 KX Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.Globalization;
/*
 * http://tools.ietf.org/rfc/rfc2230.txt
 * 
 * 3.1 KX RDATA format

   The KX DNS record has the following RDATA format:

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                  PREFERENCE                   |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   EXCHANGER                   /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

   where:

   PREFERENCE      A 16 bit non-negative integer which specifies the
                   preference given to this RR among other KX records
                   at the same owner.  Lower values are preferred.

   EXCHANGER       A <domain-name> which specifies a host willing to
                   act as a mail exchange for the owner name.

   KX records MUST cause type A additional section processing for the
   host specified by EXCHANGER.  In the event that the host processing
   the DNS transaction supports IPv6, KX records MUST also cause type
   AAAA additional section processing.

   The KX RDATA field MUST NOT be compressed.

 */
namespace Heijden.DNS
{
    /// <summary>
    /// KX - Key eXchanger record
    /// <para>
    /// Used with some cryptographic systems (not including DNSSEC) to
    /// identify a key management agent for the associated domain-name. 
    /// Note that this has nothing to do with DNS Security. It is 
    /// Informational status, rather than being on the IETF standards-track.
    /// It has always had limited deployment, but is still in use.
    /// </para>
    /// <para>http://tools.ietf.org/html/rfc2230</para>
    /// </summary>
	public class RecordKX : Record, IComparable
	{
        public ushort PREFERENCE { get; set; }
        public string EXCHANGER { get; set; }

		public RecordKX(RecordReader rr)
		{
			PREFERENCE = rr.ReadUInt16();
			EXCHANGER = rr.ReadDomainName();
		}

		public override string ToString()
		{
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}", PREFERENCE, EXCHANGER);
		}

		public int CompareTo(object objA)
		{
			RecordKX recordKX = objA as RecordKX;
			if (recordKX == null)
				return -1;
			else if (this.PREFERENCE > recordKX.PREFERENCE)
				return 1;
			else if (this.PREFERENCE < recordKX.PREFERENCE)
				return -1;
			else // they are the same, now compare case insensitive names
				return string.Compare(this.EXCHANGER, recordKX.EXCHANGER, true);
		}

        public static bool operator ==(RecordKX left, RecordKX right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }

        public static bool operator !=(RecordKX left, RecordKX right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return (CompareTo(obj) == 0);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
	}
}
