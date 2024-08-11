using APM.Core.ProviderInterfaces;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Collections.Generic;
using APM.Main.FileTypes.Kakadu.PasswordVerify;
using System;
using APM.Main.FileTypes.Kakadu.CreatePassword;

namespace APM.Main.FileTypes.Kakadu
{
    public class KKDExtension : IFileProperty, IReadWriteFile
    {
        public string? FileTitle => "Kakadu File";

        public List<string> FileExtension => ["*.kkd"];

        public bool IsSecure => true;

        public void ReadFile(Window? owner, IStorageFile file)
        {
            bool isEntered = false;
            var passwordVerify = new PasswordVerify.PasswordVerify(file.Path.AbsolutePath);

            var passwordWindow = new Window
            {
                Title = "Введите пароль",
                Height = 200,
                Width = 300,
                Content = passwordVerify
            };

            passwordVerify.AcceptButtonClicked += (sender, e) =>
            {
                isEntered = true;
                passwordWindow.Close();
            };

            passwordWindow.Closed += (sender, e) =>
            {
                if (isEntered)
                {
                    var dataContext = passwordVerify.DataContext as PasswordVerifyViewModel;

                    AppDocument.Provider = new KKDProvider()
                    {
                        FilePath = Uri.UnescapeDataString(file.Path.AbsolutePath),
                        Password = dataContext.Password.ToCharArray()
                    };
                    AppDocument.Provider.Open();
                }
            };
             
            passwordWindow.ShowDialog(owner);
        }

        public void SaveFile(Window? owner, IStorageFile file)
        {
            bool isEntered = false;
            var createPassword = new CreatePassword.CreatePassword(file.Path.AbsolutePath);
            var passwordWindow = new Window
            {
                Title = "Введите пароль",
                Height = 200,
                Width = 300,
                Content = createPassword
            };
            createPassword.AcceptButtonClicked += (sender, e) =>
            {
                isEntered = true;
                passwordWindow.Close();
            };
            passwordWindow.Closed += (sender, e) =>
            {
                if (isEntered)
                {
                    var dataContext = createPassword.DataContext as CreatePasswordViewModel;

                    AppDocument.Provider = new KKDProvider()
                    {
                        FilePath = Uri.UnescapeDataString(file.Path.AbsolutePath),
                        Password = dataContext.Password.ToCharArray()
                    };
                    AppDocument.Provider.Save();
                }
            };
            passwordWindow.ShowDialog(owner);
        }
    }
}
