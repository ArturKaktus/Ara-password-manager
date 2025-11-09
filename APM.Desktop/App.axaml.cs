using System;
using System.Text;
using APM.Core.Enums;
using APM.Desktop.Devices;
using APM.Desktop.Features.MainWindow;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;

namespace APM.Desktop
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Регистрация провайдера кодировок
            AvaloniaXamlLoader.Load(this);
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

        private void CastomModules() => DeviceFinder.Instance.InitSearch();

        private void LoadThemeResources()
        {
            //if (OperatingSystem.IsWindows())
            //{
            //    var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            //    var baseUri = new Uri($"avares://{assemblyName}/Themes/");

            //    var styles = new Styles
            //    {
            //        new StyleInclude(new Uri(baseUri, "Styles.Windows.axaml"))
            //        {
            //            Source = new Uri(baseUri, "Styles.Windows.axaml")
            //        }
            //    };
            //    this.Styles.Add(styles);

            //    ResourceInclude resource = new ResourceInclude(new Uri(baseUri, "ResourceDictionary.Windows.axaml"))
            //    {
            //        Source = new Uri(baseUri, "ResourceDictionary.Windows.axaml")
            //    };

            //    this.Resources.MergedDictionaries.Add(resource);
            //}
        }
    }
}