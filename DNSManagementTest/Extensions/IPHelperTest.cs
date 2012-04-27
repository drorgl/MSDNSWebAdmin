using DNSManagement.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.Net;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for IPHelperTest and is intended
    ///to contain all IPHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IPHelperTest
    {



        /// <summary>
        ///A test for GetProtoByName
        ///</summary>
        [TestMethod()]
        public void GetProtoByNameTest()
        {
            string name = "tcp";
            int expected = 6;
            int actual;
            actual = IPHelper.GetProtoByName(name);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetServByName
        ///</summary>
        [TestMethod()]
        public void GetServByNameTest()
        {
            string name = "finger";
            string protocol = "tcp";
            int expected = 79;
            int actual;
            actual = IPHelper.GetServByName(name, protocol);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ParseIP
        ///</summary>
        [TestMethod()]
        public void ParseIPTest()
        {
            string ipaddr = "1.2.3.4";
            IPAddress expected = new IPAddress(new byte[]{1,2,3,4});
            IPAddress actual;
            actual = IPHelper.ParseIP(ipaddr);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            uint address = 16909060;
            string expected = "1.2.3.4";
            string actual;
            actual = IPHelper.ToString(address);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToUint32
        ///</summary>
        [TestMethod()]
        public void ToUint32Test()
        {
            string address = "1.2.3.4";
            uint expected = 16909060;
            uint actual;
            actual = IPHelper.ToUint32(address);
            Assert.AreEqual(expected, actual);
        }
    }
}
