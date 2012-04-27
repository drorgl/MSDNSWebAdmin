using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for WKSTypeTest and is intended
    ///to contain all WKSTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WKSTypeTest
    {

        public static WKSType GetWKSRR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "wks." + zone.Name);
            if (record == null)
                record = WKSType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "wks." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null,"192.168.0.1","tcp","finger");
            return (WKSType)record.UnderlyingRecord;

        }

        /// <summary>
        ///A test for WKSType Constructor
        ///</summary>
        [TestMethod()]
        
        public void WKSTypeConstructorTest()
        {
            
            var target = GetWKSRR();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for CreateInstanceFromPropertyData
        ///</summary>
        [TestMethod()]
        public void CreateInstanceFromPropertyDataTest()
        {
            var target = GetWKSRR();
            target.Delete();
        }

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            var target = GetWKSRR();


            var oldttl = target.TTL;
            var oldaddress = target.InternetAddress;
            var oldproto = target.IPProtocol;
            var oldservices = target.Services;


            var newttl = oldttl + TimeSpan.FromSeconds(10);
            var newaddress = "1.1.1.2";
            var newproto = "udp";
            var newservices = "tftp";


            var newwks = target.Modify(newttl, newaddress, newproto, newservices);
            Assert.AreEqual(newttl, newwks.TTL, "Unable to modify TTL");
            Assert.AreEqual(newaddress, newwks.InternetAddress, "Unable to modify InternetAddress");
            Assert.AreEqual(newproto, newwks.IPProtocol, "Unable to modify IPProtocol");
            Assert.AreEqual(newservices, newwks.Services, "Unable to modify Services");

            target = newwks.Modify(oldttl, oldaddress, oldproto, oldservices);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(oldaddress, target.InternetAddress, "Unable to revert InternetAddress");
            Assert.AreEqual(oldproto, target.IPProtocol, "Unable to revert IPProtocol");
            Assert.AreEqual(oldservices, target.Services, "Unable to revert Services");

        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetWKSRR(); 
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for IPProtocol
        ///</summary>
        [TestMethod()]
        public void IPProtocolTest()
        {
            
            var target = GetWKSRR(); 
            var actual = target.IPProtocol;
            Assert.IsTrue((actual.ToLower() == "tcp" || actual.ToLower() == "udp"),"IPProtocol is not udp or tcp");
        }

        /// <summary>
        ///A test for InternetAddress
        ///</summary>
        [TestMethod()]
        public void InternetAddressTest()
        {
            
            var target = GetWKSRR(); 
            var actual = target.InternetAddress;
            var ip = DNSManagement.Extensions.IPHelper.ParseIP(actual);
            Assert.IsTrue((ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 || ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork), "InternetAddress is not an IP");
        }

        /// <summary>
        ///A test for Services
        ///</summary>
        [TestMethod()]
        public void ServicesTest()
        {
            
            var target = GetWKSRR(); 
            var actual = target.Services;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "Services not returning anything");
        }
    }
}
