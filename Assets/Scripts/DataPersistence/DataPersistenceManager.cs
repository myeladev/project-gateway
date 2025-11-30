using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ProjectGateway.Common;
using ProjectGateway.SaveData;
using ProjectGateway.UI;
using UnityEngine;

namespace ProjectGateway.DataPersistence
{
    public class DataPersistenceManager : MonoBehaviour
    {
        private IFileManager fileManager;
        private GameMetaData currentGameMetaData;
        public static DataPersistenceManager Instance;
        private void Awake()
        {
            Instance = this;
            // TODO: add functionality to use either disk or steam cloud persistence
            fileManager = new DiskFileManager();
        }

        private void Update()
        {
            if (currentGameMetaData != null && Input.GetKeyDown(KeyCode.F5))
            {
                SaveProfile();
                FeedbackMessageUIManager.instance.ShowMessage("Game saved");
            }
        }

        // Collect all SaveAgents and write one file
        public void SaveProfile(string profileName = null)
        {
            var metaData = new GameMetaData()
            {
                profileName = profileName ?? currentGameMetaData.profileName,
                createdDate = currentGameMetaData.createdDate,
                gameVersion = Application.version,
                lastSavedDate = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
            };
            
            fileManager.SaveProfileThumbnail(metaData.profileName, Utilities.SaveCameraView(Camera.main));
            
            fileManager.SaveProfileMetaData(metaData.profileName, metaData);
            
            var agents = FindObjectsByType<SaveAgent>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            GameData gameData = new GameData();
            for (var i = 0; i < agents.Length; i++)
            {
                agents[i].SaveData(ref gameData);
            }

            MyPlayer.instance.SaveData(ref gameData);
            
            fileManager.SaveProfileData(metaData.profileName, gameData);
        }

        // Feed records back to existing instances by instanceId (guid)
        public void LoadProfile(string profileName)
        {
            currentGameMetaData = fileManager.LoadProfileMetaData(profileName);
            GameData gameData = fileManager.LoadProfileData(profileName);

            // Build a lookup of current agents
            var agents = FindObjectsByType<SaveAgent>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            
            // Restore all agents
            foreach (var a in agents) a.LoadData(gameData);

            MyPlayer.instance.LoadData(gameData);
            
            // TODO: spawned objects
            // If not found: this is a spawned/missing object → instantiate via your PrefabRegistry, then Restore(rec)
        }

        public void LoadRecentProfile()
        {
            var profiles = fileManager.GetAllProfileMetaData();
            GameMetaData mostRecent = null;
            DateTime mostRecentDate = DateTime.MinValue;

            foreach (var profile in profiles)
            {
                if(profile is null || string.IsNullOrEmpty(profile.lastSavedDate)) continue;
                var savedDate = DateTime.Parse(profile.lastSavedDate, CultureInfo.InvariantCulture);
                if (savedDate > mostRecentDate)
                {
                    mostRecentDate = savedDate;
                    mostRecent = profile;
                }
            }
            
            if (mostRecent != null)
            {
                LoadProfile(mostRecent.profileName);
            }
            else
            { 
                var newSave = CreateNewProfile("Placeholder");
                LoadProfile("Placeholder");
            }
        }

        public GameData CreateNewProfile(string profileName, bool ambientMode = false)
        {
            var metaData = new GameMetaData
            {
                profileName = profileName,
                createdDate = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                lastSavedDate = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                gameVersion = Application.version,
                ambientMode = ambientMode
            };

            currentGameMetaData = metaData;
            var gameData = new GameData();

            fileManager.SaveProfileMetaData(profileName, metaData);
            fileManager.SaveProfileData(profileName, gameData);

            return gameData;
        }

        public List<(GameMetaData, Texture)> LoadProfileList()
        {
            var profiles = fileManager.GetAllProfileMetaData();
            var result = new List<(GameMetaData metadata, Texture thumbnail)>();

            foreach (var profile in profiles)
            {
                if (profile != null 
                && profile.profileName != "Placeholder")
                {
                    var thumbnail = fileManager.LoadProfileThumbnail(profile.profileName);
                    result.Add((profile, thumbnail));
                }
            }

            return result.OrderByDescending(m => m.metadata.lastSavedDate).ToList();
        }
    }
}
