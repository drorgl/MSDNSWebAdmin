using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;
using DNSManagement.Extensions;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for ATypeTest and is intended
    ///to contain all ATypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ATypeTest
    {

        public static AType GetARR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "a." + zone.Name);
            if (record == null)
                record = AType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "a." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, "192.168.0.1");
            return (AType)record.UnderlyingRecord;
        }


        /// <summary>
        ///A test for AType Constructor
        ///</summary>
        [TestMethod()]
        
        public void ATypeConstructorTest()
        {
            
            var target = GetARR();
            Assert.IsNotNull(target, "Unable to get A record");
        }

        /// <summary>
        ///A test for CreateInstanceFromPropertyData
        ///</summary>
        [TestMethod()]
        public void CreateInstanceFromPropertyDataTest()
        {
            var target = GetARR();
            target.Delete();
        }

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            
            var target = GetARR();

            var oldttl = target.TTL;
            var oldip = target.IPAddress;

            var newttl = oldttl + TimeSpan.FromSeconds(5);
            var newip = "1.1.1.1";

            var newa = target.Modify(newttl, newip);

            Assert.AreEqual(newttl, newa.TTL, "Unable to modify TTL");
            Assert.AreEqual(newip, newa.IPAddress, "Unable to modify IPAddress");

            target = newa.Modify(oldttl, oldip);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(oldip, target.IPAddress, "Unable to revert IPAddress");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetARR();
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for IPAddress
        ///</summary>
        [TestMethod()]
        public void IPAddressTest()
        {
            
            var target = GetARR();
            var actual = target.IPAddress;
            var ip = IPHelper.ParseIP(actual);

            Assert.IsFalse(string.IsNullOrEmpty(actual), "IPAddress not returning anything");

            Assert.IsTrue(ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork, "IPAddress is not an ip");
        }
    }
}
