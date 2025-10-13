using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
    /// Interaction logic for AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private int failedAttempts = 0;
        private User currentUser;

        public AuthPage()
        {
            InitializeComponent();
        }

        public static string GetHash(String password)
        {
            using(var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x=>x.ToString("X2")));
            }
        }

        private void TextBoxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text)) {
                MessageBox.Show("Введите логин или пароль");
                return;
            }

            string hashedPassword = GetHash(PasswordBox.Password);

            using(var db = new DBEntities())
            {
                var user = db.Users.AsNoTracking().FirstOrDefault(u => u.Loign == TextBoxLogin.Text && u.Password == hashedPassword);

                if (user != null) {
                    MessageBox.Show("Пользователь с такими данными не найден!");
                    failedAttempts++;
                } else { 
                    MessageBox.Show("Пользователь успешно найден!");

                    switch (user.Role) {
                        case "User":
                            break;
                        case "Admin":
                            break;
                    }
                }
            }
        }

        private void ButtonReg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegPage());
        }

        private void ButtonChangePassword_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ChangePassPage());
        }

    }
}
