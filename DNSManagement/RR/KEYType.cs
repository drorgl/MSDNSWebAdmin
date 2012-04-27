///*
//DNSManagement - Wrapper for WMI Management of MS DNS
//Copyright (C) 2011 Dror Gluska
	
//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public License
//(LGPL) as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
//The terms of redistributing and/or modifying this software also
//include exceptions to the LGPL that facilitate static linking.
 	
//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//Lesser General Public License for more details.
 	
//You should have received a copy of the GNU Lesser General Public License
//along with this library; if not, write to Free Software Foundation, Inc.,
//51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA


//Change log:
//2011-05-17 - Initial version
//2012-03-09 - Fixed all saving methods to return connected records
 
//*/


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using DNSManagement.Extensions;
//using System.Management;

//namespace DNSManagement.RR
//{
//    /// <summary>
//    /// Represents a KEY RR. 
//    /// </summary>
//    public class KEYType : ResourceRecord
//    {
//        /// <summary>
//        /// Algorithm used with the key specified in the resource record
//        /// </summary>
//        public enum AlgorithmEnum : uint
//        {
//            /// <summary>
//            /// RSA/MD5 (RFC 2537)
//            /// </summary>
//            RSA_MD5 = 1,
//            /// <summary>
//            /// Diffie-Hellman (RFC 2539)
//            /// </summary>
//            Diffie_Hellman = 2,
//            /// <summary>
//            /// DSA (RFC 2536)
//            /// </summary>
//            DSA = 3,
//            /// <summary>
//            /// Elliptic curve cryptography
//            /// </summary>
//            ECC = 4
//        }

//        /// <summary>
//        /// Protocol for which the key specified in the resource record can be used
//        /// </summary>
//        public enum ProtocolEnum : uint
//        {
//            TLS = 1,
//            Email = 2,
//            DNSSEC = 3,
//            IPSec = 4,
//            /// <summary>
//            /// All Protocols
//            /// </summary>
//            All = 255
//        }


//        private ManagementObject m_mo;

//        internal KEYType(ManagementObject mo)
//            : base(mo)
//        {
//            m_mo = mo;
//        }

//        /// <summary>
//        /// Algorithm used with the key specified in the resource record
//        /// </summary>
//        public AlgorithmEnum Algorithm
//        {
//            get
//            {
//                return (AlgorithmEnum)Convert.ToUInt16(m_mo["Algorithm"]);
//            }
//        }

//        /// <summary>
//        /// Flags used to specify mapping, as described in IETF RFC 2535.
//        /// </summary>
//        public UInt16 Flags
//        {
//            get
//            {
//                return Convert.ToUInt16(m_mo["Flags"]);
//            }
//        }

//        /// <summary>
//        /// Protocol for which the key specified in the resource record can be used.
//        /// </summary>
//        public ProtocolEnum Protocol
//        {
//            get
//            {
//                return (ProtocolEnum)Convert.ToUInt16(m_mo["Protocol"]);
//            }
//        }

//        /// <summary>
//        /// Public key, represented in base 64 as described in Appendix A of RFC 2535.
//        /// </summary>
//        public string PublicKey
//        {
//            get
//            {
//                return Convert.ToString(m_mo["PublicKey"]);
//            }
//        }



//        /// <summary>
//        /// Instantiates a KEY Resource Record based on the data in the method's input parameters: the record's DNS Server Name, Container Name, Owner Name, class (default = IN), time-to-live value, and WINS mapping flag, reverse look-up time out, WINS cache time out and domain name to append. It returns a reference to the new object as an output parameter. 
//        /// </summary>
//        /// <param name="server">Server object</param>
//        /// <param name="dnsServerName">Fully Qualified Domain Name (FQDN) or IP address of the DNS Server that contains this RR.</param>
//        /// <param name="containerName">Name of the Container for the Zone, Cache, or RootHints instance which contains this RR.</param>
//        /// <param name="ownerName">Owner FQDN for the RR.</param>
//        /// <param name="recordClass">Class of the RR.</param>
//        /// <param name="ttl">Time, in seconds, that the RR can be cached by a DNS resolver.</param>
//        ///  <param name="algorithm">Algorithm used with the key specified in the resource record</param>
//        /// <param name="flags">Flags used to specify mapping, as described in IETF RFC 2535.</param>
//        /// <param name="protocol">Protocol for which the key specified in the RR can be used. </param>
//        /// <param name="publicKey">Public key, represented in base 64 as described in Appendix A of RFC 2535.</param>
//        /// <returns>the new object.</returns>
//        public static KEYType CreateInstanceFromPropertyData(
//            Server server,
//            string dnsServerName,
//            string containerName,
//            string ownerName,
//            RecordClassEnum? recordClass,
//            TimeSpan? ttl,
//            UInt16 flags, ProtocolEnum protocol,
//            AlgorithmEnum algorithm, string publicKey)
//        {
//            if (server == null)
//                throw new ArgumentNullException("server is required");

