namespace ProjectGateway.DataPersistence
{
    public interface IFileManager
    {
        public void SaveProfileData(string profileName, GameData data);
        public GameData LoadProfileData(string profileName);
    }
}