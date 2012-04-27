using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for WINSTypeTest and is intended
    ///to contain all WINSTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WINSTypeTest
    {

        public static WINSType GetWINSRR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.ResourceRecordType == ResourceRecord.ResourceRecordEnum.WINS);
            if (record == null)
                record = WINSType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, WINSType.MappingFlagEnum.NonReplication, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10),"1.1.1.1");
            return (WINSType)record.UnderlyingRecord;
        }

        /// <summary>
        ///A test for WINSType Constructor
        ///</summary>
        [TestMethod()]
        
        public void WINSTypeConstructorTest()
        {
            
            var target = GetWINSRR();
            Assert.IsNotNull(target);
        }

        ///// <summary>
        /////A test for CreateInstanceFromPropertyData
        /////</summary>
        //[TestMethod()]
        //public void CreateInstanceFromPropertyDataTest()
        //{
        //    var target = GetWINSRR();
        //    target.Delete();
        //}

        ///// <summary>
        /////A test for Modify
        /////</summary>
        //[TestMethod()]
        //public void ModifyTest()
        //{
        //    var target = GetWINSRR();


        //    var oldttl = target.TTL;
        //    var oldmf = target.MappingFlag;
        //    var oldlookuptm = target.LookupTimeout;
        //    var oldcacheto = target.CacheTimeout;
        //    var oldwinsservers = target.WinsServers;


        //    var newttl = oldttl + TimeSpan.FromSeconds(10);
        //    var newmf = DNSManagement.RR.WINSType.MappingFlagEnum.Replication;
        //    var newlookuptm = oldlookuptm + TimeSpan.FromSeconds(10);
        //    var newcacheto = oldcacheto + TimeSpan.FromSeconds(10);
        //    string newwinsservers = null;


        //    var newwinsr = target.Modify(newttl, newmf, newlookuptm, newcacheto, newwinsservers);
        //    Assert.AreEqual(newttl, newwinsr.TTL, "Unable to modify TTL");
        //    Assert.AreEqual(newmf, newwinsr.MappingFlag, "Unable to modify MappingFlag");
        //    Assert.AreEqual(newlookuptm, newwinsr.LookupTimeout, "Unable to modify LookupTimeout");
        //    Assert.AreEqual(newcacheto, newwinsr.CacheTimeout, "Unable to modify CacheTimeout");
        //    Assert.AreEqual(newwinsservers, newwinsr.WinsServers, "Unable to modify WinsServers");

        //    target = newwinsr.Modify(oldttl, oldmf, oldlookuptm, oldcacheto, oldwinsservers);

        //    Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
        //    Assert.AreEqual(oldmf, target.MappingFlag, "Unable to revert MappingFlag");
        //    Assert.AreEqual(oldlookuptm, target.LookupTimeout, "Unable to revert LookupTimeout");
        //    Assert.AreEqual(oldcacheto, target.CacheTimeout, "Unable to revert CacheTimeout");
        //    Assert.AreEqual(oldwinsservers, target.WinsServers, "Unable to revert WinsServers");

        //}

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetWINSRR(); 
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for CacheTimeout
        ///</summary>
        [TestMethod()]
        public void CacheTimeoutTest()
        {
            
            var target = GetWINSRR(); 
            var actual = target.CacheTimeout;
            
        }

        /// <summary>
        ///A test for LookupTimeout
        ///</summary>
        [TestMethod()]
        public void LookupTimeoutTest()
        {
            
            var target = GetWINSRR(); 
            var actual = target.LookupTimeout;
        }

        /// <summary>
        ///A test for MappingFlag
        ///</summary>
        [TestMethod()]
        public void MappingFlagTest()
        {
            
            var target = GetWINSRR(); 
            var actual = target.MappingFlag;
            
        }

        /// <summary>
        ///A test for WinsServers
        ///</summary>
        [TestMethod()]
        public void WinsServersTest()
        {
            
            var target = GetWINSRR(); 
            string actual;
            actual = target.WinsServers;
            var splitlist = actual.Split(',');
            foreach (var item in splitlist)
            {
                var ip = DNSManagement.Extensions.IPHelper.ParseIP(item);
                Assert.IsTrue((ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork || ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6), "Server is not an ip");
            }
        }
    }
}
