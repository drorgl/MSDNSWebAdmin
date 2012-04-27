using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for PTRTypeTest and is intended
    ///to contain all PTRTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PTRTypeTest
    {

        public static PTRType GetPTRRR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "ptr." + zone.Name);
            if (record == null)
                record = PTRType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "ptr." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, "ptrdomain1");
            return (PTRType)record.UnderlyingRecord;
        }

        /// <summary>
        ///A test for PTRType Constructor
        ///</summary>
        [TestMethod()]
        public void PTRTypeConstructorTest()
        {
            
            var target = GetPTRRR();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for CreateInstanceFromPropertyData
        ///</summary>
        [TestMethod()]
        public void CreateInstanceFromPropertyDataTest()
        {
            var target = GetPTRRR();
            target.Delete();

        }

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            var target = GetPTRRR();

            var oldttl = target.TTL;
            var oldptrdomname = target.PTRDomainName;

            var newttl = oldttl + TimeSpan.FromSeconds(5);
            var newptrdomname = "hello" + oldptrdomname;

            var newptr = target.Modify(newttl, newptrdomname);

            Assert.AreEqual(newttl, newptr.TTL, "Unable to modify TTL");
            Assert.AreEqual(newptrdomname, newptr.PTRDomainName, "Unable to modify PTRDomainName");

            target = newptr.Modify(oldttl, oldptrdomname);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(oldptrdomname, target.PTRDomainName, "Unable to revert PTRDomainName");


        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetPTRRR();
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for PTRDomainName
        ///</summary>
        [TestMethod()]
        public void PTRDomainNameTest()
        {
            
            var target = GetPTRRR();
            var actual = target.PTRDomainName;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "PTRDomainName not returning anything");
        }
    }
}
