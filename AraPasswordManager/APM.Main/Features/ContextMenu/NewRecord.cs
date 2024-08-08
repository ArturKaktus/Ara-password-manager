using System;
using System.Windows.Input;
using APM.Core;
using APM.Core.Enums;
using APM.Main;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Reflection.Metadata;

namespace Ara_password_manager.Features.ContextMenu;

public class NewRecord : MenuItem, IContextMenu
{
    public NewRecord()
    {
        ExecuteCommand = new RelayCommand(Relay);
    }

    private void Relay()
    {
        Execute(null);
    }
    public string Title { get; } = "Новая запись";
    public bool CanExecute(object parameter)
    {
        if (parameter is ObjectType otParam)
        {
            return otParam == ObjectType.Record;
        }
        return false;
    }

    public bool IsEnabledMenu(object parameter) => AppDocument.NodeTransfer.SelectedTreeNode != null;

    public void Execute(object parameter)
    {
        
    }
    public ICommand ExecuteCommand { get; }
}