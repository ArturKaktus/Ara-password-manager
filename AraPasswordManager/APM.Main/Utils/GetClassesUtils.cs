using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APM.Main.Utils
{
    internal static class GetClassesUtils
    {
        /// <summary>
        /// Сбор MenuItem
        /// </summary>
        /// <param name="mainObj"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<MenuItem> GenerateMenuItems(object? mainObj, object? obj)
        {
            return AppDocument.ContextMenuList
                .Where(item => item.CanExecute(obj))
                .OrderBy(i => i.Order)
                .Select(item => item.ReturnMenuItem(mainObj, obj))
                .Where(menuItem => menuItem != null)
                .ToList();
        }

        /// <summary>
        /// Сбор всех классов с определнным интерфейсом
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="types"></param>
        /// <returns></returns>
        public static List<T> GenerateClassList<T>(Type[] types)
        {
            List<T> menuList = new();
            var classes = types.Where(t => t.GetInterfaces().Contains(typeof(T)) && t.IsClass);
            foreach (var t in classes)
            {
                var classInstance = Activator.CreateInstance(t);

                if (classInstance is not T tClass) continue;
                menuList.Add(tClass);
            }
            return menuList;
        }
    }
}
