/*  
 *  Автор: Миловидов Артур
 *  Время: 22.10.2023 18:23
 *  Статус: ОК - Класс переведен
 */

using CryptoUSB.CustomClasses;
using System.IO.Ports;
using System.Threading;

namespace CryptoUSB.Services
{
    public class KakaduWriter
    {
        //оригинал использует
        //private int _port;
        private string _port;
        private KakaduDeviceSerialPort _serialPort;

        public void SetPort(string port)
        {
            this._port = port;
        }

        public byte[] SendAndReceive(byte[] sendBytes)
        {
            this._serialPort = new KakaduDeviceSerialPort(this._port);
            this._serialPort.OpenPort();
            this._serialPort.Write(sendBytes, 0, sendBytes.Length);
            Thread.Sleep(150);
            byte[] receivedBytes = new byte[this._serialPort.BytesToRead];
            this._serialPort.Read(receivedBytes, 0, receivedBytes.Length);
            this._serialPort.ClosePort();
            //if (receivedBytes.Length == 0)
            //    throw new Exception("0 bytes in serial port");
            return receivedBytes;
        }
        public byte[] SendAndReceive(byte sendBytes)
        {
            this._serialPort = new KakaduDeviceSerialPort(this._port);
            byte[] single = new byte[1];
            single[0] = sendBytes;
            this._serialPort.OpenPort();
            this._serialPort.Write(single, 0, single.Length);
            Thread.Sleep(150);
            byte[] receivedBytes = new byte[this._serialPort.BytesToRead];
            this._serialPort.Read(receivedBytes, 0, receivedBytes.Length);
            this._serialPort.ClosePort();
            //if (receivedBytes.Length == 0)
            //    throw new Exception("0 bytes in serial port");
            return receivedBytes;
        }
        public byte? SendAndReceiveWithWait(byte sendByte, int attempts)
        {
            int errorCount = 0;
            byte[] sendBytes = new byte[] { sendByte };
            _serialPort = new KakaduDeviceSerialPort(this._port);
            this._serialPort.OpenPort();
            this._serialPort.Write(sendBytes, 0, sendBytes.Length);
            for (errorCount = 0; errorCount < attempts; ++errorCount)
            {
                Thread.Sleep(100);
                if (this._serialPort.BytesToRead > 0)
                {
                    byte[] receivedBytes = new byte[this._serialPort.BytesToRead];
                    this._serialPort.Read(receivedBytes, 0, receivedBytes.Length);
                    this._serialPort.Close();
                    return receivedBytes[0];
                }
            }
            this._serialPort.ClosePort();
            return null;
        }
        public byte[] SendAndReceiveWithWait(byte[] sendBytes, int attempts)
        {
            int errorCount = 0;
            this._serialPort = new KakaduDeviceSerialPort(this._port);
            this._serialPort.OpenPort();
            this._serialPort.Write(sendBytes, 0, sendBytes.Length);
            while (errorCount <= attempts)
            {
                Thread.Sleep(100);
                if (this._serialPort.BytesToRead > 0)
                {
                    byte[] receivedBytes = new byte[this._serialPort.BytesToRead];
                    this._serialPort.Read(receivedBytes, 0, receivedBytes.Length);
                    this._serialPort.Close();
                    return receivedBytes;
                }
                errorCount++;
            }
            this._serialPort.ClosePort();
            return null;
            //throw new Exception("attempts are over at" + errorCount);
        }
        public void WriteBytesWithNoAnswer(byte[] bytesToWrite)
        {
            this._serialPort = new KakaduDeviceSerialPort(this._port);
            this._serialPort.OpenPort();
            byte[] bs = new byte[this._serialPort.BytesToRead];
            this._serialPort.Read(bs, 0, bs.Length);
            this._serialPort.Write(bytesToWrite, 0, bytesToWrite.Length);
            this._serialPort.ClosePort();
        }
        public byte[] WriteBytesWithAnswerDelay(byte[] bytesToWrite, int bytesToread)
        {
            byte[] error = new byte[1];
            bool wait = true;
            int counter = 0;
            error[0] = 0;
            this._serialPort = new KakaduDeviceSerialPort(this._port);
            try
            {
                this._serialPort.OpenPort();
                this._serialPort.Write(bytesToWrite, 0, bytesToWrite.Length);
                while (wait)
                {
                    if (counter == 10)
                    {
                        this._serialPort.Close();
                        return error;
                    }
                    if (this._serialPort.BytesToRead > 0)
                    {
                        wait = false;
                        int bytesToRead = this._serialPort.BytesToRead;
                        byte[] recievedBytes = new byte[bytesToRead];
                        this._serialPort.Read(recievedBytes, 0, bytesToRead);
                        this._serialPort.ClosePort();
                        return recievedBytes;
                    }
                }
            }
            catch
            {
                return error;
            }
            return error;

        }
        public byte WriteByteWithAnswerDelay(byte byteToWrite, int bytesToread)
        {
            byte[] error = new byte[1];
            byte[] single = new byte[1];
            single[0] = byteToWrite;
            bool wait = true;
            error[0] = 0;
            int countError = 0;
            try
            {
                this._serialPort = new KakaduDeviceSerialPort(this._port);
                this._serialPort.OpenPort();
                this._serialPort.Write(single, 0, single.Length);
                while (wait)
                {
                    if (countError == 10)
                    {
                        this._serialPort.Close();
                        return error[0];
                    }
                    if (this._serialPort.BytesToRead > bytesToread)
                    {
                        wait = false;
                        int bytesToRead = this._serialPort.BytesToRead;
                        byte[] recievedBytes = new byte[bytesToRead];
                        this._serialPort.Read(recievedBytes, 0, bytesToRead);
                        this._serialPort.ClosePort();
                        return recievedBytes[0];
                    }
                    countError++;
                }
            }
            catch
            {
                this._serialPort.Close();
                return error[0];
            }
            return error[0];
        }
        public void FlushPort()
        {
            this._serialPort = new KakaduDeviceSerialPort(this._port);
            this._serialPort.OpenPort();
            this._serialPort.ClosePort();
        }
    }
}
