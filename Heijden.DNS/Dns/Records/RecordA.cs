/*
 A Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.Globalization;
/*
 3.4.1. A RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                    ADDRESS                    |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

ADDRESS         A 32 bit Internet address.

Hosts that have multiple Internet addresses will have multiple A
records.
 * 
 */
namespace Heijden.DNS
{
    /// <summary>
    /// A (Host Address) RDATA format
    /// <para>3.4.1</para>
    /// </summary>
	public class RecordA : Record
	{
        private System.Net.IPAddress m_address;
        /// <summary>
        /// A 32 bit Internet address.
        /// </summary>
        public System.Net.IPAddress Address { get {return m_address;} set{m_address = value;} }

		public RecordA(RecordReader rr)
		{
            System.Net.IPAddress.TryParse(string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}.{3}",
				rr.ReadByte(),
				rr.ReadByte(),
				rr.ReadByte(),
				rr.ReadByte()), out this.m_address);
		}

		public override string ToString()
		{
			return Address.ToString();
		}

	}
}
