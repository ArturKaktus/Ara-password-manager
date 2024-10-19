using APM.Core.ProviderInterfaces;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using APM.Core;
using APM.Core.Models;

namespace APM.Main
{
    public static class AppDocument
    {
        static AppDocument()
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Находим все типы в сборке
            var types = assembly.GetTypes();

            // Фильтруем типы, чтобы найти те, которые реализуют интерфейс IFileProperty
            var implementingTypes = types.Where(t => t.GetInterfaces().Contains(typeof(IFileProperty)) && t.IsClass);
            foreach (var t in implementingTypes)
            {
                var classInstance = Activator.CreateInstance(t);

                if (classInstance is not IFileProperty ifp) continue;
                FileInstances.Add(ifp);
                FilePickerFileTypeFilter.Add(new FilePickerFileType(ifp.FileTitle)
                {
                    Patterns = ifp.FileExtension
                });
            }
        }

        /// <summary>
        /// Поддерживаемые расширения файлов
        /// </summary>
        public static readonly List<FilePickerFileType> FilePickerFileTypeFilter = [];

        /// <summary>
        /// Лист всех поддерживаемых типов файлов
        /// </summary>
        public static readonly List<IFileProperty> FileInstances = [];

        /// <summary>
        /// Текущий используемый тип файла
        /// </summary>
        public static object? CurrentFileInstance { get; set; }

        /// <summary>
        /// Текущая библиотека папкой и паролей
        /// </summary>
        public static DatabaseModel CurrentDatabaseModel { get; } = new();
        public static TreeNodeTransfer NodeTransfer { get; } = new();
    }
}
