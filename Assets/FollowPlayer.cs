using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGateway
{
    public class FollowPlayer : MonoBehaviour
    {
        private Vector3 offset;
        private Transform player;
        
        // Start is called before the first frame update
        void Awake()
        {
            offset = transform.position;
            player = GameObject.FindWithTag("Player").transform;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = player.transform.position + offset;
        }
    }
}
