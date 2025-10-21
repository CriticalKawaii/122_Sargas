using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace _122_Sargas.Pages.AdminPages
{
    /// <summary>
    /// Interaction logic for AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : Page
    {
        /// <summary>
        /// Текущий редактируемый или создаваемый пользователь
        /// </summary>
        private User _currentUser = new User();

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddUserPage"/>.
        /// </summary>
        /// <param name="selectedUser">
        /// Пользователь для редактирования. Если null, создаётся новый пользователь.
        /// </param>
        /// <remarks>
        /// При передаче существующего пользователя страница работает в режиме редактирования.
        /// При передаче null создаётся новый пустой пользователь.
        /// </remarks>
        public AddUserPage(User selectedUser)
        {
            InitializeComponent();

            if (selectedUser != null)
                _currentUser = selectedUser;
            
            DataContext = _currentUser;
        }

        /// <summary>
        /// Обработчик нажатия кнопки сохранения пользователя.
        /// Проверяет валидность всех данных и сохраняет пользователя в базу данных.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        /// <remarks>
        /// Обязательные поля для заполнения:
        /// - Логин
        /// - Пароль
        /// - Роль (Admin или User)
        /// - ФИО
        /// Поле "Фото" является опциональным.
        /// Если ID пользователя = 0, добавляет новую запись в базу данных.
        /// В противном случае обновляет существующую запись.
        /// </remarks>
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentUser.Login))
                errors.AppendLine("Укажите логин!");
            if (string.IsNullOrWhiteSpace(_currentUser.Password))
                errors.AppendLine("Укажите пароль!");
            if ((_currentUser.Role == null) || (cmbRole.Text == ""))
                errors.AppendLine("Выберите роль!");
            else
                _currentUser.Role = cmbRole.Text;
            if (string.IsNullOrWhiteSpace(_currentUser.FIO))
                errors.AppendLine("Укажите ФИО");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentUser.ID == 0)
                DBEntities.GetContext().Users.Add(_currentUser);

            try
            {
                DBEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки очистки формы.
        /// Очищает все поля ввода и сбрасывает выбор роли.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void ButtonClean_Click(object sender, RoutedEventArgs e)
        {
            TBLogin.Text = "";
            TBPass.Text = "";
            cmbRole.SelectedItem = null;
            TBFio.Text = "";
            TBPhoto.Text = "";
        }
    }
}
