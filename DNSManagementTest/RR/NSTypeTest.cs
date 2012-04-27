using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for NSTypeTest and is intended
    ///to contain all NSTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class NSTypeTest
    {

        public static NSType GetNSRR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "ns." + zone.Name);
            if (record == null)
                record = NSType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "ns." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, "nshost1");
            return (NSType)record.UnderlyingRecord;
        }

        /// <summary>
        ///A test for NSType Constructor
        ///</summary>
        [TestMethod()]
        
        public void NSTypeConstructorTest()
        {
            
            var target = GetNSRR();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for CreateInstanceFromPropertyData
        ///</summary>
        [TestMethod()]
        public void CreateInstanceFromPropertyDataTest()
        {
            var target = GetNSRR();
            target.Delete();
        }

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            var target = GetNSRR();

            var oldttl = target.TTL;
            var oldnshost = target.NSHost;

            var newttl = oldttl + TimeSpan.FromSeconds(5);
            var newnshost = "hello" + oldnshost;

            var newns = target.Modify(newttl, newnshost);

            Assert.AreEqual(newttl, newns.TTL, "Unable to modify TTL");
            Assert.AreEqual(newnshost, newns.NSHost, "Unable to modify NSHost");

            target = newns.Modify(oldttl, oldnshost);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(oldnshost, target.NSHost, "Unable to revert NSHost");

        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetNSRR();
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for NSHost
        ///</summary>
        [TestMethod()]
        public void NSHostTest()
        {
            
            var target = GetNSRR();
            var actual = target.NSHost;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "NSHost not returning anything");
        }
    }
}
