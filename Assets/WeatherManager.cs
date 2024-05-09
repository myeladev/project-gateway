using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Serialization;

namespace ProjectGateway
{
    public class WeatherManager : MonoBehaviour
    {
        [Range(0, 24)]
        public float timeOfDay;

        [Description("Length of time per game day in seconds")]
        public float dayLength;
        
        [SerializeField]
        private Light sun;
        [SerializeField]
        private Light moon;
        [SerializeField]
        private Volume volume;
        

        private bool isNight;

        private void Update()
        {
            timeOfDay += Time.deltaTime * (24f / dayLength);
            if (timeOfDay > 24)
            {
                timeOfDay = 0;
            }
            
            UpdateTime();
            UpdateSky();
        }
        
        void OnValidate()
        {
            UpdateTime();
        }

        void UpdateTime()
        {
            var alpha = timeOfDay / 24f;
            var sunRotation = Mathf.Lerp(-90, 270, alpha);
            var moonRotation = sunRotation - 180;

            sun.transform.rotation = Quaternion.Euler(sunRotation, 90f, 0);
            moon.transform.rotation = Quaternion.Euler(moonRotation, 90f, 0);
        }

        private Vector3 skyRotation;
        void UpdateSky()
        {
            if (volume.profile.TryGet(out PhysicallyBasedSky sky))
            {
                sky.spaceRotation.value = new Vector3(0, skyRotation.y += Time.deltaTime * 0.2f, 0);
            }
        }

        void CheckDayNightTransition()
        {
            if (isNight)
            {
                if (moon.transform.rotation.eulerAngles.x > 170)
                {
                    StartDay();
                }
            }
            else
            {
                if (moon.transform.rotation.eulerAngles.x > 170)
                {
                    StartNight();
                }
            }
        }

        private void StartDay()
        {
            isNight = false;
            sun.shadows = LightShadows.Soft;
            moon.shadows = LightShadows.None;
        }
        
        private void StartNight()
        {
            isNight = true;
            sun.shadows = LightShadows.None;
            moon.shadows = LightShadows.Soft;
        }
    }
}
