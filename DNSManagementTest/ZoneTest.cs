using DNSManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using System.Collections.Generic;
using DNSManagement.Extensions;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for ZoneTest and is intended
    ///to contain all ZoneTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ZoneTest
    {

        public static Zone GetZone()
        {
            var server = ServerTest.GetServer();
            var zone = server.GetZones().FirstOrDefault(i => i.Name == "TestZone");
            if (zone == null)
            {
                zone = Zone.CreateZone(server, "TestZone", Zone.ZoneTypeCreate.Primary, false, "", new string[] {"192.168.0.1"}, "test@test.com");
            }
            return zone;
        }

        public static Zone ZoneGetReverseZone()
        {
            var server = ServerTest.GetServer();
            var zone = server.GetZones().FirstOrDefault(i => i.Name == "34.33.167.in-addr.arpa");
            if (zone == null)
            {
                zone = Zone.CreateZone(server, "34.33.167.in-addr.arpa", Zone.ZoneTypeCreate.Primary, false, "", new string[] {"192.168.0.1"}, "test@test.com");

            }
            return zone;
        }

        ///// <summary>
        /////A test for GetDistinguishedName
        /////</summary>
        //[TestMethod()]
        //not sure what its supposed to test...
        //public void GetDistinguishedNameTest()
        //{

        //    var target = GetZone();
        //    var actual = target.GetDistinguishedName();
        //    Assert.IsFalse(string.IsNullOrEmpty(actual), "GetDistinguishedName not returning anything");
        //}

        /// <summary>
        ///A test for Zone Constructor
        ///</summary>
        [TestMethod()]
        
        public void ZoneConstructorTest()
        {
            Zone target = GetZone();
            Assert.IsNotNull(target, "Zone TestZone not found, it was either not automatically created or not found");
        }

       

        /// <summary>
        ///A test for AgeAllRecords
        ///</summary>
        [TestMethod()]
        public void AgeAllRecordsTest()
        {
            //TODO: have an AD server so I can test this method
            Zone target = GetZone();
            if (target.DsIntegrated == false)
                Assert.Inconclusive("Can't test because zone is not DsIntegrated");
            var actual = target.AgeAllRecords("", true);
            Assert.AreEqual(0, actual);
        }

        ///// <summary>
        /////A test for ChangeZoneType
        /////</summary>
        //[TestMethod()]
        //public void ChangeZoneTypeTest()
        //{
        //    //TODO: zone change is unstable, sometimes its not reverting, so I've disabled it.
        //    Zone target = GetZone();
        //    var filename = target.DataFile;
        //    Assert.AreEqual(DNSManagement.Zone.ZoneTypeEnum.Primary, target.ZoneType, "Starting zonetype is not Primary");
        //    var newzone = target.ChangeZoneType(Zone.ZoneTypeCreate.Secondary, null, new string[] {"192.168.0.1"}, null);
        //    Assert.AreEqual(DNSManagement.Zone.ZoneTypeEnum.Secondary, newzone.ZoneType, "Failed to change zonetype to secondary");


        //    newzone = newzone.ChangeZoneType(Zone.ZoneTypeCreate.Primary, filename, null, null);
        //    Assert.AreEqual(DNSManagement.Zone.ZoneTypeEnum.Primary, newzone.ZoneType, "Failed to change back zonetype to primary");
        //}

        /// <summary>
        ///A test for CreateZone
        ///</summary>
        [TestMethod()]
        public void CreateDeleteZoneTest()
        {
            var server = ServerTest.GetServer();

            var zonename = Guid.NewGuid().ToString();

            var zone = Zone.CreateZone(server, zonename, Zone.ZoneTypeCreate.Primary, false, string.Empty, new string[] { "192.168.0.1" }, "test@test.com");
            Assert.IsNotNull(zone, "Unable to create zone");
            //cleanup
            zone.Delete();
            
        }



        /// <summary>
        ///A test for ForceRefresh
        ///</summary>
        [TestMethod()]
        public void ForceRefreshTest()
        {
            //need to test on secondary zones, otherwise it will fail
            var server = ServerTest.GetServer();
            var secondaryzone = server.GetZones().FirstOrDefault(i => i.ZoneType == Zone.ZoneTypeEnum.Secondary);
            Assert.IsNotNull(secondaryzone, "Need a secondary zone to run this test, please create one and rerun");
            secondaryzone.ForceRefresh();
            
        }

        /// <summary>
        ///A test for PauseZone
        ///</summary>
        [TestMethod()]
        public void PauseResumeZoneTest()
        {
            Zone target = GetZone(); 
            target.PauseZone();
            target = GetZone();

            Assert.AreEqual(true, target.Paused, "Couldn't pause zone");

            target.ResumeZone();
            target = GetZone();

            Assert.AreEqual(false, target.Paused, "Couldn't resume zone");
        }

        /// <summary>
        ///A test for ReloadZone
        ///</summary>
        [TestMethod()]
        public void ReloadZoneTest()
        {
            Zone target = GetZone(); 
            target.ReloadZone();
        }

        /// <summary>
        ///A test for ResetSecondaries
        ///</summary>
        [TestMethod()]
        public void ResetSecondariesTest()
        {
            Zone target = GetZone();
            var actual = target.ResetSecondaries(target.SecondaryServers, Zone.SecondarySecurityEnum.NoSecurity, target.NotifyServers, Zone.NotifyLevelEnum.AllSecondaries);
            Assert.AreEqual(actual.SecureSecondaries, DNSManagement.Zone.SecureSecondariesEnum.All);
            Assert.AreEqual(actual.Notify, true);
            
            actual = target.ResetSecondaries(target.SecondaryServers, Zone.SecondarySecurityEnum.NoXFR, target.NotifyServers, Zone.NotifyLevelEnum.Off);
            Assert.AreEqual(actual.SecureSecondaries, DNSManagement.Zone.SecureSecondariesEnum.DoNotSend);
            Assert.AreEqual(actual.Notify, false);
            //Assert.Inconclusive("Not implemented");
        }

        

        /// <summary>
        ///A test for Save
        ///</summary>
        [TestMethod()]
        public void SaveTest()
        {
            Zone target = GetZone(); 
            var original = target.RefreshInterval;
            var newval = original + TimeSpan.FromHours(10);
            target.RefreshInterval = newval;
            target.Save();
            target = GetZone();
            Assert.AreEqual(newval, target.RefreshInterval, "Unable to save changed to zone");
            target.RefreshInterval = original;
            target.Save();
            target = GetZone();
            Assert.AreEqual(original, target.RefreshInterval, "Unable to revert changes");
            
        }

        /// <summary>
        ///A test for ToConfigurationFile
        ///</summary>
        [TestMethod()]
        public void ToConfigurationFileTest()
        {
            Zone target = GetZone();
            string actual;
            actual = target.ToConfigurationFile();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToConfigurationFile not returning anything");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            Zone target = GetZone(); 
            string actual;
            actual = target.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
        }

        /// <summary>
        ///A test for UpdateFromDS
        ///</summary>
        [TestMethod()]
        public void UpdateFromDSTest()
        {
            var server = ServerTest.GetServer();
            var target = server.GetZones().FirstOrDefault(i => i.DsIntegrated == true);
            if (target == null)
                Assert.Inconclusive("No DsIntegrated zones were found for this test");
            target.UpdateFromDS();
        }

        /// <summary>
        ///A test for WriteBackZone
        ///</summary>
        [TestMethod()]
        public void WriteBackZoneTest()
        {
            Zone target = GetZone(); 
            target.WriteBackZone();
        }


        /// <summary>
        ///A test for Aging
        ///</summary>
        [TestMethod()]
        public void AgingTest()
        {
            Zone target = GetZone();
            var val = target.Aging;
            target.Aging = val;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for AllowUpdate
        ///</summary>
        [TestMethod()]
        public void AllowUpdateTest()
        {
            Zone target = GetZone();
            var val = target.AllowUpdate;
            target.AllowUpdate = val;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for AutoCreated
        ///</summary>
        [TestMethod()]
        public void AutoCreatedTest()
        {
            Zone target = GetZone(); 
            var actual = target.AutoCreated;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for AvailForScavengeTime
        ///</summary>
        [TestMethod()]
        public void AvailForScavengeTimeTest()
        {
            Zone target = GetZone(); 
            var actual = target.AvailForScavengeTime;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for DataFile
        ///</summary>
        [TestMethod()]
        public void DataFileTest()
        {
            Zone target = GetZone();
            var val = target.DataFile;
            target.DataFile = val;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for DisableWINSRecordReplication
        ///</summary>
        [TestMethod()]
        public void DisableWINSRecordReplicationTest()
        {
            Zone target = GetZone(); 
            var actual = target.DisableWINSRecordReplication;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for DsIntegrated
        ///</summary>
        [TestMethod()]
        public void DsIntegratedTest()
        {
            Zone target = GetZone(); 
            var actual = target.DsIntegrated;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for ForwarderSlave
        ///</summary>
        [TestMethod()]
        public void ForwarderSlaveTest()
        {
            Zone target = GetZone();
            var val = target.ForwarderSlave;
            target.ForwarderSlave = val;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for ForwarderTimeout
        ///</summary>
        [TestMethod()]
        public void ForwarderTimeoutTest()
        {
            Zone target = GetZone(); 
            var actual = target.ForwarderTimeout;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for LastSuccessfulSoaCheck
        ///</summary>
        [TestMethod()]
        public void LastSuccessfulSoaCheckTest()
        {
            Zone target = GetZone(); 
            var actual = target.LastSuccessfulSoaCheck;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for LastSuccessfulXfr
        ///</summary>
        [TestMethod()]
        public void LastSuccessfulXfrTest()
        {
            Zone target = GetZone(); 
            var actual = target.LastSuccessfulXfr;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for LocalMasterServers
        ///</summary>
        [TestMethod()]
        public void LocalMasterServersTest()
        {
            Zone target = GetZone();
            var val = target.LocalMasterServers;
            target.LocalMasterServers = val;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for MasterServers
        ///</summary>
        [TestMethod()]
        public void MasterServersTest()
        {
            Zone target = GetZone();
            var val = target.MasterServers;
            target.MasterServers = val;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for NoRefreshInterval
        ///</summary>
        [TestMethod()]
        public void NoRefreshIntervalTest()
        {
            Zone target = GetZone();
            var val = target.NoRefreshInterval;
            target.NoRefreshInterval = val;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for Notify
        ///</summary>
        [TestMethod()]
        public void NotifyTest()
        {
            Zone target = GetZone(); 
            var actual = target.Notify;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for NotifyServers
        ///</summary>
        [TestMethod()]
        public void NotifyServersTest()
        {
            Zone target = GetZone(); 
            var actual = target.NotifyServers;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for Paused
        ///</summary>
        [TestMethod()]
        public void PausedTest()
        {
            Zone target = GetZone(); 
            var actual = target.Paused;
            //tested on pause/resume
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for RefreshInterval
        ///</summary>
        [TestMethod()]
        public void RefreshIntervalTest()
        {
            Zone target = GetZone();
            var val = target.RefreshInterval;
            target.RefreshInterval = val;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for Reverse
        ///</summary>
        [TestMethod()]
        public void ReverseTest()
        {
            Zone target = GetZone(); 
            var actual = target.Reverse;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for ScavengeServers
        ///</summary>
        [TestMethod()]
        public void ScavengeServersTest()
        {
            Zone target = GetZone();
            var val = target.ScavengeServers;
            target.ScavengeServers = val;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for SecondaryServers
        ///</summary>
        [TestMethod()]
        public void SecondaryServersTest()
        {
            Zone target = GetZone();
            var val = target.SecondaryServers;
            target.SecondaryServers = val;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for SecureSecondaries
        ///</summary>
        [TestMethod()]
        public void SecureSecondariesTest()
        {
            Zone target = GetZone(); 
            var actual = target.SecureSecondaries;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for Shutdown
        ///</summary>
        [TestMethod()]
        public void ShutdownTest()
        {
            Zone target = GetZone(); 
            var actual = target.Shutdown;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for UseNBStat
        ///</summary>
        [TestMethod()]
        public void UseNBStatTest()
        {
            Zone target = GetZone(); 
            var actual = target.UseNBStat;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for UseWins
        ///</summary>
        [TestMethod()]
        public void UseWinsTest()
        {
            Zone target = GetZone(); 
            var actual = target.UseWins;
            //simple test, no meaning.
        }

        /// <summary>
        ///A test for ZoneType
        ///</summary>
        [TestMethod()]
        public void ZoneTypeTest()
        {
            Zone target = GetZone();
            var val = target.ZoneType;
            target.ZoneType = val;
            //simple test, no meaning.
        }
    }
}
