using System;
using System.Collections.Generic;
using ElevatorService.Interfaces;
using ElevatorService.Models;
using ElevatorService.Services;

namespace ElevatorSimulation
{
    public class Program
    {        
        public static void Main()
        {
            const int MAX_FLOORS = 10;
            const int GROUND_FLOOR = 0;

            List<Passenger> waitingPassengers = new List<Passenger>();

            Console.WriteLine("How many people are waiting for the lift (on any floor)?");
            int passengers = Convert.ToInt32(Console.ReadLine());

            int origin;
            int destination;
            //get passenger inputs
            //& create waitingPassengers
            for (int i = 0; i < passengers; i++)
            {
                //input validation
                do
                {
                    Console.WriteLine($"Enter origin floor (0-9) for passenger {i + 1}:");
                    origin = Convert.ToInt32(Console.ReadLine());
                } while (origin > MAX_FLOORS || origin < GROUND_FLOOR);

                do
                {
                    Console.WriteLine($"Enter destination floor (0-9) for passenger {i + 1}:");
                    destination = Convert.ToInt32(Console.ReadLine());
                } while (destination > MAX_FLOORS || destination < GROUND_FLOOR);
                
                waitingPassengers.Add(new Passenger(origin, destination));
            }

            // initialise elevator inside Control .ctor
            var elevatorControl = new ElevatorControl(waitingPassengers);

            int steps =0;
            while (elevatorControl.AnyPassengersWaiting() || elevatorControl.AnyPassengersTravelling())
            {
                //display Current floor
                Console.WriteLine($"Current floor: {elevatorControl.Elevator.CurrentFloorString}");
                elevatorControl.ElevatorRun();
                steps++;
            }
            Console.WriteLine($"{steps} steps");
        }
    }
}
