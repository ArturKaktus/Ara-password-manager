using APM.Core.ProviderInterfaces;
using APM.Main.Devices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace APM.Main.Features.MenuStrip
{
    public partial class MenuStripViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _IsConnectedDevice;

        public MenuStripViewModel()
        {
            DeviceFinder.Instance.PropertyChanged += FindDevice_Property;
        }

        private void FindDevice_Property(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "IsConnected" && sender is DeviceFinder fd)
            {
                IsConnectedDevice = fd.IsConnected;
            }
        }

        [RelayCommand]
        public void Create()
        {
            AppDocument.ClearDocument();
        }

        [RelayCommand]
        public async void Open()
        {
            Window? owner = ((ClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime!)?.MainWindow;
            if (owner == null)
                return;

            List<FilePickerFileType> fileTypes = AppDocument.FilePickerFileTypeFilter;

            var result = await owner.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Text File",
                AllowMultiple = false,
                FileTypeFilter = fileTypes
            });
            if (result != null && result.Count > 0)
            {
                var namePaths = ((IStorageFile)result[0]).Name.Split('.');
                var instance =
                    AppDocument.FileInstances.FirstOrDefault(obj =>
                        obj.FileExtension.Contains($"*.{namePaths.LastOrDefault()}"));
                AppDocument.CurrentFileInstance = instance;

                if (instance is IReadWriteFile irwf)
                {
                    irwf.ReadFile(owner, result[0]);
                }
            }
        }

        [RelayCommand]
        public async void Save()
        {
            Window? owner = ((ClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime!)?.MainWindow;
            if (owner == null)
                return;
            List<FilePickerFileType> fileTypes = AppDocument.FilePickerFileTypeFilter;
            var result = await owner.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
            {
                Title = "Save File",
                FileTypeChoices = AppDocument.FilePickerFileTypeFilter
            });
            if (result != null)
            {
                var namePaths = ((IStorageFile)result).Name.Split('.');
                var instance =
                    AppDocument.FileInstances.FirstOrDefault(obj =>
                        obj.FileExtension.Contains($"*.{namePaths.LastOrDefault()}"));
                if (instance is IReadWriteFile irwf)
                {
                    irwf.SaveFile(owner, result);
                }
            }
        }

        [RelayCommand]
        public void Exit()
        {
            var app = Application.Current;
            if (app?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }

        [RelayCommand]
        public void OpenFromDevice()
        {
            DeviceFinder.Instance.SelectedDevice.ReadDevice();
        }

        [RelayCommand]
        public void SaveToDevice()
        {
            DeviceFinder.Instance.SelectedDevice.SaveDevice();
        }
    }
}
