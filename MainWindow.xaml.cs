using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TIS;

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
            dc.DrawTree(this, _tis);
        }

        return visual;
    }

    private void AddVisualToCanvas(DrawingVisual visual)
    {
        var img = new Image
        {
            Source = new DrawingImage(visual.Drawing)
        };

        canvas.Children.Add(img);
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

    private void SearchByNumberButton_Click(object sender, RoutedEventArgs e)
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
