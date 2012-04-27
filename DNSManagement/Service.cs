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
using DNSManagement.Extensions;

namespace DNSManagement
{
    public class Service
    {
        private ManagementObject m_mo;

        internal Service(ManagementObject mo)
        {
            m_mo = mo;
        }

        /// <summary>
        /// Severity of the error if this service fails to start during startup. The value indicates the action taken by the startup program if failure occurs. All errors are logged by the computer system.
        /// </summary>
        public enum ErrorControlEnum
        {
            /// <summary>
            /// User is not notified.
            /// </summary>
            Ignore = 0,
            /// <summary>
            /// User is notified.
            /// </summary>
            Normal = 1,
            /// <summary>
            /// System is restarted with the last-known-good configuration.
            /// </summary>
            Severe = 2,
            /// <summary>
            /// System attempts to restart with a good configuration.
            /// </summary>
            Critical = 3,
            /// <summary>
            /// Severity of the error is unknown.
            /// </summary>
            Unknown
        }

        /// <summary>
        /// Type of service provided to calling processes.
        /// </summary>
        public enum ServiceTypeEnum
        {
            Kernel_Driver = 1,
            File_System_Driver = 2,
            Adapter = 4,
            Recognizer_Driver = 8,
            Own_Process = 16,
            Share_Process = 32,
            Interactive_Process = 256
        }

        /// <summary>
        /// Start mode of the Windows base service.
        /// </summary>
        public enum StartModeEnum
        {
            /// <summary>
            /// Device driver started by the operating system loader (valid only for driver services).
            /// </summary>
            Boot,
            /// <summary>
            /// Device driver started by the operating system initialization process. This value is valid only for driver services.
            /// </summary>
            System,
            /// <summary>
            /// Service to be started automatically by the service control manager during system startup.
            /// </summary>
            Auto,
            /// <summary>
            /// Service to be started by the Service Control Manager when a process calls the StartService method.
            /// </summary>
            Manual,
            /// <summary>
            /// Service that cannot be started.
            /// </summary>
            Disabled
        }

        /// <summary>
        /// state of the base service.
        /// </summary>
        public enum StateEnum
        {
            Stopped,
            Start_Pending,
            Stop_Pending,
            Running,
            Continue_Pending,
            Pause_Pending,
            Paused,
            Unknown
        }

        /// <summary>
        /// status of the object.
        /// </summary>
        public enum StatusEnum
        {
            OK,
            Error,
            Degraded,
            Unknown,
            Pred_Fail,
            Starting,
            Stopping,
            Service
        }


        

        /// <summary>
        /// Service can be paused.
        /// </summary>
        public bool AcceptPause
        {
            get
            {
                return Convert.ToBoolean(m_mo["AcceptPause"]);
            }
        }

        /// <summary>
        /// Service can be stopped.
        /// </summary>
        public bool AcceptStop
        {
            get
            {
                return Convert.ToBoolean(m_mo["AcceptStop"]);
            }
        }

        /// <summary>
        /// Short description of the object—a one-line string.
        /// </summary>
        public string Caption
        {
            get
            {
                return Convert.ToString(m_mo["Caption"]);
            }
        }

        /// <summary>
        /// Value that the service increments periodically to report its progress during a long start, stop, pause, or continue operation. For example, the service increments this value as it completes each step of its initialization when it is starting up. The user interface program that invokes the operation on the service uses this value to track the progress of the service during a lengthy operation. This value is not valid and should be zero when the service does not have a start, stop, pause, or continue operation pending.
        /// </summary>
        public UInt32 CheckPoint
        {
            get
            {
                return Convert.ToUInt32(m_mo["CheckPoint"]);
            }
        }

        /// <summary>
        /// Name of the first concrete class to appear in the inheritance chain used in the creation of an instance. When used with the other key properties of the class, this property allows all instances of this class and its subclasses to be uniquely identified.
        /// </summary>
        public string CreationClassName
        {
            get
            {
                return Convert.ToString(m_mo["CreationClassName"]);
            }
        }

        /// <summary>
        /// Description of the object.
        /// </summary>
        public string Description
        {
            get
            {
                return Convert.ToString(m_mo["Description"]);
            }
        }

