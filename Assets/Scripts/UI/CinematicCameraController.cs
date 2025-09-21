using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using ProjectGateway.Core;
using UnityEngine;

namespace ProjectGateway.UI
{
    public class CinematicCameraController : MonoBehaviour
    {
        private List<CinemachineVirtualCamera> cameras = new ();
        private int index;
        
        void Start()
        {
            cameras = GetComponentsInChildren<CinemachineVirtualCamera>().ToList();
            index = Random.Range(0, cameras.Count);
            foreach (var cam in cameras)
            {
                cam.gameObject.SetActive(false);
            }
            cameras[index].gameObject.SetActive(true);
        }

        private void Update()
        {
            if (SceneLoader.Instance.IsInMainMenu)
            {
                if(!cameras[index].isActiveAndEnabled) cameras[index].gameObject.SetActive(true);
                if (Mathf.Approximately(cameras[index].GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition,
                        1f))
                {
                    cameras[index].gameObject.SetActive(false);
                    index++;
                    index %= cameras.Count;
                    cameras[index].gameObject.SetActive(true);
                    Camera.main.transform.position = cameras[index].transform.position;
                }
            }
            else
            {
                cameras.ForEach(c => c.gameObject.SetActive(false));
            }
        }
    }
}