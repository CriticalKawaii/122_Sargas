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

namespace _122_Sargas.Pages.AdminPages
{
    /// <summary>
    /// Interaction logic for UsersTabPage.xaml
    /// </summary>
    public partial class UsersTabPage : Page
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UsersTabPage"/>.
        /// </summary>
        public UsersTabPage()
        {
            InitializeComponent();
            DataGridUser.ItemsSource = DBEntities.GetContext().Users.ToList();
            IsVisibleChanged += Page_IsVisibleChanged;
        }

        /// <summary>
        /// Обработчик изменения видимости страницы.
        /// Обновляет данные в таблице при возврате на страницу.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события с информацией об изменении свойства</param>
        /// <remarks>
        /// Перезагружает все записи из контекста Entity Framework для отображения актуальных данных.
        /// Это необходимо для отображения изменений, сделанных на других страницах.
        /// </remarks>
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                DBEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                DataGridUser.ItemsSource = DBEntities.GetContext().Users.ToList();
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки редактирования пользователя.
        /// Переходит на страницу редактирования выбранного пользователя.
        /// </summary>
        /// <param name="sender">Кнопка редактирования в строке DataGrid</param>
        /// <param name="e">Аргументы события</param>
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddUserPage((sender as Button).DataContext as User));
        }

        /// <summary>
        /// Обработчик нажатия кнопки добавления нового пользователя.
        /// Переходит на страницу создания нового пользователя.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddUserPage(null));
        }

        /// <summary>
        /// Обработчик нажатия кнопки удаления пользователей.
        /// Удаляет выбранных в таблице пользователей после подтверждения.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        /// <remarks>
        /// Запрашивает подтверждение у пользователя перед удалением.
        /// Показывает количество выбранных для удаления элементов.
        /// При возникновении ошибки (например, у пользователя есть платежи) показывает сообщение об ошибке.
        /// </remarks>
        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            var usersForRemoving = DataGridUser.SelectedItems.Cast<User>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {usersForRemoving.Count()} элементов ? ", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DBEntities.GetContext().Users.RemoveRange(usersForRemoving);
                    DBEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                    DataGridUser.ItemsSource = DBEntities.GetContext().Users.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