        /// <summary>
        /// Service can create or communicate with windows on the desktop.
        /// </summary>
        public bool DesktopInteract
        {
            get
            {
                return Convert.ToBoolean(m_mo["DesktopInteract"]);
            }
        }

        /// <summary>
        /// Display name of the service. This string has a maximum length of 256 characters. The name is case-preserved in the Service Control Manager. However, DisplayName comparisons are always case-insensitive. 
        /// </summary>
        public string DisplayName
        {
            get
            {
                return Convert.ToString(m_mo["DisplayName"]);
            }
        }

        /// <summary>
        /// Severity of the error if this service fails to start during startup. The value indicates the action taken by the startup program if failure occurs. All errors are logged by the computer system.
        /// </summary>
        public ErrorControlEnum ErrorControl
        {
            get
            {
                return (ErrorControlEnum)Enum.Parse(typeof(ErrorControlEnum), Convert.ToString(m_mo["ErrorControl"]));
            }
        }

        /// <summary>
        /// Windows error code that defines errors encountered in starting or stopping the service. This property is set to ERROR_SERVICE_SPECIFIC_ERROR (1066) when the error is unique to the service represented by this class, and information about the error is available in the ServiceSpecificExitCode property. The service sets this value to NO_ERROR when running, and again upon normal termination.
        /// </summary>
        public UInt32 ExitCode
        {
            get
            {
                return Convert.ToUInt32(m_mo["ExitCode"]);
            }
        }

        /// <summary>
        /// Date object is installed. This property does not require a value to indicate that the object is installed.
        /// </summary>
        public DateTime? InstallDate
        {
            get
            {
                if (m_mo["InstallDate"] == null)
                    return null;
                return ManagementDateTimeConverter.ToDateTime(m_mo["InstallDate"] as string);
            }
        }

        /// <summary>
        /// Unique identifier of the service that provides an indication of the functionality that is managed. This functionality is described in the Description property of the object.
        /// </summary>
        public string Name
        {
            get
            {
                return Convert.ToString(m_mo["Name"]);
            }
        }

        /// <summary>
        /// Fully-qualified path to the service binary file that implements the service.
        /// </summary>
        public string PathName
        {
            get
            {
                return Convert.ToString(m_mo["PathName"]);
            }
        }

        /// <summary>
        /// Process identifier of the service.
        /// </summary>
        public UInt32 ProcessId
        {
            get
            {
                return Convert.ToUInt32(m_mo["ProcessId"]);
            }
        }

        /// <summary>
        /// Service-specific error code for errors that occur while the service is either starting or stopping. The exit codes are defined by the service represented by this class. This value is only set when the ExitCode property value is ERROR_SERVICE_SPECIFIC_ERROR (1066).
        /// </summary>
        public UInt32 ServiceSpecificExitCode
        {
            get
            {
                return Convert.ToUInt32(m_mo["ServiceSpecificExitCode"]);
            }
        }

        /// <summary>
        /// Type of service provided to calling processes.
        /// </summary>
        public ServiceTypeEnum ServiceType
        {
            get
            {
                return (ServiceTypeEnum)Enum.Parse(typeof(ServiceTypeEnum), Convert.ToString(m_mo["ServiceType"]).Replace(" ", "_"));
            }
        }

        /// <summary>
        /// Service has been started.
        /// </summary>
        public bool Started
        {
            get
            {
                return Convert.ToBoolean(m_mo["Started"]);
            }
        }

        /// <summary>
        /// Start mode of the Windows base service.
        /// </summary>
        public StartModeEnum StartMode
        {
            get
            {
                return (StartModeEnum)Enum.Parse(typeof(StartModeEnum), Convert.ToString(m_mo["StartMode"]));
            }
        }

        /// <summary>
        /// Account name under which a service runs. Depending on the service type, the account name may be in the form of DomainName\Username. The service process is logged by using one of these two forms when it runs. If the account belongs to the built-in domain, then .\Username can be specified. For kernel or system-level drivers, StartName contains the driver object name (that is, \FileSystem\Rdr or \Driver\Xns) which the I/O system uses to load the device driver. Additionally, if NULL is specified, the driver runs with a default object name created by the I/O system based on the service name. 
        /// </summary>
        public string StartName
        {
            get
            {
                return Convert.ToString(m_mo["StartName"]);
            }
        }

