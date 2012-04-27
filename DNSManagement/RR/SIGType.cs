/*
DNSManagement - Wrapper for WMI Management of MS DNS
Copyright (C) 2011 Dror Gluska
	
This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public License
(LGPL) as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.
The terms of redistributing and/or modifying this software also
include exceptions to the LGPL that facilitate static linking.
 	
This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
Lesser General Public License for more details.
 	
You should have received a copy of the GNU Lesser General Public License
along with this library; if not, write to Free Software Foundation, Inc.,
51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA


Change log:
2011-05-17 - Initial version
2012-03-09 - Fixed all saving methods to return connected records
2012-04-07 - Looks like SIG records are unsupported by WMI, missing information from documentation and all calls fails either with Generic Error
                or return no records.
 
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNSManagement.Extensions;
using System.Management;

namespace DNSManagement.RR
{
    /// <summary>
    /// Represents a Signature (SIG) RR. 
    /// </summary>
    public class SIGType : ResourceRecord
    {
        /// <summary>
        /// Algorithm used with the key specified in the resource record
        /// </summary>
        public enum AlgorithmEnum : uint
        {
            /// <summary>
            /// RSA/MD5 (RFC 2537)
            /// </summary>
            RSA_MD5 = 1,
            /// <summary>
            /// Diffie-Hellman (RFC 2539)
            /// </summary>
            Diffie_Hellman = 2,
            /// <summary>
            /// DSA (RFC 2536)
            /// </summary>
            DSA = 3,
            /// <summary>
            /// Elliptic curve cryptography
            /// </summary>
            ECC = 4
        }

        private ManagementObject m_mo;

        internal SIGType(ManagementObject mo)
            : base(mo)
        {
            m_mo = mo;
        }

        /// <summary>
        /// Algorithm used with the key specified in the resource record. The assigned values are shown in the following table. 
        /// </summary>
        public AlgorithmEnum Algorithm
        {
            get
            {
                return (AlgorithmEnum) Convert.ToUInt16(m_mo["Algorithm"]);
            }
        }

        /// <summary>
        /// Method used to choose a key that verifies a SIG. See RFC 2535, Appendix C for the method used to calculate a KeyTag.
        /// </summary>
        public UInt16 KeyTag
        {
            get
            {
                return Convert.ToUInt16(m_mo["KeyTag"]);
            }
        }

        /// <summary>
        /// Unsigned count of labels in the original SIG RR owner name. The count does not include the NULL label for the root, nor any initial wildcards.
        /// </summary>
        public UInt16 Labels
        {
            get
            {
                return Convert.ToUInt16(m_mo["Labels"]);
            }
        }

        /// <summary>
        /// TTL of the RR set signed by the SIG.
        /// </summary>
        public TimeSpan OriginalTTL
        {
            get
            {
                return TimeSpan.FromSeconds(Convert.ToUInt32(m_mo["OriginalTTL"]));
            }
        }

        /// <summary>
        /// Signature, represented in base 64, formatted as defined in RFC 2535, Appendix A.
        /// </summary>
        public string Signature
        {
            get
            {
                return Convert.ToString(m_mo["Signature"]);
            }
        }

        /// <summary>
        /// Signature expiration date, expressed in seconds since the beginning of January 1, 1970, Greenwich Mean Time (GMT), excluding leap seconds.
        /// </summary>
        public UnixDateTime SignatureExpiration
        {
            get
            {
                return new UnixDateTime((double)Convert.ToUInt32(m_mo["SignatureExpiration"]));
            }
        }

        /// <summary>
        /// Date and time at which the Signature becomes valid, expressed in seconds since the beginning of January 1, 1970, Greenwich Mean Time (GMT), excluding leap seconds.
        /// </summary>
        public UnixDateTime SignatureInception
        {
            get
            {
                return new UnixDateTime((double)Convert.ToUInt32(m_mo["SignatureInception"]));
            }
        }

        /// <summary>
        /// Domain name of the signer that generated the SIG RR.
        /// </summary>
        public string SignerName
        {
            get
            {
                return Convert.ToString(m_mo["SignerName"]);
            }
        }

        /// <summary>
        /// Type of RR covered by this SIG.
        /// </summary>
        public UInt16 TypeCovered
        {
            get
            {
                return Convert.ToUInt16(m_mo["TypeCovered"]);
            }
        }

        /// <summary>
        /// Instantiates an SIG RR based on the data in the method's input parameters: the record's DNS Server Name, Container Name, Owner Name, class (default = IN), time-to-live value, and WINS mapping flag, reverse look-up time out, WINS cache time out and domain name to append. It returns a reference to the new object as an output parameter. 
        /// </summary>
        /// <param name="server">Server object</param>
        /// <param name="dnsServerName">Fully Qualified Domain Name (FQDN) or IP address of the DNS Server that contains this RR.</param>
        /// <param name="containerName">Name of the Container for the Zone, Cache, or RootHints instance which contains this RR.</param>
        /// <param name="ownerName">Owner FQDN for the RR.</param>
        /// <param name="recordClass">Class of the RR.</param>
        /// <param name="ttl">Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="algorithm">Algorithm used with the key specified in the resource record. </param>
        /// <param name="keyTag">Method used to choose a key that verifies an SIG. See RFC 2535, Appendix C for the method used to calculate a KeyTag.</param>
        /// <param name="labels">Unsigned count of labels in the original SIG RR owner name. The count does not include the NULL label for the root, nor any initial wildcards.</param>
        /// <param name="originalTTL">TTL of the RR set signed by the SIG.</param>
        /// <param name="signature">Signature, represented in base 64, formatted as defined in RFC 2535, Appendix A.</param>
        /// <param name="signatureExpiration">Signature expiration date, expressed in seconds since the beginning of January 1, 1970, Greenwich Mean Time (GMT), excluding leap seconds.</param>
        /// <param name="signatureInception">Date and time at which the Signature becomes valid, expressed in seconds since the beginning of January 1, 1970, Greenwich Mean Time (GMT), excluding leap seconds.</param>
        /// <param name="signerName">Domain name of the signer that generated the SIG RR.</param>
        /// <param name="typeCovered">Type of RR covered by this SIG.</param>
        /// <returns>the new object.</returns>
        public static SIGType CreateInstanceFromPropertyData(
            Server server,
            string dnsServerName,
            string containerName,
            string ownerName,
            RecordClassEnum? recordClass,
            TimeSpan? ttl,
             UInt16 typeCovered,
             AlgorithmEnum algorithm,
             UInt16 labels,
             TimeSpan originalTTL,
             UnixDateTime signatureExpiration,
             UnixDateTime signatureInception,
             UInt16 keyTag,
             string signerName,
             string signature)
        {
            if (server == null)
                throw new ArgumentNullException("server is required");

            ManagementClass dnsClass = new ManagementClass(server.m_scope, new ManagementPath("MicrosoftDNS_SIGType"), null);
            ManagementBaseObject inParams = dnsClass.GetMethodParameters("CreateInstanceFromPropertyData");
            inParams["DnsServerName"] = dnsServerName;
            inParams["ContainerName"] = containerName;
            inParams["OwnerName"] = ownerName;
            if (recordClass != null)
                inParams["RecordClass"] = (UInt32)recordClass.Value;
            if (ttl != null)
                inParams["TTL"] = ttl.Value.TotalSeconds;
            inParams["Algorithm"] = (UInt16)algorithm;
            inParams["Labels"] = labels;
            inParams["OriginalTTL"] = originalTTL.TotalSeconds;
            inParams["SignatureExpiration"] = signatureExpiration.Unix;
            inParams["SignatureInception"] = signatureInception.Unix;
            inParams["KeyTag"] = keyTag;
            inParams["SignerName"] = signerName;
            inParams["Signature"] = signature;

            //return new SIGType((ManagementObject)dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null));
            try
            {
                return new SIGType(new ManagementObject(server.m_scope, new ManagementPath(dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null)["RR"].ToString()), null));
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }


        /// <summary>
        /// Updates the TTL, Mapping Flag, Look-up Time out, Cache Time out and Result Domain to the values specified as the input parameters of this method. If a new value for a parameter is not specified, then the current value for the parameter is not changed. The method returns a reference to the modified object as an output parameter. 
        /// </summary>
        /// <param name="ttl">Optional - Time, in seconds, that the RR can be cached by a DNS resolver.</param>
        /// <param name="algorithm">Algorithm used with the key specified in the resource record. </param>
        /// <param name="keyTag">Method used to choose a key that verifies an SIG. See RFC 2535, Appendix C for the method used to calculate a KeyTag.</param>
        /// <param name="labels">Unsigned count of labels in the original SIG RR owner name. The count does not include the NULL label for the root, nor any initial wildcards.</param>
        /// <param name="originalTTL">TTL of the RR set signed by the SIG.</param>
        /// <param name="signature">Signature, represented in base 64, formatted as defined in RFC 2535, Appendix A.</param>
        /// <param name="signatureExpiration">Signature expiration date, expressed in seconds since the beginning of January 1, 1970, Greenwich Mean Time (GMT), excluding leap seconds.</param>
        /// <param name="signatureInception">Date and time at which the Signature becomes valid, expressed in seconds since the beginning of January 1, 1970, Greenwich Mean Time (GMT), excluding leap seconds.</param>
        /// <param name="signerName">Domain name of the signer that generated the SIG RR.</param>
        /// <param name="typeCovered">Type of RR covered by this SIG.</param>
        /// <returns>the modified object.</returns>
        public SIGType Modify(TimeSpan? ttl, UInt16 typeCovered,
             AlgorithmEnum algorithm,
             UInt16 labels,
             TimeSpan originalTTL,
             UnixDateTime signatureExpiration,
             UnixDateTime signatureInception,
             UInt16 keyTag,
             string signerName,
             string signature)
        {
            ManagementBaseObject inParams = m_mo.GetMethodParameters("Modify");
            if ((ttl != null) && (ttl != this.TTL))
                inParams["TTL"] = ttl.Value.TotalSeconds;

            //these are not optional, need to check if record is deleted if the same
            inParams["Algorithm"] = (UInt16)algorithm;
            inParams["Labels"] = labels;
            inParams["OriginalTTL"] = originalTTL.TotalSeconds;
            inParams["SignatureExpiration"] = signatureExpiration.Unix;
            inParams["SignatureInception"] = signatureInception.Unix;
            inParams["KeyTag"] = keyTag;
            inParams["SignerName"] = signerName;
            inParams["Signature"] = signature;

            //return new SIGType((ManagementObject)m_mo.InvokeMethod("Modify", inParams, null));
            try
            {

                return new SIGType(new ManagementObject(m_mo.Scope, new ManagementPath(m_mo.InvokeMethod("Modify", inParams, null)["RR"].ToString()), null));
            }
            catch (ManagementException me)
            {
                throw new WMIException(me);
            }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLineFormat("Algorithm={0}", Algorithm);
            sb.AppendLineFormat("KeyTag={0}", KeyTag);
            sb.AppendLineFormat("Labels={0}", Labels);
            sb.AppendLineFormat("OriginalTTL={0}", OriginalTTL);
            sb.AppendLineFormat("Signature={0}", Signature);
            sb.AppendLineFormat("SignatureExpiration={0}", SignatureExpiration);
            sb.AppendLineFormat("SignatureInception={0}", SignatureInception);
            sb.AppendLineFormat("SignerName={0}", SignerName);
            sb.AppendLineFormat("TypeCovered={0}", TypeCovered);


            //RR
            base.ToString(sb);

            return sb.ToString();
        }
    }
}
