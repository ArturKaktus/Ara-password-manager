using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APM.Core.ProviderInterfaces;
using APM.Main.Features;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;

namespace APM.Main.Features.MenuStrip
{
    public class MenuStripViewModel : ViewModelBase
    {
        public async Task CreateNew_Clicked()
        {
            
        }

        public async Task OpenFile_Clicked()
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
        public async Task SaveFile_Clicked()
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
        public async Task Exit_Clicked()
        {

        }
    }
}
