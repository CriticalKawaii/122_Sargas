using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace _122_Sargas.Pages
{
    /// <summary>
    /// Interaction logic for RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RegPage"/>
        /// </summary>
        public RegPage()
        {
            InitializeComponent();
            comboBoxRole.SelectedIndex = 0;
        }

        /// <summary>
        /// Вычисляет хеш-значение пароля с использованием алгоритма SHA1
        /// </summary>
        /// <param name="password">Пароль для хеширования</param>
        /// <returns>Хеш-значение в виде строки в шестнадцатеричном формате</returns>
        public static string GetHash(String password)
        {
            using (var hash = SHA1.Create())
            {
                return
                string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x =>
                x.ToString("X2")));
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку перехода на страницу авторизации
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AuthPage());
        }

        private void ButtonReg_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text) ||
                string.IsNullOrEmpty(TextBoxFIO.Text) ||
                string.IsNullOrEmpty(PasswordBox.Password) ||
                string.IsNullOrEmpty(PasswordBoxConfirm.Password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            using (DBEntities db = new DBEntities())
            {
                var user = db.Users
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Login == TextBoxLogin.Text);

                if (user != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!");
                    return;
                }

                if (PasswordBox.Password.Length < 6)
                {
                    MessageBox.Show("Пароль слишком короткий, должно быть минимум 6 символов!");
                    return;
                }

                bool en = true;
                bool number = false;

                for (int i = 0; i < PasswordBox.Password.Length; i++)
                {
                    if (PasswordBox.Password[i] >= '0' && PasswordBox.Password[i] <= '9')
                    {
                        number = true;
                    }
                    else if (!((PasswordBox.Password[i] >= 'A' && PasswordBox.Password[i] <= 'Z') ||
                               (PasswordBox.Password[i] >= 'a' && PasswordBox.Password[i] <= 'z')))
                    {
                        en = false;
                    }
                }

                if (!en)
                {
                    MessageBox.Show("Используйте только английскую раскладку!");
                    return;
                }

                if (!number)
                {
                    MessageBox.Show("Добавьте хотя бы одну цифру!");
                    return;
                }

                if (PasswordBox.Password != PasswordBoxConfirm.Password)
                {
                    MessageBox.Show("Пароли не совпадают!");
                    return;
                }

                User userObject = new User
                {
                    FIO = TextBoxFIO.Text,
                    Login = TextBoxLogin.Text,
                    Password = GetHash(PasswordBox.Password),
                    Role = comboBoxRole.Text
                };

                db.Users.Add(userObject);
                db.SaveChanges();

                MessageBox.Show("Пользователь успешно зарегистрирован!");

                TextBoxLogin.Clear();
                PasswordBox.Clear();
                PasswordBoxConfirm.Clear();
                TextBoxFIO.Clear();
                comboBoxRole.SelectedIndex = 1;

                NavigationService?.Navigate(new AuthPage());
            }
        }
    }
}
