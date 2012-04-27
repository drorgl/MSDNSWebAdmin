//using DNSManagement.RR;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Management;
//using DNSManagement;
//using System.Linq;

//namespace DNSManagementTest
//{
    
    
//    /// <summary>
//    ///This is a test class for NXTTypeTest and is intended
//    ///to contain all NXTTypeTest Unit Tests
//    ///</summary>
//    [TestClass()]
//    public class NXTTypeTest
//    {

//        public static NXTType GetNXTRR()
//        {
//            var server = ServerTest.GetServer();
//            var zone = ZoneTest.GetZone();

//            //delete me
//            var records = zone.GetRecords();

//            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "nxt." + zone.Name);
//            if (record == null)
//                record = NXTType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "nxt." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null,"nextdomainname1","MX");
//            return (NXTType)record.UnderlyingRecord;
//        }

//        /// <summary>
//        ///A test for NXTType Constructor
//        ///</summary>
//        [TestMethod()]
        
//        public void NXTTypeConstructorTest()
//        {
            
//            var target = GetNXTRR();
//            Assert.IsNotNull(target);

//        }

//        /// <summary>
//        ///A test for CreateInstanceFromPropertyData
//        ///</summary>
//        [TestMethod()]
//        public void CreateInstanceFromPropertyDataTest()
//        {
//            var target = GetNXTRR();
//            target.Delete();

//        }

//        /// <summary>
//        ///A test for Modify
//        ///</summary>
//        [TestMethod()]
//        public void ModifyTest()
//        {
//            var target = GetNXTRR();

//            var oldttl = target.TTL;
//            var oldnextdomainname = target.NextDomainName;
//            var oldtypes = target.Types;


//            var newttl = oldttl + TimeSpan.FromSeconds(10);
//            var newnextdomainname = "hello" + oldnextdomainname;
//            var newtypes = "hello" + oldtypes;


//            var newnxt = target.Modify(newttl, newnextdomainname, newtypes);
//            Assert.AreEqual(newttl, newnxt.TTL, "Unable to modify TTL");
//            Assert.AreEqual(newnextdomainname, newnxt.NextDomainName, "Unable to modify NextDomainName");
//            Assert.AreEqual(newtypes, newnxt.Types, "Unable to modify Types");

//            target = newnxt.Modify(oldttl, oldnextdomainname, oldtypes);

//            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
//            Assert.AreEqual(oldnextdomainname, target.NextDomainName, "Unable to revert NextDomainName");
//            Assert.AreEqual(oldtypes, target.Types, "Unable to revert Types");

//        }

//        /// <summary>
//        ///A test for ToString
//        ///</summary>
//        [TestMethod()]
//        public void ToStringTest()
//        {
            
//            var target = GetNXTRR();
//            var actual = target.ToString();
//            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
//        }

//        /// <summary>
//        ///A test for NextDomainName
//        ///</summary>
//        [TestMethod()]
//        public void NextDomainNameTest()
//        {
            
//            var target = GetNXTRR();
//            var actual = target.NextDomainName;
//            Assert.IsFalse(string.IsNullOrEmpty(actual), "NextDomainName not returning anything");
//        }

//        /// <summary>
//        ///A test for Types
//        ///</summary>
//        [TestMethod()]
//        public void TypesTest()
//        {
            
//            var target = GetNXTRR();
//            var actual = target.Types;
//            Assert.IsFalse(string.IsNullOrEmpty(actual), "Types not returning anything");
//        }
//    }
//}
