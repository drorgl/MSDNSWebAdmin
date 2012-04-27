using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for MINFOTypeTest and is intended
    ///to contain all MINFOTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MINFOTypeTest
    {

        public static MINFOType GetMINFORR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "minfo." + zone.Name);
            if (record == null)
            {
                record = MINFOType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "minfo." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, "respMailbox", "errMailbox");
                zone.WriteBackZone();
                record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "minfo." + zone.Name);
            }
            return (MINFOType)record.UnderlyingRecord;
        }

        /// <summary>
        ///A test for MINFOType Constructor
        ///</summary>
        [TestMethod()]
        
        public void MINFOTypeConstructorTest()
        {
            
            var target = GetMINFORR();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for CreateInstanceFromPropertyData
        ///</summary>
        [TestMethod()]
        public void CreateInstanceFromPropertyDataTest()
        {
            var target = GetMINFORR();
            target.Delete();
        }

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {

            var target = GetMINFORR();

            var oldttl = target.TTL;
            var olderrmb = target.ErrorMailbox;
            var oldrmb = target.ResponsibleMailbox;


            var newttl = oldttl + TimeSpan.FromSeconds(10);
            var newerrmb = "hello" + olderrmb;
            var newrmb = "hello" + oldrmb;


            var newisdn = target.Modify(newttl, newrmb, newerrmb);
            Assert.AreEqual(newttl, newisdn.TTL, "Unable to modify TTL");
            Assert.AreEqual(newerrmb, newisdn.ErrorMailbox, "Unable to modify ErrorMailbox");
            Assert.AreEqual(newrmb, newisdn.ResponsibleMailbox, "Unable to modify ResponsibleMailbox");

            target = newisdn.Modify(oldttl, oldrmb, olderrmb);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(olderrmb, target.ErrorMailbox, "Unable to revert ErrorMailbox");
            Assert.AreEqual(oldrmb, target.ResponsibleMailbox, "Unable to revert ResponsibleMailbox");
            
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetMINFORR();
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for ErrorMailbox
        ///</summary>
        [TestMethod()]
        public void ErrorMailboxTest()
        {
            
            var target = GetMINFORR();
            var actual = target.ErrorMailbox;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ErrorMailbox not returning anything");
        }

        /// <summary>
        ///A test for ResponsibleMailbox
        ///</summary>
        [TestMethod()]
        public void ResponsibleMailboxTest()
        {
            
            var target = GetMINFORR();
            var actual = target.ResponsibleMailbox;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ResponsibleMailbox not returning anything");
        }
    }
}
