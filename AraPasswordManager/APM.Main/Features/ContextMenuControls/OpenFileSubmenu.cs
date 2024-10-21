using APM.Core;
using APM.Core.ProviderInterfaces;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace APM.Main.Features.ContextMenuControls
{
    internal class OpenFileSubmenu : IContextMenu
    {
        public ICommand? Execute => new DelegateCommand(ExecAsync);

        public int Order => 20;

        public bool CanExecute(object parameter)
        {
            return parameter is MenuItem mi && mi.Name == "File";
        }

        public void Exec(object parameter)
        {
            throw new NotImplementedException();
        }

        public async Task ExecAsync(object parameter)
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

        public bool IsEnabledMenu(object parameter) => true;

        public MenuItem ReturnMenuItem(object? mainObj, object? obj)
        {
            return new MenuItem()
            {
                Header = "Открыть файл",
                Command = Execute
            };
        }
    }
}
