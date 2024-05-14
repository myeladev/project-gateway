using System.ComponentModel;
using UnityEngine;

namespace ProjectGateway
{
    public class WeatherManager : MonoBehaviour
    {
        [Range(0, 24)]
        public float timeOfDay;

        [Range(0, 1)]
        public float precipitation;

        [Description("Length of time per game day in seconds")]
        public float dayLength;
        
        [SerializeField]
        private Light sun;
        [SerializeField]
        private Light moon;
        [SerializeField]
        private ParticleSystem rain;

        private const float MaxRainParticles = 3000f;
        [SerializeField]
        private float rainDaySeed = 0f;

        [Range(0, 1)]
        public float testX, testY;
        

        private bool isNight;

        private void Update()
        {
            timeOfDay += Time.deltaTime * (24f / dayLength);
            if (timeOfDay > 24)
            {
                timeOfDay = 0;
            }
            CheckDayNightTransition();

            testX = timeOfDay / 24f;
            testY = rainDaySeed;
            var precipitationNoise = (Mathf.PerlinNoise(timeOfDay / 24f, rainDaySeed));// - 0.5f) * 25f;
            precipitation = Mathf.Clamp(precipitationNoise, 0, 1);
            
            UpdateTime();
            UpdateRain();
        }
        
        void OnValidate()
        {
            UpdateTime();
            UpdateRain();
        }

        void UpdateTime()
        {
            var alpha = timeOfDay / 24f;
            var sunRotation = Mathf.Lerp(-90, 270, alpha);
            var moonRotation = sunRotation - 170;

            sun.transform.rotation = Quaternion.Euler(sunRotation, 90f, 0);
            moon.transform.rotation = Quaternion.Euler(moonRotation, 90f, 0);
        }


        [SerializeField]
        private float lerpedPrecipitation;
        void UpdateRain()
        {
            var emission = rain.emission;
            lerpedPrecipitation = Mathf.Lerp(lerpedPrecipitation, precipitation, Time.deltaTime * 20f);
            emission.rateOverTime = Mathf.RoundToInt(lerpedPrecipitation * MaxRainParticles);
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
                if (sun.transform.rotation.eulerAngles.x > 170)
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
            rainDaySeed = Random.Range(0, 1000);
        }
        
        private void StartNight()
        {
            isNight = true;
            sun.shadows = LightShadows.None;
            moon.shadows = LightShadows.Soft;
        }

        public string GetFriendlyTimeString()
        {
            var hour = Mathf.FloorToInt(timeOfDay);
            var minute = (timeOfDay % 1f) * 60f;

            return $"{hour:00}:{minute:00}";
        }
    }
}
