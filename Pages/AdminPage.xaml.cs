using _122_Sargas.Pages.AdminPages;
using System.Windows;
using System.Windows.Controls;

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
