using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp1;

public partial class MainWindow : Window
{
    private readonly TrainInformationSystem _tis;
    private DrawingVisual _visual;

    public MainWindow()
    {
        InitializeComponent();

        _tis = new TrainInformationSystem();
        
        InsertTrainInformation();

        ReDrawTree();
    }

    private void InsertTrainInformation()
    {
        var random = new Random();
        var usedNumbers = new HashSet<int>();

        for (int i = 0; i < 20; i++)
        {
            int number;

            do
            {
                number = random.Next(1, 99);
            } while (usedNumbers.Contains(number));

            usedNumbers.Add(number);

            var destination = "Destination " + i;
            var departureTime = DateTime.Now.AddHours(i);
            _tis.InsertTrain(number, destination, departureTime);
        }
    }

    private DrawingVisual DrawTrainInformation()
    {
        var visual = new DrawingVisual();

        using (var dc = visual.RenderOpen())
        {
            DrawTree(dc);
        }

        return visual;
    }

    private void AddVisualToCanvas(DrawingVisual visual)
    {
        var img = new Image { Source = new DrawingImage(visual.Drawing) };
        canvas.Children.Add(img);
    }

    private void DrawTree(DrawingContext dc, Train train = null, double x = 500, int y = 30, int level = 1, double dx = 300, int dy = 100)
    {
        train ??= _tis.Root;

        if (train != null)
        {
            DrawNode(dc, train, x, y);

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

                DrawTree(dc, train.Left, childX, childY, level + 1, leftDx, leftDy);
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

                DrawTree(dc, train.Right, childX, childY, level + 1, rightDx, rightDy);
                dc.DrawLine(new Pen(Brushes.Black, 2), new Point(x, y + 15), new Point(childX - 10, childY - 15));
            }
        }
    }

    private void DrawNode(DrawingContext dc, Train train, double x, int y)
    {
        dc.DrawEllipse(Brushes.Red, null, new Point(x, y), 15, 15);

        var dpi = VisualTreeHelper.GetDpi(this).PixelsPerDip;
        var formattedText = new FormattedText(train.Number.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 14, Brushes.Black, dpi);
        dc.DrawText(formattedText, new Point(x - 7, y - 7));
    }

    private void AddTrainButton_Click(object sender, RoutedEventArgs e)
    {
        _ = int.TryParse(numberTextBox.Text, out int number);

        var destination = cityTextBox.Text;
        var departureDate = departureDatePicker.SelectedDate;

        if (number == 0 || string.IsNullOrEmpty(destination) || departureDate == null || departureDate < DateTime.Now)
        {
            MessageBox.Show("Один или несколько параметров были заданы неверно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            try
            {
                _tis.InsertTrain(number, destination, (DateTimeOffset)departureDate);
            }
            catch (ArgumentException)
            {
                MessageBox.Show($"Поезд с номером {number} уже существует в системе!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ReDrawTree();
            }
        }
    }

    private void ReDrawTree()
    {
        canvas.Children.Clear();

        _visual = DrawTrainInformation();
        AddVisualToCanvas(_visual);
    }

    private void SearchByIdentifierButton_Click(object sender, RoutedEventArgs e)
    {
        var id = Convert.ToInt32(numberToFindTextBox.Text);

        var trainInformation = $"Поезд с номером {numberToFindTextBox.Text} не найден!";
        var messageBoxCaption = "Ошибка";
        var messageBoxImage = MessageBoxImage.Error;

        if (_tis.FindTrain(id, out var train))
        {
            trainInformation = $"Номер: {train.Number}\r\nСтанция назначения: {train.Destination}\r\nВремя прибытия: {train.DepartureTime}";
            messageBoxCaption = "Результат";
            messageBoxImage = MessageBoxImage.Information;
        }

        MessageBox.Show(trainInformation, messageBoxCaption, MessageBoxButton.OK, messageBoxImage);
    }

    private void SearchByDestinationButton_Click(object sender, RoutedEventArgs e)
    {
        var destination = destinationToFindTextBox.Text;

        var trains = _tis.FindTrainsByDestination(destination);

        var trainsInformation = string.Empty;
        var messageBoxCaption = "Ошибка";
        var messageBoxImage = MessageBoxImage.Error;

        if (trains.Any())
        {
            foreach (var train in trains)
            {
                trainsInformation += $"Номер: {train.Number}\r\nСтанция назначения: {train.Destination}\r\nВремя прибытия: {train.DepartureTime}\r\n\r\n";
                messageBoxCaption = "Результат";
                messageBoxImage = MessageBoxImage.Information;
            }
        }
        else
        {
            trainsInformation = $"Поезда со станцией назначения '{destination}' не найдены!";
        }

        MessageBox.Show(trainsInformation, messageBoxCaption, MessageBoxButton.OK, messageBoxImage);
    }

    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (!char.IsDigit(e.Text, e.Text.Length - 1))
        {
            e.Handled = true;
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            if (!int.TryParse(textBox.Text, out _))
            {
                textBox.Text = string.Empty;
            }
        }
    }
}
