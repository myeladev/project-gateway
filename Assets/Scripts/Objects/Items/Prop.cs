﻿using System;
using System.Collections.Generic;
using System.Linq;
using ProjectGateway.DataPersistence;
using ProjectGateway.Logic;
using ProjectGateway.SaveData;
using UnityEngine;

namespace ProjectGateway.Objects.Items
{
    public class Prop : MonoBehaviour, IInteractable, IDataPersistence
    {
        public bool IsInteractable => MyPlayer.instance.Character.CanInteract;

        public List<string> GetInteractOptions(InteractContext context)
        {
            return context == InteractContext.Default ? 
                new List<string>
                {
                    "Grab",
                }
                : new List<string>();
        }

        private MyCharacterController _player;
        protected Rigidbody Rigidbody;
        private Vector3 _originalPosition;
        private Quaternion _originalRotation;

        protected void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            _player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<MyCharacterController>();
            _originalPosition = transform.position;
            _originalRotation = transform.rotation;
        }

        private void Update()
        {
            // Item has fallen out of the world for some reason
            if (transform.position.y < -1000f)
            {
                // Reset it
                transform.position = _originalPosition;
                transform.rotation = _originalRotation;
                Rigidbody.linearVelocity = Vector3.zero;
            }
        }

        public void Interact(string option, InteractContext context)
        {
            switch (option)
            {
                case "Grab":
                    
                    break;
            }
        }

        public void LoadData(GameData data)
        {
            PropSaveData saveData = data.props.FirstOrDefault(m => GetComponent<SaveAgent>().id == m.id);
            if (saveData == null) return;

            transform.SetPositionAndRotation(
                new Vector3(saveData.position[0], saveData.position[1], saveData.position[2]),
                Quaternion.Euler(saveData.rotation[0], saveData.rotation[1], saveData.rotation[2])
            );
        }

        public void SaveData(ref GameData data)
        {
            data.props.Add(new PropSaveData
            {
                id = GetComponent<SaveAgent>().id,
                position = new[] { transform.position.x, transform.position.y, transform.position.z },
                rotation = new[] { transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z },
            });
        }
    }
    
    [Serializable]
    public class PropSaveData
    {
        public string id;
        public float[] position;
        public float[] rotation;
    }
}