        /// <summary>
        /// Current state of the base service.
        /// </summary>
        public StateEnum State
        {
            get
            {
                return (StateEnum)Enum.Parse(typeof(StateEnum), Convert.ToString(m_mo["State"]).Replace(" ", "_"));
            }
        }

        /// <summary>
        /// Current status of the object. Various operational and nonoperational statuses can be defined. Operational statuses include: "OK", "Degraded", and "Pred Fail" (an element, such as a SMART-enabled hard disk drive, may be functioning properly but predicting a failure in the near future). Nonoperational statuses include: "Error", "Starting", "Stopping", and "Service". The latter, "Service", could apply during mirror-resilvering of a disk, reload of a user permissions list, or other administrative work. Not all such work is online, yet the managed element is neither "OK" nor in one of the other states.
        /// </summary>
        public StatusEnum Status
        {
            get
            {
                return (StatusEnum)Enum.Parse(typeof(StatusEnum), Convert.ToString(m_mo["Status"]).Replace(" ", "_"));
            }
        }

        /// <summary>
        /// Type name of the system that hosts this service.
        /// </summary>
        public string SystemCreationClassName
        {
            get
            {
                return Convert.ToString(m_mo["SystemCreationClassName"]);
            }
        }

        /// <summary>
        /// Name of the system that hosts this service.
        /// </summary>
        public string SystemName
        {
            get
            {
                return Convert.ToString(m_mo["SystemName"]);
            }
        }

        /// <summary>
        /// Unique tag value for this service in the group. A value of 0 (zero) indicates that the service does not have a tag. A tag can be used to order service startup within a load order group by specifying a tag order vector in the registry located at: 
        /// <para>HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\    GroupOrderList</para>
        /// <para>Tags are only evaluated for Kernel Driver and File System Driver start type services that have Boot or System start modes. </para>
        /// </summary>
        public UInt32 TagId
        {
            get
            {
                return Convert.ToUInt32(m_mo["TagId"]);
            }
        }

        /// <summary>
        /// Estimated time required, in milliseconds, for a pending start, stop, pause, or continue operation. After the specified time has 
        /// elapsed, the service makes its next call to the SetServiceStatus method with either an incremented CheckPoint 
        /// value or a change in CurrentState. If the amount of time specified by WaitHint passes, and CheckPoint has not been 
        /// incremented, or CurrentState has not changed, the service control manager or service control program assumes 
        /// that an error has occurred.
        /// </summary>
        public TimeSpan WaitHint
        {
            get
            {
                return TimeSpan.FromMilliseconds(Convert.ToUInt32(m_mo["WaitHint"]));
            }
        }

