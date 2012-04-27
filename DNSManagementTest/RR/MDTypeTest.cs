using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for MDTypeTest and is intended
    ///to contain all MDTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MDTypeTest
    {
        public static MDType GetMDRR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "md." + zone.Name);
            if (record == null)
                record = MDType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "md." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, "mdhost");
            return (MDType)record.UnderlyingRecord;
        }


        /// <summary>
        ///A test for MDType Constructor
        ///</summary>
        [TestMethod()]
        
        public void MDTypeConstructorTest()
        {
            
            var target = GetMDRR();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for CreateInstanceFromPropertyData
        ///</summary>
        [TestMethod()]
        public void CreateInstanceFromPropertyDataTest()
        {
            var target = GetMDRR();
            target.Delete();

        }

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            var target = GetMDRR();

            var oldttl = target.TTL;
            var oldmd = target.MDHost;

            var newttl = oldttl + TimeSpan.FromSeconds(5);
            var newmdhost = "hello" + oldmd;

            var newmd = target.Modify(newttl, newmdhost);

            Assert.AreEqual(newttl, newmd.TTL, "Unable to modify TTL");
            Assert.AreEqual(newmdhost, newmd.MDHost, "Unable to modify MDHost");

            target = newmd.Modify(oldttl, oldmd);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(oldmd, target.MDHost, "Unable to revert MDHost");

        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetMDRR(); 
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for MDHost
        ///</summary>
        [TestMethod()]
        public void MDHostTest()
        {
            
            var target = GetMDRR(); 
            var actual = target.MDHost;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "MDHost not returning anything");
        }
    }
}
