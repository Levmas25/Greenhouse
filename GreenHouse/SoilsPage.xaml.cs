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
using System.Windows.Markup.Localizer;
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

            MakeChart();

            soilId.SelectedIndex = 0;
            humBtn.Content = Properties.Settings.Default.FirstSoilState;

            for (int i = 1; i < 7; i++)
            {
                soilId.Items.Add(i.ToString());
            }
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

            for (int i = 0; i < 6; i++)
            {
                Indication indication = new Indication();
                indication.Date = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
                indication.Sensor_Id = soilRows[i].Id;
                indication.Humidity = Convert.ToDecimal(soilRows[i].Humidity);
                indication.Sensor_type = 2;
                Utils.db.Indication.Add(indication);
            }
            Utils.db.SaveChanges();

            SoilsChart.Series["Первый"].Points.AddXY(now, soilRows[0].Humidity);
            SoilsChart.Series["Второй"].Points.AddXY(now, soilRows[1].Humidity);
            SoilsChart.Series["Третий"].Points.AddXY(now, soilRows[2].Humidity);
            SoilsChart.Series["Четвёртый"].Points.AddXY(now, soilRows[3].Humidity);
            SoilsChart.Series["Пятый"].Points.AddXY(now, soilRows[4].Humidity);
            SoilsChart.Series["Шестой"].Points.AddXY(now, soilRows[5].Humidity);

            if (Properties.Settings.Default.ExtraMode == 0)
            {
                CheckValue(soilRows[soilId.SelectedIndex].Humidity);
            }
        }

        private void CheckValue(double value)
        {
            if (value > Properties.Settings.Default.MaxSoilHum)
            {
                humBtn.IsEnabled = false;
            }
            else
            {
                humBtn.IsEnabled = true;
            }
        }

        public SoilRow GetRequest(int Id)
        {
            HttpClient client = new HttpClient();

            string response = client.GetStringAsync($"https://dt.miet.ru/ppo_it/api/hum/{Id}").Result;
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            int id = Convert.ToInt32(values["id"]);
            double humidity = Convert.ToDouble(values["humidity"]);

            SoilRow result = new SoilRow(DateTime.Now.ToString(), id, humidity);
            return result;
        }

        private void GridLoaded(object sender, RoutedEventArgs e)
        {
            UpdateGrid(sender, e);
        }

        private void MakeChart()
        {
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

        private void PatchRequest(int id, int state)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"https://dt.miet.ru/ppo_it/api/watering&id={id}&state={state}");

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

        private void SoilSelection_Changed(object sender, RoutedEventArgs e)
        {
            ChangeContent(soilId.SelectedIndex + 1);
            switch (soilId.SelectedIndex)
            {
                case 0:
                    ChangeContent(1);
                    humBtn.Content = Properties.Settings.Default.FirstSoilState;
                    break;
                case 1:
                    ChangeContent(2);
                    humBtn.Content = Properties.Settings.Default.SecondSoilState;
                    break;
                case 2:
                    ChangeContent(3);
                    humBtn.Content = Properties.Settings.Default.ThirdSoilState;
                    break;
                case 3:
                    ChangeContent(4);
                    humBtn.Content = Properties.Settings.Default.FourthSoilState;
                    break;
                case 4:
                    ChangeContent(5);
                    humBtn.Content = Properties.Settings.Default.FifsSoilState;
                    break;
                case 5:
                    ChangeContent(6);
                    humBtn.Content = Properties.Settings.Default.SixthSoilState;
                    break;
            }
            if (soilsGrid.Items.Count > 0 && Properties.Settings.Default.ExtraMode == 0)
            {
                CheckValue(((SoilRow)soilsGrid.Items[soilId.SelectedIndex]).Humidity);
            }

        }

        private void ChangeContent(int id)
        {
            if (humBtn.Content.ToString() == "Открыть полив")
            {
                PatchRequest(id, 1);
                humBtn.Content = "Зарыть полив";
            }
            else
            {
                PatchRequest(id, 0);
                humBtn.Content = "Открыть полив";
            }
        }

        private void humBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (soilId.SelectedIndex)
            {
                case 0:
                    ChangeContent(1);
                    Properties.Settings.Default.FirstSoilState = humBtn.Content.ToString();
                    break;
                case 1:
                    ChangeContent(2);
                    Properties.Settings.Default.SecondSoilState = humBtn.Content.ToString();
                    break;
                case 2:
                    ChangeContent(3);
                    Properties.Settings.Default.ThirdSoilState = humBtn.Content.ToString();
                    break;
                case 3:
                    ChangeContent(4);
                    Properties.Settings.Default.FourthSoilState = humBtn.Content.ToString();
                    break;
                case 4:
                    ChangeContent(5);
                    Properties.Settings.Default.FifsSoilState = humBtn.Content.ToString();
                    break;
                case 5:
                    ChangeContent(6);
                    Properties.Settings.Default.SixthSoilState = humBtn.Content.ToString();
                    break;
            }
            Properties.Settings.Default.Save();
        }
    }
}
