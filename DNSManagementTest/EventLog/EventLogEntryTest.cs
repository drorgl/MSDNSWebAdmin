using DNSManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for EventLogEntryTest and is intended
    ///to contain all EventLogEntryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EventLogEntryTest
    {



        

        /// <summary>
        ///A test for Category
        ///</summary>
        [TestMethod()]
        public void CategoryTest()
        {
            var target = EventLoggingTest.GetFirst();
            Assert.IsNotNull(target, "Check GetFirst");
            var actual = target.Category;
            //if no exception, its ok.
        }

        /// <summary>
        ///A test for CategoryString
        ///</summary>
        [TestMethod()]
        public void CategoryStringTest()
        {
            var target = EventLoggingTest.GetFirst();
            Assert.IsNotNull(target, "Check GetFirst");
            var actual = target.CategoryString;
            //if no exception, its ok.
        }

        /// <summary>
        ///A test for ComputerName
        ///</summary>
        [TestMethod()]
        public void ComputerNameTest()
        {
            var target = EventLoggingTest.GetFirst();
            Assert.IsNotNull(target, "Check GetFirst");
            var actual = target.ComputerName;
            //if no exception, its ok.
        }

        /// <summary>
        ///A test for Data
        ///</summary>
        [TestMethod()]
        public void DataTest()
        {
            var target = EventLoggingTest.GetFirst();
            Assert.IsNotNull(target, "Check GetFirst");
            var actual = target.Data;
            //if no exception, its ok.
        }

        /// <summary>
        ///A test for EventCode
        ///</summary>
        [TestMethod()]
        public void EventCodeTest()
        {
            var target = EventLoggingTest.GetFirst();
            Assert.IsNotNull(target, "Check GetFirst");
            var actual = target.EventCode;
            //if no exception, its ok.
        }

        /// <summary>
        ///A test for EventIdentifier
        ///</summary>
        [TestMethod()]
        public void EventIdentifierTest()
        {
            var target = EventLoggingTest.GetFirst();
            Assert.IsNotNull(target, "Check GetFirst");
            var actual = target.EventIdentifier;
            //if no exception, its ok.
        }

        /// <summary>
        ///A test for EventType
        ///</summary>
        [TestMethod()]
        public void EventTypeTest()
        {
            var target = EventLoggingTest.GetFirst();
            Assert.IsNotNull(target, "Check GetFirst");
            var actual = target.EventType;
            //if no exception, its ok.
        }

        /// <summary>
        ///A test for InsertionStrings
        ///</summary>
        [TestMethod()]
        public void InsertionStringsTest()
        {
            var target = EventLoggingTest.GetFirst();
            Assert.IsNotNull(target, "Check GetFirst");
            var actual = target.InsertionStrings;
            //if no exception, its ok.
        }

        /// <summary>
        ///A test for Logfile
        ///</summary>
        [TestMethod()]
        public void LogfileTest()
        {
            var target = EventLoggingTest.GetFirst();
            Assert.IsNotNull(target, "Check GetFirst");
            var actual = target.Logfile;
            //if no exception, its ok.
        }

        /// <summary>
        ///A test for Message
        ///</summary>
        [TestMethod()]
        public void MessageTest()
        {
            var target = EventLoggingTest.GetFirst();
            Assert.IsNotNull(target, "Check GetFirst");
            var actual = target.Message;
            //if no exception, its ok.
        }

        /// <summary>
        ///A test for RecordNumber
        ///</summary>
        [TestMethod()]
        public void RecordNumberTest()
        {
            var target = EventLoggingTest.GetFirst();
            Assert.IsNotNull(target, "Check GetFirst");
            var actual = target.RecordNumber;
            //if no exception, its ok.
        }

        /// <summary>
        ///A test for SourceName
        ///</summary>
        [TestMethod()]
        public void SourceNameTest()
        {
            var target = EventLoggingTest.GetFirst();
            Assert.IsNotNull(target, "Check GetFirst");
            var actual = target.SourceName;
            //if no exception, its ok.
        }

        /// <summary>
        ///A test for TimeGenerated
        ///</summary>
        [TestMethod()]
        public void TimeGeneratedTest()
        {
            var target = EventLoggingTest.GetFirst();
            Assert.IsNotNull(target, "Check GetFirst");
            var actual = target.TimeGenerated;
            //if no exception, its ok.
        }

        /// <summary>
        ///A test for TimeWritten
        ///</summary>
        [TestMethod()]
        public void TimeWrittenTest()
        {
            var target = EventLoggingTest.GetFirst();
            Assert.IsNotNull(target, "Check GetFirst");
            var actual = target.TimeWritten;
            //if no exception, its ok.
        }

        /// <summary>
        ///A test for Type
        ///</summary>
        [TestMethod()]
        public void TypeTest()
        {
            var target = EventLoggingTest.GetFirst();
            Assert.IsNotNull(target, "Check GetFirst");
            var actual = target.Type;
            //if no exception, its ok.
        }

        /// <summary>
        ///A test for User
        ///</summary>
        [TestMethod()]
        public void UserTest()
        {
            var target = EventLoggingTest.GetFirst();
            Assert.IsNotNull(target, "Check GetFirst");
            var actual = target.User;
            //if no exception, its ok.
        }
    }
}
