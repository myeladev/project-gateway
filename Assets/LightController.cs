using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectGateway
{
    public class LightController : MonoBehaviour
    {
        [SerializeField]
        private List<Light> attachedLights;
        [SerializeField]
        private List<Renderer> attachedNeonObjects;

        public bool isOn;

        private void Awake()
        {
            isOn = attachedLights.Max(m => m.isActiveAndEnabled);
        }

        void Start()
        {
            Toggle(isOn);
        }

        public void Refresh()
        {
            foreach (var attachedLight in attachedLights)
            {
                attachedLight.enabled = isOn;
            }
            
            for (var index = 0; index < attachedNeonObjects.Count; index++)
            {
                var attachedNeonObject = attachedNeonObjects[index];
                attachedNeonObject.material = isOn ? MaterialDatabase.Instance.neonMaterial : MaterialDatabase.Instance.defaultMaterial;
            }
        }

        public void Toggle(bool state)
        {
            isOn = state;
            Refresh();
        }
    }
}
