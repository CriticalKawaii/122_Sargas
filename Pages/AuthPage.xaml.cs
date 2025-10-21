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
    /// Страница авторизации пользователей.
    /// </summary>
    public partial class AuthPage : Page
    {
        /// <summary>
        /// Счётчик неудачных попыток входа
        /// </summary>
        private int failedAttempts = 0;

        /// <summary>
        /// Текущий пользователь (если авторизован)
        /// </summary>
        private User currentUser;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AuthPage"/>
        /// </summary>
        public AuthPage()
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
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки входа.
        /// Проверяет учётные данные пользователя и выполняет вход в систему.
        /// После 3 неудачных попыток активируется капча.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Введите логин или пароль");
                return;
            }

            string hashedPassword = GetHash(PasswordBox.Password);

            using (var db = new DBEntities())
            {
                var user = db.Users.AsNoTracking().FirstOrDefault(u => u.Login.Trim() == TextBoxLogin.Text && u.Password.Trim() == hashedPassword);

                if (user == null)
                {
                    MessageBox.Show("Пользователь с такими данными не найден!");
                    failedAttempts++;
                    if (failedAttempts >= 3)
                    {
                        if (captcha.Visibility != Visibility.Visible)
                        {
                            CaptchaSwitch();
                        }
                        CaptchaChange();
                    }
                    return;
                }
                else
                {

                    switch (user.Role)
                    {
                        case "User":
                            NavigationService?.Navigate(new UserPage());
                            break;
                        case "Admin":
                            NavigationService?.Navigate(new AdminPage());
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Переключает видимость элементов капчи и формы входа
        /// </summary>
        private void CaptchaSwitch()
        {
            switch (captcha.Visibility)
            {
                case Visibility.Visible:
                    TextBoxLogin.Clear();
                    PasswordBox.Clear();

                    captcha.Visibility = Visibility.Collapsed;
                    captchaInput.Visibility = Visibility.Collapsed;
                    textBlockCaptcha.Visibility = Visibility.Collapsed;
                    submitCaptcha.Visibility = Visibility.Collapsed;

                    textBlockLogin.Visibility = Visibility.Visible;
                    textBlockPassword.Visibility = Visibility.Visible;
                    TextBoxLogin.Visibility = Visibility.Visible;
                    PasswordBox.Visibility = Visibility.Visible;
                    
                    textBlockNoAcc.Visibility = Visibility.Visible;
                    textBlockForgotPass.Visibility = Visibility.Visible;

                    ButtonChangePassword.Visibility = Visibility.Visible;
                    ButtonEnter.Visibility = Visibility.Visible;
                    ButtonReg.Visibility = Visibility.Visible;
                    return;
                case Visibility.Collapsed:
                    captcha.Visibility = Visibility.Visible;
                    captchaInput.Visibility = Visibility.Visible;
                    textBlockCaptcha.Visibility = Visibility.Visible;
                    submitCaptcha.Visibility = Visibility.Visible;

                    textBlockLogin.Visibility = Visibility.Collapsed;
                    textBlockPassword.Visibility = Visibility.Collapsed;
                    TextBoxLogin.Visibility = Visibility.Collapsed;
                    PasswordBox.Visibility = Visibility.Collapsed;

                    textBlockNoAcc.Visibility = Visibility.Collapsed;
                    textBlockForgotPass.Visibility = Visibility.Collapsed;

                    ButtonChangePassword.Visibility = Visibility.Collapsed;
                    ButtonEnter.Visibility = Visibility.Collapsed;
                    ButtonReg.Visibility = Visibility.Collapsed;
                    return;
            }
        }

        /// <summary>
        /// Генерирует новый случайный код капчи длиной 6 символов.
        /// </summary>
        private void CaptchaChange()
        {
            String allowchar = " ";
            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";
            char[] a = { ',' };
            String[] ar = allowchar.Split(a);
            String pwd = "";
            string temp = "";
            Random r = new Random();

            for (int i = 0; i < 6; i++)
            {
                temp = ar[(r.Next(0, ar.Length))];
                pwd += temp;
            }
            captcha.Text = pwd;
        }

        /// <summary>
        /// Обработчик нажатия кнопки подтверждения капчи.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void submitCaptcha_Click(object sender, RoutedEventArgs e)
        {
            if (captchaInput.Text != captcha.Text)
            {
                MessageBox.Show("Неверно введена капча", "Ошибка");
                CaptchaChange();
            }
            else
            {
                MessageBox.Show("Капча введена успешно, можете продолжить авторизацию", "Успех");

                CaptchaSwitch();
                failedAttempts = 0;
            }
        }

        /// <summary>
        /// Запрещает операции копирования, вырезания и вставки в поле капчи
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void TextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
        e.Command == ApplicationCommands.Cut ||
        e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки регистрации.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void ButtonReg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegPage());
        }

        /// <summary>
        /// Обработчик нажатия кнопки смены пароля.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void ButtonChangePassword_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ChangePassPage());
        }
    }
}
