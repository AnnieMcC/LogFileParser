using System;
using System.Collections.Generic;
using System.Linq;
using ElevatorService.Enums;
using ElevatorService.Interfaces;
using ElevatorService.Models;

namespace ElevatorService.Services
{
    public class ElevatorControl : IElevatorControl
    {
        public Elevator Elevator { get; set; }
        public List<Passenger> WaitingPassengers { get; set; }

        // WaitingPassengers = input
        // Status = Stopped
        public ElevatorControl(List<Passenger> waitingPassengers)
        {
            Elevator = new Elevator();
            WaitingPassengers = waitingPassengers;
        }

        public void PrintStatus()
        {
            if (Elevator.ElevatorStatus == ElevatorStatus.Travelling)
            {
                //display current status
                Console.WriteLine($"The elevator is {Elevator.ElevatorStatus} {Elevator.Direction}");
            }
            else
            {
                Console.WriteLine($"The elevator is {Elevator.ElevatorStatus}");
            }

           // Console.ReadLine();
        }

        public void Update(int currentFloor, int destinationFloor)
        {
            UpdateElevator(e =>
            {
                e.CurrentFloor = currentFloor;
                e.DestinationFloor = destinationFloor;
            });
        }

        private void UpdateElevator(Action<Elevator> update)
        {
            update(Elevator);
        }

        public bool AnyPassengersWaiting()
        {
            return WaitingPassengers.Any();
        }

        public bool AnyPassengersTravelling()
        {
            return Elevator.TravellingPassengers.Any();
        }

        public void ElevatorRun()
        {
          //  PrintStatus();

            //unload passengers
            var removedPassengers = Elevator.TravellingPassengers.Where(p => p.DestinationFloor == Elevator.CurrentFloor).ToList();
            if (removedPassengers.Any())
            {
                // STOP
                UpdateElevator(e => e.ElevatorStatus = ElevatorStatus.Stopped);
                PrintStatus();
                var passengersOut = removedPassengers.Count == 1 ? "passenger" : "passengers";
                //OPEN DOORS
                UpdateElevator(e => e.ElevatorStatus = ElevatorStatus.Opening);
                PrintStatus();
                Console.WriteLine($"{removedPassengers.Count} {passengersOut} getting out...");
                //CLOSE DOORS
                UpdateElevator(e => e.ElevatorStatus = ElevatorStatus.Closing);
                PrintStatus();
            }
            Elevator.TravellingPassengers = Elevator.TravellingPassengers.Where(p => p.DestinationFloor != Elevator.CurrentFloor).ToList();

            //load passengers
            var waitingFloors = WaitingPassengers.GroupBy(passenger => new { passenger.OriginFloor, passenger.Direction }).ToList();
            WaitingPassengers.GroupBy(passenger => new { passenger.OriginFloor, passenger.Direction }).ToList()
            .ForEach(waitingFloor =>
            {
                var elevatorAvailable = Elevator.CurrentFloor == waitingFloor.Key.OriginFloor &&
                    Elevator.Direction == waitingFloor.Key.Direction
                    || !Elevator.TravellingPassengers.Any();
                if (elevatorAvailable && Elevator.CurrentFloor == waitingFloor.Key.OriginFloor)
                {
                    // STOP
                    UpdateElevator(e => e.ElevatorStatus = ElevatorStatus.Stopped);
                    PrintStatus();

                    var loadingPassengers = waitingFloor.ToList();
                    if(loadingPassengers.Any())
                    { 
                        var passengersIn = loadingPassengers.Count == 1 ? "passenger" : "passengers";
                        //OPEN DOORS
                        UpdateElevator(e => e.ElevatorStatus = ElevatorStatus.Opening);
                        PrintStatus();
                        Console.WriteLine($"{loadingPassengers.Count} {passengersIn} getting in...");
                        //CLOSE DOORS
                        UpdateElevator(e => e.ElevatorStatus = ElevatorStatus.Closing);
                        PrintStatus();
                    }
                    UpdateElevator(e => Elevator.TravellingPassengers.AddRange(loadingPassengers));
                    WaitingPassengers = WaitingPassengers.Where(wp => loadingPassengers.All(lp => lp.Id != wp.Id)).ToList();
                }
            });

            //update elevator
            int destinationFloor;
            if (Elevator.TravellingPassengers.Any())
            {
                var closestDestinationFloor =
                    Elevator.TravellingPassengers.OrderBy(p => (p.DestinationFloor - Elevator.CurrentFloor))
                    .First()
                    .DestinationFloor;
                destinationFloor = closestDestinationFloor;
            }
            else if ((Elevator.DestinationFloor == 0 || Elevator.DestinationFloor != Elevator.CurrentFloor) && WaitingPassengers.Any())
            {
                destinationFloor = WaitingPassengers.GroupBy(passenger => new { passenger.OriginFloor })
                    .OrderBy(group => group.Count())
                    .First()
                    .Key.OriginFloor;
            }
            else
            {
                destinationFloor = Elevator.DestinationFloor;
            }

            var currentFloor = Elevator.CurrentFloor +
                (destinationFloor > Elevator.CurrentFloor ?
                1 :
                -1);

            UpdateElevator(e => e.ElevatorStatus = ElevatorStatus.Travelling);
            Update(currentFloor, destinationFloor);
        }
    }
}
