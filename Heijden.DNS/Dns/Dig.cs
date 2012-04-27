/*
 Dig Helper for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups.
 2011-03-18 Dror Gluska - Add Trace method 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Net;

namespace Heijden.DNS
{
    /// <summary>
    /// DNS Dig
    /// </summary>
    public class Dig
    {
        
        /// <summary>
        /// underlying resolver
        /// </summary>
        public Resolver resolver { get; private set; }

        /// <summary>
        /// Message delegate to show progress.
        /// </summary>
        /// <param name="text"></param>
        public delegate void OnMessage(string text);

        /// <summary>
        /// Delegate accessor
        /// </summary>
        public OnMessage MessageDelegate { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public Dig()
        {
            resolver = new Resolver();
            resolver.OnVerbose += new Resolver.VerboseEventHandler(resolver_OnVerbose);
        }

        /// <summary>
        /// render message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Message(object sender, Resolver.VerboseEventArgs e)
        {
            Debug.WriteLine(e.Message);
            if (MessageDelegate != null)
                MessageDelegate(e.Message);
        }

        /// <summary>
        /// render message
        /// </summary>
        /// <param name="text"></param>
        /// <param name="parms"></param>
        private void Message(string text, params object[] parms)
        {
            Debug.WriteLine(text, parms);
            if (MessageDelegate != null)
                MessageDelegate(string.Format(CultureInfo.InvariantCulture, text, parms));
        }


        /// <summary>
        /// resolver delegate handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resolver_OnVerbose(object sender, Resolver.VerboseEventArgs e)
        {
            Message(sender, e);
        }

        /// <summary>
        /// private delegate for async calls
        /// </summary>
        /// <param name="name"></param>
        /// <param name="qtype"></param>
        /// <param name="qclass"></param>
        private delegate void DigItDelegate(string name, QType qtype, QClass qclass);

        /// <summary>
        /// Starts a Dig operation
        /// </summary>
        /// <param name="name"></param>
        /// <param name="qtype"></param>
        /// <param name="qclass"></param>
        public void BeginDigIt(string name, QType qtype, QClass qclass)
        {
            DigItDelegate d = new DigItDelegate(DigIt);
            d.BeginInvoke(name, qtype, qclass, null, null);
        }

        /// <summary>
        /// Starts a Trace operation
        /// </summary>
        /// <param name="name"></param>
        /// <param name="qtype"></param>
        /// <param name="qclass"></param>
        public void BeginTraceIt(string name, QType qtype, QClass qclass)
        {
            DigItDelegate d = new DigItDelegate(Trace);
            d.BeginInvoke(name, qtype, qclass, null, null);
        }

        /// <summary>
        /// Recursive tracing of nameservers
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentNameserver"></param>
        /// <param name="qtype"></param>
        /// <param name="qclass"></param>
        /// <param name="firstOnly"></param>
        /// <returns></returns>
        private IPAddress TraceNameservers(string name, KeyValuePair<string, IPAddress> parentNameserver, QType qtype, QClass qclass, bool firstOnly)
        {
            Message(";; Asking {0} ({1}) for nameserver of {2} ", parentNameserver.Key, parentNameserver.Value, name);
            resolver.DnsServer = parentNameserver.Value.ToString();

            //preserving cache flag
            var saveuseCache = resolver.UseCache;
            resolver.UseCache = false;

            //performing a nameserver search
            Response nsresponse = resolver.Query(name, QType.NS, QClass.IN);

            //performing the requested search
            Response qresponse = resolver.Query(name, qtype, qclass);

            resolver.UseCache = saveuseCache;

            //sorting answers
            List<RecordNS> nameservers = new List<RecordNS>();
            List<RecordA> nameserveraddresses = new List<RecordA>();
            List<RecordAAAA> nameserveraddresV6 = new List<RecordAAAA>();

            foreach (var ans in nsresponse.Answers)
                SortAnswers(nameservers, nameserveraddresses, nameserveraddresV6, ans.RECORD);

            foreach (var ans in nsresponse.Authorities)
                SortAnswers(nameservers, nameserveraddresses, nameserveraddresV6, ans.RECORD);

            foreach (var ans in nsresponse.Additionals)
                SortAnswers(nameservers, nameserveraddresses, nameserveraddresV6, ans.RECORD);

            foreach (var ans in qresponse.Answers)
                SortAnswers(nameservers, nameserveraddresses, nameserveraddresV6, ans.RECORD);

            foreach (var ans in qresponse.Authorities)
                SortAnswers(nameservers, nameserveraddresses, nameserveraddresV6, ans.RECORD);

            foreach (var ans in qresponse.Additionals)
                SortAnswers(nameservers, nameserveraddresses, nameserveraddresV6, ans.RECORD);



            //if one of the nameservers is the calling nameserver, we've reached the end, return the IP of this nameserver
            //if there are answers (from the requested query) - we have the name server
            //if there are no more child nameservers - we reached the end, return the nameserver we did find and attempt a query on it.
            if ((nameservers.FirstOrDefault(i => i.NSDNAME == parentNameserver.Key) != null) || (qresponse.Answers.Count > 0) || (nameservers.Count == 0))
                return parentNameserver.Value;


            foreach (var ns in nameservers)
            {
                string nsname = ns.NSDNAME;
                var nsaddress = nameserveraddresses.Where(nsa => nsa.RR.NAME == nsname).Select(nsa=>nsa.Address).FirstOrDefault();
                if (nsaddress == null)
                    nsaddress = nameserveraddresV6.Where(nsa => nsa.RR.NAME == nsname).Select(nsa=>nsa.Address).FirstOrDefault();
                if (nsaddress == null)
                {
                    Message(";; nameserver {0} did not provide an address, attempting to resolve.",nsname);
                    //continue;
                    var hostentry = Dns.GetHostEntry(nsname);
                    if ((hostentry != null) && (hostentry.AddressList.Length > 0))
                    {
                        nsaddress = hostentry.AddressList[0];
                        Message(";; nameserver {0} extrnally resolved to {1}", nsname,nsaddress);

                    }
                    else
                    {
                        Message(";; nameserver {0} doesn't have an address and failed to resolve, skipping.", nsname);
                        continue;
                    }
                    
                }
                var childns = TraceNameservers(name, new KeyValuePair<string, IPAddress>(nsname, nsaddress),qtype,qclass, firstOnly);

                if (childns != null)
                {
                    //reached the last NS.
                    return childns;
                }
                
            }
            return null;
        }

        /// <summary>
        /// sorts the answers from record into the nameserver/addresses
        /// </summary>
        /// <param name="nameservers"></param>
        /// <param name="nameserveraddresses"></param>
        /// <param name="nameserveraddresV6"></param>
        /// <param name="record"></param>
        private static void SortAnswers(List<RecordNS> nameservers, List<RecordA> nameserveraddresses, List<RecordAAAA> nameserveraddresV6, Record record)
        {
            if (record is RecordNS)
                nameservers.Add(record as RecordNS);
            else if (record is RecordA)
                nameserveraddresses.Add(record as RecordA);
            else if (record is RecordAAAA)
                nameserveraddresV6.Add(record as RecordAAAA);
        }

        /// <summary>
        /// Performs a Trace on query
        /// </summary>
        /// <param name="name"></param>
        /// <param name="qtype"></param>
        /// <param name="qclass"></param>
        public void Trace(string name, QType qtype, QClass qclass)
        {
            Message("; <<>> DigTrace.Net {0} <<>> @{1} {2} {3}", resolver.Version, resolver.DnsServer, qtype, name);
            
            Stopwatch sw = new Stopwatch();

            sw.Start();
            Message(";; Loading Root nameservers");
            var rootaddresses = resolver.LoadRootFile();

            foreach(var ra in rootaddresses)
            {
                //Message(";; Asking {0} ({1}) for nameserver for {2} ", ra.Key, ra.Value, name);
                var nsaddress = TraceNameservers(name, ra,qtype, qclass, true);
                if (nsaddress != null)
                {
                    Message(";; Using {0} as nameserver for {1}", nsaddress, name);
                    resolver.DnsServer = nsaddress.ToString();
                    break;
                }

            }

            Response response = resolver.Query(name, qtype, qclass);
            sw.Stop();

            if (response.Error != "")
            {
                Message(";; " + response.Error);
                return;
            }

            PrintAnswer(response);

            Message(";; Query time: {0} msec", sw.ElapsedMilliseconds);
            Message(";; SERVER: {0}#{1}({2})", response.Server.Address, response.Server.Port, response.Server.Address);
            Message(";; WHEN: " + response.TimeStamp.ToString("ddd MMM dd HH:mm:ss yyyy", CultureInfo.InvariantCulture));
            Message(";; MSG SIZE rcvd: " + response.MessageSize);

        }

        /// <summary>
        /// Dig a query
        /// </summary>
        /// <param name="name"></param>
        /// <param name="qtype"></param>
        /// <param name="qclass"></param>
        public void DigIt(string name, QType qtype, QClass qclass)
        {
            Message("; <<>> Dig.Net {0} <<>> @{1} {2} {3}", resolver.Version, resolver.DnsServer, qtype, name);

            Stopwatch sw = new Stopwatch();

            sw.Start();
            Response response = resolver.Query(name, qtype, qclass);
            sw.Stop();

            if (response.Error != "")
            {
                Message(";; " + response.Error);
                return;
            }

            PrintAnswer(response);

            Message(";; Query time: {0} msec", sw.ElapsedMilliseconds);
            Message(";; SERVER: {0}#{1}({2})", response.Server.Address, response.Server.Port, response.Server.Address);
            Message(";; WHEN: " + response.TimeStamp.ToString("ddd MMM dd HH:mm:ss yyyy", CultureInfo.InvariantCulture));
            Message(";; MSG SIZE rcvd: " + response.MessageSize);
        }

        private void PrintAnswer(Response response)
        {
            Message(";; Got answer:");

            Message(";; ->>HEADER<<- opcode: {0}, status: {1}, id: {2}",
                response.header.OPCODE,
                response.header.RCODE,
                response.header.ID);
            Message(";; flags: {0}{1}{2}{3}; QUERY: {4}, ANSWER: {5}, AUTHORITY: {6}, ADDITIONAL: {7}",
                response.header.QR ? " qr" : "",
                response.header.AA ? " aa" : "",
                response.header.RD ? " rd" : "",
                response.header.RA ? " ra" : "",
                response.header.QDCOUNT,
                response.header.ANCOUNT,
                response.header.NSCOUNT,
                response.header.ARCOUNT);
            Message("");

            if (response.header.QDCOUNT > 0)
            {
                Message(";; QUESTION SECTION:");
                foreach (Question question in response.Questions)
                    Message(";{0}", question);
                Message("");
            }

            if (response.header.ANCOUNT > 0)
            {
                Message(";; ANSWER SECTION:");
                foreach (AnswerRR answerRR in response.Answers)
                    Message(answerRR.ToString());
                Message("");
            }

            if (response.header.NSCOUNT > 0)
            {
                Message(";; AUTHORITY SECTION:");
                foreach (AuthorityRR authorityRR in response.Authorities)
                    Message(authorityRR.ToString());
                Message("");
            }

            if (response.header.ARCOUNT > 0)
            {
                Message(";; ADDITIONAL SECTION:");
                foreach (AdditionalRR additionalRR in response.Additionals)
                    Message(additionalRR.ToString());
                Message("");
            }
        }
    }
}
