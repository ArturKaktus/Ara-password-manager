using APM.Desktop.Devices.CryptoKakadu.Controls.OpenPinCode;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using APM.Core.Models;
using APM.Desktop.Devices.CryptoKakadu.Controls.SavePinCode;

namespace APM.Desktop.Devices.CryptoKakadu
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
                    DeviceFinder.Instance.StopSearch();
                    var dataContext = devicePinCodeWindow.DataContext as OpenPinCodeViewModel;
                    string pinCode = dataContext.PinCode;
                    double percent = 0.0;
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
                                AppDocument.SelectedDeviceSerialPort.OpenPort();
                                try
                                {
                                    byte[] pinBytes = System.Text.Encoding.ASCII.GetBytes(pinCode);
                                    AppDocument.SelectedDeviceSerialPort.WriteData(pinBytes);
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
                                        AppDocument.SelectedDeviceSerialPort.ClosePort();
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
                                    if (0 == AppDocument.SelectedDeviceSerialPort.BytesToRead)
                                    {
                                        ++errorCount;
                                    }
                                    byte[] b = new byte[AppDocument.SelectedDeviceSerialPort.BytesToRead];
                                    AppDocument.SelectedDeviceSerialPort.ReadData(b);
                                    bytesCount += b.Length;
                                    Array.Copy(b, 0, buffer, bytesCount - b.Length, b.Length);
                                    bytes += Convert.ToDouble(bytesCount);
                                    percent = bytes / bufferLength * 100.0;
                                }
                                if (errorCount != 10)
                                {
                                    AppDocument.SelectedDeviceSerialPort.ClosePort();
                                    byte[,] resultArray = new byte[numRowsToRead, 196];
                                    for (int i = 0; i < numRowsToRead; ++i)
                                    {
                                        for (int j = 0; j < 196; ++j)
                                        {
                                            resultArray[i, j] = buffer[j + 196 * i];
                                        }
                                    }
                                    AppDocument.CurrentDatabaseModel.FillFromDevice(resultArray);
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
                    catch (Exception ex) {}
                }
            };
            await passwordWindow.ShowDialog(owner);
        }

        public async void SaveDevice()
        {
            Window owner = ((ClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow;
            bool isEntered = false;
            var passwordWindow = new Window
            {
                Title = "Введите пин код",
                Height = 200,
                Width = 300
            };
            var devicePinCodeWindow = new SavePinCodeView(new SavePinCodeViewModel()); 
            passwordWindow.Content = devicePinCodeWindow;
            devicePinCodeWindow.AcceptButtonClicked += (sender, e) =>
            {
                isEntered = true;
                passwordWindow.Close();
            };

            passwordWindow.Closed += async (sender, e) =>
            {
                if (isEntered)
                {
                    DeviceFinder.Instance.StopSearch();
                    bool error = false;
                    bool endWrite = false;
                    int errorCounter = 0;
                    var dataContext = devicePinCodeWindow.DataContext as SavePinCodeViewModel;
                    string pinCode = dataContext.PinCode;
                    double percent = 0.0;
                    Commander.SetPort(DeviceFinder.Instance.PortOfSelectedDevice);
                    System.Threading.Thread.Sleep(3000);
                    bool contin = Commander.SendSET();
                    if (contin)
                    {
                        //Между Set и Pin должна быть пауза
                        Thread.Sleep(1000);
                        contin = Commander.SendPIN(pinCode);
                        if (contin)
                        {
                            contin = Commander.SendRowCount();
                            if (contin)
                            {
                                byte[] buffer = new byte[1];
                                byte[,] rows = AppDocument.CurrentDatabaseModel.GetDeveiceArray();
                                AppDocument.SelectedDeviceSerialPort.OpenPort();
                                int count = AppDocument.CurrentDatabaseModel.GetRowCount() - 1;
                                double oneRowPercent = Convert.ToDouble(count);
                                int i = 1;
                                var c = rows.GetLength(0);
                                for (int ii = 0; i <= c; ii++)
                                {
                                    byte[] bs = rows.GetRow(ii);
                                    if (error) continue;
                                    percent = Convert.ToDouble(i) / oneRowPercent * 100.0;
                                    bool waitAnswer = true;
                                    byte[] arrby = bs;
                                    int n = arrby.Length;
                                    for (int j = 0; j < n; ++j)
                                    {
                                        buffer[0] = arrby[j];

                                        AppDocument.SelectedDeviceSerialPort.WriteData(buffer);
                                    }

                                    errorCounter = 0;
                                     while (waitAnswer) 
                                     {
                                         Thread.Sleep(200);
                                        if (errorCounter == 200)
                                        {
                                            error = true;
                                            AppDocument.SelectedDeviceSerialPort.ClosePort();
                                            Thread.Sleep(3000);
                                            endWrite = true;
                                            DeviceFinder.Instance.StartSearch();
                                            break;
                                        }
                                        if (AppDocument.SelectedDeviceSerialPort.BytesToRead > 0)
                                        {
                                            waitAnswer = false;
                                            continue;
                                        }
                                        errorCounter++;
                                    }
                                    i++;
                                }
                                AppDocument.SelectedDeviceSerialPort.ClosePort();
                                endWrite = true;
                                DeviceFinder.Instance.StartSearch();
                            }
                            else
                            {
                                error = true;
                                AppDocument.SelectedDeviceSerialPort.ClosePort();
                                Thread.Sleep(3000);
                                endWrite = true;
                                DeviceFinder.Instance.StartSearch();
                            }
                        }
                        else
                        {
                            error = true;
                            AppDocument.SelectedDeviceSerialPort.ClosePort();
                            Thread.Sleep(3000);
                            endWrite = true;
                            DeviceFinder.Instance.StartSearch();
                        } 
                    }
                    else
                    {
                        error = true;
                        AppDocument.SelectedDeviceSerialPort.ClosePort();
                        Thread.Sleep(3000);
                        endWrite = true;
                        DeviceFinder.Instance.StartSearch();
                    }
                }
            };
            
            await passwordWindow.ShowDialog(owner);
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
        private int _errorCount = 15;
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
        public bool SendSET()
        {
            try
            {
                byte[] answer = SendAndReceive(CommandToBytes("cSET"));
                string answerString = BytesToString(answer);
                if (answerString.Equals("PIN"))
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
        public bool SendPIN(string pin)
        {
            try
            {
                byte[] answer = SendAndReceiveWithWait(CommandToBytes(pin), 50);
                string answerString = BytesToString(answer);
                if (answerString.Equals("OK"))
                {
                    return true;
                }
                this._errorCount = 15 - int.Parse(answerString);
                if (this._errorCount < 0)
                    this._errorCount = 0;
                return false;
            }
            catch
            {
                return false;
            }
        }
        public bool SendRowCount()
        {
            try
            {
                byte[] count = AppDocument.CurrentDatabaseModel.GetRowCountByte();
                byte?[] answer = new byte?[2];
                answer[0] = SendAndReceiveWithWait(count[0], 50);
                if (answer[0] == null || answer[0] != count[0])
                    throw new Exception("sendRowCount error with first byte");
                answer[1] = SendAndReceiveWithWait(count[1], 50);
                if (answer[1] == null || answer[1] != count[1])
                    throw new Exception("sendRowCount error with second byte");
                return true;
            }
            catch
            {
                return false;
            }
        }

        private byte? SendAndReceiveWithWait(byte sendByte, int attempts)
        {
            int errorCount = 0;
            byte[] sendBytes = [sendByte];
            AppDocument.SelectedDeviceSerialPort.OpenPort();
            AppDocument.SelectedDeviceSerialPort.WriteData(sendBytes);
            for (errorCount = 0; errorCount < attempts; ++errorCount)
            {
                Thread.Sleep(100);
                if (AppDocument.SelectedDeviceSerialPort.BytesToRead > 0)
                {
                    byte[] receivedBytes = new byte[AppDocument.SelectedDeviceSerialPort.BytesToRead];
                    AppDocument.SelectedDeviceSerialPort.ReadData(receivedBytes);
                    AppDocument.SelectedDeviceSerialPort.ClosePort();
                    return receivedBytes[0];
                }
            }
            AppDocument.SelectedDeviceSerialPort.ClosePort();
            return null;
        }
        private void FillDeviceInfo(string rawInfo)
        {
            string[] infoStrings = rawInfo.Split('|');
            if (infoStrings.Length > 1)
            {
                _deviceName = infoStrings[0];
                _kakaduVersion = infoStrings[1];
                var versionOnly = new string(infoStrings[1].Where(char.IsDigit).ToArray());
                _firmwareVersion = int.TryParse(versionOnly, out var firmwareVersion) ? firmwareVersion : 0;
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
            catch
            {
            }
            return ansString;
        }

        private byte[] SendAndReceive(byte[] sendBytes)
        {
            AppDocument.SelectedDeviceSerialPort ??= new KakaduDeviceSerialPort(this._port);
            AppDocument.SelectedDeviceSerialPort.OpenPort();
            AppDocument.SelectedDeviceSerialPort.WriteData(sendBytes);
            byte[] receivedBytes = new byte[AppDocument.SelectedDeviceSerialPort.BytesToRead];
            AppDocument.SelectedDeviceSerialPort.ReadData(receivedBytes);
            AppDocument.SelectedDeviceSerialPort.ClosePort();
            if (receivedBytes.Length == 0)
                throw new Exception("0 bytes in serial port");
            return receivedBytes;
        }
        private byte[] CommandToBytes(string command)
        {
            byte[]? commandArray = null;
            try
            {
                commandArray = Encoding.GetEncoding(this._charset).GetBytes(command);
            }
            catch
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
            catch
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
            //this._serialPort = new KakaduDeviceSerialPort(this._port);
            try
            {
                AppDocument.SelectedDeviceSerialPort.OpenPort();
                AppDocument.SelectedDeviceSerialPort.WriteData(bytesToWrite);
                while (wait)
                {
                    if (counter == 10)
                    {
                        AppDocument.SelectedDeviceSerialPort.ClosePort();
                        return error;
                    }
                    if (AppDocument.SelectedDeviceSerialPort.BytesToRead > 0)
                    {
                        wait = false;
                        int bytesToRead = AppDocument.SelectedDeviceSerialPort.BytesToRead;
                        byte[] recievedBytes = new byte[bytesToRead];
                        AppDocument.SelectedDeviceSerialPort.ReadData(recievedBytes);
                        AppDocument.SelectedDeviceSerialPort.ClosePort();
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
            AppDocument.SelectedDeviceSerialPort.OpenPort();
            AppDocument.SelectedDeviceSerialPort.WriteData(sendBytes);
            while (errorCount <= attempts)
            {
                Thread.Sleep(100);
                if (AppDocument.SelectedDeviceSerialPort.BytesToRead > 0)
                {
                    byte[] receivedBytes = new byte[AppDocument.SelectedDeviceSerialPort.BytesToRead];
                    AppDocument.SelectedDeviceSerialPort.ReadData(receivedBytes);
                    AppDocument.SelectedDeviceSerialPort.ClosePort();
                    return receivedBytes;
                }
                errorCount++;
            }
            AppDocument.SelectedDeviceSerialPort.ClosePort();
            return null;
        }
        public int ToDecStrFromBytes(byte b1, byte b2)
        {
            byte[] bytes = [b1, b2];
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
