using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1;

public partial class MainWindow : Window
{
    private readonly TrainInformationSystem _tis;
    private readonly DrawingVisual visual;
    private readonly DrawingContext dc;

    public MainWindow()
    {
        InitializeComponent();

        _tis = new TrainInformationSystem();
        
        var random = new Random();

        for (int i = 0; i < 20; i++)
        {
            var number = random.Next(1, 150);
            var destination = "Destination " + i;
            var departureTime = DateTime.Now.AddHours(i);
            _tis.InsertTrain(number, destination, departureTime);
        }

        // _tis.InsertTrain(10, "", DateTime.Now);
        // _tis.InsertTrain(1, "", DateTime.Now);
        // _tis.InsertTrain(6, "", DateTime.Now);
        // _tis.InsertTrain(70, "", DateTime.Now);
        // _tis.InsertTrain(40, "", DateTime.Now);
        // _tis.InsertTrain(14, "", DateTime.Now);
        // _tis.InsertTrain(12, "", DateTime.Now);
        // _tis.InsertTrain(68, "", DateTime.Now);
        // _tis.InsertTrain(6, "", DateTime.Now);
        // _tis.InsertTrain(8, "", DateTime.Now);
        // _tis.InsertTrain(9, "", DateTime.Now);
        // _tis.InsertTrain(11, "", DateTime.Now);
        // _tis.InsertTrain(57, "", DateTime.Now);
        // _tis.InsertTrain(43, "", DateTime.Now);
        // _tis.InsertTrain(44, "", DateTime.Now);
        // _tis.InsertTrain(89, "", DateTime.Now);
        // _tis.InsertTrain(32, "", DateTime.Now);
        // _tis.InsertTrain(33, "", DateTime.Now);
        // _tis.InsertTrain(101, "", DateTime.Now);
        // _tis.InsertTrain(2, "", DateTime.Now);
        // _tis.InsertTrain(59, "", DateTime.Now);
        // _tis.InsertTrain(17, "", DateTime.Now);
        // _tis.InsertTrain(63, "", DateTime.Now);

        visual = new DrawingVisual();
        dc = visual.RenderOpen();

        DrawTree();

        dc.Close();

        var img = new Image
        {
            Source = new DrawingImage(visual.Drawing)
        };

        canvas.Children.Add(img);
    }

    private void DrawTree(Train train = null, double x = 500, int y = 30, int level = 1, double dx = 300, int dy = 100)
    {
        train ??= _tis.Root;

        if (train != null)
        {
            dc.DrawEllipse(Brushes.Red, null, new Point(x, y), 15, 15);

            var dpi = VisualTreeHelper.GetDpi(this).PixelsPerDip;
            var formattedText = new FormattedText(train.Number.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 14, Brushes.Black, dpi);
            dc.DrawText(formattedText, new Point(x - 7, y - 7));

            if (train.Left != null)
            {
                double leftDx = dx / 2.0;
                int leftDy = dy;
                if (train.Left.Height - (train.Right?.Height ?? 0) > 1)
                {
                    leftDx *= 0.8;
                    leftDy = (int)(dy * 0.8);
                }
                double childX = x - leftDx;
                int childY = y + leftDy;
                DrawTree(train.Left, childX, childY, level + 1, leftDx, leftDy);
                dc.DrawLine(new Pen(Brushes.Black, 2), new Point(x, y + 15), new Point(childX + 10, childY - 15));
            }

            if (train.Right != null)
            {
                double rightDx = dx / 2.0;
                int rightDy = dy;
                if ((train.Right?.Height ?? 0) - train.Left?.Height > 1)
                {
                    rightDx *= 0.8;
                    rightDy = (int)(dy * 0.8);
                }
                double childX = x + rightDx;
                int childY = y + rightDy;
                DrawTree(train.Right, childX, childY, level + 1, rightDx, rightDy);
                dc.DrawLine(new Pen(Brushes.Black, 2), new Point(x, y + 15), new Point(childX - 10, childY - 15));
            }
        }
    }
}
