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
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AdminPage"/>
        /// </summary>
        public AdminPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Обработчик нажатия кнопки "Таблица пользователей".
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void ButtonTab1e_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new UsersTabPage());
        /// <summary>
        /// Обработчик нажатия кнопки "Таблица категорий".
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void ButtonTab1e2_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new CategoryTabPage());
        /// <summary>
        /// Обработчик нажатия кнопки "Таблица платежей".
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void ButtonTab1e3_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new PaymentTabPage());
        /// <summary>
        /// Обработчик нажатия кнопки "Диаграммы".
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void ButtonTab1e4_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new DiagrammPage());
    }
}
