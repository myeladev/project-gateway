using System.Collections.Generic;
using System.IO;
using ProjectGateway.Common;
using UnityEngine;

namespace ProjectGateway.DataPersistence
{
    public class DiskFileManager : IFileManager
    {
        private string SaveDataPath => $"{Application.persistentDataPath}/Saves/";
        
        public void SaveProfileData(string profileName, GameData gameData)
        {
            var json = JsonUtility.ToJson(gameData, true);
         
            string path = Path.Combine(SaveDataPath, profileName);
            Directory.CreateDirectory(path);         

            File.WriteAllText(Path.Combine(path, "save.json"), json);
        }

        public GameData LoadProfileData(string profileName)
        {
            string path = Path.Combine(SaveDataPath, profileName, "save.json");
            if (!File.Exists(path)) return new GameData();

            var json = File.ReadAllText(path);
            GameData gameData = JsonUtility.FromJson<GameData>(json);

            return gameData;
        }
        
        public void SaveProfileMetaData(string profileName, GameMetaData metaData)
        {
            var json = JsonUtility.ToJson(metaData, true);
         
            string path = Path.Combine(SaveDataPath, profileName);
            Directory.CreateDirectory(path);         

            File.WriteAllText(Path.Combine(path, "metadata.json"), json);
        }

        public GameMetaData LoadProfileMetaData(string profileName)
        {
            string path = Path.Combine(SaveDataPath, profileName, "metadata.json");
            if (!File.Exists(path)) return new GameMetaData();

            var json = File.ReadAllText(path);
            GameMetaData metaData = JsonUtility.FromJson<GameMetaData>(json);

            return metaData;
        }

        public void SaveProfileThumbnail(string profileName, Texture2D thumbnail)
        {
            var byteArray = thumbnail.EncodeToPNG();
            var path = Path.Combine(SaveDataPath, profileName, "thumbnail.json");
            File.WriteAllBytes(path, byteArray);
        }

        public Texture2D LoadProfileThumbnail(string profileName)
        {
            var path = Path.Combine(SaveDataPath, profileName, "thumbnail.json");
            return Utilities.ReadImageFromFile(path);
        }

        public List<GameMetaData> GetAllProfileMetaData()
        {
            var metaDataList = new List<GameMetaData>();

            if (!Directory.Exists(SaveDataPath))
            {
                Directory.CreateDirectory(SaveDataPath);
                return metaDataList;
            }

            var profileDirectories = Directory.GetDirectories(SaveDataPath);
            foreach (var profileDir in profileDirectories)
            {
                var profileName = new DirectoryInfo(profileDir).Name;
                var metaData = LoadProfileMetaData(profileName);
                metaDataList.Add(metaData);
            }

            return metaDataList;
        }
    }
}