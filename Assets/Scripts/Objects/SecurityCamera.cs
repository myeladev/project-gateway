using ProjectDaydream.Logic;
using UnityEngine;

namespace ProjectDaydream.Objects
{
    public class SecurityCamera : MonoBehaviour
    {
        [SerializeField] private Transform anchor;
        [SerializeField] private Renderer cameraObjectRenderer;

        void Update()
        {
            if (!cameraObjectRenderer.isVisible)
            {
                anchor.LookAt(PlayerController.Instance.character.transform.position + (Vector3.up * 0.75f));
            }
        }
    }
}
