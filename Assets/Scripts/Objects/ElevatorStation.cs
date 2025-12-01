using UnityEngine;

namespace ProjectDaydream.Objects
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
