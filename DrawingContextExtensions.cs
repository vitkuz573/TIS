﻿using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace TIS;

internal static class DrawingContextExtensions
{
    public static void DrawTree(this DrawingContext dc, FrameworkElement element, TrainInformationSystem tis, Train train = null, double x = 500, int y = 30, int level = 1, double dx = 300, int dy = 100)
    {
        train ??= tis.Root;

        if (train != null)
        {
            dc.DrawNode(element, train, x, y);

            if (train.Left != null)
            {
                var leftDx = dx / 2.0;
                var leftDy = dy;

                if (train.Left.Height - (train.Right?.Height ?? 0) > 1)
                {
                    leftDx *= 0.8;
                    leftDy = (int)(dy * 0.8);
                }

                var childX = x - leftDx;
                var childY = y + leftDy;

                DrawTree(dc, element, tis, train.Left, childX, childY, level + 1, leftDx, leftDy);
                dc.DrawLine(new Pen(Brushes.Black, 2), new Point(x, y + 15), new Point(childX + 10, childY - 15));
            }

            if (train.Right != null)
            {
                var rightDx = dx / 2.0;
                var rightDy = dy;

                if ((train.Right?.Height ?? 0) - train.Left?.Height > 1)
                {
                    rightDx *= 0.8;
                    rightDy = (int)(dy * 0.8);
                }

                var childX = x + rightDx;
                var childY = y + rightDy;

                DrawTree(dc, element, tis, train.Right, childX, childY, level + 1, rightDx, rightDy);
                dc.DrawLine(new Pen(Brushes.Black, 2), new Point(x, y + 15), new Point(childX - 10, childY - 15));
            }
        }
    }

    private static void DrawNode(this DrawingContext dc, FrameworkElement element, Train train, double x, int y)
    {
        dc.DrawEllipse(Brushes.Red, null, new Point(x, y), 15, 15);

        var dpi = VisualTreeHelper.GetDpi(element).PixelsPerDip;
        var formattedText = new FormattedText(train.Number.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 14, Brushes.Black, dpi);
        dc.DrawText(formattedText, new Point(x - 7, y - 7));
    }
}
