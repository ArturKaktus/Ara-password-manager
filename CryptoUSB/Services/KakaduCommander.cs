using CryptoUSB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CryptoUSB.Services
{
    public class KakaduCommander
    {
        private string _port;
        private string _charset = "ASCII";
        private int _firmwareVersion = 0;
        private string _kakaduVersion = string.Empty;
        private string _deviceName = string.Empty;
        private int _answerInt;
        private int _deviceRows = 0;
        private string _answerString;
        private KakaduWriter _kakaduWriter = new KakaduWriter();
        private int _errorCount = 15;

        public void SetPort(string port)
        {
            this._port = port;
            this._kakaduWriter.SetPort(port);
        }
        public int GetFWVersion()
        {
            return this._firmwareVersion;
        }
        public string GetKakaduVersion()
        {
            return this._kakaduVersion;
        }
        public int GetErrorCount()
        {
            return this._errorCount;
        }
        public int GetDeviceRows()
        {
            return this._deviceRows;
        }
        public string GetDeviceName()
        {
            return this._deviceName;
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
        public bool SendWAY()
        {
            byte[] answer = this._kakaduWriter.SendAndReceive(CommandToBytes("cWAY"));
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
                byte[] answer = this._kakaduWriter.SendAndReceive(CommandToBytes("cSET"));
                string answerString = BytesToString(answer);
                if (answerString.Equals("PIN"))
                    return true;
            }
            catch (Exception exception) { }
            return false;
        }
        public bool SendCHK()
        {
            return true;
        }
        private bool cUFW(byte[] commandArray)
        {
            byte[] answer = this._kakaduWriter.WriteBytesWithAnswerDelay(commandArray, 3);
            string answerString = BytesToString(answer);
            if (answerString.Equals("OK"))
                return true;
            return true;
        }
        private bool cCHK(byte[] commandArray)
        {
            byte[] answer = this._kakaduWriter.WriteBytesWithAnswerDelay(commandArray, 3);
            string answerString = BytesToString(answer);
            if (answerString.Equals("PIN"))
                return true;
            return false;
        }
        private bool cSET(byte[] commandArray)
        {
            for (int i = 0; i < 4; i++)
            {
                byte[] answer = this._kakaduWriter.WriteBytesWithAnswerDelay(commandArray, 3);
                string answerString = BytesToString(answer);
                if (answerString.Equals("PIN"))
                {
                    return true;
                }
            }
            return false;
        }
        public bool SendPIN(string pin)
        {
            try
            {
                byte[] answer = this._kakaduWriter.SendAndReceiveWithWait(CommandToBytes(pin), 50);
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
            catch (Exception e)
            {
                return false;
            }
        }
        public void FlushPort()
        {
            this._kakaduWriter.FlushPort();
        }
        public bool SendRowCount()
        {
            try
            {
                byte[] count = DatabaseModel.Instance.GetRowCountByte();
                byte?[] answer = new byte?[2];
                answer[0] = _kakaduWriter.SendAndReceiveWithWait(count[0], 50);
                if (answer[0] == null || answer[0] != count[0])
                    throw new Exception("sendRowCount error with first byte");
                answer[1] = _kakaduWriter.SendAndReceiveWithWait(count[1], 50);
                if (answer[1] == null || answer[1] != count[1])
                    throw new Exception("sendRowCount error with second byte");
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        private bool cNUM(byte[] commandArray)
        {
            try
            {
                byte[] answer = this._kakaduWriter.SendAndReceiveWithWait(commandArray, 1);
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
        
        private void FillDeviceInfo(string rawInfo)
        {
            string[] infoStrings = rawInfo.Split('|');
            if (infoStrings.Length > 1)
            {
                this._deviceName = infoStrings[0];
                this._kakaduVersion = infoStrings[1];
                string versionOnly = new string(infoStrings[1].Where(char.IsDigit).ToArray());
                if (int.TryParse(versionOnly, out int firmwareVersion))
                {
                    this._firmwareVersion = firmwareVersion;
                }
                else
                {
                    this._firmwareVersion = 0; // Default value in case parsing fails
                }
            }
            else
            {
                this._deviceName = "Kakadu";
                this._kakaduVersion = " Legacy";
                this._firmwareVersion = 100;
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
                //System.err.println(e);
            }
            return ansString;
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
                Console.Error.WriteLine(e);
            }
            return commandArray;
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
