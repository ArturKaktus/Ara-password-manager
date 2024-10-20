using APM.Core;
using APM.Core.Models;
using APM.Core.ProviderInterfaces;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            // Фильтруем типы, чтобы найти те, которые реализуют интерфейс IContextMenu
            var contextMenuTypes = types.Where(t => t.GetInterfaces().Contains(typeof(IContextMenu)) && t.IsClass);

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

            foreach (var t in contextMenuTypes)
            {
                var classInstance = Activator.CreateInstance(t);

                if (classInstance is not IContextMenu ifp) continue;
                ContextMenuList.Add(ifp);
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
        public static List<IContextMenu> ContextMenuList { get; set; } = new();
    }
}
