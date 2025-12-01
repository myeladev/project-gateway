using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using ProjectDaydream.Core;
using UnityEngine;

namespace ProjectDaydream.UI
{
    public class CinematicCameraController : MonoBehaviour
    {
        [SerializeField] private float switchDuration = 10f;
        private float timer;
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
            if (SceneManager.Instance.IsInMainMenu)
            {
                if(!cameras[index].isActiveAndEnabled) cameras[index].gameObject.SetActive(true);
                timer += Time.deltaTime;
                if (timer >= switchDuration)
                {
                    timer = 0f;
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