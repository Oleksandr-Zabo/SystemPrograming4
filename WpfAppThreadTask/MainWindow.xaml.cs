using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Windows;

namespace ThreadedRangeProcessor
{
    public partial class MainWindow : Window
    {
        private ConcurrentBag<string> _results; // Thread-safe collection to store results
        private object _lockObject = new object();
        private int _current = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void StartProcessing_Click(object sender, RoutedEventArgs e)
        {
            int startRange, endRange, taskCount;

            try
            {
                // Parse user inputs
                startRange = int.Parse(StartRangeTextBox.Text);
                endRange = int.Parse(EndRangeTextBox.Text);
                taskCount = int.Parse(ThreadCountTextBox.Text);

                if (startRange > endRange)
                {
                    (startRange, endRange) = (endRange, startRange);
                }

                _results = new ConcurrentBag<string>();

                ResultsTextBox.Clear();
                ResultsTextBox.AppendText("Starting tasks...\n");

                int rangePerTask = (endRange - startRange + 1) / taskCount;
                Task[] tasks = new Task[taskCount];

                for (int i = 0; i < taskCount; i++)
                {
                    int taskStart = startRange + i * rangePerTask;
                    int taskEnd = (i == taskCount - 1) ? endRange : taskStart + rangePerTask - 1;

                    tasks[i] = Task.Run(() => ProcessRange(taskStart, taskEnd));
                }

                await Task.WhenAll(tasks);

                // Ensure results are appended to the ResultsTextBox after all tasks complete
                foreach (var result in _results)
                {
                    ResultsTextBox.AppendText(result + "\n");
                }

                ResultsTextBox.AppendText("All tasks completed.\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ProcessRange(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                lock (_lockObject)
                {
                    while (_current != i - 1)
                    {
                        Monitor.Wait(_lockObject);
                    }

                    string result = $"Task processed number: {i}";
                    _results.Add(result); // Thread-safe addition

                    _current = i;
                    Monitor.PulseAll(_lockObject);
                }
            }
        }
    }
}
