using UnityEngine;

namespace ProjectGateway.Objects
{
    public class SecurityCamera : MonoBehaviour
    {
        [SerializeField] private Transform anchor;
        [SerializeField] private Renderer cameraObjectRenderer;

        void Update()
        {
            if (!cameraObjectRenderer.isVisible)
            {
                anchor.LookAt(MyPlayer.instance.Character.transform.position + (Vector3.up * 0.75f));
            }
        }
    }
}
