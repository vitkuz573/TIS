﻿using System;

namespace WpfApp1;

class TrainInformationSystem
{
    public Train? Root { get; private set; }
    public int Count { get; private set; }

    public void InsertTrain(int number, string destination, DateTime departureTime)
    {
        if (Root == null)
        {
            Root = new Train(number, destination, departureTime);
            Count = 1;

            return;
        }

        Root = InsertTrain(Root, number, destination, departureTime);
        Count++;
    }

    private Train InsertTrain(Train node, int number, string destination, DateTime departureTime)
    {
        if (node == null)
        {
            return new Train(number, destination, departureTime);
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

        node.Height = Math.Max(GetHeight(node.Left), GetHeight(node.Right)) + 1;
        newRoot.Height = Math.Max(GetHeight(newRoot.Left), GetHeight(newRoot.Right)) + 1;

        return newRoot;
    }

    private static Train RotateRight(Train node)
    {
        var newRoot = node.Left;
        var newSubtree = newRoot.Right;

        newRoot.Right = node;
        node.Left = newSubtree;

        node.Height = Math.Max(GetHeight(node.Left), GetHeight(node.Right)) + 1;
        newRoot.Height = Math.Max(GetHeight(newRoot.Left), GetHeight(newRoot.Right)) + 1;

        return newRoot;
    }

    private static int GetHeight(Train node)
    {
        if (node == null)
        {
            return 0;
        }

        return node.Height;
    }

    private static int GetBalance(Train node)
    {
        if (node == null)
        {
            return 0;
        }

        return GetHeight(node.Left) - GetHeight(node.Right);
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
}
