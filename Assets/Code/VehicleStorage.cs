using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectGateway
{
    public class VehicleStorage : MonoBehaviour, IInteractable
    {
        private readonly List<Furniture> _storedFurniture = new();
        private List<Transform> _furniturePositions = new();

        private void Awake()
        {
            _furniturePositions = transform.GetComponentsInChildren<Transform>().ToList();
            _furniturePositions.Remove(transform);
        }

        private void Update()
        {
            for (var i = 0; i < _furniturePositions.Count; i++)
            {
                // If this position should have furniture
                if (_storedFurniture.Count > i)
                {
                    _storedFurniture[i].transform.position = _furniturePositions[i].position;
                    _storedFurniture[i].transform.rotation = _furniturePositions[i].rotation;
                }
            }
        }

        private void StowFurniture(Furniture furnitureToStow)
        {
            if (furnitureToStow)
            {
                if (_furniturePositions.Count > _storedFurniture.Count)
                {
                    _storedFurniture.Add(furnitureToStow);
                    furnitureToStow.gameObject.SetActive(true);
                    furnitureToStow.GetComponent<Collider>().enabled = false;
                    MyPlayer.instance.Character.ReleaseHeldFurniture();
                }
                else
                {
                    FeedbackMessageUIManager.instance.ShowMessage("Not enough room in vehicle");
                }
            }
            else
            {
                FeedbackMessageUIManager.instance.ShowMessage("Not holding furniture");
            }
        }

        public bool IsInteractable => MyPlayer.instance.Character.CanInteract || MyPlayer.instance.Character.movingFurniture;
        public List<string> GetInteractOptions(InteractContext context)
        {
            var interactKeys = new List<string> { $"Stow Furniture" };

            if (_storedFurniture.Any())
            {
                interactKeys.Add("Remove Furniture");
            }

            return interactKeys;
        }

        public void Interact(string option, InteractContext context)
        {
            switch (option)
            {
                case "Stow Furniture":
                    StowFurniture(MyPlayer.instance.Character.movingFurniture);
                    break;
                case "Remove Furniture":
                    var furnitureToMove = GetFurnitureToRemove();
                    if (furnitureToMove)
                    {
                        MyPlayer.instance.Character.MoveFurniture(furnitureToMove);
                        _storedFurniture.Remove(furnitureToMove);
                        furnitureToMove.gameObject.SetActive(false);
                    }
                    else
                    {
                        FeedbackMessageUIManager.instance.ShowMessage("No furniture to remove");
                    }

                    break;
            }
        }

        private Furniture GetFurnitureToRemove()
        {
            return _storedFurniture.Any() ? _storedFurniture.FirstOrDefault() : null;
        }
    }
}
