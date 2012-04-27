using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for MBTypeTest and is intended
    ///to contain all MBTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MBTypeTest
    {
        public static MBType GetMBRR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "mb." + zone.Name);
            if (record == null)
                record = MBType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "mb." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, "mbhost");
            return (MBType)record.UnderlyingRecord;
        }


        /// <summary>
        ///A test for MBType Constructor
        ///</summary>
        [TestMethod()]
        
        public void MBTypeConstructorTest()
        {
            
            var target = GetMBRR();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for CreateInstanceFromPropertyData
        ///</summary>
        [TestMethod()]
        public void CreateInstanceFromPropertyDataTest()
        {
            var target = GetMBRR();
            target.Delete();
        }

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {

            var target = GetMBRR(); 

            var oldttl = target.TTL;
            var oldmb = target.MBHost;

            var newttl = oldttl + TimeSpan.FromSeconds(5);
            var newmbhost = "hello" + oldmb;

            var newmb = target.Modify(newttl, newmbhost);

            Assert.AreEqual(newttl, newmb.TTL, "Unable to modify TTL");
            Assert.AreEqual(newmbhost, newmb.MBHost, "Unable to modify MBHost");

            target = newmb.Modify(oldttl, oldmb);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(oldmb, target.MBHost, "Unable to revert MBHost");

        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetMBRR(); 
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for MBHost
        ///</summary>
        [TestMethod()]
        public void MBHostTest()
        {
            
            var target = GetMBRR(); 
            var actual = target.MBHost;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "MBHost not returning anything");
        }
    }
}
