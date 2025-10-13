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
using System.Windows.Shapes;
using _122_Sargas.Pages;

namespace _122_Sargas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame?.Navigate(new AuthPage());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (o, t) => { DateTimeNow.Text = DateTime.Now.ToString(); };
            timer.Start();
        }

        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите закрыть окно?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                e.Cancel = true;
            else
                e.Cancel = false;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
                MainFrame?.GoBack();
        }

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
