using _122_Sargas.Pages.AdminPages;
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

namespace _122_Sargas.Pages
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        private void ButtonTab1e_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new UsersTabPage());

        private void ButtonTab1e2_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new CategoryTabPage());

        private void ButtonTab1e3_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new PaymentTabPage());

        private void ButtonTab1e4_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new DiagrammPage());
    }
}
