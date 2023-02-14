using System.Collections.Generic;
using System;

namespace WpfApp1;

class TrainInformationSystem
{
    public Train? Root { get; private set; }

    public void InsertTrain(int number, string destination, DateTime departureTime)
    {
        if (Root == null)
        {
            Root = new Train(number, destination, departureTime);
            return;
        }

        var current = Root;

        while (true)
        {
            if (number < current.Number)
            {
                if (current.Left == null)
                {
                    current.Left = new Train(number, destination, departureTime);
                    break;
                }

                current = current.Left;
            }
            else
            {
                if (current.Right == null)
                {
                    current.Right = new Train(number, destination, departureTime);
                    break;
                }

                current = current.Right;
            }
        }
    }

    public void PrintAllTrains()
    {
        if (Root != null)
        {
            InOrderTraverse(Root);
        }
    }

    private void InOrderTraverse(Train current)
    {
        if (current == null)
        {
            return;
        }

        InOrderTraverse(current.Left);
        Console.WriteLine("Train Number: " + current.Number + ", Destination: " + current.Destination + ", Departure Time: " + current.DepartureTime);
        InOrderTraverse(current.Right);
    }

    public Train? FindTrain(int number)
    {
        var current = Root;

        while (current != null)
        {
            if (number < current.Number)
            {
                current = current.Left;
            }
            else if (number > current.Number)
            {
                current = current.Right;
            }
            else
            {
                return current;
            }
        }

        return null;
    }

    public List<Train> FindTrainsByDestination(string destination)
    {
        var trains = new List<Train>();
        
        if (Root != null)
        {
            FindTrainsByDestination(Root, destination, trains);
        }

        return trains;
    }

    private void FindTrainsByDestination(Train current, string destination, List<Train> trains)
    {
        if (current == null)
        {
            return;
        }

        FindTrainsByDestination(current.Left, destination, trains);

        if (current.Destination == destination)
        {
            trains.Add(current);
        }

        FindTrainsByDestination(current.Right, destination, trains);
    }

    public int[] GetAllTrainNumbers()
    {
        var numbers = new List<int>();

        if (Root != null)
        {
            GetAllTrainNumbers(Root, numbers);
        }

        return numbers.ToArray();
    }

    private void GetAllTrainNumbers(Train current, List<int> numbers)
    {
        if (current == null)
        {
            return;
        }

        GetAllTrainNumbers(current.Left, numbers);
        numbers.Add(current.Number);
        GetAllTrainNumbers(current.Right, numbers);
    }
}
