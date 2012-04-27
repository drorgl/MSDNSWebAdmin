using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for ISDNTypeTest and is intended
    ///to contain all ISDNTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ISDNTypeTest
    {

        public static ISDNType GetISDNRR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "isdn." + zone.Name);
            if (record == null)
                record = ISDNType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "isdn." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null,"123456","subaddr1");
            return (ISDNType)record.UnderlyingRecord;
        }
        /// <summary>
        ///A test for ISDNType Constructor
        ///</summary>
        [TestMethod()]
        
        public void ISDNTypeConstructorTest()
        {
            
            var target = GetISDNRR();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for CreateInstanceFromPropertyData
        ///</summary>
        [TestMethod()]
        public void CreateInstanceFromPropertyDataTest()
        {
            var target = GetISDNRR();
            target.Delete();
            
        }

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            var target = GetISDNRR();

            var oldttl = target.TTL;
            var oldisdnnumber = target.ISDNNumber;
            var oldsubaddr = target.SubAddress;


            var newttl = oldttl + TimeSpan.FromSeconds(10);
            var newisdnnumber = "123" + oldisdnnumber;
            var newsubaddr = "hello" + oldsubaddr;


            var newisdn = target.Modify(newttl, newisdnnumber, newsubaddr);
            Assert.AreEqual(newttl, newisdn.TTL, "Unable to modify TTL");
            Assert.AreEqual(newisdnnumber, newisdn.ISDNNumber, "Unable to modify ISDNNumber");
            Assert.AreEqual(newsubaddr, newisdn.SubAddress, "Unable to modify SubAddress");

            target = newisdn.Modify(oldttl, oldisdnnumber, oldsubaddr);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(oldisdnnumber, target.ISDNNumber, "Unable to revert ISDNNumber");
            Assert.AreEqual(oldsubaddr, target.SubAddress, "Unable to revert SubAddress");

        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetISDNRR();
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for ISDNNumber
        ///</summary>
        [TestMethod()]
        public void ISDNNumberTest()
        {
            
            var target = GetISDNRR();
            var actual = target.ISDNNumber;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ISDNNumber not returning anything");
        }

        /// <summary>
        ///A test for SubAddress
        ///</summary>
        [TestMethod()]
        public void SubAddressTest()
        {
            
            var target = GetISDNRR();
            var actual = target.SubAddress;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "SubAddress not returning anything");
        }
    }
}