        /// <summary>
        /// Modifies a service
        /// </summary>
        /// <param name="displayName">The display name of the service. This string has a maximum length of 256 characters. The name is case-preserved in the service control manager. DisplayName comparisons are always case-insensitive. </param>
        /// <param name="pathName">The fully-qualified path to the executable file that implements the service, for example, "\SystemRoot\System32\drivers\afd.sys".</param>
        /// <param name="serviceType">The type of services provided to processes that call them.</param>
        /// <param name="errorControl">Severity of the error if this service fails to start during startup. The value indicates the action taken by the startup program if failure occurs. All errors are logged by the system.</param>
        /// <param name="startMode">Start mode of the Windows base service.</param>
        /// <param name="desktopInteract">If true, the service can create or communicate with a window on the desktop.</param>
        /// <param name="startName">Account name the service runs under. Depending on the service type, the account name may be in the form of DomainName\Username or .\Username. The service process will be logged using one of these two forms when it runs. If the account belongs to the built-in domain, .\Username can be specified. If NULL is specified, the service will be logged on as the LocalSystem account. For kernel or system-level drivers, StartName contains the driver object name (that is, \FileSystem\Rdr or \Driver\Xns) that the input and output (I/O) system uses to load the device driver. If NULL is specified, the driver runs with a default object name created by the I/O system based on the service name, for example, "DWDOM\Admin".   You also can use the User Principal Name (UPN) format to specify the StartName, for example, Username@DomainName.</param>
        /// <param name="startPassword">Password to the account name specified by the StartName parameter. Specify NULL if you are not changing the password. Specify an empty string if the service has no password.  Note  When changing a service from a local system to a network, or from a network to a local system, StartPassword must be an empty string ("") and not NULL. </param>
        /// <param name="loadOrderGroup">Group name that it is associated with. Load order groups are contained in the system registry, and determine the sequence in which services are loaded into the operating system. If the pointer is NULL, or if it points to an empty string, the service does not belong to a group. Dependencies between groups should be listed in the LoadOrderGroupDependencies parameter. Services in the load-ordering group list are started first, followed by services in groups not in the load-ordering group list, followed by services that do not belong to a group. The system registry has a list of load ordering groups located at HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\ServiceGroupOrder. </param>
        /// <param name="loadOrderGroupDependencies">List of load-ordering groups that must start before this service starts. The array is doubly null-terminated. If the pointer is NULL, or if it points to an empty string, the service has no dependencies. Group names must be prefixed by the SC_GROUP_IDENTIFIER (defined in the Winsvc.h file) character to differentiate them from service names because services and service groups share the same namespace. Dependency on a group means that this service can run if at least one member of the group is running after an attempt to start all of the members of the group.</param>
        /// <param name="serviceDependencies">List that contains the names of services that must start before this service starts. The array is doubly NULL-terminated. If the pointer is NULL, or if it points to an empty string, the service has no dependencies. Dependency on a service indicates that this service can run only if the service it depends on is running.</param>
        /// <returns></returns>
        public ServiceManager.MethodExecutionResultEnum Change(
                string displayName,
                string pathName,
                ServiceTypeEnum serviceType,
                ErrorControlEnum errorControl,
                StartModeEnum startMode,
                bool desktopInteract,
                string startName,
                string startPassword,
                string loadOrderGroup,
                string loadOrderGroupDependencies,
                string serviceDependencies
            )
        {
            throw new NotImplementedException("Not tested, out of scope");

            ManagementBaseObject inParams = m_mo.GetMethodParameters("Change");
            inParams["DisplayName"] = displayName;
            inParams["PathName"] = pathName;
            inParams["ServiceType"] = (UInt32)serviceType;
            inParams["ErrorControl"] = (UInt32)errorControl;
            inParams["StartMode"] = Convert.ToString(startMode); //TODO: make sure it works...
            inParams["DesktopInteract"] = desktopInteract;
            inParams["StartName"] = startName;
            inParams["StartPassword"] = startPassword;
            inParams["LoadOrderGroup"] = loadOrderGroup;
            inParams["LoadOrderGroupDependencies"] = loadOrderGroupDependencies;
            inParams["ServiceDependencies"] = serviceDependencies;

            return (ServiceManager.MethodExecutionResultEnum)Convert.ToUInt32(m_mo.InvokeMethod("Change", inParams, null)["ReturnValue"]);
        }

        /// <summary>
        /// modifies the start mode of a service.
        /// </summary>
        /// <param name="startMode">Start mode of the Windows base service. </param>
        /// <returns></returns>
        public ServiceManager.MethodExecutionResultEnum ChangeStartMode(
                StartModeEnum startMode
            )
        {
            throw new NotImplementedException("Out of scope for project");

            ManagementBaseObject inParams = m_mo.GetMethodParameters("ChangeStartMode");
           
            inParams["StartMode"] = Convert.ToString(startMode); //TODO: make sure it works...

            return (ServiceManager.MethodExecutionResultEnum)Convert.ToUInt32(m_mo.InvokeMethod("ChangeStartMode", inParams, null)["ReturnValue"]);
        }

        /// <summary>
        /// deletes a service.
        /// </summary>
        /// <returns></returns>
        public ServiceManager.MethodExecutionResultEnum Delete()
        {
            throw new NotImplementedException("Out of scope for project");
            return (ServiceManager.MethodExecutionResultEnum)Convert.ToUInt32(m_mo.InvokeMethod("Delete", null, null)["ReturnValue"]);
        }

        /// <summary>
        /// requests that the referenced service update its state to the service manager.
        /// </summary>
        /// <returns></returns>
        public ServiceManager.MethodExecutionResultEnum InterrogateService()
        {
            return (ServiceManager.MethodExecutionResultEnum)Convert.ToUInt32(m_mo.InvokeMethod("InterrogateService", null, null)["ReturnValue"]);
        }

