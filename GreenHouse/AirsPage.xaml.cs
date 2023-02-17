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
            AirsChart.Titles.Add("Показания температуры с датчиков воздуха");

            var firstSens = new Series("Первый")
            {
                IsVisibleInLegend = true,
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Line,
            };

            AirsChart.Series.Add(firstSens);

            var secondSens = new Series("Второй")
            {
                IsVisibleInLegend = true,
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Line,
            };
            AirsChart.Series.Add(secondSens);

            var thirdSens = new Series("Третий")
            {
                IsVisibleInLegend = true,
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Line,
            };
            AirsChart.Series.Add(thirdSens);

            var fourthSens = new Series("Четвёртый")
            {
                IsVisibleInLegend = true,
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Line,
            };
            AirsChart.Series.Add(fourthSens);

            AirsChart.Legends.Add(new Legend("First"));
            typeSelection.SelectedIndex = 0;
        }


        private void UpdateGrid(object sender, EventArgs e)
        {
            List<AirRow> airRows = new List<AirRow>();

            airRows.Add(GetRequest(1));
            airRows.Add(GetRequest(2));
            airRows.Add(GetRequest(3));
            airRows.Add(GetRequest(4));

            airsGrid.ItemsSource = airRows;


            if (typeSelection.SelectedIndex == 0)
            {
                AirsChart.Series["Первый"].Points.AddXY(DateTime.Now.ToString(), airRows[0].Temperature);
                AirsChart.Series["Второй"].Points.AddXY(DateTime.Now.ToString(), airRows[1].Temperature);
                AirsChart.Series["Третий"].Points.AddXY(DateTime.Now.ToString(), airRows[2].Temperature);
                AirsChart.Series["Четвёртый"].Points.AddXY(DateTime.Now.ToString(), airRows[3].Temperature);
            }
            else
            {
                AirsChart.Series["Первый"].Points.AddXY(DateTime.Now.ToString(), airRows[0].Humidity);
                AirsChart.Series["Второй"].Points.AddXY(DateTime.Now.ToString(), airRows[1].Humidity);
                AirsChart.Series["Третий"].Points.AddXY(DateTime.Now.ToString(), airRows[2].Humidity);
                AirsChart.Series["Четвёртый"].Points.AddXY(DateTime.Now.ToString(), airRows[3].Humidity);
            }
            
        }

        private void TypeSelection_Changed(object sender, RoutedEventArgs e)
        {
            if (typeSelection.SelectedIndex == 1)
            {
                AirsChart.Titles[0].Text = "Показания влажности с датчиков воздуха";
            }
            else
            {
                AirsChart.Titles[0].Text = "Показания температуры с датчиков воздуха";
            }
            AirsChart.Series["Первый"].Points.Clear();
            AirsChart.Series["Второй"].Points.Clear();
            AirsChart.Series["Третий"].Points.Clear();
            AirsChart.Series["Четвёртый"].Points.Clear();
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

        private void typeSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
