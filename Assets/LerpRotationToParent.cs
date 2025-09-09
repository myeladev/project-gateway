using UnityEngine;

namespace ProjectGateway
{
    public class LerpRotationToParent : MonoBehaviour
    {
        [SerializeField] private float lerpSpeed = 5f;

        private void Update()
        {
            if (transform.parent != null)
            {
                transform.localRotation = Quaternion.Lerp(transform.rotation, 
                    transform.parent.rotation, 
                    Time.deltaTime * lerpSpeed);
            }
        }
    }
}
