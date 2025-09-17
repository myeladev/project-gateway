using System;
using TMPro;
using UnityEngine;

namespace ProjectGateway
{
    public class GameVersionText : MonoBehaviour
    {
        private TextMeshProUGUI textMesh;
        private void Awake()
        {
            textMesh = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            textMesh.text = $"Version {Application.version}";
        }
    }
}
