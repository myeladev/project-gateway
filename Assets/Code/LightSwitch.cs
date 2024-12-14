using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGateway
{
    public class LightSwitch : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private List<Light> attachedLights;

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
            Refresh();
        }

        // Update is called once per frame
        void Refresh()
        {
            foreach (var attachedLight in attachedLights)
            {
                attachedLight.enabled = isOn;
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
