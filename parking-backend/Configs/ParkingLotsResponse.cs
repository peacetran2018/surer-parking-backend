using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace parking_backend.Configs
{
    public class ParkingLotsResponse
    {
        public CarParkItems[] items { get; set; }
    }

    public class CarParkItems
    {
        public DateTime timestamp { get; set; }
        public CarParkData[] carpark_data { get; set; }
    }

    public class CarParkData
    {
        public string carpark_number { get; set; }
        public DateTime update_datetime { get; set; }
        public CarParkInfo[] carpark_info { get; set; }
    }

    public class CarParkInfo
    {
        public string total_lots { get; set; }
        public string lot_type { get; set; }
        public string lots_available { get; set; }
    }
}
