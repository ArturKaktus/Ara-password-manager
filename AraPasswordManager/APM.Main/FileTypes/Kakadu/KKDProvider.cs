using APM.Core;
using APM.Core.Models;
using APM.Core.ProviderInterfaces;
using APM.Main.FileTypes.Kakadu.CreatePassword;
using APM.Main.FileTypes.Kakadu.PasswordVerify;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

namespace APM.Main.FileTypes.Kakadu;

/// <summary>
/// Чтения и записи файлов .kkd
/// </summary>
public class KKDProvider : IFileProvider
{
    public string? FileTitle => "Kakadu File";
    public List<string> FileExtension => ["*.kkd"];
    public string FilePath { get; set; }
    public char[] Password { get; set; }
    public FileInfo CurrentFile { get; set; }
    public bool HasChange { get; set; }

    public void ReadFile(Window? owner, IStorageFile file)
    {
        bool isEntered = false;
        var passwordVerify = new PasswordVerify.PasswordVerify(file.Path.AbsolutePath);
        var passwordWindow = WindowManager.NewWindow("Введите пароль", passwordVerify);

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

                if (AppDocument.CurrentFileInstance != null && AppDocument.CurrentFileInstance is IFileProvider fileProvider)
                {
                    fileProvider.FilePath = Uri.UnescapeDataString(file.Path.AbsolutePath);
                    fileProvider.Password = dataContext.Password.ToCharArray();

                    var jsonString = DecryptData(ReadFile(fileProvider.FilePath), Password);
                    if (!jsonString.Equals("error"))
                    {
                        CurrentFile = new FileInfo(FilePath);
                    }

                    FillKakaduJson(jsonString);
                }
            }
        };

        passwordWindow.ShowDialog(owner);
    }
    public void SaveFile(Window? owner, IStorageFile file)
    {
        bool isEntered = false;
        var createPassword = new CreatePassword.CreatePassword(file.Path.AbsolutePath);
        var passwordWindow = WindowManager.NewWindow("Введите пароль", createPassword);

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
                if (AppDocument.CurrentFileInstance != null && AppDocument.CurrentFileInstance is IFileProvider fileProvider)
                {
                    fileProvider.FilePath = Uri.UnescapeDataString(file.Path.AbsolutePath);
                    fileProvider.Password = dataContext.Password.ToCharArray();
                    using FileStream backUpStream = new(fileProvider.FilePath, FileMode.Create);
                    backUpStream.Write(EncryptBackup(GetJsonString(), Password));
                }
            }
        };
        passwordWindow.ShowDialog(owner);
    }

    #region Static methods
    private static byte[] ReadFile(string filePath)
    {
        byte[] error = new byte[1];
        try
        {
            string emptyStringArray = string.Empty;
            byte[] array = File.ReadAllBytes(Path.Combine(filePath, emptyStringArray));
            return array;
        }
        catch
        {
            return error;
        }
    }
    private static byte[] EncryptBackup(string backupString, char[] password)
    {
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(new string(password)));

        using Aes aes = Aes.Create();
        aes.Key = hash;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;

        using ICryptoTransform encryptor = aes.CreateEncryptor();
        byte[] byteCipherText = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(backupString), 0, backupString.Length);
        return byteCipherText;
    }
    private static string DecryptData(byte[] sourceArray, char[] password)
    {
        try
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            using var aesAlg = Aes.Create();
            aesAlg.Key = hash;
            aesAlg.Mode = CipherMode.ECB;
            aesAlg.Padding = PaddingMode.PKCS7;

            var decrypt = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using MemoryStream ms = new(sourceArray);
            using CryptoStream cs = new(ms, decrypt, CryptoStreamMode.Read);
            using StreamReader sr = new(cs, Encoding.UTF8);
            var decryptedText = sr.ReadToEnd();
            return decryptedText;
        }
        catch
        {
            return "error";
        }
    }
    private static char[] StringToCharArray(string value)
    {
        return value.ToCharArray();
    }
    private static void FillKakaduJson(string json)
    {
        List<GroupModel> groupsArrayList = [];
        List<RecordModel> recordsArrayList = [];

        var obj = JsonNode.Parse(json);
        var jsonObj = (JsonObject)obj!;

        var groupArray = (JsonArray)jsonObj["groups"]!;
        var size = groupArray.Count;
        for (var i = 0; i < size; i++)
        {
            var groupObj = (JsonObject)groupArray[i]!;
            groupsArrayList.Add(new GroupModel(
                Convert.ToInt32(groupObj["id"]!.ToString()),
                Convert.ToInt32(groupObj["pid"]!.ToString()),
                groupObj["name"]!.ToString()));
        }

        var recordArray = (JsonArray)jsonObj["records"]!;
        size = recordArray.Count;
        for (int i = 0; i < size; i++)
        {
            JsonObject recordObj = (JsonObject)recordArray[i];
            recordsArrayList.Add(new RecordModel(
                Convert.ToInt32(recordObj!["id"]!.ToString()),
                Convert.ToInt32(recordObj["pid"]!.ToString()),
                recordObj["name"]!.ToString(),
                recordObj["login"]!.ToString(),
                recordObj["password"]!.ToString(),
                recordObj["url"]!.ToString(),
                recordObj["loginSymbol"]!.ToString(),
                recordObj["passwordSymbol"]!.ToString(),
                recordObj["urlSymbol"]!.ToString()));
        }

        AppDocument.CurrentDatabaseModel.FillLists(groupsArrayList,recordsArrayList);
    }
    private static string GetJsonString()
    {
        JsonArray groupsArray = [];
        JsonArray recordsArray = [];
        JsonObject backupObj = new();
        foreach (var groupJson in AppDocument.CurrentDatabaseModel.GroupsArrayList.Select(groupModel => new JsonObject()
                 {
                     { "id", groupModel.Id },
                     { "pid", groupModel.Pid },
                     { "name", groupModel.Title }
                 }))
        {
            groupsArray.Add(groupJson);
        }
        foreach (var recordJson in AppDocument.CurrentDatabaseModel.RecordsArrayList.Select(recordModel => new JsonObject()
                 {
                     { "id", recordModel.Id },
                     { "pid", recordModel.Pid },
                     { "name", recordModel.Title },
                     { "login", recordModel.Login },
                     { "password", new string(recordModel.Password) },
                     { "url", recordModel.Url },
                     { "loginSymbol", recordModel.GetAfterLoginString() },
                     { "passwordSymbol", recordModel.GetAfterPasswordString() },
                     { "urlSymbol", recordModel.GetAfterUrlString() }
                 }))
        {
            recordsArray.Add(recordJson);
        }
        backupObj.Add("groups", groupsArray);
        backupObj.Add("records", recordsArray);
        return backupObj.ToString();
    }
    #endregion
}