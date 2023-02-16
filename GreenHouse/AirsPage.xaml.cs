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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Forms.DataVisualization.Charting;

namespace GreenHouse
{
    /// <summary>
    /// Логика взаимодействия для AirsPage.xaml
    /// </summary>
    public partial class AirsPage : Page
    {
        public AirsPage()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(UpdateGrid);
            timer.Interval = new TimeSpan(0, 0, 3);
            timer.Start();

            typeSelection.Items.Add("Температура");
            typeSelection.Items.Add("Влажность");

            AirsChart.ChartAreas.Add(new ChartArea("Main"));

            var firstSens = new Series("First")
            {
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Line,
            };

            AirsChart.Series.Add(firstSens);

            var secondSens = new Series("Second")
            {
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Line,
            };
            AirsChart.Series.Add(secondSens);

            var thirdSens = new Series("Third")
            {
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Line,
            };
            AirsChart.Series.Add(thirdSens);

            var fourthSens = new Series("Fourth")
            {
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Line,
            };
            AirsChart.Series.Add(fourthSens);
        }

        private void UpdateGrid(object sender, EventArgs e)
        {
            List<AirRow> airRows = new List<AirRow>();

            airRows.Add(GetRequest(1));
            airRows.Add(GetRequest(2));
            airRows.Add(GetRequest(3));
            airRows.Add(GetRequest(4));

            airsGrid.ItemsSource = airRows;

            if (typeSelection.SelectedItem.ToString() == "Температура")
            {
                AirsChart.Series["First"].Points.AddXY(DateTime.Now.ToString(), airRows[0].Temperature);
                AirsChart.Series["Second"].Points.AddXY(DateTime.Now.ToString(), airRows[1].Temperature);
                AirsChart.Series["Third"].Points.AddXY(DateTime.Now.ToString(), airRows[2].Temperature);
                AirsChart.Series["Fourth"].Points.AddXY(DateTime.Now.ToString(), airRows[3].Temperature);
            }
        }

        private void GridLoaded(object sender, RoutedEventArgs e)
        {
            UpdateGrid(sender, e);
        }

        private AirRow GetRequest(int Id)
        {
            HttpClient client = new HttpClient();

            string response = client.GetStringAsync($"https://dt.miet.ru/ppo_it/api/temp_hum/{Id}").Result;
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            int id = Convert.ToInt32(values["id"]);
            double temperature = Convert.ToDouble((values["temperature"]));
            double humidity = Convert.ToDouble(values["humidity"]);

            AirRow result = new AirRow(id, temperature, humidity);
            return result;
        }
    }
}
