using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectGateway.Objects.Vehicles
{
    public class ExitLocation : MonoBehaviour
    {
        public bool IsFree => !currentlyCollidingWith.Any();
        
        [SerializeField]
        private List<Collider> currentlyCollidingWith = new();

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player")) return;
            if(!currentlyCollidingWith.Contains(other))
                currentlyCollidingWith.Add(other);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if(currentlyCollidingWith.Contains(other))
                currentlyCollidingWith.Remove(other);
        }
    }
}
