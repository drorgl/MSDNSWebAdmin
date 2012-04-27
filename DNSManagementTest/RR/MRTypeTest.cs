using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for MRTypeTest and is intended
    ///to contain all MRTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MRTypeTest
    {
        public static MRType GetMRRR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "mr." + zone.Name);
            if (record == null)
                record = MRType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "mr." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null,"mrmailbox1");
            return (MRType)record.UnderlyingRecord;
        }

        /// <summary>
        ///A test for MRType Constructor
        ///</summary>
        [TestMethod()]
        
        public void MRTypeConstructorTest()
        {
            
            var target = GetMRRR();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for CreateInstanceFromPropertyData
        ///</summary>
        [TestMethod()]
        public void CreateInstanceFromPropertyDataTest()
        {
            var target = GetMRRR();
            target.Delete();
        }

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            var target = GetMRRR();

            var oldttl = target.TTL;
            var oldmrmb = target.MRMailbox;

            var newttl = oldttl + TimeSpan.FromSeconds(10);
            var newmrmb = "hello" + oldmrmb;

            var newmr = target.Modify(newttl, newmrmb);
            Assert.AreEqual(newttl, newmr.TTL, "Unable to modify TTL");
            Assert.AreEqual(newmrmb, newmr.MRMailbox, "Unable to modify MRMailbox");

            target = newmr.Modify(oldttl, oldmrmb);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(oldmrmb, target.MRMailbox, "Unable to revert MRMailbox");


        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetMRRR();
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for MRMailbox
        ///</summary>
        [TestMethod()]
        public void MRMailboxTest()
        {
            
            var target = GetMRRR();
            var actual = target.MRMailbox;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "MRMailbox not returning anything");
        }
    }
}
