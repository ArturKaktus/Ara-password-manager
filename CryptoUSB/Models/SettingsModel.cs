using CryptoUSB.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Models
{
    public class SettingsModel
    {
        private readonly string settingsWinDir = "\\Documents\\Crypto Kakadu\\config\\";
        private readonly string firmwareWinDir = "\\Documents\\Crypto Kakadu\\update\\";
        private readonly string settingsFile = "config.properties";

        private readonly Dictionary<string, string> defaultProperties = new();
        private readonly Dictionary<string, string> userProperties = new();

        private string appLanguage = string.Empty;
        private bool appFirstStart;
        private bool appShowFirstModal;
        private bool appShowAlerts;

        public static readonly SettingsModel Instance = new();

        private SettingsModel()
        {
            SetDefaultSettings();
            CreateDirectory();
            CreateSettingsFile();
            ReadSettingsFile();
            LoadSettings();
            //ClearUploadDir();
        }

        public string FirmwareWinDir
        {
            get { return SystemInfoService.INSTANCE.UserHome + firmwareWinDir; }
        }

        public void SetAppFirstStart(bool appFirstStart)
        {
            this.appFirstStart = appFirstStart;
            SaveSettings();
        }

        public static string Version
        {
            get { return "2.3.1"; }
        }

        public static int IntVersion
        {
            get
            {
                string tempVer = "2.3.1".Replace(",", "").Replace(".", "");
                return Convert.ToInt32(tempVer);
            }
        }

        public string AppLanguage
        {
            get { return this.appLanguage; }
        }

        public bool AppShowFirstModal
        {
            get { return this.appShowFirstModal; }
        }

        public bool AppShowAlerts
        {
            get { return this.appShowAlerts; }
        }

        public void SetAppLanguage(string appLanguage)
        {
            this.appLanguage = appLanguage;
        }

        public void SetAppShowAlerts(bool appShowAlerts)
        {
            this.appShowAlerts = appShowAlerts;
        }

        public void SetAppShowFirstModal(bool appShowFirstModal)
        {
            this.appShowFirstModal = appShowFirstModal;
        }

        public bool IsFirstStart()
        {
            return this.appFirstStart;
        }

        private void CreateDirectory()
        {
            if (SystemInfoService.INSTANCE.IsWindows())
            {
                string dirPath = SystemInfoService.INSTANCE.UserHome + settingsWinDir;
                Directory.CreateDirectory(dirPath);

                dirPath = SystemInfoService.INSTANCE.UserHome + firmwareWinDir;
                Directory.CreateDirectory(dirPath);
            }
        }

        private void CreateSettingsFile()
        {
            if (SystemInfoService.INSTANCE.IsWindows())
            {
                string filePath = SystemInfoService.INSTANCE.UserHome + settingsWinDir + settingsFile;
                if (!File.Exists(filePath))
                {
                    try
                    {
                        File.Create(filePath).Close();

                        //using (FileStream settingsStream = new FileStream(filePath, FileMode.OpenOrCreate))
                        //{
                        //    defaultProperties.Store(settingsStream, "Crypto Kakadu default config");
                        //}
                        using var settingsStream = new StreamWriter(filePath);
                        foreach (var kvp in defaultProperties)
                        {
                            settingsStream.WriteLine($"{kvp.Key}={kvp.Value}");
                        }
                        settingsStream.Flush();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        private void SetDefaultSettings()
        {
            defaultProperties.Add("appLanguage", SystemInfoService.INSTANCE.OsLanguage);
            defaultProperties.Add("appFirstStart", "true");
            defaultProperties.Add("appShowFirstModal", "true");
            defaultProperties.Add("appShowAlerts", "true");
        }

        private void ReadSettingsFile()
        {
            try
            {
                string filePath = SystemInfoService.INSTANCE.UserHome + settingsWinDir + settingsFile;
                //using (FileStream settingsStream = new FileStream(filePath, FileMode.Open))
                //{
                //    //userProperties.Load(settingsStream);
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void LoadSettings()
        {
            //appFirstStart = bool.Parse(userProperties.GetProperty("appFirstStart", "true"));
            //appLanguage = userProperties.GetProperty("appLanguage", SystemInfoService.Instance.OsLanguage);
            //appShowFirstModal = bool.Parse(userProperties.GetProperty("appShowFirstModal", "true"));
            //appShowAlerts = bool.Parse(userProperties.GetProperty("appShowAlerts", "true"));
        }
        public void SaveSettings()
        {
            userProperties.Add("appFirstStart", appFirstStart.ToString());
            userProperties.Add("appLanguage", appLanguage);
            userProperties.Add("appShowFirstModal", appFirstStart.ToString());
            userProperties.Add("appShowAlerts", appShowAlerts.ToString());
            try
            {
                string filePath = SystemInfoService.INSTANCE.UserHome + settingsWinDir + settingsFile;
                //using (FileStream settingsStream = new FileStream(filePath, FileMode.OpenOrCreate))
                //{
                //    userProperties.Store(settingsStream, "Crypto Kakadu config");
                //}
                using var settingsStream = new StreamWriter(filePath);
                foreach (var kvp in userProperties)
                {
                    settingsStream.WriteLine($"{kvp.Key}={kvp.Value}");
                }
                settingsStream.Flush();
            }
            catch { }
        }
        public void ClearUploadDir()
        {
            string dirPath = Path.Combine(SystemInfoService.INSTANCE.UserHome, firmwareWinDir);
            DirectoryInfo dir = new(dirPath);
            foreach (FileInfo file in dir.GetFiles())
            {
                if (!file.Directory.Exists)
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }
    }
}
