using DNSManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for CacheTest and is intended
    ///to contain all CacheTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CacheTest
    {


        public static Cache[] GetCache()
        {
            var server = ServerTest.GetServer();
            return server.GetCache();
        }

        /// <summary>
        ///A test for Cache Constructor
        ///</summary>
        [TestMethod()]
        
        public void CacheConstructorTest()
        {
            
            var target = GetCache();
            Assert.IsTrue(target.Length > 0, "No cache?");
        }

        /// <summary>
        ///A test for ClearCache
        ///</summary>
        [TestMethod()]
        public void ClearCacheTest()
        {
            Server server = ServerTest.GetServer();
            Cache.ClearCache(server);
        }
    }
}
