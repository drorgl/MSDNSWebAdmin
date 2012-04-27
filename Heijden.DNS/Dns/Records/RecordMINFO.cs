/*
 MINFO Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.Globalization;
/*
 3.3.7. MINFO RDATA format (EXPERIMENTAL)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                    RMAILBX                    /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                    EMAILBX                    /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

RMAILBX         A <domain-name> which specifies a mailbox which is
                responsible for the mailing list or mailbox.  If this
                domain name names the root, the owner of the MINFO RR is
                responsible for itself.  Note that many existing mailing
                lists use a mailbox X-request for the RMAILBX field of
                mailing list X, e.g., Msgroup-request for Msgroup.  This
                field provides a more general mechanism.


EMAILBX         A <domain-name> which specifies a mailbox which is to
                receive error messages related to the mailing list or
                mailbox specified by the owner of the MINFO RR (similar
                to the ERRORS-TO: field which has been proposed).  If
                this domain name names the root, errors should be
                returned to the sender of the message.

MINFO records cause no additional section processing.  Although these
records can be associated with a simple mailbox, they are usually used
with a mailing list.
 */
namespace Heijden.DNS
{
    /// <summary>
    /// MINFO RDATA format (EXPERIMENTAL)
    /// </summary>
	public class RecordMINFO : Record
	{
        /// <summary>
        /// A domain-name which specifies a mailbox which is
        /// responsible for the mailing list or mailbox.  
        /// <para>If this
        /// domain name names the root, the owner of the MINFO RR is
        /// responsible for itself.  Note that many existing mailing
        /// lists use a mailbox X-request for the RMAILBX field of
        /// mailing list X, e.g., Msgroup-request for Msgroup.  This
        /// field provides a more general mechanism.
        /// </para>
        /// </summary>
        public string RMAILBX { get; set; }
        /// <summary>
        /// A domain-name which specifies a mailbox which is to
        /// receive error messages related to the mailing list or
        /// mailbox specified by the owner of the MINFO RR
        /// <para>
        /// (similar
        /// to the ERRORS-TO: field which has been proposed).  If
        /// this domain name names the root, errors should be
        /// returned to the sender of the message.
        /// </para>
        /// </summary>
        public string EMAILBX { get; set; }

		public RecordMINFO(RecordReader rr)
		{
			RMAILBX = rr.ReadDomainName();
			EMAILBX = rr.ReadDomainName();
		}

		public override string ToString()
		{
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}", RMAILBX, EMAILBX);
		}

	}
}
