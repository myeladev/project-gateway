using UnityEngine;

namespace ProjectDaydream.Logic
{
    public class ContainerGridItem
    {
        public Item Item;
        public Sprite Icon => Item.icon;

        // Reference to where it's placed
        public Vector2Int Position;

        public ContainerGridItem(Item item)
        {
            Item = item;
        }
    }
}