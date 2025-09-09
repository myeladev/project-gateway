using UnityEngine;

namespace ProjectGateway.Logic
{
    public class WeatherManager : MonoBehaviour
    {
        [Range(0, 24)]
        public float timeOfDay;

        [SerializeField]
        private float cycleSpeed;

        [Range(0, 1)]
        public float precipitation;
        
        [SerializeField]
        private Light moon;
        [SerializeField]
        private ParticleSystem rain;

        private const float MaxRainParticles = 3000f;
        [SerializeField]
        private float rainDaySeed = 0f;

        [Range(0, 1)]
        public float testX, testY;

        private void Update()
        {
            if (timeOfDay > 24)
            {
                timeOfDay = 0;
            }

            testX = timeOfDay / 24f;
            testY = rainDaySeed;
            var precipitationNoise = (Mathf.PerlinNoise(timeOfDay / 24f, rainDaySeed));// - 0.5f) * 25f;
            precipitation = Mathf.Clamp(precipitationNoise, 0, 1);
            
            UpdateRain();
        }
        
        void OnValidate()
        {
            UpdateRain();
        }

        [SerializeField]
        private float lerpedPrecipitation;
        void UpdateRain()
        {
            var emission = rain.emission;
            lerpedPrecipitation = Mathf.Lerp(lerpedPrecipitation, precipitation, Time.deltaTime * 20f);
            emission.rateOverTime = Mathf.RoundToInt(lerpedPrecipitation * MaxRainParticles);
        }
        
        public string GetFriendlyTimeString()
        {
            var hour = Mathf.FloorToInt(timeOfDay);
            var minute = (timeOfDay % 1f) * 60f;

            return $"{hour:00}:{minute:00}";
        }
    }
}
