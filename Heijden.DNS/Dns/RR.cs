/*
 RR definitions for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */


using System;
using System.Globalization;

namespace Heijden.DNS
{
	#region RFC info
	/*
	3.2. RR definitions

	3.2.1. Format

	All RRs have the same top level format shown below:

										1  1  1  1  1  1
		  0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
		+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
		|                                               |
		/                                               /
		/                      NAME                     /
		|                                               |
		+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
		|                      TYPE                     |
		+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
		|                     CLASS                     |
		+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
		|                      TTL                      |
		|                                               |
		+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
		|                   RDLENGTH                    |
		+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--|
		/                     RDATA                     /
		/                                               /
		+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+


	where:

	NAME            an owner name, i.e., the name of the node to which this
					resource record pertains.

	TYPE            two octets containing one of the RR TYPE codes.

	CLASS           two octets containing one of the RR CLASS codes.

	TTL             a 32 bit signed integer that specifies the time interval
					that the resource record may be cached before the source
					of the information should again be consulted.  Zero
					values are interpreted to mean that the RR can only be
					used for the transaction in progress, and should not be
					cached.  For example, SOA records are always distributed
					with a zero TTL to prohibit caching.  Zero values can
					also be used for extremely volatile data.

	RDLENGTH        an unsigned 16 bit integer that specifies the length in
					octets of the RDATA field.

	RDATA           a variable length string of octets that describes the
					resource.  The format of this information varies
					according to the TYPE and CLASS of the resource record.
	*/
	#endregion

	/// <summary>
	/// Resource Record (rfc1034 3.6.)
	/// </summary>
	public class RR
	{
		/// <summary>
		/// The name of the node to which this resource record pertains
		/// </summary>
        public string NAME { get; set; }

		/// <summary>
		/// Specifies type of resource record
		/// </summary>
        public Type Type { get; set; }

		/// <summary>
		/// Specifies type class of resource record, mostly IN but can be CS, CH or HS 
		/// </summary>
        public Class Class { get; set; }

		/// <summary>
		/// Time to live, the time interval that the resource record may be cached
		/// </summary>
		public uint TTL
		{
			get
			{
				return (uint)Math.Max(0, m_TTL - TimeLived);
			}
			set
			{
				m_TTL = value;
			}
		}

		private uint m_TTL;

		/// <summary>
		/// Length of original RDATA
		/// </summary>
		public ushort RDLENGTH;

		/// <summary>
		/// One of the Record* classes
        /// <para>typed RDATA</para>
		/// </summary>
		public Record RECORD;

        
        public int TimeLived { get; set; }

		public RR(RecordReader rr)
		{
			TimeLived = 0;
			NAME = rr.ReadDomainName();
			Type = (Type)rr.ReadUInt16();
			Class = (Class)rr.ReadUInt16();
			TTL = rr.ReadUInt32();
			RDLENGTH = rr.ReadUInt16();
			RECORD = rr.ReadRecord(Type);
			RECORD.RR = this;
		}

		public override string ToString()
		{
            return string.Format(CultureInfo.InvariantCulture, "{0,-32} {1}\t{2}\t{3}\t{4}",
				NAME,
				TTL,
				Class,
				Type,
				RECORD);
		}
	}

	public class AnswerRR : RR
	{
		public AnswerRR(RecordReader br)
			: base(br)
		{
		}
	}

	public class AuthorityRR : RR
	{
		public AuthorityRR(RecordReader br)
			: base(br)
		{
		}
	}

	public class AdditionalRR : RR
	{
		public AdditionalRR(RecordReader br)
			: base(br)
		{
		}
	}
}
