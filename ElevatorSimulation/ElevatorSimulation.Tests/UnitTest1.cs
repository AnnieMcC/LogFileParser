using System;
using Xunit;
using ElevatorService;
using ElevatorService.Services;
using ElevatorService.Models;
using System.Collections.Generic;
using ElevatorService.Enums;

namespace ElevatorSimulation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void OnePass()
        {
            var passengers = new List<Passenger>
            {
                new Passenger(3, 7)
            };
            int steps = RunTests(passengers);

            Assert.Equal(8, steps);
        }

        [Fact]
        public void TwoPassengers1()
        {
            var passengers = new List<Passenger>
            {
                new Passenger(0, 5),
                new Passenger(1, 5)
            };
            int steps = RunTests(passengers);

            Assert.Equal(6, steps);
        }

        [Fact]
        public void TwoPassengers2()
        {
            var passengers = new List<Passenger>
            {
                new Passenger(2, 6), 
                new Passenger(4, 0)  
            };
            int steps = RunTests(passengers);

            // 0-6-0 = 13
            Assert.Equal(13, steps); 
        }

        [Fact]
        public void ThreePassengers()
        {
            var passengers = new List<Passenger>
            {
                new Passenger(0, 5),
                new Passenger(4, 0),
                new Passenger(10, 0)
            };

            int steps = RunTests(passengers);

            //0 -> 5 -> 0 -> 10 -> 0 = 31
            Assert.Equal(31, steps);
        }

        private int RunTests(List<Passenger> passengers)
        {
            ElevatorControl elevatorControl = new ElevatorControl(passengers);

            int steps = 0;
            while (elevatorControl.AnyPassengersWaiting() || elevatorControl.AnyPassengersTravelling())
            {
                elevatorControl.ElevatorRun();
                steps++;
            }

            return steps;
        }
    }
}
