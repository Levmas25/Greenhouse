using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenHouse
{
    public class SoilRow
    {
        public string Date { get; set; }
        public int Id { get; set; }
        public double Humidity { get; set; }

        public SoilRow(string date, int id, double humidity)
        {
            Date = date;
            Id = id;
            Humidity = humidity;
        }
    }
}
