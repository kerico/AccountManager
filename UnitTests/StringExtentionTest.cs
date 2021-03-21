using AccountManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class StringExtentionTest
    {
        [TestMethod]
        public void Encrypt_ShouldChangeTheValue()
        {
            string initialString = Guid.NewGuid().ToString();

            string encryptedString = initialString.Encrypt();

            Assert.AreNotEqual(initialString, encryptedString);
        }
        [TestMethod]
        public void Decrypt_ShouldChangeEncryptedValueBack()
        {
            string initialString = Guid.NewGuid().ToString();

            string encryptedString = initialString.Encrypt();

            Assert.AreNotEqual(initialString, encryptedString);

            string decryptedString = encryptedString.Decrypt();

            Assert.AreEqual(initialString, decryptedString);
        }
    }
}
