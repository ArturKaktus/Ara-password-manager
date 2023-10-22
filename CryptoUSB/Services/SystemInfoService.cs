using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CryptoUSB.Services
{
    public class SystemInfoService
    {
        private string _osName;
        private string _osVersion;
        private string _osLanguage;
        private string _osEncoding;
        private string _userHome;
        public static readonly SystemInfoService INSTANCE = new SystemInfoService();
        public SystemInfoService()
        {
            this._osName = System.Environment.OSVersion.VersionString;
            this._osVersion = System.Environment.OSVersion.Version.ToString();
            this._osLanguage = System.Globalization.CultureInfo.CurrentUICulture.Name;
            this._osEncoding = System.Text.Encoding.Default.WebName;
            this._userHome = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
        }
        public string OsName { get => this._osName; }
        public string OsVersion { get => this._osVersion; }
        public string OsLanguage { get => this._osLanguage; }
        public string OsEncoding { get => this._osEncoding; }
        public string UserHome { get => this._userHome; }

        public bool IsWindows() => OsName.Contains("Windows");
        public bool IsMac() => OsName.Contains("Mac");
        public bool IsLinux() => OsName.Contains("Linux");
    }
}
