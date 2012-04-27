using DNSManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement.RR;
using System.Text;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for DomainTest and is intended
    ///to contain all DomainTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DomainTest
    {
        public static Domain[] GetDomain()
        {
            var server = ServerTest.GetServer();
            var domains = server.GetDomains();
            return domains;
        }


        /// <summary>
        ///A test for Domain Constructor
        ///</summary>
        [TestMethod()]
        
        public void DomainConstructorTest()
        {
            
            var target = GetDomain();
            Assert.IsTrue(target.Length > 0, "No Domains?");
        }

        
        /// <summary>
        ///A test for GetRecords
        ///</summary>
        [TestMethod()]
        public void GetRecordsTest()
        {

            var target = GetDomain().FirstOrDefault(); ; 
            var actual = target.GetRecords();
            Assert.IsTrue(actual.Length > 0, "No Records?");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetDomain(); 
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

       

        /// <summary>
        ///A test for ContainerName
        ///</summary>
        [TestMethod()]
        public void ContainerNameTest()
        {
            
            var target = GetDomain().FirstOrDefault(); 
            var actual = target.ContainerName;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ContainerName not returning anything");
        }

        /// <summary>
        ///A test for DnsServerName
        ///</summary>
        [TestMethod()]
        public void DnsServerNameTest()
        {
            
            var target = GetDomain().FirstOrDefault(); 
            var actual = target.DnsServerName;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "DnsServerName not returning anything");
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {

            var target = GetDomain().FirstOrDefault() ; 
            var actual = target.Name;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "Name not returning anything");
        }
    }
}
