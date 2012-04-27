using DNSManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DNSManagement.RR;
using System.Collections.Generic;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for ServerTest and is intended
    ///to contain all ServerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ServerTest
    {

        public static Server GetServer()
        {
            string host = Settings.Hostname;
            string username = Settings.Username;
            string password = Settings.Password;
            return new Server(host, username, password);
        }


        /// <summary>
        ///A test for Server Constructor
        ///</summary>
        [TestMethod()]
        public void ServerConstructorTest()
        {
            string host = Settings.Hostname;
            string username = Settings.Username;
            string password = Settings.Password;
            Server target = new Server(host, username, password);
        }

        /// <summary>
        ///A test for Server Constructor
        ///</summary>
        [TestMethod()]
        public void ServerConstructorTest1()
        {
            Server target = new Server();
            //Exception is ok if local machine doesn't have DNS installed.

        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        public void DisposeTest()
        {
            Server target = GetServer();
            target.Dispose();
        }

        /// <summary>
        ///A test for GetCache
        ///</summary>
        [TestMethod()]
        public void GetCacheTest()
        {
            Server target = GetServer();
            var actual = target.GetCache();
            Assert.IsTrue(actual.Length > 0, "No Cache records");
        }

        

        /// <summary>
        ///A test for GetDomains
        ///</summary>
        [TestMethod()]
        public void GetDomainsTest()
        {
            Server target = GetServer();
            Domain[] actual;
            actual = target.GetDomains();
            Assert.IsTrue(actual.Length > 0, "No Domains?");
        }

        ///// <summary>
        /////A test for GetRecords
        /////</summary>
        //[TestMethod()]
        //public void GetRecordsTest()
        //{
        //    Server target = GetServer();
        //    var actual = target.GetRecords();
        //    Assert.IsTrue(actual.Length > 0, "No Records?");
        //}

        /// <summary>
        ///A test for GetRootHints
        ///</summary>
        [TestMethod()]
        public void GetRootHintsTest()
        {
            Server target = GetServer();
            var actual = target.GetRootHints();
            Assert.IsTrue(actual.Length > 0, "No RootHints?");
        }

        /// <summary>
        ///A test for GetStatistics
        ///</summary>
        [TestMethod()]
        public void GetStatisticsTest()
        {
            Server target = GetServer();
            var actual = target.GetStatistics();
            Assert.IsTrue(actual.Length > 0, "No Statistics?");
        }

        /// <summary>
        ///A test for GetZones
        ///</summary>
        [TestMethod()]
        public void GetZonesTest()
        {
            Server target = GetServer();
            var actual = target.GetZones();
            Assert.IsTrue(actual.Length > 0, "No Zones?");
        }

        /// <summary>
        ///A test for Save
        ///</summary>
        [TestMethod()]
        public void SaveTest()
        {
            Server target = GetServer();
            target.Save();
        }

        /// <summary>
        ///A test for StartScavenging
        ///</summary>
        [TestMethod()]
        public void StartScavengingTest()
        {
            Server target = GetServer();
            uint expected = 0; //success
            uint actual;
            actual = target.StartScavenging();
            Assert.AreEqual(expected, actual);
        }

        ///// <summary>
        /////A test for StartService
        /////</summary>
        //[TestMethod()]
        //public void StartServiceTest()
        //{
        //    Server target = GetServer();
        //    uint expected = 0; // success
        //    uint actual;
        //    actual = target.StartService();
        //    Assert.AreEqual(expected, actual);
        //}

        ///// <summary>
        /////A test for StopService
        /////</summary>
        //[TestMethod()]
        //public void StopServiceTest()
        //{
        //    Server target = GetServer();
        //    uint expected = 0; //success
        //    uint actual;
        //    actual = target.StopService();
        //    Assert.AreEqual(expected, actual);
        //}

        [TestMethod()]
        public void StopStartServiceTest()
        {
            Server target = GetServer();
            uint expected = 0; //success
            uint actual;
            actual = target.StopService();
            Assert.AreEqual(expected, actual);
            actual = target.StartService();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToConfigurationFile
        ///</summary>
        [TestMethod()]
        public void ToConfigurationFileTest()
        {
            Server target = GetServer();
            var actual = target.ToConfigurationFile();
            Assert.IsFalse(string.IsNullOrWhiteSpace(actual), "ToConfigurationFile is empty");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            Server target = GetServer();
            string expected = string.Empty; 
            string actual;
            actual = target.ToString();
            Assert.IsFalse(string.IsNullOrWhiteSpace(actual), "ToString is empty");
        }

        
        /// <summary>
        ///A test for AddressAnswerLimit
        ///</summary>
        [TestMethod()]
        public void AddressAnswerLimitTest()
        {
            Server target = GetServer();

            var previous = target.AddressAnswerLimit;
            if (previous != 28)
            {
                target.AddressAnswerLimit = 28;
                target.Save();
                target = GetServer();
                Assert.AreEqual(target.AddressAnswerLimit,28,"AddressAnswerLimit can't change");
            }
            else
            {
                target.AddressAnswerLimit = 0;
                target.Save();
                target = GetServer();
                Assert.AreEqual(target.AddressAnswerLimit,0,"AddressAnswerLimit can't change");
            }

            target.AddressAnswerLimit = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.AddressAnswerLimit,previous,"AddressAnswerLimit did not revert");
        }

        /// <summary>
        ///A test for AllowUpdate
        ///</summary>
        [TestMethod()]
        public void AllowUpdateTest()
        {
            Server target = GetServer();

            var previous = target.AllowUpdate;
            var newvalue = Server.AllowUpdateEnum.NoRestrictions;
            if (previous == Server.AllowUpdateEnum.NoRestrictions)
                newvalue = Server.AllowUpdateEnum.NoDynamicUpdateOfNS;
            else if (previous == Server.AllowUpdateEnum.NoDynamicUpdateOfNS)
                newvalue = Server.AllowUpdateEnum.NoRestrictions;

            target.AllowUpdate = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.AllowUpdate, newvalue, "Unable to change AllowUpdate");
            target.AllowUpdate = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.AllowUpdate, previous, "Unable to revert AllowUpdate");

        }

        /// <summary>
        ///A test for AutoCacheUpdate
        ///</summary>
        [TestMethod()]
        public void AutoCacheUpdateTest()
        {
            Server target = GetServer();

            var previous = target.AutoCacheUpdate ;
            var newvalue = !previous;

            target.AutoCacheUpdate = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.AutoCacheUpdate, newvalue, "Unable to change AutoCacheUpdate");
            target.AutoCacheUpdate = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.AutoCacheUpdate, previous, "Unable to revert AutoCacheUpdate");

        }

        /// <summary>
        ///A test for AutoConfigFileZones
        ///</summary>
        [TestMethod()]
        public void AutoConfigFileZonesTest()
        {

            Server target = GetServer();

            var previous = target.AutoConfigFileZones;
            var newvalue = Server.AutoConfigFileZonesEnum.OnlyServersDontAllowDynamicUpdates;
            if (previous == Server.AutoConfigFileZonesEnum.AllServers)
                newvalue = Server.AutoConfigFileZonesEnum.None;
            else if (previous == Server.AutoConfigFileZonesEnum.None)
                newvalue = Server.AutoConfigFileZonesEnum.AllServers;

            target.AutoConfigFileZones = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.AutoConfigFileZones, newvalue, "Unable to change AutoConfigFileZones");
            target.AutoConfigFileZones = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.AutoConfigFileZones, previous, "Unable to revert AutoConfigFileZones");

        }

        /// <summary>
        ///A test for BindSecondaries
        ///</summary>
        [TestMethod()]
        public void BindSecondariesTest()
        {
            Server target = GetServer();

            var previous = target.BindSecondaries;
            var newvalue = !previous;

            target.BindSecondaries = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.BindSecondaries, newvalue, "Unable to change BindSecondaries");
            target.BindSecondaries = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.BindSecondaries, previous, "Unable to revert BindSecondaries");

        }

        /// <summary>
        ///A test for BootMethod
        ///</summary>
        [TestMethod()]
        public void BootMethodTest()
        {
            var target = GetServer();
            var previous = target.BootMethod;
            var newvalue = Server.BootMethodEnum.DirectoryAndRegisty;
            if (previous == Server.BootMethodEnum.File)
                newvalue = Server.BootMethodEnum.Registry;
            else if (previous == Server.BootMethodEnum.Registry)
                newvalue = Server.BootMethodEnum.File;

            target.BootMethod = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.BootMethod, newvalue, "Unable to change BootMethod");
            target.BootMethod = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.BootMethod, previous, "Unable to revert BootMethod");

        }

        /// <summary>
        ///A test for DefaultAgingState
        ///</summary>
        [TestMethod()]
        public void DefaultAgingStateTest()
        {
            Server target = GetServer();

            var previous = target.DefaultAgingState;
            var newvalue = !previous;

            target.DefaultAgingState = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.DefaultAgingState, newvalue, "Unable to change DefaultAgingState");
            target.DefaultAgingState = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.DefaultAgingState, previous, "Unable to revert DefaultAgingState");

        }

        /// <summary>
        ///A test for DefaultNoRefreshInterval
        ///</summary>
        [TestMethod()]
        public void DefaultNoRefreshIntervalTest()
        {
            Server target = GetServer();
            var previous = target.DefaultNoRefreshInterval;
            var newval = previous + TimeSpan.FromHours( 10);
            target.DefaultNoRefreshInterval = newval;
            target.Save();
            target = GetServer();
            Assert.AreEqual(newval, target.DefaultNoRefreshInterval, "Unable to change DefaultNoRefreshInterval");
            target.DefaultNoRefreshInterval = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(previous, target.DefaultNoRefreshInterval, "Unable to revert DefaultNoRefreshInterval");

        }

        /// <summary>
        ///A test for DisableAutoReverseZones
        ///</summary>
        [TestMethod()]
        public void DisableAutoReverseZonesTest()
        {
            Server target = GetServer();

            var previous = target.DisableAutoReverseZones;
            var newvalue = !previous;

            target.DisableAutoReverseZones = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.DisableAutoReverseZones, newvalue, "Unable to change DisableAutoReverseZones");
            target.DisableAutoReverseZones = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.DisableAutoReverseZones, previous, "Unable to revert DisableAutoReverseZones");

        }

        /// <summary>
        ///A test for DisjointNets
        ///</summary>
        [TestMethod()]
        public void DisjointNetsTest()
        {
            Server target = GetServer();

            var previous = target.DisjointNets;
            var newvalue = !previous;

            target.DisjointNets = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.DisjointNets, newvalue, "Unable to change DisjointNets");
            target.DisjointNets = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.DisjointNets, previous, "Unable to revert DisjointNets");

        }

        /// <summary>
        ///A test for DsAvailable
        ///</summary>
        [TestMethod()]
        public void DsAvailableTest()
        {
            Server target = GetServer();

            var previous = target.DsAvailable;
            var newvalue = !previous;

            target.DsAvailable = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.DsAvailable, newvalue, "Unable to change DsAvailable");
            target.DsAvailable = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.DsAvailable, previous, "Unable to revert DsAvailable");

        }

        /// <summary>
        ///A test for DsPollingInterval
        ///</summary>
        [TestMethod()]
        public void DsPollingIntervalTest()
        {
            Server target = GetServer();
            var previous = target.DsPollingInterval;
            var newval = previous + TimeSpan.FromSeconds(10);
            target.DsPollingInterval = newval;
            target.Save();
            target = GetServer();
            Assert.AreEqual(newval, target.DsPollingInterval, "Unable to change DsPollingInterval");
            target.DsPollingInterval = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(previous, target.DsPollingInterval, "Unable to revert DsPollingInterval");


        }

        /// <summary>
        ///A test for DsTombstoneInterval
        ///</summary>
        [TestMethod()]
        public void DsTombstoneIntervalTest()
        {
            Server target = GetServer();
            var previous = target.DsTombstoneInterval;
            var newval = previous + TimeSpan.FromSeconds(10);
            target.DsTombstoneInterval = newval;
            target.Save();
            target = GetServer();
            Assert.AreEqual(newval, target.DsTombstoneInterval, "Unable to change DsTombstoneInterval");
            target.DsTombstoneInterval = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(previous, target.DsTombstoneInterval, "Unable to revert DsTombstoneInterval");

        }

        /// <summary>
        ///A test for EDnsCacheTimeout
        ///</summary>
        [TestMethod()]
        public void EDnsCacheTimeoutTest()
        {
            Server target = GetServer();
            var previous = target.EDnsCacheTimeout;
            var newval = previous + TimeSpan.FromSeconds(10);
            target.EDnsCacheTimeout = newval;
            target.Save();
            target = GetServer();
            Assert.AreEqual(newval, target.EDnsCacheTimeout, "Unable to change EDnsCacheTimeout");
            target.EDnsCacheTimeout = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(previous, target.EDnsCacheTimeout, "Unable to revert EDnsCacheTimeout");

        }

        /// <summary>
        ///A test for EnableDirectoryPartitions
        ///</summary>
        [TestMethod()]
        public void EnableDirectoryPartitionsTest()
        {
            Server target = GetServer();

            var previous = target.EnableDirectoryPartitions;
            var newvalue = !previous;

            target.EnableDirectoryPartitions = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.DsAvailable, newvalue, "Unable to change EnableDirectoryPartitions");
            target.EnableDirectoryPartitions = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.EnableDirectoryPartitions, previous, "Unable to revert EnableDirectoryPartitions");

        }

        /// <summary>
        ///A test for EnableDnsSec
        ///</summary>
        [TestMethod()]
        public void EnableDnsSecTest()
        {
            var target = GetServer();
            var previous = target.EnableDnsSec;
            var newvalue = Server.EnableDnsSecEnum.IncludedRFC2535;
            if (previous == Server.EnableDnsSecEnum.IncludedRFC2671)
                newvalue = Server.EnableDnsSecEnum.NoDNSSEC;
            else if (previous == Server.EnableDnsSecEnum.NoDNSSEC)
                newvalue = Server.EnableDnsSecEnum.IncludedRFC2671;

            target.EnableDnsSec = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.EnableDnsSec, newvalue, "Unable to change EnableDnsSec");
            target.EnableDnsSec = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.EnableDnsSec, previous, "Unable to revert EnableDnsSec");

        }

        /// <summary>
        ///A test for EnableEDnsProbes
        ///</summary>
        [TestMethod()]
        public void EnableEDnsProbesTest()
        {
            Server target = GetServer();

            var previous = target.EnableEDnsProbes;
            var newvalue = !previous;

            target.EnableEDnsProbes = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.EnableEDnsProbes, newvalue, "Unable to change EnableEDnsProbes");
            target.EnableEDnsProbes = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.EnableEDnsProbes, previous, "Unable to revert EnableEDnsProbes");

        }

        /// <summary>
        ///A test for EventLogLevel
        ///</summary>
        [TestMethod()]
        public void EventLogLevelTest()
        {
            var target = GetServer();
            var previous = target.EventLogLevel;
            var newvalue = Server.EventLogLevelEnum.None;
            if (previous == Server.EventLogLevelEnum.ErrorsOnly)
                newvalue = Server.EventLogLevelEnum.All;
            else if (previous == Server.EventLogLevelEnum.WarningsAndErrors)
                newvalue = Server.EventLogLevelEnum.ErrorsOnly;

            target.EventLogLevel = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.EventLogLevel, newvalue, "Unable to change EventLogLevel");
            target.EventLogLevel = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.EventLogLevel, previous, "Unable to revert EventLogLevel");


        }

        /// <summary>
        ///A test for ForwardDelegations
        ///</summary>
        [TestMethod()]
        public void ForwardDelegationsTest()
        {
            Server target = GetServer();

            var previous = target.ForwardDelegations;
            var newvalue = !previous;

            target.ForwardDelegations = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.ForwardDelegations, newvalue, "Unable to change ForwardDelegations");
            target.ForwardDelegations = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.ForwardDelegations, previous, "Unable to revert ForwardDelegations");

        }

        /// <summary>
        ///A test for Forwarders
        ///</summary>
        [TestMethod()]
        public void ForwardersTest()
        {
            Server target = GetServer();
            var existing = target.Forwarders;
            var newarr = new string[] {"1.1.1.1"};

            target.Forwarders = newarr;
            target.Save();
            target = GetServer();
            CollectionAssert.AreEqual(newarr,target.Forwarders,"Unable to change Forwarders");
            target.Forwarders = existing;
            target.Save();
            target = GetServer();
            CollectionAssert.AreEqual(existing,target.Forwarders,"Unable to revert Forwarders");

        }

        /// <summary>
        ///A test for ForwardingTimeout
        ///</summary>
        [TestMethod()]
        public void ForwardingTimeoutTest()
        {
            Server target = GetServer();
            var previous = target.ForwardingTimeout;
            var newval = previous + TimeSpan.FromSeconds(5);
            target.ForwardingTimeout = newval;
            target.Save();
            target = GetServer();
            Assert.AreEqual(newval, target.ForwardingTimeout, "Unable to change ForwardingTimeout");
            target.ForwardingTimeout = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(previous, target.ForwardingTimeout, "Unable to revert ForwardingTimeout");

        }

        /// <summary>
        ///A test for IsSlave
        ///</summary>
        [TestMethod()]
        public void IsSlaveTest()
        {
            Server target = GetServer();

            var previous = target.IsSlave;
            var newvalue = !previous;

            target.IsSlave = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.IsSlave, newvalue, "Unable to change IsSlave");
            target.IsSlave = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.IsSlave, previous, "Unable to revert IsSlave");

        }

        /// <summary>
        ///A test for ListenAddresses
        ///</summary>
        [TestMethod()]
        public void ListenAddressesTest()
        {
            Server target = GetServer();
            var existing = target.ListenAddresses;
            //might error depending on ServerAddresses, IPv6 are treated as invalid IPs, SP might fix it.
            var newarr = new string[] { target.ServerAddresses[1] };

            target.ListenAddresses = newarr;
            target.Save();
            target = GetServer();
            CollectionAssert.AreEqual(newarr, target.ListenAddresses, "Unable to change ListenAddresses");
            target.ListenAddresses = existing;
            target.Save();
            target = GetServer();
            CollectionAssert.AreEqual(existing, target.ListenAddresses, "Unable to revert ListenAddresses");

        }

        /// <summary>
        ///A test for LocalNetPriority
        ///</summary>
        [TestMethod()]
        public void LocalNetPriorityTest()
        {
            Server target = GetServer();

            var previous = target.LocalNetPriority;
            var newvalue = !previous;

            target.LocalNetPriority = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.LocalNetPriority, newvalue, "Unable to change LocalNetPriority");
            target.LocalNetPriority = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.LocalNetPriority, previous, "Unable to revert LocalNetPriority");

        }

        /// <summary>
        ///A test for LogFileMaxSize
        ///</summary>
        [TestMethod()]
        public void LogFileMaxSizeTest()
        {
            Server target = GetServer();
            var existing = target.LogFileMaxSize;
            var newval = existing + 10;
            target.LogFileMaxSize = newval;
            target.Save();
            target = GetServer();
            Assert.AreEqual(newval, target.LogFileMaxSize, "Unable to change LogFileMaxSize");
            target.LogFileMaxSize = existing;
            target.Save();
            target = GetServer();
            Assert.AreEqual(existing, target.LogFileMaxSize, "Unable to revert LogFileMaxSize");

        }

        /// <summary>
        ///A test for LogFilePath
        ///</summary>
        [TestMethod()]
        public void LogFilePathTest()
        {
            Server target = GetServer();
            var existing = target.LogFilePath;
            var newval = @"C:\";

            target.LogFilePath = newval;
            target.Save();
            target = GetServer();
            Assert.AreEqual(newval, target.LogFilePath, "Unable to change LogFilePath");
            target.LogFilePath = existing;
            target.Save();
            target = GetServer();
            Assert.AreEqual(existing, target.LogFilePath, "Unable to revert LogFilePath");

        }

        /// <summary>
        ///A test for LogIPFilterList
        ///</summary>
        [TestMethod()]
        public void LogIPFilterListTest()
        {
            Server target = GetServer();
            var existing = target.LogIPFilterList;
            var newarr = new string[] { "127.0.0.1" };

            target.LogIPFilterList = newarr;
            target.Save();
            target = GetServer();
            CollectionAssert.AreEqual(newarr, target.LogIPFilterList, "Unable to change LogIPFilterList");
            target.LogIPFilterList = existing;
            target.Save();
            target = GetServer();
            CollectionAssert.AreEqual(existing, target.LogIPFilterList, "Unable to revert LogIPFilterList");

        }

        /// <summary>
        ///A test for LogLevel
        ///</summary>
        [TestMethod()]
        public void LogLevelTest()
        {
            var target = GetServer();
            var previous = target.LogLevel;
            var newvalue = Server.LogLevelEnum.AllPackets;
            if (previous == Server.LogLevelEnum.Answers)
                newvalue = Server.LogLevelEnum.NonqueryTransactions;
            else if (previous == Server.LogLevelEnum.Notify)
                newvalue = Server.LogLevelEnum.Query;

            target.LogLevel = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.LogLevel, newvalue, "Unable to change LogLevel");
            target.LogLevel = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.LogLevel, previous, "Unable to revert LogLevel");
            
        }

        /// <summary>
        ///A test for LooseWildcarding
        ///</summary>
        [TestMethod()]
        public void LooseWildcardingTest()
        {
            Server target = GetServer();

            var previous = target.LooseWildcarding;
            var newvalue = !previous;

            target.LooseWildcarding = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.LooseWildcarding, newvalue, "Unable to change LooseWildcarding");
            target.LooseWildcarding = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.LooseWildcarding, previous, "Unable to revert LooseWildcarding");

        }

        /// <summary>
        ///A test for MaxCacheTTL
        ///</summary>
        [TestMethod()]
        public void MaxCacheTTLTest()
        {
            Server target = GetServer();
            var previous = target.MaxCacheTTL;
            var newval = previous + TimeSpan.FromSeconds(5);
            target.MaxCacheTTL = newval;
            target.Save();
            target = GetServer();
            Assert.AreEqual(newval, target.MaxCacheTTL, "Unable to change MaxCacheTTL");
            target.MaxCacheTTL = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(previous, target.MaxCacheTTL, "Unable to revert MaxCacheTTL");

        }

        /// <summary>
        ///A test for MaxNegativeCacheTTL
        ///</summary>
        [TestMethod()]
        public void MaxNegativeCacheTTLTest()
        {
            Server target = GetServer();
            var previous = target.MaxNegativeCacheTTL;
            var newval = previous + TimeSpan.FromSeconds(5);
            target.MaxNegativeCacheTTL = newval;
            target.Save();
            target = GetServer();
            Assert.AreEqual(newval, target.MaxNegativeCacheTTL, "Unable to change MaxNegativeCacheTTL");
            target.MaxNegativeCacheTTL = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(previous, target.MaxNegativeCacheTTL, "Unable to revert MaxNegativeCacheTTL");

        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {
            Server target = GetServer();
            string actual;
            actual = target.Name;
            Assert.IsFalse(string.IsNullOrWhiteSpace(actual), "Name is empty");
        }

        /// <summary>
        ///A test for NameCheckFlag
        ///</summary>
        [TestMethod()]
        public void NameCheckFlagTest()
        {
            var target = GetServer();
            var previous = target.NameCheckFlag;
            var newvalue = Server.NameCheckFlagEnum.NonRFCANSI;
            if (previous == Server.NameCheckFlagEnum.MultibyteUTF8)
                newvalue = Server.NameCheckFlagEnum.NonRFCANSI;
            else if (previous == Server.NameCheckFlagEnum.StrictRFCANSI)
                newvalue = Server.NameCheckFlagEnum.AllNames;

            target.NameCheckFlag = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.NameCheckFlag, newvalue, "Unable to change NameCheckFlag");
            target.NameCheckFlag = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.NameCheckFlag, previous, "Unable to revert NameCheckFlag");

        }

        /// <summary>
        ///A test for NoRecursion
        ///</summary>
        [TestMethod()]
        public void NoRecursionTest()
        {
            Server target = GetServer();

            var previous = target.NoRecursion;
            var newvalue = !previous;

            target.NoRecursion = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.NoRecursion, newvalue, "Unable to change NoRecursion");
            target.NoRecursion = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.NoRecursion, previous, "Unable to revert NoRecursion");

        }

        /// <summary>
        ///A test for RecursionRetry
        ///</summary>
        [TestMethod()]
        public void RecursionRetryTest()
        {
            Server target = GetServer();
            var previous = target.RecursionRetry;
            var newval = previous + TimeSpan.FromSeconds(5);
            target.RecursionRetry = newval;
            target.Save();
            target = GetServer();
            Assert.AreEqual(newval, target.RecursionRetry, "Unable to change RecursionRetry");
            target.RecursionRetry = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(previous, target.RecursionRetry, "Unable to revert RecursionRetry");

        }

        /// <summary>
        ///A test for RecursionTimeout
        ///</summary>
        [TestMethod()]
        public void RecursionTimeoutTest()
        {
            Server target = GetServer();
            var previous = target.RecursionTimeout;
            var newval = previous + TimeSpan.FromSeconds(5);
            target.RecursionTimeout = newval;
            target.Save();
            target = GetServer();
            Assert.AreEqual(newval, target.RecursionTimeout, "Unable to change RecursionTimeout");
            target.RecursionTimeout = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(previous, target.RecursionTimeout, "Unable to revert RecursionTimeout");

        }

        /// <summary>
        ///A test for RoundRobin
        ///</summary>
        [TestMethod()]
        public void RoundRobinTest()
        {
            Server target = GetServer();

            var previous = target.RoundRobin;
            var newvalue = !previous;

            target.RoundRobin = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.RoundRobin, newvalue, "Unable to change RoundRobin");
            target.RoundRobin = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.RoundRobin, previous, "Unable to revert RoundRobin");

        }

        /// <summary>
        ///A test for RpcProtocol
        ///</summary>
        [TestMethod()]
        public void RpcProtocolTest()
        {
            var target = GetServer();
            var previous = target.RpcProtocol;
            var newvalue = Server.RpcProtocolEnum.LPC;
            if (previous == Server.RpcProtocolEnum.NamedPipes)
                newvalue = Server.RpcProtocolEnum.None;
            else if (previous == Server.RpcProtocolEnum.TCP)
                newvalue = Server.RpcProtocolEnum.NamedPipes;

            target.RpcProtocol = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.RpcProtocol, newvalue, "Unable to change RpcProtocol");
            target.RpcProtocol = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.RpcProtocol, previous, "Unable to revert RpcProtocol");

        }

        /// <summary>
        ///A test for ScavengingInterval
        ///</summary>
        [TestMethod()]
        public void ScavengingIntervalTest()
        {
            Server target = GetServer();
            var previous = target.ScavengingInterval;
            var newval = previous + TimeSpan.FromHours(5);
            target.ScavengingInterval = newval;
            target.Save();
            target = GetServer();
            Assert.AreEqual(newval, target.ScavengingInterval, "Unable to change ScavengingInterval");
            target.ScavengingInterval = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(previous, target.ScavengingInterval, "Unable to revert ScavengingInterval");

        }

        /// <summary>
        ///A test for SecureResponses
        ///</summary>
        [TestMethod()]
        public void SecureResponsesTest()
        {
            Server target = GetServer();

            var previous = target.SecureResponses;
            var newvalue = !previous;

            target.SecureResponses = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.SecureResponses, newvalue, "Unable to change SecureResponses");
            target.SecureResponses = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.SecureResponses, previous, "Unable to revert SecureResponses");

        }

        /// <summary>
        ///A test for SendPort
        ///</summary>
        [TestMethod()]
        public void SendPortTest()
        {
            Server target = GetServer();
            var existing = target.SendPort;
            var newval = existing + 10;

            target.SendPort = newval;
            target.Save();
            target = GetServer();
            Assert.AreEqual(newval, target.SendPort, "Unable to change SendPort");

            target.SendPort = existing;
            target.Save();
            target = GetServer();
            Assert.AreEqual(existing, target.SendPort, "Unable to revert SendPort");

        }

        /// <summary>
        ///A test for ServerAddresses
        ///</summary>
        [TestMethod()]
        public void ServerAddressesTest()
        {
            Server target = GetServer();
            string[] actual;
            actual = target.ServerAddresses;
            Assert.IsTrue((actual.Length > 0), "No Server Addresses?");
        }

        /// <summary>
        ///A test for StrictFileParsing
        ///</summary>
        [TestMethod()]
        public void StrictFileParsingTest()
        {
            Server target = GetServer();

            var previous = target.StrictFileParsing;
            var newvalue = !previous;

            target.StrictFileParsing = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.StrictFileParsing, newvalue, "Unable to change StrictFileParsing");
            target.StrictFileParsing = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.StrictFileParsing, previous, "Unable to revert StrictFileParsing");

        }

        /// <summary>
        ///A test for UpdateOptions
        ///</summary>
        [TestMethod()]
        public void UpdateOptionsTest()
        {
            Server target = GetServer();
            
            var actual = target.UpdateOptions;
            //ok if no exceptions.
        }

        /// <summary>
        ///A test for Version
        ///</summary>
        [TestMethod()]
        public void VersionTest()
        {
            Server target = GetServer();
            WMIVersion actual;
            actual = target.Version;
            //ok if no exceptions
            Assert.AreEqual(actual.ToString(), "6.1.7600 (0x1DB0)", "Windows 2008 version validation");
        }

        /// <summary>
        ///A test for WriteAuthorityNS
        ///</summary>
        [TestMethod()]
        public void WriteAuthorityNSTest()
        {
            Server target = GetServer();

            var previous = target.WriteAuthorityNS;
            var newvalue = !previous;

            target.WriteAuthorityNS = newvalue;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.WriteAuthorityNS, newvalue, "Unable to change WriteAuthorityNS");
            target.WriteAuthorityNS = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(target.WriteAuthorityNS, previous, "Unable to revert WriteAuthorityNS");

        }

        /// <summary>
        ///A test for XfrConnectTimeout
        ///</summary>
        [TestMethod()]
        public void XfrConnectTimeoutTest()
        {
            Server target = GetServer();
            var previous = target.XfrConnectTimeout;
            var newval = previous + TimeSpan.FromSeconds(5);
            target.XfrConnectTimeout = newval;
            target.Save();
            target = GetServer();
            Assert.AreEqual(newval, target.XfrConnectTimeout, "Unable to change XfrConnectTimeout");
            target.XfrConnectTimeout = previous;
            target.Save();
            target = GetServer();
            Assert.AreEqual(previous, target.XfrConnectTimeout, "Unable to revert XfrConnectTimeout");

        }
    }
}
