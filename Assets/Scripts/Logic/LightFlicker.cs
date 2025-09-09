using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectGateway.Logic
{
    public class LightFlicker : MonoBehaviour
    {
        [Header("Light Settings")]
        [SerializeField]
        private List<Light> lights;           // List of lights to flicker
        [SerializeField]
        private float minFlickerDelay = 60f;  // Minimum delay in seconds before flicker
        [SerializeField]
        private float maxFlickerDelay = 180f; // Maximum delay in seconds before flicker
        [SerializeField]
        private float flickerDuration = 0.2f; // Duration of the flicker in seconds
        [SerializeField]
        private int minFlickerCount = 3;         // Minimum number of flickers per event
        [SerializeField]
        private int maxFlickerCount = 6;         // Max number of flickers per event

        private List<float> _savedLightIntensityList = new();
         
        private void Start()
        {
            _savedLightIntensityList = lights.Select(l => l.intensity).ToList();
            // Start the flicker coroutine
            StartCoroutine(FlickerRoutine());
        }

        private IEnumerator FlickerRoutine()
        {
            while (true)
            {
                // Wait for a random delay between minFlickerDelay and maxFlickerDelay
                float delay = Random.Range(minFlickerDelay, maxFlickerDelay);
                yield return new WaitForSeconds(delay);

                // Perform the flicker effect
                yield return StartCoroutine(FlickerLights());
            }
        }

        private IEnumerator FlickerLights()
        {
            var flickerCount = Random.Range(minFlickerCount, maxFlickerCount);
            for (int i = 0; i < flickerCount; i++)
            {
                // Turn off all the lights
                SetLightsActive(false);

                // Wait for half the flicker duration
                yield return new WaitForSeconds(flickerDuration / flickerCount);

                // Turn on all the lights
                SetLightsActive(true);

                // Wait for the other half of the flicker duration
                yield return new WaitForSeconds(flickerDuration / flickerCount);
            }
        }

        private void SetLightsActive(bool active)
        {
            for (var i = 0; i < lights.Count; i++)
            {
                var lgt = lights[i];
                var savedIntensity = _savedLightIntensityList[i];
                if (lgt)
                {
                    lgt.intensity = active ? savedIntensity : savedIntensity / 1.4f;
                }
            }
        }
    }
}
