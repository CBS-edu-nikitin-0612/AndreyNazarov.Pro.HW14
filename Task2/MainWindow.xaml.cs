using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;


namespace Task2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await ConnectAsync();
            }
            catch (Exception exception)
            {
                TextBox.Dispatcher.Invoke(() => TextBox.Text = exception.Message);
                _tokenSource = new CancellationTokenSource();
            }

        }

        private void ButtonDisconnect_Click(object sender, RoutedEventArgs e)
        {
            _tokenSource.Cancel();
        }

        private async Task ConnectAsync()
        {
            try
            {
                await Task.Run(Connect, _tokenSource.Token);
                await GetDataAsync();
            }
            catch (Exception e)
            {
                throw;
            }

        }

        private bool Connect()
        {
            for (int i = 0; i < 5; i++)
                try
                {
                    _tokenSource.Token.ThrowIfCancellationRequested();
                    TextBox.Dispatcher.Invoke(() => TextBox.Text = $"Соединение... {i}");
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    throw;
                }

            TextBox.Dispatcher.Invoke(() => TextBox.Text = "Подключен к базе данных");
            return true;
        }

        private async Task GetDataAsync()
        {
            Task task = new Task(() =>
            {
                int i = 1;
                try
                {
                    while (true)
                    {
                        _tokenSource.Token.ThrowIfCancellationRequested();
                        Thread.Sleep(100);
                        TextBoxData.Dispatcher.Invoke(() => TextBoxData.Text = "Данные получены " + i);
                        i++;
                    }
                }
                catch (Exception e)
                {
                    TextBoxData.Dispatcher.Invoke(() => TextBoxData.Text = e.Message);
                    TextBox.Dispatcher.Invoke(() => TextBox.Text = "Соединнение разорвано");
                    _tokenSource = new CancellationTokenSource();
                }
            });

            task.Start();
            await task;
        }
    }
}
