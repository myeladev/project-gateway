using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace ProjectGateway.Objects
{
    public class Elevator : MonoBehaviour
    {
        [SerializeField] 
        private ElevatorCarriageController carriage;
        
        [SerializeField] 
        private ElevatorDoor frontCarriageDoor;
        [SerializeField] 
        private ElevatorDoor backCarriageDoor;
        [SerializeField]
        private float elevatorSpeed;

        [SerializeField]
        private List<ElevatorButton> buttons;

        private List<ElevatorStation> targetStations = new ();

        private float idleTimer = 0f;

        private void Start()
        {
            if (buttons.Any())
            {
                var closestStation = buttons.OrderBy(m => Vector3.Distance(m.transform.position, transform.position)).First().targetStation;
                OnArriveAtStation(closestStation);
                transform.position = closestStation.transform.position;
            }
        }

        private ElevatorStation currentStation;
        void Update()
        {
            if(idleTimer > 0f) idleTimer -= Time.deltaTime;
            if (targetStations.Any() && idleTimer <= 0f)
            {
                if (!currentStation)
                {
                    currentStation = targetStations[0];
                    CloseDoors();
                    carriage.carriageDriver.DOMove(currentStation.transform.position, Vector3.Distance(carriage.carriageDriver.transform.position, currentStation.transform.position) * 0.5f)
                        .SetEase(Ease.InOutCubic)
                        .OnComplete(() =>
                        {
                            OnArriveAtStation(currentStation);
                            targetStations.Remove(currentStation);
                            currentStation = null;
                            idleTimer = 5f;
                        });
                }
            }
        }

        public void PressElevatorButton(ElevatorButton button)
        {
            var target = button.targetStation;

            if (!targetStations.Contains(target))
            {
                button.OnPressed();
                targetStations.Add(target);
            }
        }

        private void OnArriveAtStation(ElevatorStation stationArrivedAt)
        {
            OpenDoor(stationArrivedAt.doorToOpen);
            foreach (var elevatorButton in buttons.Where(m => m.targetStation == stationArrivedAt))
            {
                elevatorButton.Reset();
            }
        }

        private void OpenDoor(ElevatorStationDirection doorToOpen)
        {
            switch (doorToOpen)
            {
                case ElevatorStationDirection.Front:
                    frontCarriageDoor.Open();
                    break;
                case ElevatorStationDirection.Back:
                    backCarriageDoor.Open();
                    break;
            }
        }

        private void CloseDoors()
        {
            frontCarriageDoor.Close();
            backCarriageDoor.Close();
        }
    }
}
