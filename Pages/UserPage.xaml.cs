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
    /// Interaction logic for UserPage.xaml
    /// </summary>
    /// <remarks>
    /// Функции:
    /// - Поиск по ФИО (нечувствителен к регистру)
    /// - Сортировка по ФИО (по возрастанию/убыванию)
    /// - Фильтр "Только администраторы"
    /// - Отображение фотографий пользователей
    /// </remarks>
    public partial class UserPage : Page
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UserPage"/>.
        /// Загружает список пользователей и настраивает отображение фотографий.
        /// </summary>
        /// <remarks>
        /// При отсутствии фотографии устанавливает placeholder изображение.
        /// </remarks>
        public UserPage()
        {
            InitializeComponent();
            var currentUsers = DBEntities.GetContext().Users.ToList();
            foreach (var user in currentUsers)
            {
                if (string.IsNullOrWhiteSpace(user.Photo))
                    user.Photo = null;
                else
                {
                    user.Photo = "pack://application:,,,/Resources/" + user.Photo;
                }
            }
            ListUser.ItemsSource = currentUsers;
        }

        /// <summary>
        /// Обработчик изменения выбранного типа сортировки.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateUsers();
        /// <summary>
        /// Обработчик снятия галочки фильтра "Только администраторы".
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void onlyAdminCheckBox_Unchecked(object sender, RoutedEventArgs e) => UpdateUsers();
        /// <summary>
        /// Обработчик установки галочки фильтра "Только администраторы".
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void onlyAdminCheckBox_Checked(object sender, RoutedEventArgs e) => UpdateUsers();
        /// <summary>
        /// Обработчик изменения текста в поле поиска по ФИО.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void fioFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => UpdateUsers();

        /// <summary>
        /// Обновляет отображаемый список пользователей с учётом всех фильтров и сортировки.
        /// </summary>
        /// <remarks>
        /// Применяет следующие фильтры и операции:
        /// 1. Поиск по ФИО (нечувствителен к регистру)
        /// 2. Фильтр по роли (только администраторы)
        /// 3. Сортировка по ФИО (возрастание/убывание)
        /// Обрабатывает исключения для предотвращения сбоев при некорректных данных.
        /// </remarks>
        private void UpdateUsers()
        {
            if (!IsInitialized)
            {
                return;
            }
            try
            {
                List<User> currentUsers = DBEntities.GetContext().Users.ToList();

                if (!string.IsNullOrWhiteSpace(fioFilterTextBox.Text))
                {
                    currentUsers = currentUsers.Where(x => x.FIO.ToLower().Contains(fioFilterTextBox.Text.ToLower())).ToList();
                }

                if (onlyAdminCheckBox.IsChecked.Value)
                {
                    currentUsers = currentUsers.Where(x => x.Role == "Admin").ToList();
                }

                ListUser.ItemsSource = (sortComboBox.SelectedIndex == 0) ? currentUsers.OrderBy(x => x.FIO).ToList() : currentUsers.OrderByDescending(x => x.FIO).ToList();
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки очистки фильтров.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        /// <remarks>
        /// Сбрасывает:
        /// - Текст поиска по ФИО
        /// - Сортировку (устанавливает по возрастанию)
        /// - Фильтр "Только администраторы"
        /// </remarks>
        private void clearFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            fioFilterTextBox.Text = "";
            sortComboBox.SelectedIndex = 0;
            onlyAdminCheckBox.IsChecked = false;
        }
    }
}
