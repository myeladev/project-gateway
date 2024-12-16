using System;
using System.Collections;
using System.Collections.Generic;
using ProjectGateway.Code;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectGateway
{
    public class PhysicsDoor : MonoBehaviour
    {
        private bool gracePeriod = true;
        public HingeJoint physicalDoor;
        private Vector3 _startingPosition;
        private Quaternion _startingRotation;
        private const float LatchDistance = 0.02f;
        private AudioSource _audio;

        [SerializeField]
        private AudioClip sfxClosed;
        [SerializeField]
        private AudioClip sfxOpen;

        private void Awake()
        {
            _startingPosition = physicalDoor.transform.position;
            _startingRotation = physicalDoor.transform.rotation;
            _audio = GetComponent<AudioSource>();
        }

        void Update()
        {
            if (Vector3.Distance(physicalDoor.transform.position, _startingPosition) < LatchDistance)
            {
                Latch();
            }
            
            if (gracePeriod && Vector3.Distance(physicalDoor.transform.position, _startingPosition) > LatchDistance * 1.1f)
            {
                Unlatch();
            }
        }

        public void Shut()
        {
            physicalDoor.transform.position = _startingPosition;
            physicalDoor.transform.rotation = _startingRotation;
            physicalDoor.GetComponent<Rigidbody>().isKinematic = true;
            physicalDoor.GetComponent<DoorObject>().shut = true;
            gracePeriod = true;
        }
        
        public void Open()
        {
            _audio.PlayOneShot(sfxOpen);
            gracePeriod = true;
            physicalDoor.GetComponent<Rigidbody>().isKinematic = false;
            physicalDoor.GetComponent<DoorObject>().shut = false;
        }

        private void Latch()
        {
            if (!gracePeriod)
            {
                Shut();
                _audio.PlayOneShot(sfxClosed);
            }
        }

        private void Unlatch()
        {
            gracePeriod = false;
        }
    }
}
