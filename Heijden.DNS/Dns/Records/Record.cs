/*
 abstract Record for DNS
 Stuff records are made of
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */



namespace Heijden.DNS
{
    /// <summary>
    /// base abstract Record
    /// </summary>
	public abstract class Record
	{
		/// <summary>
		/// The Resource Record this RDATA record belongs to
		/// </summary>
        public RR RR { get; set; }
	}
}
