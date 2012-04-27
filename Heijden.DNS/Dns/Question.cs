/*
 Request object for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Heijden.DNS
{
	#region Rfc 1034/1035
	/*
	4.1.2. Question section format

	The question section is used to carry the "question" in most queries,
	i.e., the parameters that define what is being asked.  The section
	contains QDCOUNT (usually 1) entries, each of the following format:

										1  1  1  1  1  1
		  0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
		+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
		|                                               |
		/                     QNAME                     /
		/                                               /
		+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
		|                     QTYPE                     |
		+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
		|                     QCLASS                    |
		+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

	where:

	QNAME           a domain name represented as a sequence of labels, where
					each label consists of a length octet followed by that
					number of octets.  The domain name terminates with the
					zero length octet for the null label of the root.  Note
					that this field may be an odd number of octets; no
					padding is used.

	QTYPE           a two octet code which specifies the type of the query.
					The values for this field include all codes valid for a
					TYPE field, together with some more general codes which
					can match more than one type of RR.


	QCLASS          a two octet code that specifies the class of the query.
					For example, the QCLASS field is IN for the Internet.
	*/
	#endregion

    /// <summary>
    /// Question structure for DNS
    /// </summary>
	public class Question
	{
		private string m_QName;

        /// <summary>
        /// Query Name
        /// <para>Usually domain name</para>
        /// </summary>
		public string QName
		{
			get
			{
				return m_QName;
			}
			set
			{
				m_QName = value;
				if (!m_QName.EndsWith("."))
					m_QName += ".";
			}
		}

        /// <summary>
        /// Query Type
        /// <para>Usually A/NS/SRV/SOA/MX etc'</para>
        /// </summary>
        public QType QType { get; set; }

        /// <summary>
        /// Query Class
        /// <para>Usually IN</para>
        /// </summary>
        public QClass QClass { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="QName"></param>
        /// <param name="QType"></param>
        /// <param name="QClass"></param>
		public Question(string QName,QType QType,QClass QClass)
		{
			this.QName = QName;
			this.QType = QType;
			this.QClass = QClass;
		}

        /// <summary>
        /// .ctor using record (from response)
        /// </summary>
        /// <param name="rr"></param>
		public Question(RecordReader rr)
		{
			QName = rr.ReadDomainName();
			QType = (QType)rr.ReadUInt16();
			QClass = (QClass)rr.ReadUInt16();
		}

        /// <summary>
        /// string to byte[], ASCII encoding
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
		private byte[] WriteName(string src)
		{
			if (!src.EndsWith("."))
				src += ".";

			if (src == ".")
				return new byte[1];

			StringBuilder sb = new StringBuilder();
			int intI, intJ, intLen = src.Length;
			sb.Append('\0');
			for (intI = 0, intJ = 0; intI < intLen; intI++, intJ++)
			{
				sb.Append(src[intI]);
				if (src[intI] == '.')
				{
					sb[intI - intJ] = (char)(intJ & 0xff);
					intJ = -1;
				}
			}
			sb[sb.Length - 1] = '\0';
			return System.Text.Encoding.ASCII.GetBytes(sb.ToString());
		}

        /// <summary>
        /// Retrieves Data for sending to DNS
        /// </summary>
		public byte[] Data
		{
			get
			{
				List<byte> data = new List<byte>();
				data.AddRange(WriteName(QName));
				data.AddRange(WriteShort((ushort)QType));
				data.AddRange(WriteShort((ushort)QClass));
				return data.ToArray();
			}
		}

        /// <summary>
        /// get byte array for unsigned short.
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
		private byte[] WriteShort(ushort sValue)
		{
			return BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)sValue));
		}

        /// <summary>
        /// ToString override Name Class Type
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return string.Format("{0,-32}\t{1}\t{2}", QName, QClass, QType);
		}
	}
}
