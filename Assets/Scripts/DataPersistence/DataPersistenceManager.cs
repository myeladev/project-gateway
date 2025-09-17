using System;
using System.Collections.Generic;
using System.IO;
using ProjectGateway.SaveData;
using UnityEngine;

namespace ProjectGateway.DataPersistence
{
    public class DataPersistenceManager : MonoBehaviour
    {
        private IFileManager fileManager;
        private void Awake()
        {
            // TODO: add functionality to use either disk or steam cloud persistence
            fileManager = new DiskFileManager();
        }

        // Collect all SaveAgents and write one file
        public void SaveProfile(string profileName)
        {
            var agents = FindObjectsByType<SaveAgent>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            GameData gameData = new GameData();
            for (var i = 0; i < agents.Length; i++)
            {
                agents[i].SaveData(ref gameData);
            }

            MyPlayer.instance.SaveData(ref gameData);
            
            fileManager.SaveProfileData(profileName, gameData);
        }

        // Feed records back to existing instances by instanceId (guid)
        public void LoadProfile(string profileName)
        {
            GameData gameData = fileManager.LoadProfileData(profileName);

            // Build a lookup of current agents
            var agents = FindObjectsByType<SaveAgent>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            
            // Restore all agents
            foreach (var a in agents) a.LoadData(gameData);

            MyPlayer.instance.LoadData(gameData);
            
            // TODO: spawned objects
            // If not found: this is a spawned/missing object → instantiate via your PrefabRegistry, then Restore(rec)
        }
    }
}
