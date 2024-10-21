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
    internal class SaveFileSubmenu : IContextMenu
    {
        public ICommand? Execute => new DelegateCommand(ExecAsync);

        public int Order => 30;

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

        public bool IsEnabledMenu(object parameter) => true;

        public MenuItem ReturnMenuItem(object? mainObj, object? obj)
        {
            return new MenuItem()
            {
                Header = "Сохранить файл",
                Command = Execute
            };
        }
    }
}
