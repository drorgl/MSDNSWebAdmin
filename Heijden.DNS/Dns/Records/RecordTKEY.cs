/*
 TKEY Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.Globalization;
/*
 * http://tools.ietf.org/rfc/rfc2930.txt
 * 
2. The TKEY Resource Record

   The TKEY resource record (RR) has the structure given below.  Its RR
   type code is 249.

      Field       Type         Comment
      -----       ----         -------
       Algorithm:   domain
       Inception:   u_int32_t
       Expiration:  u_int32_t
       Mode:        u_int16_t
       Error:       u_int16_t
       Key Size:    u_int16_t
       Key Data:    octet-stream
       Other Size:  u_int16_t
       Other Data:  octet-stream  undefined by this specification

 */

namespace Heijden.DNS
{
    /// <summary>
    /// Secret Key Establishment for DNS (TKEY RR)
    /// <para>http://tools.ietf.org/rfc/rfc2930.txt</para>
    /// </summary>
	public class RecordTKEY : Record
	{
        /// <summary>
        /// The algorithm name is in the form of a domain name with the same
        /// meaning as in [RFC 2845].  
        /// <para>
        /// The algorithm determines how the secret
        /// keying material agreed to using the TKEY RR is actually used to
        /// derive the algorithm specific key.
        /// </para>
        /// </summary>
        public string ALGORITHM { get; set; }

        /// <summary>
        /// The inception time and expiration times are in number of seconds
        /// since the beginning of 1 January 1970 GMT ignoring leap seconds
        /// treated as modulo 2**32 using ring arithmetic [RFC 1982].
        /// <para>
        /// In messages
        ///   between a DNS resolver and a DNS server where these fields are
        ///   meaningful, they are either the requested validity interval for the
        ///   keying material asked for or specify the validity interval of keying
        ///   material provided.
        ///
        ///   To avoid different interpretations of the inception and expiration
        ///   times in TKEY RRs, resolvers and servers exchanging them must have
        ///   the same idea of what time it is.  One way of doing this is with the
        ///   NTP protocol [RFC 2030] but that or any other time synchronization
        ///   used for this purpose MUST be done securely.
        /// </para>
        /// </summary>
        public UInt32 INCEPTION { get; set; }

        /// <summary>
        /// See Inception
        /// </summary>
        public UInt32 EXPIRATION { get; set; }

        /// <summary>
        /// The mode field specifies the general scheme for key agreement or the
        ///   purpose of the TKEY DNS message.
        /// <para>
        ///         0        - reserved, see section 7
        ///          1       server assignment
        ///          2       Diffie-Hellman exchange
        ///          3       GSS-API negotiation
        ///          4       resolver assignment
        ///          5       key deletion
        ///         6-65534   - available, see section 7
        ///         65535     - reserved, see section 7
        /// </para>
        /// </summary>
        public UInt16 MODE { get; set; }

        /// <summary>
        /// The error code field is an extended RCODE.
        /// <para>
        ///  0       - no error
        ///  1-15   a non-extended RCODE
        ///  16     BADSIG   (TSIG)
        ///  17     BADKEY   (TSIG)
        ///  18     BADTIME  (TSIG)
        ///  19     BADMODE
        ///  20     BADNAME
        ///  21     BADALG
        /// </para>
        /// </summary>
        public UInt16 ERROR { get; set; }

        /// <summary>
        ///  The key data size field is an unsigned 16 bit integer in network
        ///   order which specifies the size of the key exchange data field in
        ///   octets. The meaning of this data depends on the mode.
        /// </summary>
        public UInt16 KEYSIZE { get; set; }
        /// <summary>
        /// See KeySize
        /// </summary>
        public byte[] KEYDATA { get; set; }
        /// <summary>
        /// The Other Size and Other Data fields are not used in this
        ///   specification but may be used in future extensions.  The RDLEN field
        ///   MUST equal the length of the RDATA section through the end of Other
        ///   Data or the RR is to be considered malformed and rejected.
        /// </summary>
        public UInt16 OTHERSIZE { get; set; }
        /// <summary>
        /// See OtherSize
        /// </summary>
        public byte[] OTHERDATA { get; set; }

		public RecordTKEY(RecordReader rr)
		{
			ALGORITHM = rr.ReadDomainName();
			INCEPTION = rr.ReadUInt32();
			EXPIRATION = rr.ReadUInt32();
			MODE = rr.ReadUInt16();
			ERROR = rr.ReadUInt16();
			KEYSIZE = rr.ReadUInt16();
			KEYDATA = rr.ReadBytes(KEYSIZE);
			OTHERSIZE = rr.ReadUInt16();
			OTHERDATA = rr.ReadBytes(OTHERSIZE);
		}

		public override string ToString()
		{
            return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4}",
				ALGORITHM,
				INCEPTION,
				EXPIRATION,
				MODE,
				ERROR);
		}

	}
}
