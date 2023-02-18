using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenHouse
{
    internal class MedianRow
    {
        public string date {get; set; } 
        public double medianTemp { get; set; }
        public double medianAirHum { get; set; }
        public double medianSoilHum { get; set; }

        public MedianRow(string date, double medianTemp, double medianAirHum, double medianSoilHum)
        {
            this.date = date;
            this.medianTemp = medianTemp;
            this.medianAirHum = medianAirHum;
            this.medianSoilHum = medianSoilHum;
        }
    }
}
