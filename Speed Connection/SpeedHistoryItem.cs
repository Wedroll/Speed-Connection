using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speed_Connection
{
    public class SpeedHistoryItem
    {

        public DateTime Timestamp { get; set; }
        public double DownloadSpeed { get; set; }
        public double NetworkInterfaceSpeed { get; set; }
        public double WebClientSpeed { get; set; }
        public double Ping { get; internal set; }
    }
}
