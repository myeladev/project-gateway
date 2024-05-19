using UnityEngine;

namespace ProjectGateway
{
    public class ElevatorStation : MonoBehaviour
    {
        public ElevatorStationDirection doorToOpen;
    }

    public enum ElevatorStationDirection
    {
        Front,
        Back
    }
}
