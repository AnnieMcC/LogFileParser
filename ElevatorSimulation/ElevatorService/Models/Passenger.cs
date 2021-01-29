using System;
using ElevatorService.Enums;

namespace ElevatorService.Models
{
    public class Passenger
    {
        public readonly Guid Id;
        public readonly int OriginFloor;
        public readonly int DestinationFloor;

//        public int CurrentFloor;

        public Passenger(int originFloor, int destinationFloor)
        {
            Id = Guid.NewGuid();
            OriginFloor = originFloor;
            DestinationFloor = destinationFloor;

 //           CurrentFloor = OriginFloor;
        }

        public Direction Direction
        {
            get
            {
                return OriginFloor < DestinationFloor ?
                    Direction.Up :
                    Direction.Down;
            }
        }

        //public PassengerStatus Status
        //{
        //    get
        //    {
        //        return CurrentFloor == OriginFloor ?
        //            PassengerStatus.Waiting :
        //            CurrentFloor != DestinationFloor ?
        //                PassengerStatus.Travelling :
        //                PassengerStatus.Arrived;
        //    }
        //}
    }
}
