using DNSManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for ServiceTest and is intended
    ///to contain all ServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ServiceTest
    {

        /// <summary>
        /// Gets the DNS service, TEST ONLY ON TEST SERVERS!
        /// </summary>
        /// <returns></returns>
        public static Service GetService()
        {
            var servicemanager = ServiceManagerTest.GetServiceManager();
            return servicemanager.List().FirstOrDefault(i => i.Name == "DNS");
        }

      

        ///// <summary>
        /////A test for Change
        /////</summary>
        //[TestMethod()]
        //public void ChangeTest()
        //{
        //    Assert.Inconclusive("Not implemented, out of scope");

        //    //
        //    //var target = GetService();; 
        //    //string displayName = string.Empty; 
        //    //string pathName = string.Empty; 
        //    //Service.ServiceTypeEnum serviceType = new Service.ServiceTypeEnum(); 
        //    //Service.ErrorControlEnum errorControl = new Service.ErrorControlEnum(); 
        //    //Service.StartModeEnum startMode = new Service.StartModeEnum(); 
        //    //bool desktopInteract = false; 
        //    //string startName = string.Empty; 
        //    //string startPassword = string.Empty; 
        //    //string loadOrderGroup = string.Empty; 
        //    //string loadOrderGroupDependencies = string.Empty; 
        //    //string serviceDependencies = string.Empty; 
        //    //ServiceManager.MethodExecutionResultEnum expected = new ServiceManager.MethodExecutionResultEnum(); 
        //    //ServiceManager.MethodExecutionResultEnum actual;
        //    //actual = target.Change(displayName, pathName, serviceType, errorControl, startMode, desktopInteract, startName, startPassword, loadOrderGroup, loadOrderGroupDependencies, serviceDependencies);
        //    //Assert.AreEqual(expected, actual);
        //    //Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for ChangeStartMode
        /////</summary>
        //[TestMethod()]
        //public void ChangeStartModeTest()
        //{
        //    Assert.Inconclusive("Not implemented, out of scope");
        //    //
        //    //var target = GetService();; 
        //    //Service.StartModeEnum startMode = new Service.StartModeEnum(); 
        //    //ServiceManager.MethodExecutionResultEnum expected = new ServiceManager.MethodExecutionResultEnum(); 
        //    //ServiceManager.MethodExecutionResultEnum actual;
        //    //actual = target.ChangeStartMode(startMode);
        //    //Assert.AreEqual(expected, actual);
        //    //Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Delete
        /////</summary>
        //[TestMethod()]
        //public void DeleteTest()
        //{
        //    Assert.Inconclusive("Not implemented, out of scope");
        //    //
        //    //var target = GetService();; 
        //    //ServiceManager.MethodExecutionResultEnum expected = new ServiceManager.MethodExecutionResultEnum(); 
        //    //ServiceManager.MethodExecutionResultEnum actual;
        //    //actual = target.Delete();
        //    //Assert.AreEqual(expected, actual);
        //    //Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetSecurityDescriptor
        /////</summary>
        //[TestMethod()]
        //public void GetSecurityDescriptorTest()
        //{
        //    Assert.Inconclusive("Not implemented, out of scope");


        //    //
        //    //var target = GetService();; 
        //    //object descriptor = null; 
        //    //object descriptorExpected = null; 
        //    //ServiceManager.MethodExecutionResultEnum expected = new ServiceManager.MethodExecutionResultEnum(); 
        //    //ServiceManager.MethodExecutionResultEnum actual;
        //    //actual = target.GetSecurityDescriptor(out descriptor);
        //    //Assert.AreEqual(descriptorExpected, descriptor);
        //    //Assert.AreEqual(expected, actual);
        //    //Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        /// <summary>
        ///A test for InterrogateService
        ///</summary>
        [TestMethod()]
        public void InterrogateServiceTest()
        {
            var target = GetService();
            var value = target.InterrogateService();
            Assert.AreEqual(value, DNSManagement.ServiceManager.MethodExecutionResultEnum.Success, "InterrogateService success");

        }

        /// <summary>
        ///A test for PauseService
        ///</summary>
        [TestMethod()]
        public void PauseResumeServiceTest()
        {
            var target = GetService();

            var actual = target.PauseService();
            Assert.AreEqual(DNSManagement.ServiceManager.MethodExecutionResultEnum.Success, actual, "Can't Pause");

             actual = target.ResumeService();
            Assert.AreEqual(DNSManagement.ServiceManager.MethodExecutionResultEnum.Success, actual, "Can't resume");
        }

        

        ///// <summary>
        /////A test for SetSecurityDescriptor
        /////</summary>
        //[TestMethod()]
        //public void SetSecurityDescriptorTest()
        //{
        //    Assert.Inconclusive("Not implemented, out of scope");
        //    //
        //    //var target = GetService();; 
        //    //object descriptor = null; 
        //    //ServiceManager.MethodExecutionResultEnum expected = new ServiceManager.MethodExecutionResultEnum(); 
        //    //ServiceManager.MethodExecutionResultEnum actual;
        //    //actual = target.SetSecurityDescriptor(descriptor);
        //    //Assert.AreEqual(expected, actual);
        //    //Assert.Inconclusive("Verify the correctness of this test method.");
        //}

    

        /// <summary>
        ///A test for StopService
        ///</summary>
        [TestMethod()]
        public void StopStartServiceTest()
        {
            var target = GetService();

            var actual = target.StopService();
            Assert.AreEqual(DNSManagement.ServiceManager.MethodExecutionResultEnum.Success, actual, "Can't stop");

             actual = target.StartService();
            Assert.AreEqual(DNSManagement.ServiceManager.MethodExecutionResultEnum.Success, actual, "Can't start");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            var target = GetService();

            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrWhiteSpace(actual),"ToString empty?");
        }

        ///// <summary>
        /////A test for UserControlService
        /////</summary>
        //[TestMethod()]
        //public void UserControlServiceTest()
        //{
        //    Assert.Inconclusive("Not implemented, out of scope");

        //    //
        //    //var target = GetService();; 
        //    //byte controlCode = 0; 
        //    //ServiceManager.MethodExecutionResultEnum expected = new ServiceManager.MethodExecutionResultEnum(); 
        //    //ServiceManager.MethodExecutionResultEnum actual;
        //    //actual = target.UserControlService(controlCode);
        //    //Assert.AreEqual(expected, actual);
        //    //Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        /// <summary>
        ///A test for AcceptPause
        ///</summary>
        [TestMethod()]
        public void AcceptPauseTest()
        {
            var target = GetService();
            var actual = target.AcceptPause;
            //ok if no exception
        }

        /// <summary>
        ///A test for AcceptStop
        ///</summary>
        [TestMethod()]
        public void AcceptStopTest()
        {
            var target = GetService();
            var actual = target.AcceptStop;
            //ok if no exception
        }

        /// <summary>
        ///A test for Caption
        ///</summary>
        [TestMethod()]
        public void CaptionTest()
        {
            var target = GetService();

            var actual = target.Caption;
            Assert.IsFalse(string.IsNullOrWhiteSpace(actual), "Caption empty?");
        }

        /// <summary>
        ///A test for CheckPoint
        ///</summary>
        [TestMethod()]
        public void CheckPointTest()
        {
            var target = GetService();
            var actual = target.CheckPoint;
            //ok if no exception
        }

        /// <summary>
        ///A test for CreationClassName
        ///</summary>
        [TestMethod()]
        public void CreationClassNameTest()
        {
            var target = GetService();
            var actual = target.CreationClassName;
            Assert.IsFalse(string.IsNullOrWhiteSpace(actual), "CreationClassName empty?");

        }

        /// <summary>
        ///A test for Description
        ///</summary>
        [TestMethod()]
        public void DescriptionTest()
        {
            var target = GetService();
            var actual = target.Description;
            Assert.IsFalse(string.IsNullOrWhiteSpace(actual), "Description empty?");
        }

        /// <summary>
        ///A test for DesktopInteract
        ///</summary>
        [TestMethod()]
        public void DesktopInteractTest()
        {
            var target = GetService();
            var actual = target.DesktopInteract;
            //ok if no exception
        }

        /// <summary>
        ///A test for DisplayName
        ///</summary>
        [TestMethod()]
        public void DisplayNameTest()
        {
            var target = GetService();
            var actual = target.DisplayName;
            Assert.IsFalse(string.IsNullOrWhiteSpace(actual), "DisplayName empty?");
        }

        /// <summary>
        ///A test for ErrorControl
        ///</summary>
        [TestMethod()]
        public void ErrorControlTest()
        {
            
            var target = GetService(); 
            var actual = target.ErrorControl;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for ExitCode
        ///</summary>
        [TestMethod()]
        public void ExitCodeTest()
        {
            
            var target = GetService();; 
            var actual = target.ExitCode;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for InstallDate
        ///</summary>
        [TestMethod()]
        public void InstallDateTest()
        {
            
            var target = GetService();; 
            var actual = target.InstallDate;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {
            
            var target = GetService();; 
            var actual = target.Name;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "Name not returning anything");
        }

        /// <summary>
        ///A test for PathName
        ///</summary>
        [TestMethod()]
        public void PathNameTest()
        {
            
            var target = GetService();; 
            var actual = target.PathName;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "PathName not returning anything");
        }

        /// <summary>
        ///A test for ProcessId
        ///</summary>
        [TestMethod()]
        public void ProcessIdTest()
        {
            
            var target = GetService();; 
            var actual = target.ProcessId;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for ServiceSpecificExitCode
        ///</summary>
        [TestMethod()]
        public void ServiceSpecificExitCodeTest()
        {
            
            var target = GetService();; 
            var actual = target.ServiceSpecificExitCode;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for ServiceType
        ///</summary>
        [TestMethod()]
        public void ServiceTypeTest()
        {
            
            var target = GetService();; 
            var actual = target.ServiceType;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for StartMode
        ///</summary>
        [TestMethod()]
        public void StartModeTest()
        {
            
            var target = GetService();; 
            var actual = target.StartMode;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for StartName
        ///</summary>
        [TestMethod()]
        public void StartNameTest()
        {
            
            var target = GetService();; 
            var actual = target.StartName;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "StartName not returning anything");
        }

        /// <summary>
        ///A test for Started
        ///</summary>
        [TestMethod()]
        public void StartedTest()
        {
            
            var target = GetService();; 
            var actual = target.Started;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for State
        ///</summary>
        [TestMethod()]
        public void StateTest()
        {
            
            var target = GetService();; 
            var actual = target.State;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for Status
        ///</summary>
        [TestMethod()]
        public void StatusTest()
        {
            
            var target = GetService();; 
            var actual = target.Status;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for SystemCreationClassName
        ///</summary>
        [TestMethod()]
        public void SystemCreationClassNameTest()
        {
            
            var target = GetService();; 
            var actual = target.SystemCreationClassName;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "SystemCreationClassName not returning anything");
        }

        /// <summary>
        ///A test for SystemName
        ///</summary>
        [TestMethod()]
        public void SystemNameTest()
        {
            
            var target = GetService();; 
            var actual = target.SystemName;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "SystemName not returning anything");
        }

        /// <summary>
        ///A test for TagId
        ///</summary>
        [TestMethod()]
        public void TagIdTest()
        {
            
            var target = GetService();
            var actual = target.TagId;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for WaitHint
        ///</summary>
        [TestMethod()]
        public void WaitHintTest()
        {
            
            var target = GetService();
            var actual = target.WaitHint;
            //simple test, no meaning.
        }
    }
}
