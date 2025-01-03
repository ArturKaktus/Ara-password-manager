using System;
using System.Text;
using APM.Core.Enums;
using APM.Main.Devices;
using APM.Main.Features.MainWindow;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;

namespace APM.Main
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Регистрация провайдера кодировок
            AvaloniaXamlLoader.Load(this);
            AppDocument.CurrentNameOS = GetOperatingSystem();
            LoadThemeResources();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators.RemoveAt(0);
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
                CastomModules();
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void CastomModules() => DeviceFinder.Instance.StartSearch();

        private void LoadThemeResources()
        {
            if (AppDocument.CurrentNameOS == NameOS.Windows)
            {
                var styles = new Styles
                {
                    new StyleInclude(new Uri("avares://APM.Main/Theme/Styles.Windows.axaml"))
                    {
                        Source = new Uri("avares://APM.Main/Theme/Styles.Windows.axaml")
                    }
                };
                this.Styles.Add(styles);

                ResourceInclude resource = new ResourceInclude(new Uri("avares://APM.Main/Theme/ResourceDictionary.Windows.axaml"))
                {
                    Source = new Uri("avares://APM.Main/Theme/ResourceDictionary.Windows.axaml")
                };
                
                this.Resources.MergedDictionaries.Add(resource);
            }
        }
        private NameOS GetOperatingSystem()
        {
            if (OperatingSystem.IsWindows())
            {
                return NameOS.Windows;
            }
            else if (OperatingSystem.IsLinux())
            {
                return NameOS.Linux;
            }
            else if (OperatingSystem.IsMacOS())
            {
                return NameOS.MacOS;
            }
            else
            {
                return NameOS.NONE;
            }
        }
    }
}