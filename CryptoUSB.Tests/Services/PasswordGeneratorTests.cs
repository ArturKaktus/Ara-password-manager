using CryptoUSB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Tests.Services
{
    public class PasswordGeneratorTests
    {
        [Fact]
        public void GetPassword_7()
        {
            int len = 7;

            PasswordGenerator passwordGenerator = new PasswordGenerator();

            string password = passwordGenerator.GetPassword(len);

            Assert.Equal(7, password.Length);
        }
    }
}
