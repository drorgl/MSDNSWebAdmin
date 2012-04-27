using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for MGTypeTest and is intended
    ///to contain all MGTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MGTypeTest
    {

        public static MGType GetMGRR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "mg." + zone.Name);
            if (record == null)
                record = MGType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "mg." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null,"mgmailbox1");
            return (MGType)record.UnderlyingRecord;
        }
        /// <summary>
        ///A test for MGType Constructor
        ///</summary>
        [TestMethod()]
        
        public void MGTypeConstructorTest()
        {
            
            var target = GetMGRR();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for CreateInstanceFromPropertyData
        ///</summary>
        [TestMethod()]
        public void CreateInstanceFromPropertyDataTest()
        {
            var target = GetMGRR();
            target.Delete();
        }

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            var target = GetMGRR();

            var oldttl = target.TTL;
            var oldmg = target.MGMailbox;

            var newttl = oldttl + TimeSpan.FromSeconds(5);
            var newmgmb = "hello" + oldmg;

            var newmf = target.Modify(newttl, newmgmb);

            Assert.AreEqual(newttl, newmf.TTL, "Unable to modify TTL");
            Assert.AreEqual(newmgmb, newmf.MGMailbox, "Unable to modify MGMailbox");

            target = newmf.Modify(oldttl, oldmg);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(oldmg, target.MGMailbox, "Unable to revert MGMailbox");

        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetMGRR();
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for MGMailbox
        ///</summary>
        [TestMethod()]
        public void MGMailboxTest()
        {
            
            var target = GetMGRR();
            var actual = target.MGMailbox;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "MGMailbox not returning anything");
        }
    }
}
