using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for AFSDBTypeTest and is intended
    ///to contain all AFSDBTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AFSDBTypeTest
    {
        public static AFSDBType GetAFSDBRR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "afsdb." + zone.Name);
            if (record == null)
                record = AFSDBType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "afsdb." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, AFSDBType.SubtypeEnum.AuthNS, "hello");
            return (AFSDBType)record.UnderlyingRecord;
        }

        /// <summary>
        ///A test for AFSDBType Constructor
        ///</summary>
        [TestMethod()]
        
        public void AFSDBTypeConstructorTest()
        {
            
            var target = GetAFSDBRR();
            Assert.IsNotNull(target, "Unable to get AFSDB record");
        }

        /// <summary>
        ///A test for CreateInstanceFromPropertyData
        ///</summary>
        [TestMethod()]
        public void CreateInstanceFromPropertyDataTest()
        {
            var target = GetAFSDBRR();
            target.Delete();
            
        }

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            
            var target = GetAFSDBRR();

            var oldservername = target.ServerName;
            var oldsubtype = target.Subtype;
            var oldttl = target.TTL;

            var newservername = "new" +  oldservername ;
            var newsubtype = DNSManagement.RR.AFSDBType.SubtypeEnum.Ver3;
            var newttl = oldttl + TimeSpan.FromSeconds(5);

            var newafsdb = target.Modify(newttl, newsubtype, newservername);

            Assert.AreEqual(newservername, newafsdb.ServerName, "Unable to modify ServerName");
            Assert.AreEqual(newsubtype, newafsdb.Subtype, "Unable to modify Subtype");
            Assert.AreEqual(newttl, newafsdb.TTL, "Unable to modify TTL");

            target = newafsdb.Modify(oldttl, oldsubtype, oldservername);

            Assert.AreEqual(oldservername, target.ServerName, "Unable to revert ServerName");
            Assert.AreEqual(oldsubtype, target.Subtype, "Unable to revert Subtype");
            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");

        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetAFSDBRR();
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for ServerName
        ///</summary>
        [TestMethod()]
        public void ServerNameTest()
        {
            
            var target = GetAFSDBRR();
            var actual = target.ServerName;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ServerName not returning anything");
        }

        /// <summary>
        ///A test for Subtype
        ///</summary>
        [TestMethod()]
        public void SubtypeTest()
        {
            
            var target = GetAFSDBRR();
            var actual = target.Subtype;
        }
    }
}
