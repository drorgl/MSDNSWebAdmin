using DNSManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for StatisticTest and is intended
    ///to contain all StatisticTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StatisticTest
    {

        public static Statistic[] GetStatistic()
        {
            var server = ServerTest.GetServer();
            return server.GetStatistics();
        }

        /// <summary>
        ///A test for Statistic Constructor
        ///</summary>
        [TestMethod()]
        
        public void StatisticConstructorTest()
        {
            
            var target = GetStatistic();
            Assert.IsTrue((target.Length > 0), "No statistics?");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetStatistic().FirstOrDefault(); 
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for CollectionId
        ///</summary>
        [TestMethod()]
        public void CollectionIdTest()
        {

            var target = GetStatistic().FirstOrDefault(); 
            var actual = target.CollectionId;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for CollectionName
        ///</summary>
        [TestMethod()]
        public void CollectionNameTest()
        {

            var target = GetStatistic().FirstOrDefault();  
            var actual = target.CollectionName;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "CollectionName not returning anything");
        }

        /// <summary>
        ///A test for DnsServerName
        ///</summary>
        [TestMethod()]
        public void DnsServerNameTest()
        {

            var target = GetStatistic().FirstOrDefault(); 
            var actual = target.DnsServerName;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "DnsServerName not returning anything");

        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {

            var target = GetStatistic().FirstOrDefault(); 
            var actual = target.Name;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "Name not returning anything");
        }

        /// <summary>
        ///A test for StringValue
        ///</summary>
        [TestMethod()]
        public void StringValueTest()
        {

            var target = GetStatistic().FirstOrDefault(); 
            var actual = target.StringValue;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "StringValue not returning anything");
        }

        /// <summary>
        ///A test for Value
        ///</summary>
        [TestMethod()]
        public void ValueTest()
        {

            var target = GetStatistic().FirstOrDefault(); 
            var actual = target.Value;
            //simple test, no meaning.
        }
    }
}
