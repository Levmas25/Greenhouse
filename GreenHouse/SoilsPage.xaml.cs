using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GreenHouse
{
    /// <summary>
    /// Логика взаимодействия для SoilsPage.xaml
    /// </summary>
    public partial class SoilsPage : Page
    {
        public SoilsPage()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(UpdateGrid);
            timer.Interval = new TimeSpan(0, 0, 3);
            timer.Start();

            SoilsChart.ChartAreas.Add(new ChartArea("Main"));
            SoilsChart.Titles.Add("Показания влажности с датчиков почвы");

            var firstSens = new Series("Первый")
            {
                IsVisibleInLegend = true,
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Line,
            };

            SoilsChart.Series.Add(firstSens);

            var secondSens = new Series("Второй")
            {
                IsVisibleInLegend = true,
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Line,
            };
            SoilsChart.Series.Add(secondSens);

            var thirdSens = new Series("Третий")
            {
                IsVisibleInLegend = true,
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Line,
            };
            SoilsChart.Series.Add(thirdSens);

            var fourthSens = new Series("Четвёртый")
            {
                IsVisibleInLegend = true,
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Line,
            };
            SoilsChart.Series.Add(fourthSens);

            var fifsSens = new Series("Пятый")
            {
                IsVisibleInLegend = true,
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Line,
            };
            SoilsChart.Series.Add(fifsSens);

            var sixthSens = new Series("Шестой")
            {
                IsVisibleInLegend = true,
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Line,
            };
            SoilsChart.Series.Add(sixthSens);

            SoilsChart.Legends.Add(new Legend("First"));
        }

        private void UpdateGrid(object sender, EventArgs e)
        {
            List<SoilRow> soilRows = new List<SoilRow>();

            soilRows.Add(GetRequest(1));
            soilRows.Add(GetRequest(2));
            soilRows.Add(GetRequest(3));
            soilRows.Add(GetRequest(4));
            soilRows.Add(GetRequest(5));
            soilRows.Add(GetRequest(6));

            soilsGrid.ItemsSource = soilRows;

            string now = DateTime.Now.ToString();

            SoilsChart.Series["Первый"].Points.AddXY(now, soilRows[0].Humidity);
            SoilsChart.Series["Второй"].Points.AddXY(now, soilRows[1].Humidity);
            SoilsChart.Series["Третий"].Points.AddXY(now, soilRows[2].Humidity);
            SoilsChart.Series["Четвёртый"].Points.AddXY(now, soilRows[3].Humidity);
            SoilsChart.Series["Пятый"].Points.AddXY(now, soilRows[4].Humidity);
            SoilsChart.Series["Шестой"].Points.AddXY(now, soilRows[5].Humidity);
        }

        private SoilRow GetRequest(int Id)
        {
            HttpClient client = new HttpClient();

            string response = client.GetStringAsync($"https://dt.miet.ru/ppo_it/api/hum/{Id}").Result;
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            int id = Convert.ToInt32(values["id"]);
            double humidity = Convert.ToDouble(values["humidity"]);

            SoilRow result = new SoilRow(id, humidity);
            return result;
        }

        private void GridLoaded(object sender, RoutedEventArgs e)
        {
            UpdateGrid(sender, e);
        }
    }
}
