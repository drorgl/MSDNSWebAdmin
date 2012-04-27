using DNSManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for EventLoggingTest and is intended
    ///to contain all EventLoggingTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EventLoggingTest
    {
        /// <summary>
        ///A test for EventLogging Constructor
        ///</summary>
        [TestMethod()]
        public void EventLoggingCountSystemLog()
        {
            string host = Settings.Hostname;
            string username = Settings.Username;
            string password = Settings.Password;
            EventLogging target = new EventLogging(host, username, password);
            var system = target.Count(Settings.EventLogName);
            Assert.IsTrue(system > 0, "No Events in System log, check why");
        }

        /// <summary>
        ///A test for Get
        ///</summary>
        [TestMethod()]
        public void GetTest()
        {
            string host = Settings.Hostname;
            string username = Settings.Username;
            string password = Settings.Password;
            EventLogging target = new EventLogging(host, username, password); 
            string logName = Settings.EventLogName;
            int startIndex = 0; 
            int length = 5;
            
            var actual = target.Get(logName, startIndex, length);

            Assert.AreEqual(length, actual.Count, "Asked for 5 log events, got other");
        }

        /// <summary>
        ///A test for GetAll
        ///</summary>
        [TestMethod()]
        public void GetAllTest()
        {
            string host = Settings.Hostname;
            string username = Settings.Username;
            string password = Settings.Password;
            EventLogging target = new EventLogging(host, username, password); 
            string logName = Settings.EventLogName;

            var count = target.Count(logName);

            var actual = target.GetAll(logName);
            Assert.AreEqual(count, actual.Count, "Asked for all log events, got different than count");
        }

        private static EventLogEntry _entry = null;

        public static EventLogEntry GetFirst()
        {
            if (_entry == null)
            {
                string host = Settings.Hostname;
                string username = Settings.Username;
                string password = Settings.Password;
                EventLogging target = new EventLogging(host, username, password);
                string logName = Settings.EventLogName;

                _entry = target.GetAll(logName).FirstOrDefault();
            }
            return _entry;
        }
    }
}
