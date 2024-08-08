using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APM.Core.ProviderInterfaces;
using Ara_password_manager.Features;
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
                    AppDocument.ClassInstances.FirstOrDefault(obj =>
                        obj.FileExtension.Contains($"*.{namePaths.LastOrDefault()}"));
                AppDocument.CurrentInstance = instance;

                if (instance is IReadWriteFile irwf)
                {
                    irwf.ReadFile(owner, result[0]);
                }
            }
        }

        public async Task Exit_Clicked()
        {

        }
    }
}
