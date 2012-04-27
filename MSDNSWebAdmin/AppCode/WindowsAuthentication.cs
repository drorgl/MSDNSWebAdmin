/*
DNS Web Admin - MS DNS Web Administration
Copyright (C) 2011 Dror Gluska
	
This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.



Change log:
2011-05-17 - Initial version

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;

namespace MSDNSWebAdmin.AppCode
{
    /// <summary>
    /// Windows Authentication
    /// <para>Used to authenticate against domain/server username/password</para>
    /// </summary>
    public class WindowsAuthentication
    {

        public enum LogonType : int
        {
            /// <summary>
            /// This logon type is intended for users who will be interactively using the computer, such as a user being logged on  
            /// by a terminal server, remote shell, or similar process.
            /// This logon type has the additional expense of caching logon information for disconnected operations;
            /// therefore, it is inappropriate for some client/server applications,
            /// such as a mail server.
            /// </summary>
            LOGON32_LOGON_INTERACTIVE = 2,

            /// <summary>
            /// This logon type is intended for high performance servers to authenticate plaintext passwords.

            /// The LogonUser function does not cache credentials for this logon type.
            /// </summary>
            LOGON32_LOGON_NETWORK = 3,

            /// <summary>
            /// This logon type is intended for batch servers, where processes may be executing on behalf of a user without
            /// their direct intervention. This type is also for higher performance servers that process many plaintext
            /// authentication attempts at a time, such as mail or Web servers.
            /// The LogonUser function does not cache credentials for this logon type.
            /// </summary>
            LOGON32_LOGON_BATCH = 4,

            /// <summary>
            /// Indicates a service-type logon. The account provided must have the service privilege enabled.
            /// </summary>
            LOGON32_LOGON_SERVICE = 5,

            /// <summary>
            /// This logon type is for GINA DLLs that log on users who will be interactively using the computer.
            /// This logon type can generate a unique audit record that shows when the workstation was unlocked.
            /// </summary>
            LOGON32_LOGON_UNLOCK = 7,

            /// <summary>
            /// This logon type preserves the name and password in the authentication package, which allows the server to make
            /// connections to other network servers while impersonating the client. A server can accept plaintext credentials
            /// from a client, call LogonUser, verify that the user can access the system across the network, and still
            /// communicate with other servers.
            /// NOTE: Windows NT:  This value is not supported.
            /// </summary>
            LOGON32_LOGON_NETWORK_CLEARTEXT = 8,

            /// <summary>
            /// This logon type allows the caller to clone its current token and specify new credentials for outbound connections.
            /// The new logon session has the same local identifier but uses different credentials for other network connections.
            /// NOTE: This logon type is supported only by the LOGON32_PROVIDER_WINNT50 logon provider.
            /// NOTE: Windows NT:  This value is not supported.
            /// </summary>
            LOGON32_LOGON_NEW_CREDENTIALS = 9,
        }

        /// <summary>
        /// Logon Provider
        /// </summary>
        public enum LogonProvider
        {
            /// <summary>
            /// Use the standard logon provider for the system.
            /// The default security provider is negotiate, unless you pass NULL for the domain name and the user name
            /// is not in UPN format. In this case, the default provider is NTLM.
            /// NOTE: Windows 2000/NT:   The default security provider is NTLM.
            /// </summary>
            LOGON32_PROVIDER_DEFAULT = 0//,
            

            //LOGON32_PROVIDER_WINNT35 = 1,
            //LOGON32_PROVIDER_WINNT40 = 2,
            //LOGON32_PROVIDER_WINNT50 = 3
        }


        /// <summary>
        /// The LogonUser function attempts to log a user on to the local computer. The local computer is the computer from which LogonUser was called. You cannot use LogonUser to log on to a remote computer. You specify the user with a user name and domain and authenticate the user with a plaintext password. If the function succeeds, you receive a handle to a token that represents the logged-on user. You can then use this token handle to impersonate the specified user or, in most cases, to create a process that runs in the context of the specified user.
        /// </summary>
        /// <param name="pszUsername">A pointer to a null-terminated string that specifies the name of the user. This is the name of the user account to log on to. If you use the user principal name (UPN) format, User@DNSDomainName, the lpszDomain parameter must be NULL. </param>
        /// <param name="pszDomain">A pointer to a null-terminated string that specifies the name of the domain or server whose account database contains the lpszUsername account. If this parameter is NULL, the user name must be specified in UPN format. If this parameter is ".", the function validates the account by using only the local account database.</param>
        /// <param name="pszPassword">A pointer to a null-terminated string that specifies the plaintext password for the user account specified by lpszUsername. When you have finished using the password, clear the password from memory by calling the SecureZeroMemory function. For more information about protecting passwords, see Handling Passwords.</param>
        /// <param name="dwLogonType">The type of logon operation to perform. </param>
        /// <param name="dwLogonProvider"></param>
        /// <param name="phToken"></param>
        /// <returns></returns>
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool LogonUser(string pszUsername, string pszDomain, string pszPassword, LogonType dwLogonType, LogonProvider dwLogonProvider, ref IntPtr phToken);

        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="hObject">A valid handle to an open object.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// Attemps to authenticate a Windows user
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="domain">Domain</param>
        /// <returns></returns>
        public static bool Authenticate(string username, string password, string domain)
        {
            IntPtr pExistingTokenHandle = new IntPtr(0);
            pExistingTokenHandle = IntPtr.Zero;

            //attempt to log in with credentials
            bool bImpersonated = LogonUser(username, domain, password, LogonType.LOGON32_LOGON_NEW_CREDENTIALS, LogonProvider.LOGON32_PROVIDER_DEFAULT, ref pExistingTokenHandle);

            if (bImpersonated == false)
            {
                //if failed, get error code
                int nErrorCode = Marshal.GetLastWin32Error();

                throw new System.Security.SecurityException("Authentication failed", Marshal.GetExceptionForHR(nErrorCode));
            }

            if (pExistingTokenHandle != IntPtr.Zero)
                CloseHandle(pExistingTokenHandle); // close existing handle

            return true;
        }
    }
}