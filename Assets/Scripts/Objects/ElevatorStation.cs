using UnityEngine;

namespace ProjectGateway.Objects
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
