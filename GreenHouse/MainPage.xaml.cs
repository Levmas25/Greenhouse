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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void airBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AirsPage());
        }

        private void soilBtn_Click(Object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SoilsPage());
        }

        private void medianBtn_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new MedianPage());
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SettingsPage());
        }
    }
}
