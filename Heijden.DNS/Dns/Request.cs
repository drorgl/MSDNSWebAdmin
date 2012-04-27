/*
 Request object for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Heijden.DNS
{
    /// <summary>
    /// DNS Request object
    /// </summary>
	public class Request
	{
        public Header header { get; set; }

		private List<Question> questions;

		public Request()
		{
			header = new Header();
			header.OPCODE = OPCode.Query;
			header.QDCOUNT = 0;

			questions = new List<Question>();
		}

		public void AddQuestion(Question question)
		{
			questions.Add(question);
		}

		public byte[] Data
		{
			get
			{
				List<byte> data = new List<byte>();
				header.QDCOUNT = (ushort)questions.Count;
				data.AddRange(header.Data);
				foreach (Question q in questions)
					data.AddRange(q.Data);
				return data.ToArray();
			}
		}
	}
}
