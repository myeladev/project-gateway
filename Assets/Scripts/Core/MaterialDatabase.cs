using UnityEngine;

namespace ProjectDaydream.Core
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
