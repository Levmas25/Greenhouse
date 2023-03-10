using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenHouse
{
    public class AirRow
    {
        public string Date { get; set; }
        public int Id { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }

        public AirRow(int Id, double Temperature, double Humidity, string Date)
        {
            this.Date = Date;
            this.Id = Id;
            this.Temperature = Temperature;
            this.Humidity = Humidity;
        }
    }
}
