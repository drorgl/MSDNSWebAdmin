/*
 NSEC3PARAM Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
/*
4.2. NSEC3PARAM RDATA Wire Format


   The RDATA of the NSEC3PARAM RR is as shown below:

                        1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 3 3
    0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |   Hash Alg.   |     Flags     |          Iterations           |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |  Salt Length  |                     Salt                      /
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+

   Hash Algorithm is a single octet.

   Flags field is a single octet.

   Iterations is represented as a 16-bit unsigned integer, with the most
   significant bit first.

   Salt Length is represented as an unsigned octet.  Salt Length
   represents the length of the following Salt field in octets.  If the
   value is zero, the Salt field is omitted.

   Salt, if present, is encoded as a sequence of binary octets.  The
   length of this field is determined by the preceding Salt Length
   field.


 */

namespace Heijden.DNS
{
    /// <summary>
    /// NSEC3PARAM - NSEC3 parameters
    /// <para>Parameter record for use with NSEC3</para>
    /// <para>http://tools.ietf.org/html/rfc5155</para>
    /// </summary>
    //[Obsolete("not-used")]
	public class  RecordNSEC3PARAM : Record
	{
        public byte[] RDATA { get; set; }

		public RecordNSEC3PARAM(RecordReader rr)
		{
			// re-read length
			ushort RDLENGTH = rr.ReadUInt16(-2);
			RDATA = rr.ReadBytes(RDLENGTH);
		}

		public override string ToString()
		{
			return string.Format("not-used");
		}

	}
}
