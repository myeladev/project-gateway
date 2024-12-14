using UnityEngine;

namespace ProjectGateway
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
