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

        public MainWindow()
        {
            InitializeComponent();
            speedChecker = new SpeedChecker();
            speedHistory = new List<SpeedHistoryItem>();
            CheckSpeed();
        }

        private async void CheckSpeed()
        {
            string selectedServerUrl = (ServerComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();

            speedChecker.SetServerUrl(selectedServerUrl);

            double downloadSpeed = await speedChecker.CheckDownloadSpeed();
            double networkInterfaceSpeed = speedChecker.CheckNetworkInterfaceSpeed();
            double webClientSpeed = speedChecker.CheckWebClientSpeed();

            DownloadSpeedTxt.Text = $"Download Speed: {downloadSpeed} Mbps";
            NetworkSpeedTxt.Text = $"Network Interface Speed: {networkInterfaceSpeed} Mbps";
            WebClientSpeedTxt.Text = $"Web Client Speed: {webClientSpeed} Mbps";

            speedHistory.Add(new SpeedHistoryItem
            {
                Timestamp = DateTime.Now,
                DownloadSpeed = downloadSpeed,
                NetworkInterfaceSpeed = networkInterfaceSpeed,
                WebClientSpeed = webClientSpeed
            });

            UpdateHistoryListView();
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

        private void RunTestButton_Click(object sender, RoutedEventArgs e)
        {
            CheckSpeed();
        }
    }
}
    