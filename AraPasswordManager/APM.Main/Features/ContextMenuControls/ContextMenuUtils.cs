using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APM.Main.Features.ContextMenuControls
{
    internal static class ContextMenuUtils
    {
        //Выборка определенных меню
        public static List<MenuItem> GenerateMenuItems(object? mainObj, object? obj)
        {
            return AppDocument.ContextMenuList
                .Where(item => item.CanExecute(obj))
                .OrderBy(i => i.Order)
                .Select(item => item.ReturnMenuItem(mainObj, obj))
                .Where(menuItem => menuItem != null)
                .ToList();
        }

        //Сбор всех классов с определнным интерфейсом
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
