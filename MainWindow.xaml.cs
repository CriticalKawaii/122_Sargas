using System;
using System.Linq;
using System.Windows;
using _122_Sargas.Pages;

namespace _122_Sargas
{
    /// <summary>
    /// Главное окно приложения.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MainWindow"/>.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            MainFrame?.Navigate(new AuthPage());
        }
        /// <summary>
        /// Обработчик события загрузки окна.
        /// Запускает таймер для обновления текущего времени каждую секунду.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (o, t) => { DateTimeNow.Text = DateTime.Now.ToString(); };
            timer.Start();
        }

        /// <summary>
        /// Обработчик события закрытия окна.
        /// Запрашивает подтверждение пользователя перед закрытием приложения.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события, позволяющие отменить закрытие</param>
        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите закрыть окно?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                e.Cancel = true;
            else
                e.Cancel = false;
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Назад".
        /// Выполняет навигацию на предыдущую страницу, если это возможно.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
                MainFrame?.GoBack();
        }

        /// <summary>
        /// Обработчик нажатия кнопки переключения между светлой (Dictionary.xaml) и тёмной (DictionaryDark.xaml) темами оформления.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void ButtonTheme_Click(object sender, RoutedEventArgs e)
        {
            var lightThemeUri = new Uri("/ResourceDictionaries/Dictionary.xaml", UriKind.Relative);
            var darkThemeUri = new Uri("/ResourceDictionaries/DictionaryDark.xaml", UriKind.Relative);

            var appResources = Application.Current.Resources.MergedDictionaries;

            var currentTheme = appResources.FirstOrDefault(d => d.Source != null &&
                (d.Source.OriginalString.Contains("Dictionary.xaml") || d.Source.OriginalString.Contains("DictionaryDark.xaml"))
            );

            Uri newThemeUri;

            if (currentTheme == null || currentTheme.Source.OriginalString.Contains("Dictionary.xaml"))
                newThemeUri = darkThemeUri;
            else
                newThemeUri = lightThemeUri;

            var newTheme = new ResourceDictionary() { Source = newThemeUri };

            if (currentTheme != null)
                appResources.Remove(currentTheme);

            appResources.Add(newTheme);
        }

    }
}
