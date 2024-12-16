using System.Collections.Generic;
using ProjectGateway.Code.Scripts;
using UnityEngine;

namespace ProjectGateway.Code
{
    public class DoorObject : Prop, IInteractable
    {
        public bool shut;
        public bool locked;
        
        private PhysicsDoor _door;
        // Start is called before the first frame update
        protected new void Awake()
        {
            base.Awake();
            _door = GetComponentInParent<PhysicsDoor>();
        }

        void Start()
        { 
            if (shut || locked)
            {
                _door.Shut();
            }
        }
        
        public new Dictionary<InteractType, string> GetInteractText(InteractContext context)
        {
            // Get the base prop interactions
            var interactList = new Dictionary<InteractType, string>();
            if (shut)
            {
                interactList.Add(InteractType.Use, "Open");
            }
            else
            {
                interactList.Add(InteractType.Grab, "Grab");
            }
            // Return the modified list
            return interactList;
        }
        
        public new void Interact(InteractType interactType, InteractContext context)
        {
            switch (interactType)
            {
                case InteractType.Use:
                    if (locked)
                    {
                        FeedbackMessageUIManager.instance.ShowMessage("It's locked");    
                    }
                    else if(shut)
                    {
                        shut = false;
                        _door.Open();
                    }
                    break;
                case InteractType.Grab:
                    if (!shut && !locked)
                    {
                        base.Interact(interactType, context);
                    }
                    break;
            }
        }
    }
}
