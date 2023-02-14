using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenHouse
{
    public class SoilRow
    {
        public int Id { get; set; }
        public double Humidity { get; set; }

        public SoilRow(int id, double humidity)
        {
            Id = id;
            Humidity = humidity;
        }
    }
}
