using System.IO;
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

        // Feed records back to existing instances by instanceId (guid)
        public GameData LoadProfileData(string profileName)
        {
            string path = Path.Combine(SaveDataPath, profileName, "save.json");
            if (!File.Exists(path)) return new GameData();

            var json = File.ReadAllText(path);
            GameData gameData = JsonUtility.FromJson<GameData>(json);

            return gameData;
        }
    }
}