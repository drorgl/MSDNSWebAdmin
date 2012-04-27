//using DNSManagement.RR;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Management;
//using DNSManagement;
//using System.Linq;

//namespace DNSManagementTest
//{
    
    
//    /// <summary>
//    ///This is a test class for KEYTypeTest and is intended
//    ///to contain all KEYTypeTest Unit Tests
//    ///</summary>
//    [TestClass()]
//    public class KEYTypeTest
//    {

//        public static KEYType GetKEYRR()
//        {
//            var server = ServerTest.GetServer();
//            var zone = ZoneTest.GetZone();

//            var records = zone.GetRecords();

//            var record = records.FirstOrDefault(i => i.OwnerName == "key." + zone.Name);
//            if (record == null)
//                record = KEYType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "key." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, 0, KEYType.ProtocolEnum.All, KEYType.AlgorithmEnum.Diffie_Hellman, "1234==");
//            return (KEYType)record.UnderlyingRecord;
//        }


//        /// <summary>
//        ///A test for KEYType Constructor
//        ///</summary>
//        [TestMethod()]
        
//        public void KEYTypeConstructorTest()
//        {
            
//            var target = GetKEYRR();
//            Assert.IsNotNull(target);
//        }

//        /// <summary>
//        ///A test for CreateInstanceFromPropertyData
//        ///</summary>
//        [TestMethod()]
//        public void CreateInstanceFromPropertyDataTest()
//        {
//            var target = GetKEYRR();
//            target.Delete();
//        }

//        /// <summary>
//        ///A test for Modify
//        ///</summary>
//        [TestMethod()]
//        public void ModifyTest()
//        {
//            throw new NotImplementedException();
//            var target = GetKEYRR(); 
//            Nullable<TimeSpan> ttl = new Nullable<TimeSpan>(); 
//            Nullable<ushort> flags = new Nullable<ushort>(); 
//            Nullable<KEYType.ProtocolEnum> protocol = new Nullable<KEYType.ProtocolEnum>(); 
//            Nullable<KEYType.AlgorithmEnum> algorithm = new Nullable<KEYType.AlgorithmEnum>(); 
//            string publicKey = string.Empty; 
//            KEYType expected = null; 
//            KEYType actual;
//            actual = target.Modify(ttl, flags, protocol, algorithm, publicKey);
//            Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("Verify the correctness of this test method.");
//        }

//        /// <summary>
//        ///A test for ToString
//        ///</summary>
//        [TestMethod()]
//        public void ToStringTest()
//        {
            
//            var target = GetKEYRR(); 
//            var actual = target.ToString();
//            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
//        }

//        /// <summary>
//        ///A test for Algorithm
//        ///</summary>
//        [TestMethod()]
//        public void AlgorithmTest()
//        {
            
//            var target = GetKEYRR(); 
//            var actual = target.Algorithm;
//        }

//        /// <summary>
//        ///A test for Flags
//        ///</summary>
//        [TestMethod()]
//        public void FlagsTest()
//        {
            
//            var target = GetKEYRR(); 
//            var actual = target.Flags;
//        }

//        /// <summary>
//        ///A test for Protocol
//        ///</summary>
//        [TestMethod()]
//        public void ProtocolTest()
//        {
            
//            var target = GetKEYRR(); 
//            var actual = target.Protocol;
//        }

//        /// <summary>
//        ///A test for PublicKey
//        ///</summary>
//        [TestMethod()]
//        public void PublicKeyTest()
//        {
            
//            var target = GetKEYRR(); 
//            var actual = target.PublicKey;
//            Assert.IsFalse(string.IsNullOrEmpty(actual), "PublicKey not returning anything");
//        }
//    }
//}
