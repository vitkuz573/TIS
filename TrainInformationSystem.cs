using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfApp1;

internal class TrainInformationSystem
{
    public Train? Root { get; private set; }

    public void InsertTrain(int number, string destination, DateTimeOffset departureTime)
    {
        Root = InsertTrain(Root, number, destination, departureTime);
    }

    private Train InsertTrain(Train node, int number, string destination, DateTimeOffset departureTime)
    {
        if (node == null)
        {
            return new Train(number, destination, departureTime);
        }

        if (number == node.Number)
        {
            throw new ArgumentException($"Train number {number} already exists in the system.");
        }

        if (number < node.Number)
        {
            node.Left = InsertTrain(node.Left, number, destination, departureTime);
        }
        else
        {
            node.Right = InsertTrain(node.Right, number, destination, departureTime);
        }

        node.Height = Math.Max(GetHeight(node.Left), GetHeight(node.Right)) + 1;
        int balance = GetBalance(node);

        // Left Left Case
        if (balance > 1 && number < node.Left.Number)
        {
            return RotateRight(node);
        }

        // Right Right Case
        if (balance < -1 && number > node.Right.Number)
        {
            return RotateLeft(node);
        }

        // Left Right Case
        if (balance > 1 && number > node.Left.Number)
        {
            node.Left = RotateLeft(node.Left);

            return RotateRight(node);
        }

        // Right Left Case
        if (balance < -1 && number < node.Right.Number)
        {
            node.Right = RotateRight(node.Right);

            return RotateLeft(node);
        }

        return node;
    }

    private static Train RotateLeft(Train node)
    {
        var newRoot = node.Right;
        var newSubtree = newRoot.Left;

        newRoot.Left = node;
        node.Right = newSubtree;

        node.Height = Math.Max(node.Left?.Height ?? 0, node.Right?.Height ?? 0) + 1;
        newRoot.Height = Math.Max(newRoot.Left?.Height ?? 0, newRoot.Right?.Height ?? 0) + 1;

        return newRoot;
    }

    private static Train RotateRight(Train node)
    {
        var newRoot = node.Left;
        var newSubtree = newRoot.Right;

        newRoot.Right = node;
        node.Left = newSubtree;

        node.Height = Math.Max(node.Left?.Height ?? 0, node.Right?.Height ?? 0) + 1;
        newRoot.Height = Math.Max(newRoot.Left?.Height ?? 0, newRoot.Right?.Height ?? 0) + 1;

        return newRoot;
    }

    private static int GetHeight(Train node) => node?.Height ?? 0;

    private static int GetBalance(Train node) => GetHeight(node?.Left) - GetHeight(node?.Right);

    public bool FindTrain(int number, out Train? train)
    {
        train = Root;

        while (train != null)
        {
            if (number < train.Number)
            {
                train = train.Left;
            }
            else if (number > train.Number)
            {
                train = train.Right;
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    public IEnumerable<Train> FindTrainsByDestination(string destination)
    {
        if (Root == null)
        {
            yield break;
        }

        foreach (var train in FindTrainsByDestination(Root, destination))
        {
            yield return train;
        }
    }

    private static IEnumerable<Train> FindTrainsByDestination(Train node, string destination)
    {
        if (node == null)
        {
            yield break;
        }

        foreach (var train in FindTrainsByDestination(node.Left, destination))
        {
            yield return train;
        }

        if (string.Equals(node.Destination, destination, StringComparison.OrdinalIgnoreCase))
        {
            yield return node;
        }

        foreach (var train in FindTrainsByDestination(node.Right, destination))
        {
            yield return train;
        }
    }
}
