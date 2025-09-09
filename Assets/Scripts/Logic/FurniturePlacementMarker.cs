using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectGateway.Logic
{
    public class FurniturePlacementMarker : MonoBehaviour
    {
        public bool IsFree => !currentlyCollidingWith.Any() && !isBlocked;
        [HideInInspector]
        public bool isBlocked;
        
        [SerializeField]
        private List<Collider> currentlyCollidingWith = new();

        [SerializeField] private Material freeMaterial;
        [SerializeField] private Material blockedMaterial;
        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            _meshRenderer.materials = new[] { IsFree ? freeMaterial : blockedMaterial };
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player")) return;
            if(!currentlyCollidingWith.Contains(other))
                currentlyCollidingWith.Add(other);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if(currentlyCollidingWith.Contains(other))
                currentlyCollidingWith.Remove(other);
        }

        public void FlushCollisions()
        {
            currentlyCollidingWith.Clear();
        }
    }
}
