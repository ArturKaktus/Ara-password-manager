using CryptoUSB.Models;
using CryptoUSB.Services;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoUSB.Controllers
{

    public class OpenFromDeviceController
    {
        public string PinPassword { get; set; }
        public CheckIsConnected checkIsConnected = new CheckIsConnected();
        public string pinCode;
        public void ClickOpen()
        {
            //this.checkIsConnected.contin = false;
            //CheckPin checkPin = new CheckPin();

            //checkPin.Start();

            DeviceReaderModel deviceReaderModel = new DeviceReaderModel(this.pinCode);
            deviceReaderModel.read();
        }
        public void CloseOpenDBPassword()
        {
            if (this.checkIsConnected.contin)
            {
                this.checkIsConnected.contin = false;
            }
        }
    }
    public class CheckPin
    {
        public void Open()
        {

        }
        //public void Run()
        //{
        //    OpenFromDeviceController.pinCode = OpenFromDeviceController.PinPassword;
        //    if (OpenFromDeviceController.pinCode.Length == 4)
        //    {
        //        Platform.RunLater(() => OpenFromDeviceController.DisableElements());
        //        OpenFromDeviceController.kakaduCommander.SetPort(DeviceFinder.Instance.Port);
        //        DeviceFinder.Instance.StopSearch();
        //        OpenFromDeviceController.kakaduCommander.FlushPort();
        //        if (OpenFromDeviceController.kakaduCommander.ExecuteCommand("cCHK"))
        //        {
        //            if (OpenFromDeviceController.kakaduCommander.SendPIN(OpenFromDeviceController.pinCode))
        //            {
        //                DeviceFinder.Instance.StartSearch();
        //                Platform.RunLater(() => OpenFromDeviceController.CloseOpenDBPassword());
        //            }
        //            else
        //            {
        //                DeviceFinder.Instance.StartSearch();
        //                Platform.RunLater(() =>
        //                {
        //                    OpenFromDeviceController.errorLabel.Style = "-fx-text-fill:#e41422";
        //                    OpenFromDeviceController.errorLabel.Text = LanguageController.Instance.AppLanguageBundle.GetString("device.open.wrong") + OpenFromDeviceController.kakaduCommander.ErrorCount;
        //                    OpenFromDeviceController.closeOpenDBPassword.IsEnabled = false;
        //                });
        //                try
        //                {
        //                    Thread.Sleep(1000);
        //                }
        //                catch (Exception exception)
        //                {
        //                    // empty catch block
        //                }
        //                Platform.RunLater(() => OpenFromDeviceController.EnableElements());
        //                OpenFromDeviceController.checkIsConnected = new CheckIsConnected();
        //                OpenFromDeviceController.checkIsConnected.Start();
        //            }
        //        }
        //        else
        //        {
        //            DeviceFinder.Instance.StartSearch();
        //            Platform.RunLater(() =>
        //            {
        //                OpenFromDeviceController.errorLabel.Style = "-fx-text-fill:#e41422";
        //                OpenFromDeviceController.errorLabel.Text = LanguageController.Instance.AppLanguageBundle.GetString("device.open.offline");
        //                OpenFromDeviceController.closeOpenDBPassword.IsEnabled = false;
        //            });
        //            try
        //            {
        //                Thread.Sleep(1000);
        //            }
        //            catch (Exception exception)
        //            {
        //                // empty catch block
        //            }
        //            while (!DeviceFinder.Instance.IsConnected)
        //            {
        //            }
        //            Platform.RunLater(() => OpenFromDeviceController.EnableElements());
        //            OpenFromDeviceController.checkIsConnected = new CheckIsConnected();
        //            OpenFromDeviceController.checkIsConnected.Start();
        //        }
        //    }
        //}
    }
    public class CheckIsConnected
    {
        public bool contin = true;

        //public void Run()
        //{
        //    while (this.contin)
        //    {
        //        if (DeviceFinder.Instance.IsConnected) continue;
        //        this.contin = false;
        //        OpenFromDeviceController.pinCode = "";
        //        OpenFromDeviceController.CloseOpenDBPassword();
        //    }
        //}
    }
}
