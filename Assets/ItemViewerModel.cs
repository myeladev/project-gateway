using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGateway
{
    public class ItemViewerModel : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshFilter = GetComponent<MeshFilter>();
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(0f, -Time.deltaTime * 20f, 0f, Space.Self);
        }

        public void SetItem(GameObject item)
        {
            var meshRenderer = item.GetComponent<MeshRenderer>();
            var meshFilter = item.GetComponent<MeshFilter>();
            _meshRenderer.materials = meshRenderer.materials;
            _meshFilter.mesh = meshFilter.mesh;
        }
    }
}
