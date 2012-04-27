using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Collections.Generic;
using System.Text;
using DNSManagement.Extensions;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for ResourceRecordTest and is intended
    ///to contain all ResourceRecordTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ResourceRecordTest
    {

        public static ResourceRecord GetRR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "aa." + zone.Name);
            if (record == null)
                record = AType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "aa." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, "192.168.0.1");
            return (AType)record.UnderlyingRecord;
        }

        /// <summary>
        ///A test for ResourceRecord Constructor
        ///</summary>
        [TestMethod()]
        
        public void ResourceRecordConstructorTest()
        {
            
            var target = GetRR();
            Assert.IsNotNull(target);
            //Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for CreateInstanceFromTextRepresentation
        ///</summary>
        [TestMethod()]
        public void CreateInstanceFromTextRepresentationTest()
        {
            var target = GetRR();
            target.Delete();
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();
            var actual = ResourceRecord.CreateInstanceFromTextRepresentation(server, server.Name, zone.Name, "aa A 192.168.0.1");
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        [TestMethod()]
        public void DeleteTest()
        {
            
            var target = GetRR(); 
            target.Delete();
            //no exception is good
        }

        ///// <summary>
        /////A test for Dump
        /////</summary>
        //[TestMethod()]
        
        //public void DumpTest()
        //{
        //    
        //    var target = GetRR(); 
        //    List<KeyValuePair<string, object>> expected = null; 
        //    List<KeyValuePair<string, object>> actual;
        //    actual = target.Dump();
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        /// <summary>
        ///A test for GetObjectByTextRepresentation
        ///</summary>
        [TestMethod()]
        public void GetObjectByTextRepresentationTest()
        {
            var aa = GetRR();
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();
            var actual = ResourceRecord.GetObjectByTextRepresentation(server, server.Name, zone.Name, "aa A 192.168.0.1");
            Assert.IsNotNull(actual);
        }

        

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest1()
        {
            
            var target = GetRR(); 
            string expected = string.Empty; 
            string actual;
            actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for ContainerName
        ///</summary>
        [TestMethod()]
        public void ContainerNameTest()
        {
            
            var target = GetRR(); 
            string actual;
            actual = target.ContainerName;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ContainerName not returning anything");
        }

        /// <summary>
        ///A test for DnsServerName
        ///</summary>
        [TestMethod()]
        public void DnsServerNameTest()
        {
            
            var target = GetRR(); 
            string actual;
            actual = target.DnsServerName;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "DnsServerName not returning anything");
        }

        /// <summary>
        ///A test for DomainName
        ///</summary>
        [TestMethod()]
        public void DomainNameTest()
        {
            
            var target = GetRR(); 
            string actual;
            actual = target.DomainName;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "DomainName not returning anything");
        }

        /// <summary>
        ///A test for OwnerName
        ///</summary>
        [TestMethod()]
        public void OwnerNameTest()
        {
            
            var target = GetRR(); 
            string actual;
            actual = target.OwnerName;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "OwnerName not returning anything");
        }

        /// <summary>
        ///A test for RecordClass
        ///</summary>
        [TestMethod()]
        public void RecordClassTest()
        {
            
            var target = GetRR(); 
            var actual = target.RecordClass;
            //no exception is good
        }

        /// <summary>
        ///A test for RecordType
        ///</summary>
        [TestMethod()]
        public void RecordTypeTest()
        {
            
            var target = GetRR(); 
            var actual = target.RecordType;
            //no exception is good
        }

        /// <summary>
        ///A test for RecordTypeText
        ///</summary>
        [TestMethod()]
        public void RecordTypeTextTest()
        {
            
            var target = GetRR(); 
            string actual;
            actual = target.RecordTypeText;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "RecordTypeText not returning anything");
        }

        /// <summary>
        ///A test for ResourceRecordType
        ///</summary>
        [TestMethod()]
        public void ResourceRecordTypeTest()
        {
            
            var target = GetRR(); 
            var actual = target.ResourceRecordType;
            //no exception is good
        }

        /// <summary>
        ///A test for TTL
        ///</summary>
        [TestMethod()]
        public void TTLTest()
        {
            
            var target = GetRR(); 
            var actual = target.TTL;
            //no exception is good
        }

        /// <summary>
        ///A test for TextRepresentation
        ///</summary>
        [TestMethod()]
        public void TextRepresentationTest()
        {
            
            var target = GetRR(); 
            string actual;
            actual = target.TextRepresentation;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "TextRepresentation not returning anything");
        }

        /// <summary>
        ///A test for TimeStamp
        ///</summary>
        [TestMethod()]
        public void TimeStampTest()
        {
            
            var target = GetRR(); 
            var actual = target.TimeStamp;
            //no exception is good
        }

        /// <summary>
        ///A test for UnderlyingRecord
        ///</summary>
        [TestMethod()]
        public void UnderlyingRecordTest()
        {
            
            var target = GetRR(); 
            var actual = target.UnderlyingRecord;
            //no exception is good
        }
    }
}
