using DNSManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management;
using System.Linq;

namespace DNSManagementTest
{
    
    
    /// <summary>
    ///This is a test class for RootHintsTest and is intended
    ///to contain all RootHintsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RootHintsTest
    {
        public static RootHints[] GetRootHints()
        {
            var server = ServerTest.GetServer();
            var roothints = server.GetRootHints();
            return roothints;
        }

        /// <summary>
        ///A test for RootHints Constructor
        ///</summary>
        [TestMethod()]
        
        public void RootHintsConstructorTest()
        {
            
            var target = GetRootHints();
            Assert.IsTrue((target.Length > 0), "No root hints?");
        }

        /// <summary>
        ///A test for WriteBackRootHintDatafile
        ///</summary>
        [TestMethod()]
        public void WriteBackRootHintDatafileTest()
        {
            
            var target = GetRootHints().FirstOrDefault();
            Server server = ServerTest.GetServer();
            target.WriteBackRootHintDatafile(server);
            
        }
    }
}
