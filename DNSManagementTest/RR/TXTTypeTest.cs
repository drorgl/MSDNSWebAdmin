using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for TXTTypeTest and is intended
    ///to contain all TXTTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TXTTypeTest
    {
        public static TXTType GetTXTRR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "txt." + zone.Name);
            if (record == null)
                record = TXTType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "txt." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, "descriptivetext1");
            return (TXTType)record.UnderlyingRecord;
        }

        /// <summary>
        ///A test for TXTType Constructor
        ///</summary>
        [TestMethod()]
        
        public void TXTTypeConstructorTest()
        {
            
            var target = GetTXTRR();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for CreateInstanceFromPropertyData
        ///</summary>
        [TestMethod()]
        public void CreateInstanceFromPropertyDataTest()
        {
            var target = GetTXTRR();
            target.Delete();

        }

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            var target = GetTXTRR();

            var oldttl = target.TTL;
            var olddesctxt = target.DescriptiveText;


            var newttl = oldttl + TimeSpan.FromSeconds(10);
            var newdesctxt = "hello" + olddesctxt;


            var newtxt = target.Modify(newttl, newdesctxt);
            Assert.AreEqual(newttl, newtxt.TTL, "Unable to modify TTL");
            Assert.AreEqual(newdesctxt, newtxt.DescriptiveText, "Unable to modify DescriptiveText");

            target = newtxt.Modify(oldttl, olddesctxt);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(olddesctxt, target.DescriptiveText, "Unable to revert DescriptiveText");

        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetTXTRR(); 
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for DescriptiveText
        ///</summary>
        [TestMethod()]
        public void DescriptiveTextTest()
        {
            
            var target = GetTXTRR(); 
            var actual = target.DescriptiveText;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "DescriptiveText not returning anything");
        }
    }
}
