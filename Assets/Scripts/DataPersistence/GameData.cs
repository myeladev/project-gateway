using System.Collections.Generic;
using ProjectGateway.Objects.Furniture;
using ProjectGateway.Objects.Items;

namespace ProjectGateway.DataPersistence
{
    [System.Serializable]
    public class GameData
    {
        public PlayerSaveData player;
        public List<PropSaveData> props = new ();
        public List<FurnitureSaveData> furniture = new ();
    }
}