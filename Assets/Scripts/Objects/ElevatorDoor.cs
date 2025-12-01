using UnityEngine;

namespace ProjectDaydream.Objects
{
    public class ElevatorDoor : MonoBehaviour
    {
        public void Open()
        {
            gameObject.SetActive(false);
        }
        
        public void Close()
        {
            gameObject.SetActive(true);
        }
    }
}