//            ManagementClass dnsClass = new ManagementClass(server.m_scope, new ManagementPath("MicrosoftDNS_KEYType"), null);
//            ManagementBaseObject inParams = dnsClass.GetMethodParameters("CreateInstanceFromPropertyData");
//            inParams["DnsServerName"] = dnsServerName;
//            inParams["ContainerName"] = containerName;
//            inParams["OwnerName"] = ownerName;
//            if (recordClass != null)
//                inParams["RecordClass"] = (UInt32)recordClass.Value;
//            if (ttl != null)
//                inParams["TTL"] = ttl.Value.TotalSeconds;
//            inParams["Flags"] = flags;
//            inParams["Protocol"] = (UInt16)protocol;
//            inParams["Algorithm"] = (UInt16)algorithm;
//            inParams["PublicKey"] = publicKey;

//            try
//            {
//                return new KEYType(new ManagementObject(server.m_scope, new ManagementPath(dnsClass.InvokeMethod("CreateInstanceFromPropertyData", inParams, null)["RR"].ToString()), null));
//            }
//            catch (ManagementException me)
//            {
//                throw new WMIException(me);
//            }
//        }


//        /// <summary>
//        /// Updates the TTL, mapping flag, look-up time out, cache time out, and result domain to the values specified as the input parameters of this method. If a new value for a parameter is not specified, then the current value for the parameter is not changed. The method returns a reference to the modified object as an output parameter. 
//        /// </summary>
//        /// <param name="ttl">Optional - Time, in seconds, that the RR can be cached by a DNS resolver.</param>
//        /// <param name="algorithm">Optional - Algorithm used with the key specified in the resource record</param>
//        /// <param name="flags">Optional - Flags used to specify mapping, as described in IETF RFC 2535.</param>
//        /// <param name="protocol">Optional - Protocol for which the key specified in the RR can be used. </param>
//        /// <param name="publicKey">Optional - Public key, represented in base 64 as described in Appendix A of RFC 2535.</param>
//        /// <returns>the modified object.</returns>
//        public KEYType Modify(TimeSpan? ttl, UInt16? flags, ProtocolEnum? protocol, 
//                                           AlgorithmEnum? algorithm, string publicKey)
//        {
//            ManagementBaseObject inParams = m_mo.GetMethodParameters("Modify");
//            if ((ttl != null) && (ttl != this.TTL))
//                inParams["TTL"] = ttl.Value.TotalSeconds;
//            if ((flags != null) && (flags != this.Flags))
//                inParams["Flags"] = flags;
//            if ((protocol != null) && (protocol != this.Protocol))
//                inParams["Protocol"] = (UInt16)protocol;
//            if ((algorithm != null) && (algorithm != this.Algorithm))
//                inParams["Algorithm"] = (UInt16)algorithm;
//            if ((!string.IsNullOrEmpty(publicKey)) && (publicKey != this.PublicKey))
//                inParams["PublicKey"] = publicKey;

//            //return new KEYType((ManagementObject)m_mo.InvokeMethod("Modify", inParams, null));
//            return new KEYType(new ManagementObject(m_mo.Scope, new ManagementPath(m_mo.InvokeMethod("Modify", inParams, null)["RR"].ToString()), null));
//        }


//        public override string ToString()
//        {
//            StringBuilder sb = new StringBuilder();

//            sb.AppendLineFormat("Algorithm={0}", Algorithm);
//            sb.AppendLineFormat("Flags={0}", Flags);
//            sb.AppendLineFormat("Protocol={0}", Protocol);
//            sb.AppendLineFormat("PublicKey={0}", PublicKey);

//            //RR
//            base.ToString(sb);

//            return sb.ToString();
//        }


//    }
//}
