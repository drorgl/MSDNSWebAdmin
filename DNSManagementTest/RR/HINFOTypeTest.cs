using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for HINFOTypeTest and is intended
    ///to contain all HINFOTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HINFOTypeTest
    {

        public static HINFOType GetHINFORR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "hinfo." + zone.Name);
            if (record == null)
                record = HINFOType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "hinfo." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null,"cpu1","os1");
            return (HINFOType)record.UnderlyingRecord;
        }

        /// <summary>
        ///A test for HINFOType Constructor
        ///</summary>
        [TestMethod()]
        
        public void HINFOTypeConstructorTest()
        {
            
            var target = GetHINFORR();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for CreateInstanceFromPropertyData
        ///</summary>
        [TestMethod()]
        public void CreateInstanceFromPropertyDataTest()
        {
            var target = GetHINFORR();
            target.Delete();

        }

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            var target = GetHINFORR();

            var oldttl = target.TTL;
            var oldcpu = target.CPU;
            var oldos = target.OS;


            var newttl = oldttl + TimeSpan.FromSeconds(10);
            var newcpu = "hello" + oldcpu;
            var newos = "hello" + oldos;


            var newhinfo = target.Modify(newttl,newcpu,newos);
            Assert.AreEqual(newttl, newhinfo.TTL, "Unable to modify TTL");
            Assert.AreEqual(newcpu, newhinfo.CPU, "Unable to modify CPU");
            Assert.AreEqual(newos, newhinfo.OS, "Unable to modify OS");

            target = newhinfo.Modify(oldttl, oldcpu,oldos);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(oldcpu, target.CPU, "Unable to revert CPU");
            Assert.AreEqual(oldos, target.OS, "Unable to revert OS");

        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetHINFORR();
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for CPU
        ///</summary>
        [TestMethod()]
        public void CPUTest()
        {
            
            var target = GetHINFORR();
            var actual = target.CPU;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "CPU not returning anything");
        }

        /// <summary>
        ///A test for OS
        ///</summary>
        [TestMethod()]
        public void OSTest()
        {
            
            var target = GetHINFORR();
            var actual = target.OS;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "OS not returning anything");
        }
    }
}
