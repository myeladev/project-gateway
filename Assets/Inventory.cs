using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectGateway
{
    public class Inventory : MonoBehaviour
    {
        private readonly List<Item> _items = new();
        private GameObject _player;

        public const int CarryLimit = 200;
        public float CurrentWeight => _items.Sum(item => item.weight);

        private void Awake()
        {
            _player = GetComponent<MyPlayer>().Character.gameObject;
        }

        public bool AttemptToAddItem(Item itemToAdd)
        {
            var hasSpaceToAddItem = CarryLimit - CurrentWeight >= itemToAdd.weight;

            if (hasSpaceToAddItem)
            {
                AddItemToInventory(itemToAdd);
                return true;
            }

            return false;
        }

        private void AddItemToInventory(Item itemToAdd)
        {
            itemToAdd.gameObject.SetActive(false);
            _items.Add(itemToAdd);
        }

        public GameObject RemoveFromInventory(Item itemToRemove)
        {
            _items.Remove(itemToRemove);
            
            itemToRemove.gameObject.SetActive(true);
            itemToRemove.transform.position = _player.transform.position;

            return itemToRemove.gameObject;
        }
    }
}
