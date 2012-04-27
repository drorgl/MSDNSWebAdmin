/*
 GPOS Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.Globalization;
/* 
 * http://tools.ietf.org/rfc/rfc1712.txt
 * 
3. RDATA Format

        MSB                                        LSB
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        /                 LONGITUDE                  /
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        /                  LATITUDE                  /
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        /                  ALTITUDE                  /
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

   where:

   LONGITUDE The real number describing the longitude encoded as a
             printable string. The precision is limited by 256 charcters
             within the range -90..90 degrees. Positive numbers
             indicate locations north of the equator.

   LATITUDE The real number describing the latitude encoded as a
            printable string. The precision is limited by 256 charcters
            within the range -180..180 degrees. Positive numbers
            indicate locations east of the prime meridian.

   ALTITUDE The real number describing the altitude (in meters) from
            mean sea-level encoded as a printable string. The precision
            is limited by 256 charcters. Positive numbers indicate
            locations above mean sea-level.

   Latitude/Longitude/Altitude values are encoded as strings as to avoid
   the precision limitations imposed by encoding as unsigned integers.
   Although this might not be considered optimal, it allows for a very
   high degree of precision with an acceptable average encoded record
   length.

 */

namespace Heijden.DNS
{
    /// <summary>
    /// GPOS - The geographical location
    /// <para>http://tools.ietf.org/rfc/rfc1712.txt</para>
    /// </summary>
	public class RecordGPOS : Record
	{
        /// <summary>
        /// The real number describing the longitude encoded as a
        /// printable string. The precision is limited by 256 charcters
        /// within the range -90..90 degrees. Positive numbers
        /// indicate locations north of the equator.
        /// </summary>
        public string LONGITUDE { get; set; }
        /// <summary>
        /// The real number describing the latitude encoded as a
        /// printable string. The precision is limited by 256 charcters
        /// within the range -180..180 degrees. Positive numbers
        /// indicate locations east of the prime meridian.
        /// </summary>
        public string LATITUDE { get; set; }
        /// <summary>
        /// The real number describing the altitude (in meters) from
        /// mean sea-level encoded as a printable string. The precision
        /// is limited by 256 charcters. Positive numbers indicate
        /// locations above mean sea-level.
        /// </summary>
        public string ALTITUDE { get; set; }

		public RecordGPOS(RecordReader rr)
		{
			LONGITUDE = rr.ReadString();
			LATITUDE = rr.ReadString();
			ALTITUDE = rr.ReadString();
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture,"{0} {1} {2}",
				LONGITUDE,
				LATITUDE,
				ALTITUDE);
		}

	}
}
