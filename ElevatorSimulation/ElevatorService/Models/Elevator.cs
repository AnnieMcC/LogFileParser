using System;
using System.Collections.Generic;
using ElevatorService.Enums;

namespace ElevatorService.Models
{
    public class Elevator
    {
        private int _currentFloor;
        public int CurrentFloor
        {
            get => _currentFloor;
            set => _currentFloor = value;
        }

        public string CurrentFloorString
        {
            get
            {
                return _currentFloor > 0
                    ? _currentFloor.ToString()
                    : "Ground";
            }
        }

        public int DestinationFloor { get; set; }
        public List<Passenger> TravellingPassengers { get; set; }

        public ElevatorStatus ElevatorStatus { get; set; } = ElevatorStatus.Stopped;

        public void UpdateStatus(ElevatorStatus status)
        {
            ElevatorStatus = status;
        }

        public Direction Direction
        {
            get
            {
                return CurrentFloor == 0
                    ? Direction.Up
                    : DestinationFloor > CurrentFloor ?
                        Direction.Up :
                        Direction.Down;
            }
        }

        //.ctor
        public Elevator()
        {
            CurrentFloor = 0;
            TravellingPassengers = new List<Passenger>();
        }
    }
}
