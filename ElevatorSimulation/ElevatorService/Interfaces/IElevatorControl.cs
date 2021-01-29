using System;
using ElevatorService.Enums;
using ElevatorService.Models;

namespace ElevatorService.Interfaces
{
    public interface IElevatorControl
    {
        void Update(int currentFloor, int destinationFloor);

        void ElevatorRun();

        bool AnyPassengersWaiting();
    }
}
