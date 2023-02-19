using System;
using System.Collections.Generic;
using System.Linq;
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

namespace GreenHouse
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            generalHum.Text = Properties.Settings.Default.MaxHumidity.ToString();
            temperature.Text = Properties.Settings.Default.MinTemperature.ToString();
            soilHum.Text = Properties.Settings.Default.MaxSoilHum.ToString();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.MaxHumidity = Convert.ToDouble(generalHum.Text);
                Properties.Settings.Default.MinTemperature = Convert.ToDouble(temperature.Text);
                Properties.Settings.Default.MaxSoilHum = Convert.ToDouble(soilHum.Text);
                Properties.Settings.Default.Save();
                MessageBox.Show("Настройки сохранены", "Сохронено", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
