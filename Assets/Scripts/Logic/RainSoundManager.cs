using UnityEngine;

namespace ProjectGateway.Logic
{
    public class RainSoundManager : MonoBehaviour
    {
        [Header("Audio Settings")] 
        public AudioSource rainAudioSource;
        public float fadeSpeed = 2.0f;
        [Range(0, 1)]
        public float shelteredVolume = 0f;
        [Range(0, 1)]
        public float normalVolume = 0.2f;

        [Header("Raycast Settings")] public float rayDistance = 300.0f;
        public LayerMask layerMask; // Specify which layers the raycast should interact with.

        private void Update()
        {
            // Perform the raycast
            var isSheltered = Physics.Raycast(transform.position, Vector3.up, out _, rayDistance, layerMask);

            // Fade the volume based on whether the raycast hits an object (indicating shelter)
            var targetVolume = isSheltered ? shelteredVolume : normalVolume;
            
            // Calculate the amount to change per second based on fadeSpeed
            float volumeChangeRate = Mathf.Abs(targetVolume - (isSheltered ? normalVolume : shelteredVolume)) / (isSheltered ? fadeSpeed : fadeSpeed / 3f);
            
            rainAudioSource.volume =
                Mathf.MoveTowards(rainAudioSource.volume, targetVolume, volumeChangeRate * Time.deltaTime);
        }

        // Optional: Visualize the ray in the Scene view for debugging
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector3.up * rayDistance);
        }
    }
}
