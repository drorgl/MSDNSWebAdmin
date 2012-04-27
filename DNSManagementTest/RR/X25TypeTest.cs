using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for X25TypeTest and is intended
    ///to contain all X25TypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class X25TypeTest
    {
        public static X25Type GetX25RR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "x25." + zone.Name);
            if (record == null)
                record = X25Type.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "x25." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, "01234567");
            return (X25Type)record.UnderlyingRecord;
        }


        /// <summary>
        ///A test for X25Type Constructor
        ///</summary>
        [TestMethod()]
        
        public void X25TypeConstructorTest()
        {
            
            var target = GetX25RR();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for CreateInstanceFromPropertyData
        ///</summary>
        [TestMethod()]
        public void CreateInstanceFromPropertyDataTest()
        {
            var target = GetX25RR();
            target.Delete();
        }

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            var target = GetX25RR();

            var oldttl = target.TTL;
            var oldaddress = target.PSDNAddress;

            var newttl = oldttl + TimeSpan.FromSeconds(10);
            var newaddress = "123" + oldaddress;

            var newx25 = target.Modify(newttl, newaddress);
            Assert.AreEqual(newttl, newx25.TTL, "Unable to modify TTL");
            Assert.AreEqual(newaddress, newx25.PSDNAddress, "Unable to modify PSDNAddress");

            target = newx25.Modify(oldttl, oldaddress);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(oldaddress, target.PSDNAddress, "Unable to revert PSDNAddress");

        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target  = GetX25RR(); 
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for PSDNAddress
        ///</summary>
        [TestMethod()]
        public void PSDNAddressTest()
        {
            
            var target  = GetX25RR(); 
            var actual = target.PSDNAddress;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "PSDNAddress not returning anything");
        }
    }
}
