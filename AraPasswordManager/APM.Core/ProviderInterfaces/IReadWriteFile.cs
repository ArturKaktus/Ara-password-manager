using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace APM.Core.ProviderInterfaces
{
    public interface IReadWriteFile
    {
        /// <summary>
        /// Чтение файла
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="file"></param>
        public void ReadFile(Window? owner, IStorageFile file);

        /// <summary>
        /// Сохранение файла
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="file"></param>
        public void SaveFile(Window? owner, IStorageFile file);
    }
}
