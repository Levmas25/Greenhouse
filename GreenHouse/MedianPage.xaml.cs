using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для MedianPage.xaml
    /// </summary>
    public partial class MedianPage : Page
    {
        public MedianPage()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(UpdateGrid);
            timer.Interval = new TimeSpan(0, 0, 7);
            timer.Start();

            Application.Current.Exit += new ExitEventHandler(OnClosing);

            windowBtn.Content = Properties.Settings.Default.WindowBtnContent.ToString();
            generalHumBtn.Content = Properties.Settings.Default.GeneralHumBtnContent.ToString();

            medianChart.ChartAreas.Add(new System.Windows.Forms.DataVisualization.Charting.ChartArea("Main"));
            medianChart.Titles.Add("Средние показания температуры и влажности со всех датчиков");
            MakeChart();

        }

        private void UpdateGrid(object sender, EventArgs e)
        {
            List<double> airMedian = AirRequest();
            double soilHum = SoilRequest();
            MedianRow current = new MedianRow(DateTime.Now.ToString(), airMedian[0], airMedian[1], soilHum);

            medianGrid.Items.Insert(0, current);
            if (medianGrid.Items.Count > 15)
            {
                medianGrid.Items.Remove(medianGrid.Items[14]);
            }
            
            string now = DateTime.Now.ToString();
            medianChart.Series["Температура воздуха"].Points.AddXY(now, airMedian[0]);
            medianChart.Series["Влажность воздуха"].Points.AddXY(now, airMedian[1]);
            medianChart.Series["Влажность почвы"].Points.AddXY(now, soilHum);

            if (Properties.Settings.Default.ExtraMode == 0)
            {
                if (airMedian[0] < Properties.Settings.Default.MinTemperature && windowBtn.Content.ToString() == "Открыть форточки")
                {
                    windowBtn.IsEnabled = false;
                }
                else
                {
                    windowBtn.IsEnabled = true;
                }

                if (airMedian[1] > Properties.Settings.Default.MaxHumidity)
                {
                    generalHumBtn.IsEnabled = false;
                }
                else
                {
                    generalHumBtn.IsEnabled = true;
                }
            }
        }

        private void GridLoaded(object sender, RoutedEventArgs e)
        {
            UpdateGrid(sender, e);
        }

        public double SoilRequest()
        {
            HttpClient client = new HttpClient();

            List<double> hums = new List<double>();

            for (int i = 1; i < 7; i++)
            {
                string response = client.GetStringAsync($"https://dt.miet.ru/ppo_it/api/hum/{i}").Result;
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

                double humidity = Convert.ToDouble(values["humidity"]);
                hums.Add(humidity);
            }

            double result = Math.Round(hums.Sum() / hums.Count(), 2);
            
            return result;
        }

        public List<double> AirRequest()
        {
            HttpClient client = new HttpClient();

            List<double> temp = new List<double>();
            List<double> hum = new List<double>();
            List<double> result = new List<double>();

            for (int i = 1; i < 5; i++)
            {
                string response = client.GetStringAsync($"https://dt.miet.ru/ppo_it/api/temp_hum/{i}").Result;
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

                temp.Add(Convert.ToDouble((values["temperature"])));
                hum.Add(Convert.ToDouble(values["humidity"]));

            }
            result.Add(Math.Round(temp.Sum() / temp.Count(), 2));
            result.Add(Math.Round(hum.Sum() / hum.Count(), 2));
            return result;
        }

        private void MakeChart()
        {
            var airTemp = new Series("Температура воздуха")
            {
                IsVisibleInLegend = true,
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Line,
            };
            medianChart.Series.Add(airTemp);

            var airHum = new Series("Влажность воздуха")
            {
                IsVisibleInLegend = true,
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Line,
            };
            medianChart.Series.Add(airHum);

            var soilHum = new Series("Влажность почвы")
            {
                IsVisibleInLegend = true,
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Line,
            };
            medianChart.Series.Add(soilHum);

            medianChart.Legends.Add(new Legend("First"));
        }

        private void PatchRequest(string uri, int state)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{uri}&state={state}");

                try
                {
                    var response = client.SendAsync(request);
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private void windowBtn_Click(object sender, EventArgs e)
        {
            if (windowBtn.Content.ToString() == "Открыть форточки")
            {

                PatchRequest("https://dt.miet.ru/ppo_it/api/fork_drive", 1);
                windowBtn.Content = "Закрыть форточки";
            }
            else
            {
                PatchRequest("https://dt.miet.ru/ppo_it/api/fork_drive", 0);
                windowBtn.Content = "Открыть форточки";
            }


        }

        private void OnClosing(object sender, ExitEventArgs e)
        {
            Properties.Settings.Default.WindowBtnContent = windowBtn.Content.ToString();
            Properties.Settings.Default.GeneralHumBtnContent = generalHumBtn.Content.ToString();
            Properties.Settings.Default.ExtraMode = 0;
            Properties.Settings.Default.Save();

        }

        private void generalHumBtn_Click(object sender, RoutedEventArgs e)
        {
            if (generalHumBtn.Content.ToString() == "Открыть увлажнение")
            {
                PatchRequest("https://dt.miet.ru/ppo_it/api/total_hum", 1);
                generalHumBtn.Content = "Закрыть увлажнение";
            }
            else
            {
                PatchRequest("https://dt.miet.ru/ppo_it/api/total_hum", 0);
                generalHumBtn.Content = "Открыть увлажнение";
            }
        }
    }
}
