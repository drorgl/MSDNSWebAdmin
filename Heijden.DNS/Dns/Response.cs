/*
 Response object for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Heijden.DNS
{
    /// <summary>
    /// DNS Resonse object
    /// </summary>
	public class Response
	{
		/// <summary>
		/// List of Question records
		/// </summary>
        public List<Question> Questions { get; private set; }
		/// <summary>
		/// List of AnswerRR records
		/// </summary>
        public List<AnswerRR> Answers { get; private set; }
		/// <summary>
		/// List of AuthorityRR records
		/// </summary>
        public List<AuthorityRR> Authorities { get; private set; }
		/// <summary>
		/// List of AdditionalRR records
		/// </summary>
        public List<AdditionalRR> Additionals { get; private set; }

        public Header header { get; set; }

		/// <summary>
		/// Error message, empty when no error
		/// </summary>
        public string Error { get; set; }

		/// <summary>
		/// The Size of the message
		/// </summary>
        public int MessageSize { get; set; }

		/// <summary>
		/// TimeStamp when cached
		/// </summary>
        public DateTime TimeStamp { get; set; }

		/// <summary>
		/// Server which delivered this response
		/// </summary>
        public IPEndPoint Server { get; set; }

		public Response()
		{
			Questions = new List<Question>();
			Answers = new List<AnswerRR>();
			Authorities = new List<AuthorityRR>();
			Additionals = new List<AdditionalRR>();

			Server = new IPEndPoint(0,0);
			Error = "";
			MessageSize = 0;
			TimeStamp = DateTime.Now;
			header = new Header();
		}

		public Response(IPEndPoint iPEndPoint, byte[] data)
		{
			Error = "";
			Server = iPEndPoint;
			TimeStamp = DateTime.Now;
			MessageSize = data.Length;
			RecordReader rr = new RecordReader(data);

			Questions = new List<Question>();
			Answers = new List<AnswerRR>();
			Authorities = new List<AuthorityRR>();
			Additionals = new List<AdditionalRR>();

			header = new Header(rr);

			for (int intI = 0; intI < header.QDCOUNT; intI++)
			{
				Questions.Add(new Question(rr));
			}

			for (int intI = 0; intI < header.ANCOUNT; intI++)
			{
				Answers.Add(new AnswerRR(rr));
			}

			for (int intI = 0; intI < header.NSCOUNT; intI++)
			{
				Authorities.Add(new AuthorityRR(rr));
			}
			for (int intI = 0; intI < header.ARCOUNT; intI++)
			{
				Additionals.Add(new AdditionalRR(rr));
			}
		}

        /// <summary>
        /// TODO: Test this works
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetRecordsByType<T>()  where T : AnswerRR
        {
            List<T> list = new List<T>();

            foreach (var ans in this.Answers)
                if (ans is T)
                    list.Add((T)ans);

            return list;
        }

		/// <summary>
		/// List of RecordMX in Response.Answers
		/// </summary>
		public RecordMX[] RecordsMX
		{
			get
			{
				List<RecordMX> list = new List<RecordMX>();
				foreach (AnswerRR answerRR in this.Answers)
				{
					RecordMX record = answerRR.RECORD as RecordMX;
					if(record!=null)
						list.Add(record);
				}
				list.Sort();
				return list.ToArray();
			}
		}

		/// <summary>
		/// List of RecordTXT in Response.Answers
		/// </summary>
		public RecordTXT[] RecordsTXT
		{
			get
			{
				List<RecordTXT> list = new List<RecordTXT>();
				foreach (AnswerRR answerRR in this.Answers)
				{
					RecordTXT record = answerRR.RECORD as RecordTXT;
					if (record != null)
						list.Add(record);
				}
				return list.ToArray();
			}
		}

		/// <summary>
		/// List of RecordA in Response.Answers
		/// </summary>
		public RecordA[] RecordsA
		{
			get
			{
				List<RecordA> list = new List<RecordA>();
				foreach (AnswerRR answerRR in this.Answers)
				{
					RecordA record = answerRR.RECORD as RecordA;
					if (record != null)
						list.Add(record);
				}
				return list.ToArray();
			}
		}

		/// <summary>
		/// List of RecordPTR in Response.Answers
		/// </summary>
		public RecordPTR[] RecordsPTR
		{
			get
			{
				List<RecordPTR> list = new List<RecordPTR>();
				foreach (AnswerRR answerRR in this.Answers)
				{
					RecordPTR record = answerRR.RECORD as RecordPTR;
					if (record != null)
						list.Add(record);
				}
				return list.ToArray();
			}
		}

		/// <summary>
		/// List of RecordCNAME in Response.Answers
		/// </summary>
		public RecordCNAME[] RecordsCNAME
		{
			get
			{
				List<RecordCNAME> list = new List<RecordCNAME>();
				foreach (AnswerRR answerRR in this.Answers)
				{
					RecordCNAME record = answerRR.RECORD as RecordCNAME;
					if (record != null)
						list.Add(record);
				}
				return list.ToArray();
			}
		}

		/// <summary>
		/// List of RecordAAAA in Response.Answers
		/// </summary>
		public RecordAAAA[] RecordsAAAA
		{
			get
			{
				List<RecordAAAA> list = new List<RecordAAAA>();
				foreach (AnswerRR answerRR in this.Answers)
				{
					RecordAAAA record = answerRR.RECORD as RecordAAAA;
					if (record != null)
						list.Add(record);
				}
				return list.ToArray();
			}
		}

		/// <summary>
		/// List of RecordNS in Response.Answers
		/// </summary>
		public RecordNS[] RecordsNS
		{
			get
			{
				List<RecordNS> list = new List<RecordNS>();
				foreach (AnswerRR answerRR in this.Answers)
				{
					RecordNS record = answerRR.RECORD as RecordNS;
					if (record != null)
						list.Add(record);
				}
				return list.ToArray();
			}
		}

		/// <summary>
		/// List of RecordSOA in Response.Answers
		/// </summary>
		public RecordSOA[] RecordsSOA
		{
			get
			{
				List<RecordSOA> list = new List<RecordSOA>();
				foreach (AnswerRR answerRR in this.Answers)
				{
					RecordSOA record = answerRR.RECORD as RecordSOA;
					if (record != null)
						list.Add(record);
				}
				return list.ToArray();
			}
		}

		public RR[] RecordsRR
		{
			get
			{
				List<RR> list = new List<RR>();
				foreach (RR rr in this.Answers)
				{
					list.Add(rr);
				}
				foreach (RR rr in this.Answers)
				{
					list.Add(rr);
				}
				foreach (RR rr in this.Authorities)
				{
					list.Add(rr);
				}
				foreach (RR rr in this.Additionals)
				{
					list.Add(rr);
				}
				return list.ToArray();
			}
		}
	}
}
