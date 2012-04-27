/*
 AAAA Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.Globalization;

#region Rfc info
/*
2.2 AAAA data format

   A 128 bit IPv6 address is encoded in the data portion of an AAAA
   resource record in network byte order (high-order byte first).
 */
#endregion

namespace Heijden.DNS
{
	public class RecordAAAA : Record
	{
        private System.Net.IPAddress m_address;
        public System.Net.IPAddress Address { get { return m_address; } set { m_address = value; } }

		public RecordAAAA(RecordReader rr)
		{
			System.Net.IPAddress.TryParse(
                string.Format(CultureInfo.InvariantCulture, "{0:x}:{1:x}:{2:x}:{3:x}:{4:x}:{5:x}:{6:x}:{7:x}",
				rr.ReadUInt16(),
				rr.ReadUInt16(),
				rr.ReadUInt16(),
				rr.ReadUInt16(),
				rr.ReadUInt16(),
				rr.ReadUInt16(),
				rr.ReadUInt16(),
				rr.ReadUInt16()), out this.m_address);
		}

		public override string ToString()
		{
			return Address.ToString();
		}

	}
}
