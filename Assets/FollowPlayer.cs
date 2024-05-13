using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGateway
{
    public class FollowPlayer : MonoBehaviour
    {
        private Vector3 offset;
        public MyPlayer player;
        
        // Start is called before the first frame update
        void Awake()
        {
            offset = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            var targetPosition =
                player.IsInVehicle ? player.drivingVehicle.transform.position : player.Character.transform.position;
            transform.position = targetPosition + offset;
        }
    }
}
