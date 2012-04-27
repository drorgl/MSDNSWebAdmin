using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for WINSRTypeTest and is intended
    ///to contain all WINSRTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WINSRTypeTest
    {

        public static WINSRType GetWINSRRR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.ZoneGetReverseZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.ResourceRecordType == ResourceRecord.ResourceRecordEnum.WINSR);
            if (record == null)
                record = WINSRType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, WINSRType.MappingFlagEnum.NonReplication,TimeSpan.FromSeconds(2), TimeSpan.FromMinutes(15), "resultDomain1.");
            return (WINSRType)record.UnderlyingRecord;
        }

        /// <summary>
        ///A test for WINSRType Constructor
        ///</summary>
        [TestMethod()]
        
        public void WINSRTypeConstructorTest()
        {
            
            var target = GetWINSRRR();
            Assert.IsNotNull(target);
        }

        ///// <summary>
        /////A test for CreateInstanceFromPropertyData
        /////</summary>
        //[TestMethod()]
        //public void CreateInstanceFromPropertyDataTest()
        //{
        //    var target = GetWINSRRR();
        //    target.Delete();

        //}

        ///// <summary>
        /////A test for Modify
        /////</summary>
        //[TestMethod()]
        //public void ModifyTest()
        //{
        //    //unsupported by WMI, maybe in the next version

        //    var target = GetWINSRRR();


        //    var oldttl = target.TTL;
        //    var oldmf = target.MappingFlag;
        //    var oldlookuptm = target.LookupTimeout;
        //    var oldcacheto = target.CacheTimeout;
        //    var oldresdom = target.ResultDomain;


        //    var newttl = oldttl + TimeSpan.FromSeconds(10);
        //    var newmf = DNSManagement.RR.WINSRType.MappingFlagEnum.NonReplication;
        //    var newlookuptm = oldlookuptm + TimeSpan.FromSeconds(10);
        //    var newcacheto = oldcacheto + TimeSpan.FromSeconds(10);
        //    var newresdom = "hello" + oldresdom;


        //    var newwinsr = target.Modify(newttl, newmf, newlookuptm, newcacheto, newresdom);
        //    Assert.AreEqual(newttl, newwinsr.TTL, "Unable to modify TTL");
        //    Assert.AreEqual(newmf, newwinsr.MappingFlag, "Unable to modify MappingFlag");
        //    Assert.AreEqual(newlookuptm, newwinsr.LookupTimeout, "Unable to modify LookupTimeout");
        //    Assert.AreEqual(newcacheto, newwinsr.CacheTimeout, "Unable to modify CacheTimeout");
        //    Assert.AreEqual(newresdom, newwinsr.ResultDomain, "Unable to modify ResultDomain");

        //    target = newwinsr.Modify(oldttl, oldmf, oldlookuptm, oldcacheto, oldresdom);

        //    Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
        //    Assert.AreEqual(oldmf, target.MappingFlag, "Unable to revert MappingFlag");
        //    Assert.AreEqual(oldlookuptm, target.LookupTimeout, "Unable to revert LookupTimeout");
        //    Assert.AreEqual(oldcacheto, target.CacheTimeout, "Unable to revert CacheTimeout");
        //    Assert.AreEqual(oldresdom, target.ResultDomain, "Unable to revert ResultDomain");

        //}

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetWINSRRR(); 
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for CacheTimeout
        ///</summary>
        [TestMethod()]
        public void CacheTimeoutTest()
        {
            
            var target = GetWINSRRR(); 
            var actual = target.CacheTimeout;
        }

        /// <summary>
        ///A test for LookupTimeout
        ///</summary>
        [TestMethod()]
        public void LookupTimeoutTest()
        {
            
            var target = GetWINSRRR(); 
            var actual = target.LookupTimeout;
        }

        /// <summary>
        ///A test for MappingFlag
        ///</summary>
        [TestMethod()]
        public void MappingFlagTest()
        {
            
            var target = GetWINSRRR(); 
            var actual = target.MappingFlag;
        }

        /// <summary>
        ///A test for ResultDomain
        ///</summary>
        [TestMethod()]
        public void ResultDomainTest()
        {
            
            var target = GetWINSRRR(); 
            var actual = target.ResultDomain;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ResultDomain not returning anything");
        }
    }
}
