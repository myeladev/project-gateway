using System.Collections.Generic;
using System.Linq;
using ProjectGateway.Code.Scripts;
using UnityEngine;

namespace ProjectGateway.Code
{
    public class ToggleableFurnitureLight : Furniture, IInteractable
    {
        [SerializeField]
        private List<Light> lights = new();
        
        [SerializeField]
        private AudioClip sfxOff;
        [SerializeField]
        private AudioClip sfxOn;
        
        private AudioSource _audio;

        protected override void Awake()
        {
            base.Awake();
            _audio = GetComponent<AudioSource>();
        }
        
        public new List<string> GetInteractOptions(InteractContext context)
        {
            var interactText = base.GetInteractOptions(context);
            interactText.Add("Toggle");
            return interactText;
        }

        public new void Interact(string option, InteractContext context)
        {
            base.Interact(option, context);
            switch (option)
            {
                case "Toggle":
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
