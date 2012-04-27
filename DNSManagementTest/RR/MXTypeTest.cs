using DNSManagement.RR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using DNSManagement;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for MXTypeTest and is intended
    ///to contain all MXTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MXTypeTest
    {

        public static MXType GetMXRR()
        {
            var server = ServerTest.GetServer();
            var zone = ZoneTest.GetZone();

            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "mx." + zone.Name);
            if (record == null)
            {
                record = MXType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "mx." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, 1, "mxrec1");
                //workaround for non-standard implementation
                zone.WriteBackZone();
                record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "mx." + zone.Name);
            }
            return (MXType)record.UnderlyingRecord;

        }
        /// <summary>
        ///A test for MXType Constructor
        ///</summary>
        [TestMethod()]
        public void MXTypeConstructorTest()
        {
            
            var target = GetMXRR();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for CreateInstanceFromPropertyData
        ///</summary>
        [TestMethod()]
        public void CreateInstanceFromPropertyDataTest()
        {
            var target = GetMXRR();
            target.Delete();


        }

        /// <summary>
        ///A test for Modify
        ///</summary>
        [TestMethod()]
        public void ModifyTest()
        {
            var target = GetMXRR();

            var oldttl = target.TTL;
            ushort oldpref = target.Preference;
            var oldmailex = target.MailExchange;


            var newttl = oldttl + TimeSpan.FromSeconds(10);
            var newpref = oldpref + 1;
            var newmailex = "hello" + oldmailex;


            var newmx = target.Modify(newttl, (ushort)newpref, newmailex);
            Assert.AreEqual(newttl, newmx.TTL, "Unable to modify TTL");
            Assert.AreEqual(newpref, newmx.Preference, "Unable to modify Preference");
            Assert.AreEqual(newmailex, newmx.MailExchange, "Unable to modify MailExchange");

            target = newmx.Modify(oldttl, oldpref, oldmailex);

            Assert.AreEqual(oldttl, target.TTL, "Unable to revert TTL");
            Assert.AreEqual(oldpref, target.Preference, "Unable to revert Preference");
            Assert.AreEqual(oldmailex, target.MailExchange, "Unable to revert MailExchange");

        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            
            var target = GetMXRR();
            var actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for MailExchange
        ///</summary>
        [TestMethod()]
        public void MailExchangeTest()
        {
            
            var target = GetMXRR();
            var actual = target.MailExchange;
            Assert.IsFalse(string.IsNullOrEmpty(actual), "MailExchange not returning anything");
        }

        /// <summary>
        ///A test for Preference
        ///</summary>
        [TestMethod()]
        public void PreferenceTest()
        {
            
            var target = GetMXRR();
            var actual = target.Preference;
        }
    }
}
