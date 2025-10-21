using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for ChangePassPage.xaml
    /// </summary>
    public partial class ChangePassPage : Page
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ChangePassPage"/>
        /// </summary>
        public ChangePassPage()
        {
            InitializeComponent();
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
        /// Обработчик нажатия кнопки для смены пароля
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBoxOld.Password) ||
                string.IsNullOrEmpty(PasswordBoxNew.Password) ||
                string.IsNullOrEmpty(PasswordBoxNewConfirm.Password) ||
                string.IsNullOrEmpty(TextBoxLogin.Text))
            {
                MessageBox.Show("Все поля обязательны к заполнению!");
                return;
            }

            string hashedPass = GetHash(PasswordBoxOld.Password);
            var user = DBEntities.GetContext().Users
                .FirstOrDefault(u => u.Login == TextBoxLogin.Text && u.Password == hashedPass);

            if (user == null)
            {
                MessageBox.Show("Текущий пароль/Логин неверный!");
                return;
            }

            if (PasswordBoxNew.Password.Length < 6)
            {
                MessageBox.Show("Пароль слишком короткий, должно быть минимум 6 символов!");
                return;
            }

            bool en = true;
            bool number = false;

            for (int i = 0; i < PasswordBoxNew.Password.Length; i++)
            {
                if (PasswordBoxNew.Password[i] >= '0' && PasswordBoxNew.Password[i] <= '9')
                {
                    number = true;
                }
                else if (!((PasswordBoxNew.Password[i] >= 'A' && PasswordBoxNew.Password[i] <= 'Z') ||
                           (PasswordBoxNew.Password[i] >= 'a' && PasswordBoxNew.Password[i] <= 'z')))
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

            if (PasswordBoxNew.Password != PasswordBoxNewConfirm.Password)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }

            if (en && number)
            {
                user.Password = GetHash(PasswordBoxNew.Password);
                DBEntities.GetContext().SaveChanges();
                MessageBox.Show("Пароль успешно изменен!");
                NavigationService?.Navigate(new AuthPage());
            }
        }
    }
}
