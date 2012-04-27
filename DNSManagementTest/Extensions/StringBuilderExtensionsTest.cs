using DNSManagement.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for StringBuilderExtensionsTest and is intended
    ///to contain all StringBuilderExtensionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StringBuilderExtensionsTest
    {



        /// <summary>
        ///A test for AppendLineFormat
        ///</summary>
        [TestMethod()]
        public void AppendLineFormatTest()
        {
            StringBuilder sb = new StringBuilder("\r\n");
            string value = "{0} World";
            object[] parameters = new object[] {"Hello" };
            string expected = "\r\nHello World\r\n";
            StringBuilder actual;
            actual = StringBuilderExtensions.AppendLineFormat(sb, value, parameters);
            Assert.AreEqual(expected, actual.ToString());
        }
    }
}
