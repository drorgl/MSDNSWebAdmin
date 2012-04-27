using DNSManagement.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for UnixDateTimeTest and is intended
    ///to contain all UnixDateTimeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UnixDateTimeTest
    {

        /// <summary>
        ///A test for UnixDateTime Constructor
        ///</summary>
        [TestMethod()]
        public void UnixDateTimeConstructorTest()
        {
            DateTime timestamp = new DateTime(2012,10,15,8,20,55, DateTimeKind.Utc); 
            UnixDateTime target = new UnixDateTime(timestamp);
            Assert.AreEqual(target.Unix, 1350289255, "Error converting DateTime to unix datetime");
        }

        /// <summary>
        ///A test for UnixDateTime Constructor
        ///</summary>
        [TestMethod()]
        public void UnixDateTimeConstructorTest1()
        {
            UnixDateTime target = new UnixDateTime(1350289255);
            Assert.AreEqual(target.DateTime.Year, 2012, "Year not ok");
            Assert.AreEqual(target.DateTime.Month, 10, "Month not ok");
            Assert.AreEqual(target.DateTime.Day, 15, "Day not ok");
            Assert.AreEqual(target.DateTime.Hour, 8, "Hour not ok");
            Assert.AreEqual(target.DateTime.Minute, 20, "Minute not ok");
            Assert.AreEqual(target.DateTime.Second, 55, "Seconds not ok");
        }

        /// <summary>
        ///A test for DateTimeToUnixTimestamp
        ///</summary>
        [TestMethod()]
        public void DateTimeToUnixTimestampTest()
        {
            DateTime dateTime = new DateTime(2012, 10, 15, 8, 20, 55, DateTimeKind.Utc);
            double expected = 1350289255;
            double actual;
            actual = UnixDateTime.DateTimeToUnixTimestamp(dateTime);
            Assert.AreEqual(expected, actual,"Error converting datetime to unix time");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            DateTime timestamp = new DateTime(2012, 10, 15, 8, 20, 55);
            UnixDateTime target = new UnixDateTime(timestamp); 
            string expected = timestamp.ToString();
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual, "String conversion from datetime are not the same");
        }

        /// <summary>
        ///A test for UnixTimeStampToDateTime
        ///</summary>
        [TestMethod()]
        public void UnixTimeStampToDateTimeTest()
        {
            double unixTimeStamp = 1350289255;
            DateTime expected = new DateTime(2012, 10, 15, 8, 20, 55);
            DateTime actual;
            actual = UnixDateTime.UnixTimeStampToDateTime(unixTimeStamp);
            Assert.AreEqual(expected, actual, "Error conversion from unix to datetime");
        }

        
    }
}
