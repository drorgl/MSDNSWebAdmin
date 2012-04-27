/*
 NSAP Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */


using System;
using System.Text;
using System.Globalization;
/*
 * http://tools.ietf.org/rfc/rfc1348.txt  
 * http://tools.ietf.org/html/rfc1706
 * 
 *	          |--------------|
              | <-- IDP -->  |
              |--------------|-------------------------------------|
              | AFI |  IDI   |            <-- DSP -->              |
              |-----|--------|-------------------------------------|
              | 47  |  0005  | DFI | AA |Rsvd | RD |Area | ID |Sel |
              |-----|--------|-----|----|-----|----|-----|----|----|
       octets |  1  |   2    |  1  | 3  |  2  | 2  |  2  | 6  | 1  |
              |-----|--------|-----|----|-----|----|-----|----|----|

                    IDP    Initial Domain Part
                    AFI    Authority and Format Identifier
                    IDI    Initial Domain Identifier
                    DSP    Domain Specific Part
                    DFI    DSP Format Identifier
                    AA     Administrative Authority
                    Rsvd   Reserved
                    RD     Routing Domain Identifier
                    Area   Area Identifier
                    ID     System Identifier
                    SEL    NSAP Selector

                  Figure 1: GOSIP Version 2 NSAP structure.


 */

namespace Heijden.DNS
{
    /// <summary>
    /// NSAP - Network Service Access Protocol records 	
    /// <para>
    /// The NSAP record specifies the address of a NSAP resource. NSAP 
    /// records are used to map domain names to NSAP addresses. This 
    /// record type is defined in RFC 1706.
    /// </para>
    /// </summary>
	public class RecordNSAP : Record
	{
        public ushort LENGTH { get; set; }
        public byte[] NSAPADDRESS { get; set; }

		public RecordNSAP(RecordReader rr)
		{
			LENGTH = rr.ReadUInt16();
			NSAPADDRESS = rr.ReadBytes(LENGTH);
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
            sb.AppendFormat(CultureInfo.InvariantCulture, "{0} ", LENGTH);
			for (int intI = 0; intI < NSAPADDRESS.Length; intI++)
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0:X00}", NSAPADDRESS[intI]);
			return sb.ToString();
		}

		public string ToGOSIPV2()
		{
            return string.Format(CultureInfo.InvariantCulture, "{0:X}.{1:X}.{2:X}.{3:X}.{4:X}.{5:X}.{6:X}{7:X}.{8:X}",
				NSAPADDRESS[0],							// AFI
				NSAPADDRESS[1]  << 8  | NSAPADDRESS[2],	// IDI
				NSAPADDRESS[3],							// DFI
				NSAPADDRESS[4]  << 16 | NSAPADDRESS[5] << 8 | NSAPADDRESS[6], // AA
				NSAPADDRESS[7]  << 8  | NSAPADDRESS[8],	// Rsvd
				NSAPADDRESS[9]  << 8  | NSAPADDRESS[10],// RD
				NSAPADDRESS[11] << 8  | NSAPADDRESS[12],// Area
				NSAPADDRESS[13] << 16 | NSAPADDRESS[14] << 8 | NSAPADDRESS[15], // ID-High
				NSAPADDRESS[16] << 16 | NSAPADDRESS[17] << 8 | NSAPADDRESS[18], // ID-Low
				NSAPADDRESS[19]);
		}

	}
}
