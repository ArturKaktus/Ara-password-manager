using CryptoUSB.CustomClasses;
using CryptoUSB.Services;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoUSB.Models
{
    public class DeviceReaderModel
    {
        KakaduDeviceSerialPort SerialPort;
        string PinString;
        bool error;
        string errorString;
        bool writing;
        string stageString;
        double percent = 0.0;
        bool endRead = false;
        KakaduCommander Сommander;

        public DeviceReaderModel(string pin)
        {
            string port = DeviceFinder.Instance.Port;
            this.SerialPort = new KakaduDeviceSerialPort(port);
            this.PinString = pin;
            this.error = false;
            this.writing = true;
            //this.stageString = this.bundle.getString("device.read.starting");
            this.Сommander = new KakaduCommander();
            this.Сommander.SetPort(port);
        }

        public void read()
        {
            DeviceFinder.Instance.StopSearch();
            //try
            //{
            //    Thread.sleep(100L);
            //}
            //catch (Exception exception)
            //{
            //    // empty catch block
            //}
            //read.start();
            Run();
        }

        public void Run()
        {
            {
                Сommander.SetPort(DeviceFinder.Instance.Port);
                System.Threading.Thread.Sleep(3000);
                bool contin = Сommander.ExecuteCommand("cNUM");
                System.Threading.Thread.Sleep(1000);
                try
                {
                    if (contin)
                    {
                       // System.Threading.Thread.Sleep(4000);
                        int numRowsToRead = Сommander.GetDeviceRows();
                        //DeviceReaderModel.Instance.StageString = DeviceReaderModel.Instance.Bundle.GetString("device.read.count") + " " + numRowsToRead.ToString();
                        contin = Сommander.ExecuteCommand("cGET");
                        if (contin)
                        {
                            //DeviceReaderModel.Instance.StageString = DeviceReaderModel.Instance.Bundle.GetString("device.check");
                            //DeviceReaderModel.Instance.SerialPort.DisablePortConfiguration();
                            SerialPort.OpenPort();
                            try
                            {
                                byte[] pinBytes = System.Text.Encoding.ASCII.GetBytes(PinString);
                                SerialPort.Write(pinBytes, 0, pinBytes.Length);
                            }
                            catch (Exception)
                            {
                                // empty catch block
                            }
                            //DeviceReaderModel.Instance.StageString = DeviceReaderModel.Instance.Bundle.GetString("device.right");
                            int bufferLength = 196 * numRowsToRead;
                            byte[] buffer = new byte[bufferLength];
                            int errorCount = 0;
                            double bytes = 0.0;
                            int bytesCount = 0;
                            while (bufferLength != bytesCount)
                            {
                                System.Threading.Thread.Sleep(1000);
                                if (errorCount == 10)
                                {
                                    SerialPort.Close();
                                    ReadError();
                                    break;
                                }
                                if (0 == SerialPort.BytesToRead)
                                {
                                    ++errorCount;
                                }
                                byte[] b = new byte[SerialPort.BytesToRead];
                                SerialPort.Read(b, 0, b.Length);
                                bytesCount += b.Length;
                                Array.Copy(b, 0, buffer, bytesCount- b.Length, b.Length);
                                bytes += Convert.ToDouble(bytesCount);
                                percent = bytes / bufferLength * 100.0;
                                //DeviceReaderModel.Instance.StageString = DeviceReaderModel.Instance.Bundle.GetString("device.read.receive") + " " + Math.Round(DeviceReaderModel.Instance.Percent).ToString() + "%";
                                SerialPort.BaudRate = 115200;
                                SerialPort.DataBits = 8;
                                SerialPort.StopBits = System.IO.Ports.StopBits.One;
                                SerialPort.Parity = System.IO.Ports.Parity.None;
                            }
                            if (errorCount != 10)
                            {
                                SerialPort.Close();
                                byte[,] resultArray = new byte[numRowsToRead, 196];
                                for (int i = 0; i < numRowsToRead; ++i)
                                {
                                    //        resultArray[i] = new byte[196];
                                    for (int j = 0; j < 196; ++j)
                                    {
                                        resultArray[i,j] = buffer[j + 196 * i];
                                    }
                                }
                                DatabaseModel.Instance.FillFromDevice(resultArray);
                                DatabaseModel.Instance.HashDatabase();
                                DatabaseModel.Instance.BuildTree();
                                System.Threading.Thread.Sleep(4000);
                                DeviceFinder.Instance.StartSearch();
                                endRead = true;
                            }
                            return;
                        }
                        error = true;
                        //DeviceReaderModel.Instance.StageString = DeviceReaderModel.Instance.Bundle.GetString("device.read.error");
                        //Console.Error.WriteLine(DeviceReaderModel.Instance.ErrorString);
                        System.Threading.Thread.Sleep(4000);
                        DeviceFinder.Instance.StartSearch();
                        endRead = true;
                        return;
                    }
                    else
                    {

                    }
                    endRead = true;
                    //DeviceReaderModel.Instance.StageString = DeviceReaderModel.Instance.Bundle.GetString("device.read.error");
                    //Console.Error.WriteLine(DeviceReaderModel.Instance.ErrorString);
                    System.Threading.Thread.Sleep(4000);
                    DeviceFinder.Instance.StartSearch();
                    endRead = true;
                }
                catch (Exception e)
                {
                    //Console.Error.WriteLine(e);
                    //ReadError();
                }
            }
        }

        private void ReadError()
        {
            this.error = true;
            //this.stageString = this.bundle.GetString("device.read.error");
            //Console.Error.WriteLine(this.errorString);
            try
            {
                Thread.Sleep(4000);
            }
            catch (Exception)
            {
                // empty catch block
            }
            DeviceFinder.Instance.StartSearch();
            this.endRead = true;
        }
    }
}
