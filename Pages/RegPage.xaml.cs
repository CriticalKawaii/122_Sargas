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
    /// Interaction logic for RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
            comboBoxRole.SelectedIndex = 0;
        }

        public static string GetHash(String password)
        {
            using (var hash = SHA1.Create())
            {
                return
                string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x =>
                x.ToString("X2")));
            }
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AuthPage());
        }

        private void ButtonReg_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text) ||
string.IsNullOrEmpty(TextBoxFIO.Text) || string.IsNullOrEmpty(PasswordBox.Password) ||
string.IsNullOrEmpty(PasswordBoxConfirm.Password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            DBEntities db = new DBEntities();
            {
                var user = db.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Login == TextBoxLogin.Text);
                if (user != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!");
                    return;
                }
            }

            if (PasswordBox.Password.Length >= 6)
            {
                bool en = true;
                bool number = false;
                for (int i = 0; i < PasswordBox.Password.Length; i++)
                {
                    if (PasswordBox.Password[i] >= '0' && PasswordBox.Password[i] <= '9') number =
                    true;
                    else if (!((PasswordBox.Password[i] >= 'A' && PasswordBox.Password[i] <=
                    'Z') || (PasswordBox.Password[i] >= 'a' && PasswordBox.Password[i] <= 'z'))) en =
                    false;
                }
                if (!en)
                    MessageBox.Show("Используйте только английскую расскладку!");
                else if (!number)
                    MessageBox.Show("Добавьте хотябы одну цифру!");
                else
                    MessageBox.Show("Пароль слишком короткий, должно быть минимум 6 символов!");

                if (en && number)
                {
                    if (PasswordBox.Password != PasswordBoxConfirm.Password)
                    {
                        MessageBox.Show("Пароли не совпадают!");
                    }
                }
                else
                {
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
                    comboBoxRole.SelectedIndex = 1;
                    TextBoxFIO.Clear();
                    return;
                }
            }

        }
    }
}
