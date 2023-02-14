using System;

namespace WpfApp1;

class Train
{
    public int Number { get; set; }
    public string Destination { get; set; }
    public DateTime DepartureTime { get; set; }

    public Train Left { get; set; }
    public Train Right { get; set; }

    public int Height { get; set; }

    public Train(int number, string destination, DateTime departureTime)
    {
        Number = number;
        Destination = destination;
        DepartureTime = departureTime;
        Height = 1;
    }
}
