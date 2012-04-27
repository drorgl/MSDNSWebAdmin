/*
 SRV Record for DNS
 
 Programmed by Alphons van der Heijden, Copyright 2008.
 Project Article - http://www.codeproject.com/KB/IP/DNS_NET_Resolver.aspx
 Released under the CPOL License - http://www.codeproject.com/info/cpol10.aspx
 
 Changelog:
 2011-03-18 Dror Gluska - Make it a Library, Comments + partial Code Analysis fixups
 
 */

using System;
using System.Globalization;
/*
 *  http://www.ietf.org/rfc/rfc2782.txt
 * 
   Priority
        The priority of this target host.  A client MUST attempt to
        contact the target host with the lowest-numbered priority it can
        reach; target hosts with the same priority SHOULD be tried in an
        order defined by the weight field.  The range is 0-65535.  This
        is a 16 bit unsigned integer in network byte order.

   Weight
        A server selection mechanism.  The weight field specifies a
        relative weight for entries with the same priority. Larger
        weights SHOULD be given a proportionately higher probability of
        being selected. The range of this number is 0-65535.  This is a
        16 bit unsigned integer in network byte order.  Domain
        administrators SHOULD use Weight 0 when there isn't any server
        selection to do, to make the RR easier to read for humans (less
        noisy).  In the presence of records containing weights greater
        than 0, records with weight 0 should have a very small chance of
        being selected.

        In the absence of a protocol whose specification calls for the
        use of other weighting information, a client arranges the SRV
        RRs of the same Priority in the order in which target hosts,
        specified by the SRV RRs, will be contacted. The following
        algorithm SHOULD be used to order the SRV RRs of the same
        priority:

        To select a target to be contacted next, arrange all SRV RRs
        (that have not been ordered yet) in any order, except that all
        those with weight 0 are placed at the beginning of the list.

        Compute the sum of the weights of those RRs, and with each RR
        associate the running sum in the selected order. Then choose a
        uniform random number between 0 and the sum computed
        (inclusive), and select the RR whose running sum value is the
        first in the selected order which is greater than or equal to
        the random number selected. The target host specified in the
        selected SRV RR is the next one to be contacted by the client.
        Remove this SRV RR from the set of the unordered SRV RRs and
        apply the described algorithm to the unordered SRV RRs to select
        the next target host.  Continue the ordering process until there
        are no unordered SRV RRs.  This process is repeated for each
        Priority.

   Port
        The port on this target host of this service.  The range is 0-
        65535.  This is a 16 bit unsigned integer in network byte order.
        This is often as specified in Assigned Numbers but need not be.

   Target
        The domain name of the target host.  There MUST be one or more
        address records for this name, the name MUST NOT be an alias (in
        the sense of RFC 1034 or RFC 2181).  Implementors are urged, but
        not required, to return the address record(s) in the Additional
        Data section.  Unless and until permitted by future standards
        action, name compression is not to be used for this field.

        A Target of "." means that the service is decidedly not
        available at this domain.

 */

namespace Heijden.DNS
{
    /// <summary>
    /// A DNS RR for specifying the location of services (DNS SRV)
    /// <para>http://www.ietf.org/rfc/rfc2782.txt</para>
    /// </summary>
	public class RecordSRV : Record
	{
        /// <summary>
        /// The priority of this target host.  
        /// <para>
        /// A client MUST attempt to
        /// contact the target host with the lowest-numbered priority it can
        /// reach; target hosts with the same priority SHOULD be tried in an
        /// order defined by the weight field.  The range is 0-65535.  This
        /// is a 16 bit unsigned integer in network byte order.
        /// </para>
        /// </summary>
        public ushort PRIORITY { get; set; }
        /// <summary>
        /// A server selection mechanism.  
        /// <para>
        /// The weight field specifies a
        /// relative weight for entries with the same priority. Larger
        /// weights SHOULD be given a proportionately higher probability of
        /// being selected. The range of this number is 0-65535. 
        /// </para>
        /// </summary>
        public ushort WEIGHT { get; set; }
        /// <summary>
        /// The port on this target host of this service.  
        /// <para>
        /// The range is 0-65535.  This is a 16 bit unsigned integer in network byte order.
        /// This is often as specified in Assigned Numbers but need not be.
        /// </para>
        /// </summary>
        public ushort PORT { get; set; }
        /// <summary>
        /// The domain name of the target host.  
        /// <para>
        /// There MUST be one or more
        /// address records for this name, the name MUST NOT be an alias (in
        /// the sense of RFC 1034 or RFC 2181).  Implementors are urged, but
        /// not required, to return the address record(s) in the Additional
        /// Data section.  Unless and until permitted by future standards
        /// action, name compression is not to be used for this field.
        /// </para>
        /// </summary>
        public string TARGET { get; set; }

		public RecordSRV(RecordReader rr)
		{
			PRIORITY = rr.ReadUInt16();
			WEIGHT = rr.ReadUInt16();
			PORT = rr.ReadUInt16();
			TARGET = rr.ReadDomainName();
		}

		public override string ToString()
		{
            return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3}",
				PRIORITY,
				WEIGHT,
				PORT,
				TARGET);
		}

	}
}
