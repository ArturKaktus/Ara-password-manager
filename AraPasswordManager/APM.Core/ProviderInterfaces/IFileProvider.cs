using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace APM.Core.ProviderInterfaces
{
    public interface IFileProvider
    {
        /// <summary>
        /// Название файла
        /// </summary>
        public string? FileTitle { get; }

        /// <summary>
        /// Расширения для файла
        /// </summary>
        public List<string> FileExtension { get; }

        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public char[] Password { get; set; }

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

        /// <summary>
        /// Уведомление об изменении
        /// </summary>
        public bool HasChange { get; set; }
    }
}
