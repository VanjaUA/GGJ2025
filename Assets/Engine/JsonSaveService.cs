#region 

using Newtonsoft.Json;
using System.IO;
using UnityEngine;

#endregion

namespace Engine
{
    public static class JsonSaveService
    {
        private static readonly string dataDirectoryPath = Application.persistentDataPath + "/PlayerData";

        public static T LoadData<T>(string filePath)
        {
            if (File.Exists(dataDirectoryPath + "/" + filePath))
            {
                string loadData = File.ReadAllText(dataDirectoryPath + "/" + filePath);
                return JsonConvert.DeserializeObject<T>(loadData);
            }
            else
            {
                Debug.LogError("There is no save files to load!");
                return default;
            }
        }

        public static void SaveData(object data, string filePath)
        {
            if (!Directory.Exists(dataDirectoryPath))
            {
                Directory.CreateDirectory(dataDirectoryPath);
            }
            string dataString = JsonConvert.SerializeObject(data);
            File.WriteAllText(dataDirectoryPath + "/" + filePath, dataString);
        }

        public static void DeleteFile(string filePath)
        {
            if (File.Exists(dataDirectoryPath + "/" + filePath))
            {
                File.Delete(dataDirectoryPath + "/" + filePath);
            }
            else
            { Debug.LogError("No such file to delete!"); }
        }

        public static void DeleteAll()
        {
            if (Directory.Exists(dataDirectoryPath))
            {
                Directory.Delete(dataDirectoryPath, true);
            }
        }
    }

    public class GameData 
    {
        public int Money;
        public int HealthPoints;
    }

}