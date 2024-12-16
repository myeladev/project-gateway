using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectGateway
{
    public class MaterialDatabase : MonoBehaviour
    {
        public static MaterialDatabase Instance;

        private void Awake()
        {
            Instance = this;
        }

        public Material defaultMaterial;
        public Material neonMaterial;
    }
}