        /// <summary>
        /// attempts to place the service in the paused state
        /// </summary>
        /// <returns></returns>
        public ServiceManager.MethodExecutionResultEnum PauseService()
        {
            return (ServiceManager.MethodExecutionResultEnum)Convert.ToUInt32(m_mo.InvokeMethod("PauseService", null, null)["ReturnValue"]);
        }

        /// <summary>
        /// attempts to place the referenced service in the resumed state
        /// </summary>
        /// <returns></returns>
        public ServiceManager.MethodExecutionResultEnum ResumeService()
        {
            return (ServiceManager.MethodExecutionResultEnum)Convert.ToUInt32(m_mo.InvokeMethod("ResumeService", null, null)["ReturnValue"]);
        }

        /// <summary>
        /// attempts to place the referenced service into its startup state
        /// </summary>
        /// <returns></returns>
        public ServiceManager.MethodExecutionResultEnum StartService()
        {
            return (ServiceManager.MethodExecutionResultEnum)Convert.ToUInt32(m_mo.InvokeMethod("StartService", null, null)["ReturnValue"]);
        }

        /// <summary>
        /// attempts to place the referenced service into its stopped state
        /// </summary>
        /// <returns></returns>
        public ServiceManager.MethodExecutionResultEnum StopService()
        {
            return (ServiceManager.MethodExecutionResultEnum)Convert.ToUInt32(m_mo.InvokeMethod("StopService", null, null)["ReturnValue"]);
        }

        /// <summary>
        /// attempts to send a user-defined control code to the referenced service. 
        /// </summary>
        /// <param name="controlCode">Specifies defined values (from 128 to 255) that provide control commands specific to a user. </param>
        /// <returns></returns>
        public ServiceManager.MethodExecutionResultEnum UserControlService(
                    byte controlCode
                )
        {
            throw new NotImplementedException("Out of scope for project");
            ManagementBaseObject inParams = m_mo.GetMethodParameters("UserControlService");

            inParams["ControlCode"] = controlCode;

            return (ServiceManager.MethodExecutionResultEnum)Convert.ToUInt32(m_mo.InvokeMethod("UserControlService", inParams, null)["ReturnValue"]);
        }

        /// <summary>
        /// returns the security descriptor that controls access to the service
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public ServiceManager.MethodExecutionResultEnum GetSecurityDescriptor(out object descriptor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// writes an updated version of the security descriptor that controls access to the service.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public ServiceManager.MethodExecutionResultEnum SetSecurityDescriptor(object descriptor)
        {
            throw new NotImplementedException();
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLineFormat("AcceptPause={0}", AcceptPause);
            sb.AppendLineFormat("AcceptStop={0}", AcceptStop);
            sb.AppendLineFormat("Caption={0}", Caption);
            sb.AppendLineFormat("CheckPoint={0}", CheckPoint);
            sb.AppendLineFormat("CreationClassName={0}", CreationClassName);
            sb.AppendLineFormat("Description={0}", Description);
            sb.AppendLineFormat("DesktopInteract={0}", DesktopInteract);
            sb.AppendLineFormat("DisplayName={0}", DisplayName);
            sb.AppendLineFormat("ErrorControl={0}", ErrorControl);
            sb.AppendLineFormat("ExitCode={0}", ExitCode);
            sb.AppendLineFormat("InstallDate={0}", InstallDate);
            sb.AppendLineFormat("Name={0}", Name);
            sb.AppendLineFormat("PathName={0}", PathName);
            sb.AppendLineFormat("ProcessId={0}", ProcessId);
            sb.AppendLineFormat("ServiceSpecificExitCode={0}", ServiceSpecificExitCode);
            sb.AppendLineFormat("ServiceType={0}", ServiceType);
            sb.AppendLineFormat("Started={0}", Started);
            sb.AppendLineFormat("StartMode={0}", StartMode);
            sb.AppendLineFormat("StartName={0}", StartName);
            sb.AppendLineFormat("State={0}", State);
            sb.AppendLineFormat("Status={0}", Status);
            sb.AppendLineFormat("SystemCreationClassName={0}", SystemCreationClassName);
            sb.AppendLineFormat("SystemName={0}", SystemName);
            sb.AppendLineFormat("TagId={0}", TagId);
            sb.AppendLineFormat("WaitHint={0}", WaitHint);


            return sb.ToString();
        }

    }
}
