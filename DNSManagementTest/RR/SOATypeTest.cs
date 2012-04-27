using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using System.Linq;
using System.Threading;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for SOATypeTest and is intended
    ///to contain all SOATypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SOATypeTest
    {


        public static SOAType GetSOARR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.ResourceRecordType == ResourceRecord.ResourceRecordEnum.SOA);
            return (SOAType)record.UnderlyingRecord;
        }

        /// <summary>
        ///A test for SOAType Constructor
        ///</summary>
        [TestMethod()]
        
        public void SOATypeConstructorTest()
        {
            
            var target = GetSOARR();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            //unpredictable method
            var target = GetSOARR();

            var oldttl = target.TTL;
            var oldserialnumber = target.SerialNumber;
            var oldprimaryserver = target.PrimaryServer;
            var oldresponsibleParty = target.ResponsibleParty;
            var oldrefreshInterval = target.RefreshInterval;
            var oldretrydelay = target.RetryDelay;
            var oldexpirelimit = target.ExpireLimit;
            var oldminimumttl = target.MinimumTTL;

            var newttl = oldttl+TimeSpan.FromSeconds(5); 
            var newserialnumber = oldserialnumber+1; 
            var newprimaryserver = "test" + oldprimaryserver;
            var newresponsibleParty = "test" + oldresponsibleParty;
            var newrefreshInterval = oldrefreshInterval + TimeSpan.FromSeconds(5);
            var newretryDelay = oldretrydelay + TimeSpan.FromSeconds(5);
            var newexpirelimit = oldexpirelimit + TimeSpan.FromSeconds(5);
            var newminimumttl = oldminimumttl + TimeSpan.FromSeconds(5);

             target.Modify(newttl, newserialnumber, newprimaryserver, newresponsibleParty, newrefreshInterval, newretryDelay, newexpirelimit, newminimumttl);
             
             var newsoa = GetSOARR();

            Assert.AreEqual(newttl, target.TTL, "Unable to change TTL");
            Assert.AreEqual(newserialnumber, target.SerialNumber, "Unable to change SerialNumber");
            Assert.AreEqual(newprimaryserver, target.PrimaryServer, "Unable to change PrimaryServer");
            Assert.AreEqual(newresponsibleParty, target.ResponsibleParty, "Unable to change ResponsibleParty");
            Assert.AreEqual(newrefreshInterval, target.RefreshInterval, "Unable to change RefreshInterval");
            Assert.AreEqual(newretryDelay, target.RetryDelay, "Unable to change RetryDelay");
            Assert.AreEqual(newexpirelimit, target.ExpireLimit, "Unable to change ExpireLimit");
            Assert.AreEqual(newminimumttl, target.MinimumTTL, "Unable to change MinimumTTL");

            newsoa.Modify(oldttl, oldserialnumber, oldprimaryserver, oldresponsibleParty, oldrefreshInterval, oldretrydelay, oldexpirelimit, oldminimumttl);
            target = GetSOARR();

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(oldserialnumber, target.SerialNumber, "Unable to revert SerialNumber");
            Assert.AreEqual(oldprimaryserver, target.PrimaryServer, "Unable to revert PrimaryServer");
            Assert.AreEqual(oldresponsibleParty, target.ResponsibleParty, "Unable to revert ResponsibleParty");
            Assert.AreEqual(oldrefreshInterval, target.RefreshInterval, "Unable to revert RefreshInterval");
            Assert.AreEqual(oldretrydelay, target.RetryDelay, "Unable to revert RetryDelay");
            Assert.AreEqual(oldexpirelimit, target.ExpireLimit, "Unable to revert ExpireLimit");
            Assert.AreEqual(oldminimumttl, target.MinimumTTL, "Unable to revert MinimumTTL");

        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetSOARR(); 
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for ExpireLimit
        ///</summary>
        [TestMethod()]
        public void ExpireLimitTest()
        {
            
            var target = GetSOARR(); 
            var actual = target.ExpireLimit;
        }

        /// <summary>
        ///A test for MinimumTTL
        ///</summary>
        [TestMethod()]
        public void MinimumTTLTest()
        {
            
            var target = GetSOARR(); 
            var actual = target.MinimumTTL;
        }

        /// <summary>
        ///A test for PrimaryServer
        ///</summary>
        [TestMethod()]
        public void PrimaryServerTest()
        {
            
            var target = GetSOARR(); 
            var actual = target.PrimaryServer;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "PrimaryServer not returning anything");
        }

        /// <summary>
        ///A test for RefreshInterval
        ///</summary>
        [TestMethod()]
        public void RefreshIntervalTest()
        {
            
            var target = GetSOARR(); 
            var actual = target.RefreshInterval;
        }

        /// <summary>
        ///A test for ResponsibleParty
        ///</summary>
        [TestMethod()]
        public void ResponsiblePartyTest()
        {
            
            var target = GetSOARR(); 
            var actual = target.ResponsibleParty;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ResponsibleParty not returning anything");
        }

        /// <summary>
        ///A test for RetryDelay
        ///</summary>
        [TestMethod()]
        public void RetryDelayTest()
        {
            
            var target = GetSOARR(); 
            var actual = target.RetryDelay;
        }

        /// <summary>
        ///A test for SerialNumber
        ///</summary>
        [TestMethod()]
        public void SerialNumberTest()
        {
            
            var target = GetSOARR(); 
            var actual = target.SerialNumber;
        }
    }
}
