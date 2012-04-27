/*
IPHelper - IP Helper methods
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
2012-03-09 - Initial version, copied ToUint32, ToString from http://stackoverflow.com/questions/461742/how-to-convert-an-ipv4-address-into-a-integer-in-c,
             copied and added protocol name, method [GetServByName] from http://stackoverflow.com/questions/3457977/convert-service-name-to-port
             modified [GetServByName] into [GetProtoByName]
             GetServByName and GetProtoByName were quick tested, need to check for memory leaks and crashes, ext api :-/
2012-04-09 - Add etcFileHelper since API proved to be inconsistant on 64/32 (I assume I'm using 32bit on 64bit).
 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Globalization;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace DNSManagement.Extensions
{
    /// <summary>
    /// IP Parsing helper
    /// <para>Converts from uint32 to string and back</para>
    /// <remarks>http://stackoverflow.com/questions/461742/how-to-convert-an-ipv4-address-into-a-integer-in-c</remarks>
    /// </summary>
    public static class IPHelper
    {
        /// <summary>
        /// Parses a string and returns uint
        /// </summary>
        /// <param name="address">ipv4 address</param>
        /// <returns></returns>
        public static UInt32 ToUint32(string address)
        {
            return BitConverter.ToUInt32(IPAddress.Parse(address).GetAddressBytes().Reverse().ToArray(), 0);
        }

        /// <summary>
        /// Converts bytes (uint) to ip dot notation
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string ToString(UInt32 address)
        {
            return new IPAddress(BitConverter.GetBytes(address).Reverse().ToArray()).ToString();
        }

        /// <summary>
        /// Gets service port number by name/protocol
        /// </summary>
        /// <remarks>http://stackoverflow.com/questions/3457977/convert-service-name-to-port</remarks>
        /// <param name="name">name of service</param>
        /// <param name="protocol">protocol the service is using</param>
        /// <returns>port number</returns>
        public static int GetServByName(string name,string protocol)
        {
            // Vorbedingung
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            //check first in services file
            if (File.Exists(etcFileHelper.GetFullFilename(etcFileHelper.ServicesFilename)))
            {
                var services = etcFileHelper.ReadServices(etcFileHelper.GetFullFilename(etcFileHelper.ServicesFilename));
                var service = services.FirstOrDefault(i => i.Name.ToLower() == name.ToLower() && i.Protocol.ToLower() == protocol.ToLower());
                if (service != null)
                    return service.Port;
            }

            //we failed to find an appropriate service in the file, use the api instead.

            int result = 0;

            int intResult = -1;
            if (int.TryParse(name, out intResult))
            {
                result = System.Convert.ToInt32(name, CultureInfo.InvariantCulture);
            }
            else
            {
                WSAData dummy = new WSAData();
                int sccuessful = NativeMethods.WSAStartup(0x0202, ref dummy);
                if (sccuessful != 0)
                {
                    throw CreateWSAException();
                }

                IntPtr intPtr = NativeMethods.GetServByName(name, protocol);

                try
                {
                    if (intPtr != IntPtr.Zero)
                    {
                        Servent servent = (Servent)Marshal.PtrToStructure(intPtr, typeof(Servent));
                        result = System.Convert.ToInt32(IPAddress.NetworkToHostOrder(servent.s_port));
                    }
                    else
                    {
                        throw CreateWSAException();
                    }
                }
                finally
                {
                    sccuessful = NativeMethods.WSACleanup();
                    if (sccuessful != 0)
                    {
                        throw CreateWSAException();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets protocol number by name (tcp/udp/icmp etc')
        /// </summary>
        /// <param name="name">protocol name</param>
        /// <returns>protocol number</returns>
        public static int GetProtoByName(string name)
        {
            // Vorbedingung
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            //check first in protocols file
            if (File.Exists(etcFileHelper.GetFullFilename(etcFileHelper.ProtocolFilename)))
            {
                var protocols = etcFileHelper.ReadProtocols(etcFileHelper.GetFullFilename(etcFileHelper.ProtocolFilename));
                var protocol = protocols.FirstOrDefault(i => i.Name.ToLower() == name.ToLower());
                if (protocol != null)
                    return protocol.ProtocolNumber;
            }

            //failed, use API.

            int result = 0;

            int intResult = -1;
            if (int.TryParse(name, out intResult))
            {
                result = System.Convert.ToInt32(name, CultureInfo.InvariantCulture);
            }
            else
            {
                WSAData dummy = new WSAData();
                int sccuessful = NativeMethods.WSAStartup(0x0202, ref dummy);
                if (sccuessful != 0)
                {
                    throw CreateWSAException();
                }

                IntPtr intPtr = NativeMethods.GetProtocolByName(name);

                try
                {
                    if (intPtr != IntPtr.Zero)
                    {
                        ProtoEnt protoent = (ProtoEnt)Marshal.PtrToStructure(intPtr, typeof(ProtoEnt));
                        result = System.Convert.ToInt32(protoent.p_proto);
                    }
                    else
                    {
                        throw CreateWSAException();
                    }
                }
                finally
                {
                    sccuessful = NativeMethods.WSACleanup();
                    if (sccuessful != 0)
                    {
                        throw CreateWSAException();
                    }
                }
            }

            return result;
        }

        private static Win32Exception CreateWSAException()
        {
            int error = NativeMethods.WSAGetLastError();
            return new Win32Exception(error);
        }

        internal static class NativeMethods
        {
            [DllImport("Ws2_32.dll", EntryPoint = "getservbyname",
                                     SetLastError = true,
                                     CharSet = CharSet.Ansi,
                                     ExactSpelling = false,
                                     CallingConvention = CallingConvention.StdCall,
                                     ThrowOnUnmappableChar = true,
                                     BestFitMapping = false)]
            public static extern IntPtr GetServByName(string strName, string strProto);

            [DllImport("ws2_32.dll", EntryPoint = "getprotobyname",
                                     SetLastError = true,
                                     CharSet = CharSet.Ansi,
                                     ExactSpelling = false,
                                     CallingConvention = CallingConvention.StdCall,
                                     ThrowOnUnmappableChar = true,
                                     BestFitMapping = false
                )]
            public static extern IntPtr GetProtocolByName(string name);

            [DllImport("Ws2_32.dll", CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true, ExactSpelling = false)]
            public static extern Int32 WSAStartup(short wVersionRequested, ref WSAData wsaData);

            [DllImport("Ws2_32.dll", CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true, ExactSpelling = false)]
            public static extern Int32 WSACleanup();

            [DllImport("Ws2_32.dll", CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true, ExactSpelling = false)]
            public static extern Int32 WSAGetLastError();
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Servent
        {
            [SuppressMessage("Microsoft.Maintainability", "CA1504:ReviewMisleadingFieldNames")]
            public string s_name;
            [SuppressMessage("Microsoft.Maintainability", "CA1504:ReviewMisleadingFieldNames")]
            public IntPtr s_aliases;


//#if _WIN64
//    public string s_proto;          /* protocol to use */
//    public short   s_port;                 /* port # */
//#else
//    public short   s_port;                 /* port # */
//    public string s_proto;          /* protocol to use */
//#endif


            [SuppressMessage("Microsoft.Maintainability", "CA1504:ReviewMisleadingFieldNames")]
            public short s_port;
            [SuppressMessage("Microsoft.Maintainability", "CA1504:ReviewMisleadingFieldNames")]
            public string s_proto;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class ProtoEnt
        {
            public string p_name;
            public IntPtr p_aliases;
            public short p_proto;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WSAData
        {
            public short version;
            public short highVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 257)]
            public string description;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
            public string systemStatus;
            public short maxSockets;
            public short maxUdpDg;
            public IntPtr vendorInfo;
        }

        /// <summary>
        /// ParseIP or return IPAddress without exceptions
        /// </summary>
        /// <param name="ipaddr"></param>
        /// <returns></returns>
        public static IPAddress ParseIP(string ipaddr)
        {
            IPAddress address;
            if (IPAddress.TryParse(ipaddr, out address))
                return address;

            return IPAddress.None;
        }

        /// <summary>
        /// Fixes hostnames to have a dot in the end, most dns records has that standard.
        /// </summary>
        /// <param name="hostname"></param>
        /// <returns></returns>
        public static string FixHostnames(string hostname)
        {
            if (string.IsNullOrWhiteSpace(hostname))
                return hostname;

            if (!hostname.EndsWith("."))
                return hostname + ".";

            return hostname;
        }
    }
}
