using System;

namespace WpfApp1;

internal class Train
{
    public int Number { get; set; }

    public string Destination { get; set; }

    public DateTimeOffset DepartureTime { get; set; }

    public Train Left { get; set; }

    public Train Right { get; set; }

    public int Height { get; set; }

    public Train(int number, string destination, DateTimeOffset departureTime)
    {
        Number = number;
        Destination = destination;
        DepartureTime = departureTime;
        Height = 1;
    }
}
