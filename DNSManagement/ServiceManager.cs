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

*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace DNSManagement
{
    public class ServiceManager
    {
        internal ManagementScope m_scope;

        /// <summary>
        /// Service - local
        /// </summary>
        public ServiceManager()
        {
            m_scope = new ManagementScope(@"root\cimv2");
            m_scope.Connect();
        }

        /// <summary>
        /// Service - host/username/password
        /// </summary>
        /// <param name="host"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public ServiceManager(string host, string username, string password)
        {
            ConnectionOptions connoptions = new ConnectionOptions();
            connoptions.Username = username;
            connoptions.Password = password;
            connoptions.Impersonation = ImpersonationLevel.Impersonate;

            m_scope = new ManagementScope(@"\\" + host + @"\root\cimv2",connoptions);
            m_scope.Connect();
        }


        public enum MethodExecutionResultEnum
        {
            Success = 0,
            Not_Supported = 1,
            Access_Denied = 2,
            Dependent_Services_Running = 3,
            Invalid_Service_Control = 4,
            Service_Cannot_Accept_Control = 5,
            Service_Not_Active = 6,
            Service_Request_Timeout = 7,
            Unknown_Failure = 8,
            Path_Not_Found = 9,
            Service_Already_Running = 10,
            Service_Database_Locked = 11,
            Service_Dependency_Deleted = 12,
            Service_Dependency_Failure = 13,
            Service_Disabled = 14,
            Service_Logon_Failure = 15,
            Service_Marked_For_Deletion = 16,
            Service_No_Thread = 17,
            Status_Circular_Dependency = 18,
            Status_Duplicate_Name = 19,
            Status_Invalid_Name = 20,
            Status_Invalid_Parameter = 21,
            Status_Invalid_Service_Account = 22,
            Status_Service_Exists = 23,
            Service_Already_Paused = 24
        }


        /// <summary>
        /// create a new system service
        /// </summary>
        /// <param name="name">Name of the service to install to the Create method. The maximum string length is 256 characters. The Service Control Manager database preserves the case of the characters, but service name comparisons are always case-insensitive. Forward-slashes (/) and double back-slashes (\) are invalid service name characters. </param>
        /// <param name="displayName">Display name of the service. This string has a maximum length of 256 characters. The name is case-preserved in the Service Control Manager. DisplayName comparisons are always case-insensitive. </param>
        /// <param name="pathName">Fully qualified path to the executable file that implements the service. </param>
        /// <param name="serviceType">Type of services provided to processes that call them. </param>
        /// <param name="errorControl">Severity of the error if the Create method fails to start. The value indicates the action taken by the startup program if failure occurs. All errors are logged by the system. </param>
        /// <param name="startMode">Start mode of the Windows base service. </param>
        /// <param name="desktopInteract">If true, the service can create or communicate with windows on the desktop. </param>
        /// <param name="startName">Account name under which the service runs. Depending on the service type, the account name may be in the form of DomainName\Username. The service process is logged using one of these two forms when it runs. If the account belongs to the built-in domain, .\Username can be specified. If NULL is specified, the service is logged on as the LocalSystem account. For a kernel or system-level drivers, StartName contains the driver object name (that is, \FileSystem\Rdr or \Driver\Xns) which the input and output (I/O) system uses to load the device driver. If NULL is specified, the driver runs with a default object name created by the I/O system based on the service name. Example: DWDOM\Admin. </param>
        /// <param name="startPassword">Password to the account name specified by the StartName parameter. Specify NULL if you are not changing the password. Specify an empty string if the service has no password. </param>
        /// <param name="loadOrderGroup">    Group name associated with the new service. Load order groups are contained in the registry, and determine the sequence in which services are loaded into the operating system. If the pointer is NULL or if it points to an empty string, the service does not belong to a group. Dependencies between groups should be listed in the LoadOrderGroupDependencies parameter. Services in the load-ordering group list are started first, followed by services in groups not in the load-ordering group list, followed by services that do not belong to a group. The registry has a list of load ordering groups located at: HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\ServiceGroupOrder </param>
        /// <param name="loadOrderGroupDependencies">Array of load-ordering groups that must start before this service. Each item in the array is delimited by NULL and the list is terminated by two NULL values. In Visual Basic or script you can pass a vbArray. If the pointer is NULL or if it points to an empty string, the service has no dependencies. Group names must be prefixed by the SC_GROUP_IDENTIFIER (defined in the Winsvc.h file) character to differentiate it from a service name, because services and service groups share the same namespace. Dependency on a group means that this service can run if at least one member of the group is running after an attempt to start all of the members of the group. </param>
        /// <param name="serviceDependencies">Array that contains names of services that must start before this service starts. Each item in the array is delimited by NULL and the list is terminated by two NULL values. In Visual Basic or script you can pass a vbArray. If the pointer is NULL, or if it points to an empty string, the service has no dependencies. Dependency on a service means that this service can only run if the service it depends on is running. </param>
        /// <returns>Error/Succes</returns>
        public MethodExecutionResultEnum Create(
                string name,
                string displayName,
                string pathName,
                Service.ServiceTypeEnum serviceType,
                Service.ErrorControlEnum errorControl,
                Service.StartModeEnum startMode,
                bool desktopInteract,
                string startName,
                string startPassword,
                string loadOrderGroup,
                string [] loadOrderGroupDependencies,
                string [] serviceDependencies
            )
        {
            throw new NotImplementedException("Not tested");
            var mc_service = new ManagementClass(m_scope, new ManagementPath("Win32_Service"), null);

            ManagementBaseObject inParams = mc_service.GetMethodParameters("Create");
            inParams["Name"] = name;
            inParams["DisplayName"] = displayName;
            inParams["PathName"] = pathName;
            inParams["ServiceType"] = (byte) serviceType;
            inParams["ErrorControl"] = (byte)errorControl;
            inParams["StartMode"] = startMode.ToString();
            inParams["DesktopInteract"] = desktopInteract;
            inParams["StartName"] = startName;
            inParams["StartPassword"] = startPassword;
            inParams["LoadOrderGroup"] = loadOrderGroup;
            inParams["LoadOrderGroupDependencies"] = loadOrderGroupDependencies;
            inParams["ServiceDependencies"] = serviceDependencies;

            return (ServiceManager.MethodExecutionResultEnum)Convert.ToUInt32(mc_service.InvokeMethod("Create", inParams, null));
        }

        /// <summary>
        /// Retrieves a list of all services
        /// </summary>
        /// <returns></returns>
        public IList<Service> List()
        {
            //string query = String.Format("SELECT * FROM Win32_Service WHERE ContainerName='{0}'", this.ContainerName);
            string query = String.Format("SELECT * FROM Win32_Service");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(m_scope, new ObjectQuery(query));

            ManagementObjectCollection collection = searcher.Get();

            List<Service> records = new List<Service>();
            foreach (ManagementObject p in collection)
            {
                records.Add(new Service(p));
            }

            return records.ToArray();
        }
    }
}
