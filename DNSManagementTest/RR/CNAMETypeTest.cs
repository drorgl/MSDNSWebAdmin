using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for CNAMETypeTest and is intended
    ///to contain all CNAMETypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CNAMETypeTest
    {


        public static CNAMEType GetCNAMERR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "cname." + zone.Name);
            if (record == null)
                record = CNAMEType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "cname." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, "primaryname1");
            return (CNAMEType)record.UnderlyingRecord;
        }

        /// <summary>
        ///A test for CNAMEType Constructor
        ///</summary>
        [TestMethod()]
        
        public void CNAMETypeConstructorTest()
        {
            
            var target = GetCNAMERR();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for CreateInstanceFromPropertyData
        ///</summary>
        [TestMethod()]
        public void CreateInstanceFromPropertyDataTest()
        {
            var target = GetCNAMERR();
            target.Delete();
        }

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            var target = GetCNAMERR();

            var oldttl = target.TTL;
            var oldname = target.PrimaryName;

            var newttl = oldttl + TimeSpan.FromSeconds(10);
            var newname ="hello" + oldname ;

            var newcname = target.Modify(newttl, newname);
            Assert.AreEqual(newttl, newcname.TTL, "Unable to modify TTL");
            Assert.AreEqual(newname, newcname.PrimaryName, "Unable to modify PrimaryName");

            target = newcname.Modify(oldttl, oldname);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(oldname, target.PrimaryName, "Unable to revert PrimaryName");

        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetCNAMERR(); 
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for PrimaryName
        ///</summary>
        [TestMethod()]
        public void PrimaryNameTest()
        {
            
            var target = GetCNAMERR(); 
            var actual = target.PrimaryName;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "PrimaryName not returning anything");
        }
    }
}
