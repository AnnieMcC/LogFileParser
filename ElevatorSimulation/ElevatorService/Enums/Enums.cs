
namespace ElevatorService.Enums
{
    public enum Direction
    {
        Up = 1,
        Down = -1
    }

    public enum ElevatorStatus
    {
        Opening = -1,
        Stopped = 0,
        Closing = 1,
        Travelling = 2
    }

    public enum PassengerStatus
    {
        Waiting = -1,
        Travelling = 0,
        Arrived = 1
    }
}
