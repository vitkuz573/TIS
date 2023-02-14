using System;

namespace TIS;

internal class Train
{
    public int Number { get; }

    public string Destination { get; }

    public DateTimeOffset DepartureTime { get; }

    public Train? Left { get; set; }

    public Train? Right { get; set; }

    public int Height { get; set; }

    public Train(int number, string destination, DateTimeOffset departureTime)
    {
        Number = number;
        Destination = destination;
        DepartureTime = departureTime;
        Height = 1;
    }
}
