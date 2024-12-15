using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectGateway
{
    public class LightSwitch : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private List<Light> attachedLights;
        [SerializeField]
        private List<Renderer> attachedNeonObjects;

        private List<Color> savedEmissionColors;
        private List<float> savedEmissionIntensities;
        
        [SerializeField]
        private bool isOn;
        
        [SerializeField]
        private AudioClip sfxOff;
        [SerializeField]
        private AudioClip sfxOn;
        
        private AudioSource _audio;

        private void Awake()
        {
            _audio = GetComponent<AudioSource>();
        }

        // Start is called before the first frame update
        void Start()
        {
            savedEmissionColors = attachedNeonObjects.Select(m => m.material.GetColor("_EmissionColor")).ToList();
            savedEmissionIntensities = savedEmissionColors.Select(m => m.maxColorComponent).ToList();
            Refresh();
        }

        // Update is called once per frame
        void Refresh()
        {
            foreach (var attachedLight in attachedLights)
            {
                attachedLight.enabled = isOn;
            }

            for (var index = 0; index < attachedNeonObjects.Count; index++)
            {
                var attachedNeonObject = attachedNeonObjects[index];
                var savedEmissionColor = savedEmissionColors[index];
                var savedEmissionIntensity = savedEmissionIntensities[index];
                if (isOn)
                {
                    // Set emission to the desired color and intensity
                    attachedNeonObject.material.SetColor("_EmissionColor", savedEmissionColor * savedEmissionIntensity);
                    attachedNeonObject.material.EnableKeyword("_EMISSION");
                }
                else
                {
                    // Turn off emission by setting the color to black
                    attachedNeonObject.material.SetColor("_EmissionColor", Color.black);
                    attachedNeonObject.material.DisableKeyword("_EMISSION");
                }
            }
        }

        public bool IsInteractable => true;
        public Dictionary<InteractType, string> GetInteractText(InteractContext context)
        {
            return context == InteractContext.Default ? 
                new Dictionary<InteractType, string>
                {
                    { InteractType.Use, "Toggle Lights" },
                }
                : new Dictionary<InteractType, string>();
        }

        public void Interact(InteractType interactType, InteractContext context)
        {
            switch (interactType)
            {
                case InteractType.Use:
                    Toggle();
                    break;
            }
        }

        public void Toggle()
        {
            isOn = !isOn;
            _audio.PlayOneShot(isOn ? sfxOn : sfxOff);
            Refresh();
        }
    }
}
