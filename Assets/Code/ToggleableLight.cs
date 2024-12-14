using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectGateway.Code
{
    public class ToggleableLight : Furniture, IInteractable
    {
        [SerializeField]
        private List<Light> lights = new();
        
        [SerializeField]
        private AudioClip sfxOff;
        [SerializeField]
        private AudioClip sfxOn;
        
        private AudioSource _audio;

        private void Awake()
        {
            _audio = GetComponent<AudioSource>();
        }
        
        public new Dictionary<InteractType, string> GetInteractText(InteractContext context)
        {
            var interactText = base.GetInteractText(context);
            interactText.Add(InteractType.Use, "Toggle");
            return interactText;
        }

        public new void Interact(InteractType interactType, InteractContext context)
        {
            base.Interact(interactType, context);
            switch (interactType)
            {
                case InteractType.Use:
                    foreach (var toggleLight in lights)
                    {
                        toggleLight.enabled = !toggleLight.enabled;
                    }
                    _audio.PlayOneShot(lights.Max(m => m.enabled) ? sfxOn : sfxOff);
                    break;
            }
        }
        
    }
}
