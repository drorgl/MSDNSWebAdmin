using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;
using System.Threading;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for SRVTypeTest and is intended
    ///to contain all SRVTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SRVTypeTest
    {

        public static SRVType GetSRVRR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "srv." + zone.Name);
            if (record == null)
                record = SRVType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "srv." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, 1,2,15,"domainname1");
            return (SRVType)record.UnderlyingRecord;
        }

        /// <summary>
        ///A test for SRVType Constructor
        ///</summary>
        [TestMethod()]
        
        public void SRVTypeConstructorTest()
        {
            
            var target = GetSRVRR();
            Assert.IsNotNull(target);
        }

        ///// <summary>
        /////A test for CreateInstanceFromPropertyData
        /////</summary>
        //[TestMethod()]
        //public void CreateInstanceFromPropertyDataTest()
        //{
        //    var target = GetSRVRR();
        //    target.Delete();
        //    Thread.Sleep(1000);
        //    target = GetSRVRR();
        //}

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            var target = GetSRVRR();


            var oldttl = target.TTL;
            var oldport = target.Port;
            var oldpriority = target.Priority;
            var oldweight = target.Weight;
            var oldsrvdomname = target.SRVDomainName;


            var newttl = oldttl + TimeSpan.FromSeconds(10);
            var newport = oldport + 1;
            var newpriority = oldpriority + 1;
            var newweight = oldweight + 1;
            var newsrvdomname = "hello" + oldsrvdomname;


            var newsrv = target.Modify(newttl, (ushort)newpriority, (ushort)newweight, (ushort)newport, newsrvdomname);
            Assert.AreEqual(newttl, newsrv.TTL, "Unable to modify TTL");
            Assert.AreEqual(newport, newsrv.Port, "Unable to modify Port");
            Assert.AreEqual(newpriority, newsrv.Priority, "Unable to modify Priority");
            Assert.AreEqual(newweight, newsrv.Weight, "Unable to modify Weight");
            Assert.AreEqual(newsrvdomname, newsrv.SRVDomainName, "Unable to modify SRVDomainName");

            target = newsrv.Modify(newttl, (ushort)oldpriority, (ushort)oldweight, (ushort)oldport, oldsrvdomname);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(oldport, target.Port, "Unable to revert Port");
            Assert.AreEqual(oldpriority, target.Priority, "Unable to revert Priority");
            Assert.AreEqual(oldweight, target.Weight, "Unable to revert Weight");
            Assert.AreEqual(oldsrvdomname, target.SRVDomainName, "Unable to revert SRVDomainName");

        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetSRVRR(); 
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for Port
        ///</summary>
        [TestMethod()]
        public void PortTest()
        {
            
            var target = GetSRVRR(); 
            var actual = target.Port;
        }

        /// <summary>
        ///A test for Priority
        ///</summary>
        [TestMethod()]
        public void PriorityTest()
        {
            
            var target = GetSRVRR(); 
            var actual = target.Priority;
        }

        /// <summary>
        ///A test for SRVDomainName
        ///</summary>
        [TestMethod()]
        public void SRVDomainNameTest()
        {
            
            var target = GetSRVRR(); 
            var actual = target.SRVDomainName;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "SRVDomainName not returning anything");
        }

        /// <summary>
        ///A test for Weight
        ///</summary>
        [TestMethod()]
        public void WeightTest()
        {
            
            var target = GetSRVRR(); 
            var actual = target.Weight;
        }
    }
}
