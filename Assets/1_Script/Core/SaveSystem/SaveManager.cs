using UnityEngine;
using System.IO;
using Leguar.TotalJSON;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Swift_Blade.SaveSystem
{
    public static class SaveManager
    {
        public static readonly string DefaultSaveFolderPath = Application.persistentDataPath + "/__Save/";
        //public static readonly string GetDefaultFull = SaveFolderPath + fileWithExtension;

        private const string fileWithExtension = fileName + fileFormat;
        private const string fileName = "savFile";
        private const string fileFormat = ".sav";
        /// <summary>
        /// false if failed
        /// </summary>
        /// <param name="directoryPath"></param>
        private static bool ValidateDirectoryPath(string directoryPath, bool autoCreateDirectory = false)
        {
            bool result = Directory.Exists(directoryPath);
            if (!result && autoCreateDirectory)
            {
                Debug.Log("creating new directory");
                Directory.CreateDirectory(directoryPath);
            }
            return result;
        }
        public static void SaveFile(JSON jsonObject, string path = null)
        {
            if (path == null)
            {
                path = DefaultSaveFolderPath;
            }

            ValidateDirectoryPath(path, true);

            string jsonString = jsonObject.CreateString();
            try
            {
                File.WriteAllText(path + fileWithExtension, jsonString);
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
            //StreamWriter writer = new StreamWriter(SavePath);
            //writer.WriteLine(jsonAsString);
            //writer.Close();
        }
        public static async ValueTask SaveFileAsync(JSON jsonObject, string path = null)
        {
            if (path == null)
            {
                path = DefaultSaveFolderPath;
            }

            ValidateDirectoryPath(path, true);

            await Awaitable.BackgroundThreadAsync();
            string jsonString = await Task.Run(() => jsonObject.CreateString());
            try
            {
                await File.WriteAllTextAsync(path + fileWithExtension, jsonString);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        public static JSON LoadFile(string path = null) //todo : exception handle
        {
            if (path == null)
            {
                path = DefaultSaveFolderPath;
            }

            bool validationSuccess = ValidateDirectoryPath(path);
            if (!validationSuccess)
            {
                return null;
            }

            string json = null;
            try
            {
                json = File.ReadAllText(path + fileWithExtension);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            //StreamReader reader = new StreamReader(SavePath);
            //string jsonAsString = reader.ReadToEnd();
            //reader.Close();
            JSON result = JSON.ParseString(json);
            Debug.Log("loaded from" + path);
            return result;
        }
        public static async ValueTask<JSON> LoadFileAsync(string path = null) //todo : exception handle
        {
            if (path == null)
            {
                path = DefaultSaveFolderPath;
            }

            bool validationSuccess = ValidateDirectoryPath(path);
            if (!validationSuccess)
            {
                return null;
            }

            await Awaitable.BackgroundThreadAsync();

            Debug.Log("loaded from" + path);
            string json = await File.ReadAllTextAsync(path + fileWithExtension);

            JSON result = await Task.Run(() => JSON.ParseString(json));
            await Awaitable.MainThreadAsync();
            return result;
        }
        public static async ValueTask ReplaceValueAsync(this JSON json, string key, object value)
        {
            await Task.Run(() => json.Replace(key, value));
        }
        public static async ValueTask Save(this JSON json, string path = null)
        {
            if (path == null)
            {
                path = DefaultSaveFolderPath;
            }
            await SaveFileAsync(json, path);
        }
    }

}
