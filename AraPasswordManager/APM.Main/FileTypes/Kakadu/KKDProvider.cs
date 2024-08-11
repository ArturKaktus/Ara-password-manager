using APM.Core.ProviderInterfaces;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Collections.Generic;
using APM.Core.Models;
using System.Text.Json.Nodes;
using System.ComponentModel;
using System.Linq;

namespace APM.Main.FileTypes.Kakadu;

/// <summary>
/// Класс чтения и записи файлов .kkd
/// </summary>
public class KKDProvider : IFileProvider
{
    public string FilePath { get; set; }
    public char[] Password { get; set; }
    public FileInfo CurrentFile { get; set; }

    public void Open()
    {
        var jsonString = DecryptData(ReadFile());
        if (!jsonString.Equals("error"))
        {
            CurrentFile = new FileInfo(FilePath);
        }

        FillKakaduJson(jsonString);
    }

    public void Save()
    {
        using FileStream backUpStream = new(FilePath, FileMode.Create);
        backUpStream.Write(EncryptBackup(GetJsonString()));
    }
    private byte[] ReadFile()
    {
        byte[] error = new byte[1];
        try
        {
            string emptyStringArray = string.Empty;
            byte[] array = File.ReadAllBytes(Path.Combine(FilePath, emptyStringArray));
            return array;
        }
        catch
        {
            return error;
        }
    }
    private byte[] EncryptBackup(string backupString)
    {
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(new string(Password)));

        using Aes aes = Aes.Create();
        aes.Key = hash;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;

        using ICryptoTransform encryptor = aes.CreateEncryptor();
        byte[] byteCipherText = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(backupString), 0, backupString.Length);
        return byteCipherText;
    }
    private string DecryptData(byte[] sourceArray)
    {
        try
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(Password));
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
                StringToCharArray(recordObj["password"]!.ToString()),
                recordObj["url"]!.ToString(),
                recordObj["loginSymbol"]!.ToString(),
                recordObj["passwordSymbol"]!.ToString(),
                recordObj["urlSymbol"]!.ToString()));
        }

        AppDocument.CurrentDatabaseModel.FillLists(groupsArrayList,recordsArrayList);
    }
    private string GetJsonString()
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
}