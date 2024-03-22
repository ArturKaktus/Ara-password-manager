using CryptoUSB.Controllers;
using CryptoUSB.Services;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CryptoUSB.Models
{
    public class DeviceWriteModel
    {
        KakaduDeviceSerialPort serialPort;
        string pinString;
        bool error;
        string errorString;
        bool writing;
        string stageString;
        private double percent;
        private bool endWrite = false;
        KakaduCommander commander;

        public DeviceWriteModel(string pin) 
        {
            try
            {
                //this.bundle = LanguageController.INSTANCE.getAppLanguageBundle();
                string port = DeviceFinder.Instance.Port;
                this.serialPort = new KakaduDeviceSerialPort(port);
                this.pinString = pin;
                this.error = false;
                this.writing = true;
                //this.stageString = this.bundle.getString("device.write.start");
                bool percent = false;
                this.commander = new KakaduCommander();
                this.commander.SetPort(port);
            }
            catch (Exception e)
            {
                this.error = true;
                this.endWrite = true;
                //this.stageString = this.bundle.getString("device.read.error");
            }
        }

        public void write()
        {
            Run();
        }
        public void Run()
        {
            int errorCounter = 0;
            percent = 0;
            bool contin = commander.SendSET();
            if (contin)
            {
                contin = commander.SendPIN(pinString);
                if (contin)
                {
                    contin = commander.SendRowCount();
                    if (contin)
                    {
                        byte[] buffer = new byte[1];
                        byte[,] rows = DatabaseModel.Instance.GetDeveiceArray();
                        serialPort.OpenPort();
                        int count = DatabaseModel.Instance.GetRowCount() - 1;
                        double oneRowPercent = Convert.ToDouble(count);
                        int i = 1;
                        var c = rows.GetLength(0);
                        for (int ii = 0; i < c; ii++)
                        {
                            byte[] bs = rows.GetRow(ii);
                            if (error) continue;
                            percent = Convert.ToDouble(i) / oneRowPercent * 100.0;
                            bool waitAnswer = true;
                            byte[] arrby = bs;
                            int n = arrby.Length;
                            for (int j = 0; j < n; ++j)
                            {
                                byte b;
                                buffer[0] = b = arrby[j];
                                try
                                {
                                    serialPort.BreakState = true;
                                }
                                catch { }
                                serialPort.Write(buffer, 0, buffer.Length);
                            }
                            errorCounter = 0;
                            while (waitAnswer) 
                            {
                                try
                                {
                                    Thread.Sleep(50);
                                }
                                catch { }
                                if (errorCounter == 200)
                                {
                                    error = true;
                                    serialPort.ClosePort();
                                    //stageString = DeviceWriteModel.this.bundle.getString("device.read.error");
                                    try
                                    {
                                        Thread.Sleep(3000);
                                    }
                                    catch (Exception exception)
                                    {
                                        // empty catch block
                                    }
                                    endWrite = true;
                                    DeviceFinder.Instance.StartSearch();
                                    //this.stop();
                                    break;
                                }
                                if (serialPort.BytesToRead > 0)
                                {
                                        waitAnswer = false;
                                        continue;
                                }
                                errorCounter++;
                            }
                            i++;
                        }
                        serialPort.ClosePort();
                        endWrite = true;
                        DatabaseModel.Instance.HashDatabase();
                        DeviceFinder.Instance.StartSearch();
                    }
                    else
                    {
                        error = true;
                        serialPort.ClosePort();
                        try
                        {
                            Thread.Sleep(3000);
                        }
                        catch (Exception exception)
                        {
                            // empty catch block
                        }
                        endWrite = true;
                        DeviceFinder.Instance.StartSearch();
                    }
                }
                else
                {
                    error = true;
                    serialPort.ClosePort();
                    try
                    {
                        Thread.Sleep(3000);
                    }
                    catch (Exception exception)
                    {
                        // empty catch block
                    }
                    endWrite = true;
                    DeviceFinder.Instance.StartSearch();
                }    
            }
            else
            {
                error = true;
                serialPort.ClosePort();
                try
                {
                    Thread.Sleep(3000);
                }
                catch (Exception exception)
                {
                    // empty catch block
                }
                endWrite = true;
                DeviceFinder.Instance.StartSearch();
            }
        }
    }
}
