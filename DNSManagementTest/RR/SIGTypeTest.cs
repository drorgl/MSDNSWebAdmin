//using DNSManagement.RR;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Management;
//using DNSManagement;
//using System.Linq;

//namespace DNSManagementTest
//{
    
    
//    /// <summary>
//    ///This is a test class for SIGTypeTest and is intended
//    ///to contain all SIGTypeTest Unit Tests
//    ///</summary>
//    [TestClass()]
//    public class SIGTypeTest
//    {

//        public static SIGType GetSIGRR()
//        {
//            var server = ServerTest.GetServer();
//            var zone = ZoneTest.GetZone();

//            var record = zone.GetRecords().FirstOrDefault(i => i.OwnerName == "sig." + zone.Name);
//            if (record == null)
//                record = SIGType.CreateInstanceFromPropertyData(server, server.Name, zone.Name, "sig." + zone.Name, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, 1, SIGType.AlgorithmEnum.Diffie_Hellman, 1, 3,4,5,6,"hello", "abcd==");
//            return (SIGType)record.UnderlyingRecord;
//        }

//        /// <summary>
//        ///A test for SIGType Constructor
//        ///</summary>
//        [TestMethod()]
        
//        public void SIGTypeConstructorTest()
//        {
            
//            var target = GetSIGRR();
//            Assert.IsNotNull(target);
//        }

//        /// <summary>
//        ///A test for CreateInstanceFromPropertyData
//        ///</summary>
//        [TestMethod()]
//        public void CreateInstanceFromPropertyDataTest()
//        {
//            var target = GetSIGRR();
//            target.Delete();

//        }

//        /// <summary>
//        ///A test for Modify
//        ///</summary>
//        [TestMethod()]
//        public void ModifyTest()
//        {
//            Assert.Inconclusive("Unsupported");
//            //throw new NotImplementedException();
//            //var target = GetSIGRR(); 
//            //Nullable<TimeSpan> ttl = new Nullable<TimeSpan>(); 
//            //ushort typeCovered = 0; 
//            //SIGType.AlgorithmEnum algorithm = new SIGType.AlgorithmEnum(); 
//            //ushort labels = 0; 
//            //uint originalTTL = 0; 
//            //uint signatureExpiration = 0; 
//            //uint signatureInception = 0; 
//            //ushort keyTag = 0; 
//            //string signerName = string.Empty; 
//            //string signature = string.Empty; 
//            //SIGType expected = null; 
//            //SIGType actual;
//            //actual = target.Modify(ttl, typeCovered, algorithm, labels, originalTTL, signatureExpiration, signatureInception, keyTag, signerName, signature);
//            //Assert.AreEqual(expected, actual);
//            //Assert.Inconclusive("Verify the correctness of this test method.");
//        }

//        /// <summary>
//        ///A test for ToString
//        ///</summary>
//        [TestMethod()]
//        public void ToStringTest()
//        {
            
//            var target = GetSIGRR(); 
//            var actual = target.ToString();
//            Assert.IsFalse(string.IsNullOrEmpty(actual), "ToString not returning anything");
//        }

//        /// <summary>
//        ///A test for Algorithm
//        ///</summary>
//        [TestMethod()]
//        public void AlgorithmTest()
//        {
            
//            var target = GetSIGRR(); 
//            var actual = target.Algorithm;
//        }

//        /// <summary>
//        ///A test for KeyTag
//        ///</summary>
//        [TestMethod()]
//        public void KeyTagTest()
//        {
            
//            var target = GetSIGRR(); 
//            var actual = target.KeyTag;
//        }

//        /// <summary>
//        ///A test for Labels
//        ///</summary>
//        [TestMethod()]
//        public void LabelsTest()
//        {
            
//            var target = GetSIGRR(); 
//            var actual = target.Labels;
//        }

//        /// <summary>
//        ///A test for OriginalTTL
//        ///</summary>
//        [TestMethod()]
//        public void OriginalTTLTest()
//        {
            
//            var target = GetSIGRR(); 
//            var actual = target.OriginalTTL;
//        }

//        /// <summary>
//        ///A test for Signature
//        ///</summary>
//        [TestMethod()]
//        public void SignatureTest()
//        {
            
//            var target = GetSIGRR(); 
//            var actual = target.Signature;
//            Assert.IsFalse(string.IsNullOrEmpty(actual), "Signature not returning anything");
//        }

//        /// <summary>
//        ///A test for SignatureExpiration
//        ///</summary>
//        [TestMethod()]
//        public void SignatureExpirationTest()
//        {
            
//            var target = GetSIGRR(); 
//            var actual = target.SignatureExpiration;
//        }

//        /// <summary>
//        ///A test for SignatureInception
//        ///</summary>
//        [TestMethod()]
//        public void SignatureInceptionTest()
//        {
            
//            var target = GetSIGRR(); 
//            var actual = target.SignatureInception;
//        }

//        /// <summary>
//        ///A test for SignerName
//        ///</summary>
//        [TestMethod()]
//        public void SignerNameTest()
//        {
            
//            var target = GetSIGRR(); 
//            var actual = target.SignerName;
//            Assert.IsFalse(string.IsNullOrEmpty(actual), "SignerName not returning anything");
//        }

//        /// <summary>
//        ///A test for TypeCovered
//        ///</summary>
//        [TestMethod()]
//        public void TypeCoveredTest()
//        {
            
//            var target = GetSIGRR(); 
//            var actual = target.TypeCovered;
//        }
//    }
//}
