/*
 HINFO Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.Globalization;

/*
 3.3.2. HINFO RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                      CPU                      /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                       OS                      /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

CPU             A <character-string> which specifies the CPU type.

OS              A <character-string> which specifies the operating
                system type.

Standard values for CPU and OS can be found in [RFC-1010].

HINFO records are used to acquire general information about a host.  The
main use is for protocols such as FTP that can use special procedures
when talking between machines or operating systems of the same type.
 */

namespace Heijden.DNS
{
    /// <summary>
    /// System Information Record (HINFO)
    /// <para>Allows definition of the Hardware type and Operating System (OS) in use at a host.</para>
    /// </summary>
	public class RecordHINFO : Record
	{
        /// <summary>
        /// CPU
        /// </summary>
        public string CPU { get; set; }
        /// <summary>
        /// OS
        /// </summary>
        public string OS { get; set; }

		public RecordHINFO(RecordReader rr)
		{
			CPU = rr.ReadString();
			OS = rr.ReadString();
		}

		public override string ToString()
		{
            return string.Format(CultureInfo.InvariantCulture, "CPU={0} OS={1}", CPU, OS);
		}

	}
}
