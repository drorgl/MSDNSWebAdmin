using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for RPTypeTest and is intended
    ///to contain all RPTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RPTypeTest
    {

        public static RPType GetRPRR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "rp." + zone.Name);
            if (record == null)
                record = RPType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "rp." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, "rpmb","domain1");
            return (RPType)record.UnderlyingRecord;
        }

        /// <summary>
        ///A test for RPType Constructor
        ///</summary>
        [TestMethod()]
        
        public void RPTypeConstructorTest()
        {
            
            var target = GetRPRR();
            Assert.IsNotNull(target);
        }

        ///// <summary>
        /////A test for CreateInstanceFromPropertyData
        /////</summary>
        //[TestMethod()]
        //public void CreateInstanceFromPropertyDataTest()
        //{
        //    var target = GetRPRR();
        //    target.Delete();

        //}

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            var target = GetRPRR();

            var oldttl = target.TTL;
            var oldrpmb = target.RPMailbox;
            var oldtxtdomname = target.TXTDomainName;


            var newttl = oldttl + TimeSpan.FromSeconds(10);
            var newrpmb = "hello" + oldrpmb;
            var newtxtdomname = "hello" + oldtxtdomname;


            var newrp = target.Modify(newttl, newrpmb, newtxtdomname);
            Assert.AreEqual(newttl, newrp.TTL, "Unable to modify TTL");
            Assert.AreEqual(newrpmb, newrp.RPMailbox, "Unable to modify RPMailbox");
            Assert.AreEqual(newtxtdomname, newrp.TXTDomainName, "Unable to modify TXTDomainName");

            target = newrp.Modify(oldttl, oldrpmb, oldtxtdomname);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(oldrpmb, target.RPMailbox, "Unable to revert RPMailbox");
            Assert.AreEqual(oldtxtdomname, target.TXTDomainName, "Unable to revert TXTDomainName");


        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetRPRR(); 
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for RPMailbox
        ///</summary>
        [TestMethod()]
        public void RPMailboxTest()
        {
            
            var target = GetRPRR(); 
            var actual = target.RPMailbox;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "RPMailbox not returning anything");
        }

        /// <summary>
        ///A test for TXTDomainName
        ///</summary>
        [TestMethod()]
        public void TXTDomainNameTest()
        {
            
            var target = GetRPRR(); 
            var actual = target.TXTDomainName;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "TXTDomainName not returning anything");
        }
    }
}
