using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using CryptoUSB.Models;


namespace CryptoUSB.Controllers
{
    public class LanguageController
    {
        private CultureInfo appLocale;
        private ResourceManager appLanguageBundle;
        public static readonly LanguageController INSTANCE = new();
        private string[] localArray;

        private LanguageController()
        {
            SetLocale();
            SetResourceBundle();
        }

        public void Reload()
        {
            SetLocale();
            SetResourceBundle();
        }

        public LanguageController(CultureInfo appLocale, ResourceManager appLanguageBundle, string[] localArray)
        {
            this.appLocale = appLocale;
            this.appLanguageBundle = appLanguageBundle;
            this.localArray = localArray;
        }

        public void SetLocale()
        {
            this.appLocale = SettingsModel.INSTANCE.AppLanguage switch
            {
                "ru" => new CultureInfo("ru-RU"),
                "en" => new CultureInfo("en-GB"),
                _ => new CultureInfo("en-GB"),
            };
            this.localArray = new string[2];
            this.localArray[0] = "English";
            this.localArray[1] = "";
        }

        public void SetResourceBundle()
        {
            this.appLanguageBundle = new ResourceManager("languages.text", typeof(LanguageController).Assembly);
        }

        public CultureInfo GetAppLocale()
        {
            return this.appLocale;
        }

        public string GetCurrentLocalName()
        {
            if ("ru".Equals(this.appLocale.TwoLetterISOLanguageName))
                return this.localArray[1];
            if ("en".Equals(this.appLocale.TwoLetterISOLanguageName))
                return this.localArray[0];
            return this.localArray[0];
        }

        public string[] GetLocaleArray()
        {
            return this.localArray;
        }

        public ResourceManager GetAppLanguageBundle()
        {
            return this.appLanguageBundle;
        }
    }
}
