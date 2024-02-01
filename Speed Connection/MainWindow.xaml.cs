using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Windows;
using System.IO;
using SpeedTest.Net;
using System.Windows.Controls;
using System.Collections.Generic;

namespace Speed_Connection
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private SpeedChecker speedChecker;
        private List<SpeedHistoryItem> speedHistory;
        private TestParameters testParameters;

        public MainWindow()
        {
            InitializeComponent();
            speedChecker = new SpeedChecker();
            speedHistory = new List<SpeedHistoryItem>();
            testParameters = new TestParameters();
            CheckSpeed();
        }

        private async void CheckSpeed()
        {
            string selectedServerUrl = (ServerComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();
            speedChecker.SetServerUrl(selectedServerUrl);
            speedChecker.SetTestParameters(testParameters);

            double downloadSpeed = await speedChecker.CheckDownloadSpeed();
            double networkInterfaceSpeed = speedChecker.CheckNetworkInterfaceSpeed();
            double webClientSpeed = speedChecker.CheckWebClientSpeed();
            double ping = await speedChecker.CheckPing(); 

            DownloadSpeedTxt.Text = $"Download Speed: {downloadSpeed} Mbps";
            NetworkSpeedTxt.Text = $"Network Interface Speed: {networkInterfaceSpeed} Mbps";
            WebClientSpeedTxt.Text = $"Web Client Speed: {webClientSpeed} Mbps";
            PingTxt.Text = $"Ping: {ping} ms"; 

            speedHistory.Add(new SpeedHistoryItem
            {
                Timestamp = DateTime.Now,
                DownloadSpeed = downloadSpeed,
                NetworkInterfaceSpeed = networkInterfaceSpeed,
                WebClientSpeed = webClientSpeed,
                Ping = ping 
            });

            UpdateHistoryListView();
        }


        private void RunTestButton_Click(object sender, RoutedEventArgs e)
        {
            testParameters.DataSize = int.Parse(DataSizeTextBox.Text);
            testParameters.Repetitions = int.Parse(RepetitionsTextBox.Text);

            CheckSpeed();
        }

        private void UpdateHistoryListView()
        {
            HistoryListView.ItemsSource = null;
            HistoryListView.ItemsSource = speedHistory.OrderByDescending(item => item.Timestamp);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CheckSpeed();
        }

        private void ServerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckSpeed(); 
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            ExportWindow exportWindow = new ExportWindow();
            bool? result = exportWindow.ShowDialog();

            if (result == true)
            {
                string exportDirectory = exportWindow.SelectedDirectory;

                string filePath = Path.Combine(exportDirectory, "SpeedTestResults.txt");
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var item in speedHistory)
                    {
                        writer.WriteLine($"Timestamp: {item.Timestamp}, Download Speed: {item.DownloadSpeed} Mbps, Network Interface Speed: {item.NetworkInterfaceSpeed} Mbps, WebClient Speed: {item.WebClientSpeed} Mbps");
                    }
                }

                MessageBox.Show($"Results exported to: {filePath}");
            }
        }
    }
}
    