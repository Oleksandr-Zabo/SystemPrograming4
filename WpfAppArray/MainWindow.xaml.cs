using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace SystemProgramming4
{
    public partial class MainWindow : Window
    {
        private short[] _numbers;
        private short _maximum;
        private short _minimum;
        private double _average;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateNumbers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int count = int.Parse(ElementCountTextBox.Text);

                if (count <= 0)
                {
                    MessageBox.Show("Please enter a positive number for the count of elements.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _numbers = GenerateNumbers(count);
        
                // Display all numbers
                ResultsTextBox.Text = "Numbers Generated:\n";
                ResultsTextBox.Text += string.Join(", ", _numbers) + "\n"; // No truncation
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CalculateStats_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_numbers == null || _numbers.Length == 0)
                {
                    MessageBox.Show("No numbers generated yet. Please generate numbers first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _maximum = _numbers.Max();
                _minimum = _numbers.Min();
                _average = (double)Sum(_numbers) / _numbers.Length;

                ResultsTextBox.Text += "\nStatistics:\n";
                ResultsTextBox.Text += $"Maximum: {_maximum}\n";
                ResultsTextBox.Text += $"Minimum: {_minimum}\n";
                ResultsTextBox.Text += $"Average: {_average:F2}\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Calculation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void WriteToFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_numbers == null || _numbers.Length == 0)
                {
                    MessageBox.Show("No numbers generated yet. Please generate numbers first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                WriteToFile("output.txt");
                MessageBox.Show("Results written to output.txt successfully!", "File Written", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private short[] GenerateNumbers(int count)
        {
            Random random = new Random();
            short[] generateNumbers = new short[count];
            for (int i = 0; i < count; i++)
            {
                generateNumbers[i] = (short)random.Next(1, 10001); // Random numbers between 1 and 10,000
            }
            return generateNumbers;
        }

        private long Sum(short[] numbers)
        {
            long sum = 0;
            foreach (short num in numbers)
            {
                sum += num;
            }
            return sum;
        }

        private void WriteToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Numbers:");
                foreach (short num in _numbers)
                {
                    writer.WriteLine(num);
                }
                writer.WriteLine();
                writer.WriteLine($"Maximum: {_maximum}");
                writer.WriteLine($"Minimum: {_minimum}");
                writer.WriteLine($"Average: {_average:F2}");
            }
        }
    }
}
