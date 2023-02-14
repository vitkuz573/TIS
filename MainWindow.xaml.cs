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
            var number = random.Next(1, 100);
            var destination = "Destination " + i;
            var departureTime = DateTime.Now.AddHours(i);
            _tis.InsertTrain(number, destination, departureTime);
        }

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
                var gg = dx / 1.3;

                if (gg < 20)
                {
                    DrawTree(train.Left, x - dx / level, y + dy, level + 1, gg, dy);
                    dc.DrawLine(new Pen(Brushes.Black, 2), new Point(x, y + 15), new Point(x - dx / level + 10 - 10, y + dy - 15));
                }
                else
                {
                    DrawTree(train.Left, x - dx / level, y + dy, level + 1, 120, dy);
                    dc.DrawLine(new Pen(Brushes.Black, 2), new Point(x, y + 15), new Point(x - dx / level + 10 - 10, y + dy - 15));
                }
            }

            if (train.Right != null)
            {
                var gg = dx / 1.3;

                if (gg < 20)
                {
                    DrawTree(train.Right, x + dx / level, y + dy, level + 1, gg, dy);
                    dc.DrawLine(new Pen(Brushes.Black, 2), new Point(x, y + 15), new Point(x + dx / level - 10 + 10, y + dy - 15));
                }
                else
                {
                    DrawTree(train.Right, x + dx / level, y + dy, level + 1, 120, dy);
                    dc.DrawLine(new Pen(Brushes.Black, 2), new Point(x, y + 15), new Point(x + dx / level - 10 + 10, y + dy - 15));
                }
            }
        }
    }
}
