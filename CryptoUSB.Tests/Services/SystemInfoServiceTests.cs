using CryptoUSB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Tests.Services
{
    public class SystemInfoServiceTests
    {
        [Fact]
        public void GetOS()
        {
            SystemInfoService systemInfoService = new SystemInfoService();
            var inst = SystemInfoService.INSTANCE;
        }
    }
}
