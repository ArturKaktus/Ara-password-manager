using CryptoUSB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Controllers
{
    public class SaveToDeviceController
    {
        public string PinPassword { get; set; }
        public CheckIsConnected checkIsConnected = new CheckIsConnected();
        public string pinCode;
        public void ClickSave()
        {
            //this.checkIsConnected.contin = false;
            //CheckPin checkPin = new CheckPin();

            //checkPin.Start();

            DeviceWriteModel deviceReaderModel = new DeviceWriteModel(this.pinCode);
            deviceReaderModel.write();
        }
    }
}
