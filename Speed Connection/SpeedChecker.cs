using SpeedTest.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Speed_Connection
{
    public class SpeedChecker
    {

        private string selectedServerUrl;

    public void SetServerUrl(string serverUrl)
    {
        selectedServerUrl = serverUrl;
    }

        public async Task<double> CheckDownloadSpeed()
        {
            var speed = await FastClient.GetDownloadSpeed(SpeedTest.Net.Enums.SpeedTestUnit.MegaBitsPerSecond);
            return Math.Round(speed.Speed, 2);
        }

        public double CheckNetworkInterfaceSpeed()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                if (adapter.Description.Contains("802.11ac"))
                {
                    return Math.Round(adapter.Speed / 10000000d, 2);
                }
            }
            return 0.0;
        }

        public double CheckWebClientSpeed()
        {
            double[] speeds = new double[5];
            for (int i = 0; i < 5; i++)
            {
                int jQueryFileSize = 261;
                WebClient client = new WebClient();
                DateTime startTime = DateTime.Now;
                client.DownloadFile("http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.js", @"test.js");
                DateTime endTime = DateTime.Now;
                speeds[i] = Math.Round(jQueryFileSize / (endTime - startTime).TotalSeconds, 2);
            }
            File.Delete(@"test.js");
            return Math.Round(speeds.Average() * 0.008f, 2);

        }
    }
}