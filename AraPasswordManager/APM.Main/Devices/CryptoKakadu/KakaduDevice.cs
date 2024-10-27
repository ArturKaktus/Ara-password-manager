using APM.Main.Devices.CryptoKakadu.Controls.OpenPinCode;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace APM.Main.Devices.CryptoKakadu
{
    public class KakaduDevice : IDevice
    {
        public KakaduCommander Commander { get; } = new();
        public bool PingDevice(string port)
        {
            try
            {
                Commander.SetPort(port);
                return Commander.SendWAY();
            }
            catch
            {
                return false;
            }
        }

        public async void ReadDevice()
        {
            Window owner = ((ClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow;
            bool isEntered = false;
            var passwordWindow = new Window
            {
                Title = "Введите пин код",
                Height = 200,
                Width = 300
            };
            var devicePinCodeWindow = new OpenPinCodeView(new OpenPinCodeViewModel());
            passwordWindow.Content = devicePinCodeWindow;
            devicePinCodeWindow.AcceptButtonClicked += (sender, e) =>
            {
                isEntered = true;
                passwordWindow.Close();
            };
            passwordWindow.Closed += (sender, e) =>
            {
                if (isEntered)
                {
                    var dataContext = devicePinCodeWindow.DataContext as OpenPinCodeViewModel;
                    string pinCode = dataContext.PinCode;
                    double percent = 0.0;
                    KakaduDeviceSerialPort SerialPort = new KakaduDeviceSerialPort(DeviceFinder.Instance.PortOfSelectedDevice);
                    DeviceFinder.Instance.StopSearch();
                    Commander.SetPort(DeviceFinder.Instance.PortOfSelectedDevice);
                    System.Threading.Thread.Sleep(3000);
                    bool contin = Commander.ExecuteCommand("cNUM");
                    System.Threading.Thread.Sleep(1000);
                    try
                    {
                        if (contin)
                        {
                            int numRowsToRead = Commander.GetDeviceRows();
                            contin = Commander.ExecuteCommand("cGET");
                            if (contin)
                            {
                                SerialPort.OpenPort();
                                try
                                {
                                    byte[] pinBytes = System.Text.Encoding.ASCII.GetBytes(pinCode);
                                    SerialPort.Write(pinBytes, 0, pinBytes.Length);
                                }
                                catch (Exception)
                                {
                                    // empty catch block
                                }
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
                                        try
                                        {
                                            Thread.Sleep(4000);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                        DeviceFinder.Instance.StartSearch();

                                        break;
                                    }
                                    if (0 == SerialPort.BytesToRead)
                                    {
                                        ++errorCount;
                                    }
                                    byte[] b = new byte[SerialPort.BytesToRead];
                                    SerialPort.Read(b, 0, b.Length);
                                    bytesCount += b.Length;
                                    Array.Copy(b, 0, buffer, bytesCount - b.Length, b.Length);
                                    bytes += Convert.ToDouble(bytesCount);
                                    percent = bytes / bufferLength * 100.0;
                                }
                                if (errorCount != 10)
                                {
                                    SerialPort.Close();
                                    byte[,] resultArray = new byte[numRowsToRead, 196];
                                    for (int i = 0; i < numRowsToRead; ++i)
                                    {
                                        for (int j = 0; j < 196; ++j)
                                        {
                                            resultArray[i, j] = buffer[j + 196 * i];
                                        }
                                    }
                                    AppDocument.CurrentDatabaseModel.FillFromDevice(resultArray);
                                    //AppDocument.CurrentDatabaseModel.HashDatabase();
                                    //AppDocument.CurrentDatabaseModel.BuildTree();
                                    System.Threading.Thread.Sleep(4000);
                                    DeviceFinder.Instance.StartSearch();
                                }
                                return;
                            }
                            System.Threading.Thread.Sleep(4000);
                            DeviceFinder.Instance.StartSearch();

                            return;
                        }
                        System.Threading.Thread.Sleep(4000);
                        DeviceFinder.Instance.StartSearch();
                    }
                    catch (Exception ex) { }
                }
            };
            await passwordWindow.ShowDialog(owner);
        }

        public void SaveDevice()
        {
            throw new NotImplementedException();
        }
    }

    public class KakaduCommander
    {
        private string _port;
        private string _charset = "ASCII";
        private string _kakaduVersion = string.Empty;
        private string _deviceName = string.Empty;
        private int _firmwareVersion = 0;
        private int _deviceRows = 0;
        private KakaduDeviceSerialPort _serialPort;
        public int GetDeviceRows()
        {
            return this._deviceRows;
        }
        public void SetPort(string port)
        {
            _port = port;
        }
        public bool SendWAY()
        {
            byte[] answer = SendAndReceive(CommandToBytes("cWAY"));
            string answerString = BytesToString(answer);
            if (answerString.Length > 0 && answerString.Contains("PhiPass"))
            {
                FillDeviceInfo(answerString);
                return true;
            }
            return false;
        }
        private void FillDeviceInfo(string rawInfo)
        {
            string[] infoStrings = rawInfo.Split('|');
            if (infoStrings.Length > 1)
            {
                _deviceName = infoStrings[0];
                _kakaduVersion = infoStrings[1];
                string versionOnly = new string(infoStrings[1].Where(char.IsDigit).ToArray());
                if (int.TryParse(versionOnly, out int firmwareVersion))
                {
                    _firmwareVersion = firmwareVersion;
                }
                else
                {
                    _firmwareVersion = 0;
                }
            }
            else
            {
                _deviceName = "Kakadu";
                _kakaduVersion = " Legacy";
                _firmwareVersion = 100;
            }
        }
        private string BytesToString(byte[] answerBytes)
        {
            string ansString = string.Empty;
            try
            {
                ansString = Encoding.GetEncoding(this._charset).GetString(answerBytes);
            }
            catch (Exception e)
            {
            }
            return ansString;
        }
        public byte[] SendAndReceive(byte[] sendBytes)
        {
            this._serialPort = new KakaduDeviceSerialPort(_port);
            this._serialPort.OpenPort();
            this._serialPort.Write(sendBytes, 0, sendBytes.Length);
            Thread.Sleep(150);
            byte[] receivedBytes = new byte[this._serialPort.BytesToRead];
            this._serialPort.Read(receivedBytes, 0, receivedBytes.Length);
            this._serialPort.ClosePort();
            if (receivedBytes.Length == 0)
                throw new Exception("0 bytes in serial port");
            return receivedBytes;
        }
        private byte[] CommandToBytes(string command)
        {
            byte[] commandArray = null;
            try
            {
                commandArray = Encoding.GetEncoding(this._charset).GetBytes(command);
            }
            catch (Exception e)
            {
            }
            return commandArray;
        }
        public bool ExecuteCommand(string command)
        {
            bool success = false;
            switch (command)
            {
                case "cCHK":
                    success = cCHK(CommandToBytes(command));
                    return success;
                case "cSET":
                    success = cSET(CommandToBytes(command));
                    return success;
                case "cGET":
                    success = cSET(CommandToBytes(command));
                    return success;
                case "cNUM":
                    success = cNUM(CommandToBytes(command));
                    return success;
                case "cUFW":
                    success = cUFW(CommandToBytes(command));
                    return success;
            }
            success = false;
            return success;
        }
        private bool cCHK(byte[] commandArray)
        {
            byte[] answer = WriteBytesWithAnswerDelay(commandArray, 3);
            string answerString = BytesToString(answer);
            if (answerString.Equals("PIN"))
                return true;
            return false;
        }
        private bool cSET(byte[] commandArray)
        {
            for (int i = 0; i < 4; i++)
            {
                byte[] answer = WriteBytesWithAnswerDelay(commandArray, 3);
                string answerString = BytesToString(answer);
                if (answerString.Equals("PIN"))
                {
                    return true;
                }
            }
            return false;
        }
        private bool cNUM(byte[] commandArray)
        {
            try
            {
                byte[] answer = SendAndReceiveWithWait(commandArray, 1);
                if (answer.Length == 2)
                {
                    int intAnswer = ToDecStrFromBytes(answer[0], answer[1]);
                    if (intAnswer > 0)
                    {
                        this._deviceRows = intAnswer;
                        return true;
                    }
                    this._deviceRows = 0;
                    return false;
                }
                return false;
            }
            catch (Exception exception)
            {
                return false;
            }
        }
        private bool cUFW(byte[] commandArray)
        {
            byte[] answer = WriteBytesWithAnswerDelay(commandArray, 3);
            string answerString = BytesToString(answer);
            if (answerString.Equals("OK"))
                return true;
            return true;
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
        }
        public int ToDecStrFromBytes(byte b1, byte b2)
        {
            byte[] bytes = new byte[] { b1, b2 };
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            short shortVal = BitConverter.ToInt16(bytes, 0);
            int x = shortVal;
            return x;
        }
    }
}
