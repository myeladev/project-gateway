using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectGateway.Logic
{
    public class FurniturePlacementMarker : MonoBehaviour
    {
        public bool IsFree => !isBlocked;
        [HideInInspector]
        public bool isBlocked;
        
        [SerializeField] private Material freeMaterial;
        [SerializeField] private Material blockedMaterial;
        [SerializeField] private LayerMask placementObstacleLayers;
        [SerializeField] private Vector3 defaultSize = Vector3.one; // Default size of the check box

        private MeshRenderer _meshRenderer;
        private Collider _collider;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _collider = GetComponent<Collider>();
            if (_collider != null)
            {
                defaultSize = _collider.bounds.size;
            }
        }

        private void Update()
        {
            CheckPlacement();
            _meshRenderer.materials = new[] { IsFree ? freeMaterial : blockedMaterial };
        }

        private void CheckPlacement()
        {
            // Check if there are any colliders in the way
            Vector3 checkSize = _collider != null ? _collider.bounds.size : defaultSize;
            Collider[] hitColliders = Physics.OverlapBox(
                transform.position + transform.up * (checkSize.y * 0.5f),
                checkSize * 0.4f,
                transform.rotation,
                placementObstacleLayers
            );

            isBlocked = hitColliders.Length > 0;
        }

        // Optional: Helper to visualize the check area in the editor
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;
            Vector3 gizmoSize = _collider != null ? _collider.bounds.size : defaultSize;
            Gizmos.DrawWireCube(Vector3.zero, gizmoSize);
        }
    }
}