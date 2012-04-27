using DNSManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for ServiceManagerTest and is intended
    ///to contain all ServiceManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ServiceManagerTest
    {

        public static ServiceManager GetServiceManager()
        {
            string host = Settings.Hostname;
            string username = Settings.Username;
            string password = Settings.Password;
            return new ServiceManager(host, username, password);
        }

        /// <summary>
        ///A test for ServiceManager Constructor
        ///</summary>
        [TestMethod()]
        public void ServiceManagerConstructorTest()
        {
            string host = Settings.Hostname;
            string username = Settings.Username;
            string password = Settings.Password;
            ServiceManager target = new ServiceManager(host, username, password);
            //no exceptions, ok
        }

        /// <summary>
        ///A test for ServiceManager Constructor
        ///</summary>
        [TestMethod()]
        public void ServiceManagerConstructorTest1()
        {
            ServiceManager target = new ServiceManager();
            //no exceptions, ok
        }

        ///// <summary>
        /////A test for Create
        /////</summary>
        //[TestMethod()]
        //public void CreateTest()
        //{
        //    Assert.Inconclusive("Not tested, out of scope");
        //    //ServiceManager target = new ServiceManager(); 
        //    //string name = string.Empty; 
        //    //string displayName = string.Empty; 
        //    //string pathName = string.Empty; 
        //    //Service.ServiceTypeEnum serviceType = new Service.ServiceTypeEnum(); 
        //    //Service.ErrorControlEnum errorControl = new Service.ErrorControlEnum(); 
        //    //Service.StartModeEnum startMode = new Service.StartModeEnum(); 
        //    //bool desktopInteract = false; 
        //    //string startName = string.Empty; 
        //    //string startPassword = string.Empty; 
        //    //string loadOrderGroup = string.Empty; 
        //    //string[] loadOrderGroupDependencies = null; 
        //    //string[] serviceDependencies = null; 
        //    //ServiceManager.MethodExecutionResultEnum expected = new ServiceManager.MethodExecutionResultEnum(); 
        //    //ServiceManager.MethodExecutionResultEnum actual;
        //    //actual = target.Create(name, displayName, pathName, serviceType, errorControl, startMode, desktopInteract, startName, startPassword, loadOrderGroup, loadOrderGroupDependencies, serviceDependencies);
        //    //Assert.AreEqual(expected, actual);
        //    //Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        /// <summary>
        ///A test for List
        ///</summary>
        [TestMethod()]
        public void ListTest()
        {
            ServiceManager target = new ServiceManager(); 
            
            var actual = target.List();
            Assert.IsTrue(actual.Count > 0, "No Services?");
        }
    }
}
