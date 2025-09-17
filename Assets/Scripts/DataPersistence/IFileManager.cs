using UnityEngine;

namespace ProjectGateway.DataPersistence
{
    public interface IFileManager
    {
        public void SaveProfileData(string profileName, GameData data);
        public GameData LoadProfileData(string profileName);
        public void SaveProfileMetaData(string profileName, GameMetaData metaData);
        public GameMetaData LoadProfileMetaData(string profileName);
        public void SaveProfileThumbnail(string profileName, Texture2D metaData);
        public Texture2D LoadProfileThumbnail(string profileName);
    }
}