using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for RTTypeTest and is intended
    ///to contain all RTTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RTTypeTest
    {

        public static RTType GetRTRR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "rt." + zone.Name);
            if (record == null)
                record = RTType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "rt." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, "imhost",1);
            return (RTType)record.UnderlyingRecord;
        }


        /// <summary>
        ///A test for RTType Constructor
        ///</summary>
        [TestMethod()]
        
        public void RTTypeConstructorTest()
        {
            
            var target = GetRTRR();
            Assert.IsNotNull(target);
        }

        ///// <summary>
        /////A test for CreateInstanceFromPropertyData
        /////</summary>
        //[TestMethod()]
        //public void CreateInstanceFromPropertyDataTest()
        //{
        //    var target = GetRTRR();
        //    target.Delete();
            
        //}

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            var target = GetRTRR();


            var oldttl = target.TTL;
            var oldimhost = target.IntermediateHost;
            var oldpref = target.Preference;


            var newttl = oldttl + TimeSpan.FromSeconds(10);
            var newimhost = "hello" + oldimhost;
            var newpref = oldpref + 1;


            var newrt = target.Modify(newttl, newimhost, (ushort)newpref);
            Assert.AreEqual(newttl, newrt.TTL, "Unable to modify TTL");
            Assert.AreEqual(newimhost, newrt.IntermediateHost, "Unable to modify IntermediateHost");
            Assert.AreEqual(newpref, newrt.Preference, "Unable to modify Preference");

            target = newrt.Modify(oldttl, oldimhost, oldpref);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(oldimhost, target.IntermediateHost, "Unable to revert IntermediateHost");
            Assert.AreEqual(oldpref, target.Preference, "Unable to revert Preference");

        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetRTRR(); 
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for IntermediateHost
        ///</summary>
        [TestMethod()]
        public void IntermediateHostTest()
        {
            
            var target = GetRTRR(); 
            var actual = target.IntermediateHost;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "IntermediateHost not returning anything");
        }

        /// <summary>
        ///A test for Preference
        ///</summary>
        [TestMethod()]
        public void PreferenceTest()
        {
            
            var target = GetRTRR(); 
            var actual = target.Preference;
        }
    }
}
