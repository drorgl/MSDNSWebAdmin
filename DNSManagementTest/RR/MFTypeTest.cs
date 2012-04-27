using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for MFTypeTest and is intended
    ///to contain all MFTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MFTypeTest
    {

        public static MFType GetMFRR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "mf." + zone.Name);
            if (record == null)
                record = MFType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "mf." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null,"mfhost");
            return (MFType)record.UnderlyingRecord;
        }

        /// <summary>
        ///A test for MFType Constructor
        ///</summary>
        [TestMethod()]
        
        public void MFTypeConstructorTest()
        {
            
            var target = GetMFRR();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for CreateInstanceFromPropertyData
        ///</summary>
        [TestMethod()]
        public void CreateInstanceFromPropertyDataTest()
        {
            var target = GetMFRR();
            target.Delete();
        }

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            var target = GetMFRR();

            var oldttl = target.TTL;
            var oldmf = target.MFHost;

            var newttl = oldttl + TimeSpan.FromSeconds(5);
            var newmfhost = "hello" + oldmf;

            var newmf = target.Modify(newttl, newmfhost);

            Assert.AreEqual(newttl, newmf.TTL, "Unable to modify TTL");
            Assert.AreEqual(newmfhost, newmf.MFHost, "Unable to modify MFHost");

            target = newmf.Modify(oldttl, oldmf);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(oldmf, target.MFHost, "Unable to revert MFHost");

        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetMFRR();
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for MFHost
        ///</summary>
        [TestMethod()]
        public void MFHostTest()
        {
            
            var target = GetMFRR();
            var actual = target.MFHost;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "MFHost not returning anything");
        }
    }
}
