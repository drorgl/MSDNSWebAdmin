using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for AAAATypeTest and is intended
    ///to contain all AAAATypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AAAATypeTest
    {

        public static AAAAType GetAAAARR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "aaaa." + zone.Name);
            if (record == null)
                record = AAAAType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "aaaa." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, "2002:2ed2:2e40::2ed2:2e40");
            return (AAAAType)record.UnderlyingRecord;
        }

        /// <summary>
        ///A test for AAAAType Constructor
        ///</summary>
        [TestMethod()]
        
        public void AAAATypeConstructorTest()
        {
            
            var target = GetAAAARR();
            Assert.IsNotNull(target, "Unable to get AAAA record");
        }

        /// <summary>
        ///A test for CreateInstanceFromPropertyData
        ///</summary>
        [TestMethod()]
        public void CreateInstanceFromPropertyDataTest()
        {
            var target = GetAAAARR();
            target.Delete();
        }

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            
            var target = GetAAAARR();
            var oldttl = target.TTL;
            var oldipv6 = target.IPv6Address;

            var newttl = oldttl + TimeSpan.FromMinutes(5);
            var newipv6 = "fe80::c036:b9c0:60d4:d530";
            var newaaaa = target.Modify(newttl, newipv6);
            Assert.AreEqual(newttl, newaaaa.TTL, "Unable to modify TTL");
            Assert.AreEqual(newipv6, newaaaa.IPv6Address, "Unable to modify IPv6Address");

            target = newaaaa.Modify(oldttl, oldipv6);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(oldipv6, target.IPv6Address, "Unable to revert IPv6Address");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetAAAARR();
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for IPv6Address
        ///</summary>
        [TestMethod()]
        public void IPv6AddressTest()
        {
            
            var target = GetAAAARR();
            string actual;
            actual = target.IPv6Address;
            var ip = DNSManagement.Extensions.IPHelper.ParseIP(actual);
            Assert.IsTrue(ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6,"IP is not IPv6");
        }
    }
}
