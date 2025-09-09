using System.Collections.Generic;
using UnityEngine;

namespace ProjectGateway.Objects.Vehicles
{
    public class CarLights : MonoBehaviour
    {
        public enum Side
        {
            Front,
            Back
        }

        [System.Serializable]
        public struct CarLight
        {
            public Light lightObj;
            public Side side;
        }

        private bool isFrontLightOn;
        private bool isBackLightOn;

        public Color frontLightOnColor;
        public Color frontLightOffColor;
        public Color backLightOnColor;
        public Color backLightOffColor;

        public List<CarLight> lights;

        void Start()
        {
            isFrontLightOn = false;
            isBackLightOn = false;
        }

        public void OperateFrontLights(bool on)
        {
            isFrontLightOn = on;

            if (isFrontLightOn)
            {
                //Turn On Lights
                foreach(var light in lights)
                {
                    if(light.side == Side.Front && light.lightObj.gameObject.activeInHierarchy == false)
                    {
                        light.lightObj.gameObject.SetActive(true);
                        light.lightObj.color = frontLightOnColor;
                    }
                }
            }
            else
            {
                //Turn Off Lights
                foreach (var light in lights)
                {
                    if (light.side == Side.Front && light.lightObj.gameObject.activeInHierarchy)
                    {
                        light.lightObj.gameObject.SetActive(false);
                        light.lightObj.color = frontLightOffColor;
                    }
                }
            }
        }

        public void OperateBackLights(bool on)
        {
            isBackLightOn = on;
            if (isBackLightOn)
            {
                //Turn On Lights
                foreach (var light in lights)
                {
                    if (light.side == Side.Back && light.lightObj.gameObject.activeInHierarchy == false)
                    {
                        light.lightObj.gameObject.SetActive(true);
                        light.lightObj.color = backLightOnColor;
                    }
                }
            }
            else
            {
                //Turn Off Lights
                foreach (var light in lights)
                {
                    if (light.side == Side.Back && light.lightObj.gameObject.activeInHierarchy == true)
                    {
                        light.lightObj.gameObject.SetActive(false);
                        light.lightObj.color = backLightOffColor;
                    }
                }
            }
        }
    }
}
