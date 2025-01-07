using System;
using APM.Core;
using APM.Core.Models;
using APM.Core.ProviderInterfaces;
using APM.Main.Devices;
using APM.Main.Utils;
using Avalonia.Platform.Storage;
using System.Collections.Generic;
using System.IO.Ports;
using System.Reflection;
using APM.Core.Enums;
using APM.Main.Devices.CryptoKakadu;

namespace APM.Main
{
    public static class AppDocument
    {
        static AppDocument()
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Находим все типы в сборке
            var types = assembly.GetTypes();

            var implementingTypes = GetClassesUtils.GenerateClassList<IFileProperty>(types);
            foreach (var t in implementingTypes)
            {
                FileInstances.Add(t);
                FilePickerFileTypeFilter.Add(new FilePickerFileType(t.FileTitle)
                {
                    Patterns = t.FileExtension
                });
            }

            ContextMenuList = GetClassesUtils.GenerateClassList<IContextMenu>(types);
            DeviceInstances = GetClassesUtils.GenerateClassList<IDevice>(types);
        }

        public static readonly List<IDevice> DeviceInstances;
        public static KakaduDeviceSerialPort SelectedDeviceSerialPort;
        
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

        /// <summary>
        /// Текущая операционная система
        /// </summary>
        public static NameOS CurrentNameOS { get; set; } = NameOS.NONE;
        public static TreeNodeTransfer NodeTransfer { get; } = new();
        public static List<IContextMenu> ContextMenuList { get; set; } = new();

        public static bool ClearDocument()
        {
            try
            {
                NodeTransfer.SelectedTreeNode = null;
                CurrentFileInstance = null;
                CurrentDatabaseModel.Clear();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
