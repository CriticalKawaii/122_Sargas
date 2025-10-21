using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace _122_Sargas.Pages.AdminPages
{
    /// <summary>
    /// Interaction logic for PaymentTabPage.xaml
    /// </summary>
    public partial class PaymentTabPage : Page
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PaymentTabPage"/>.
        /// </summary>
        public PaymentTabPage()
        {
            InitializeComponent();
            DataGridPayment.ItemsSource = DBEntities.GetContext().Payments.ToList();
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
        /// </remarks>
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                DBEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                DataGridPayment.ItemsSource = DBEntities.GetContext().Payments.ToList();
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки редактирования платежа.
        /// Переходит на страницу редактирования выбранного платежа.
        /// </summary>
        /// <param name="sender">Кнопка редактирования в строке DataGrid</param>
        /// <param name="e">Аргументы события</param>
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPaymentPage((sender as Button).DataContext as Payment));
        }

        /// <summary>
        /// Обработчик нажатия кнопки добавления нового платежа.
        /// Переходит на страницу создания нового платежа.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddPaymentPage(null));
        }

        /// <summary>
        /// Обработчик нажатия кнопки удаления платежей.
        /// Удаляет выбранные в таблице платежи после подтверждения.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        /// <remarks>
        /// Запрашивает подтверждение у пользователя перед удалением.
        /// Показывает количество выбранных для удаления элементов.
        /// При возникновении ошибки показывает сообщение об ошибке.
        /// </remarks>
        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            var paymentForRemoving = DataGridPayment.SelectedItems.Cast<Payment>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве { paymentForRemoving.Count()} элементов ? ", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) 
            {
                try
                {

                    DBEntities.GetContext().Payments.RemoveRange(paymentForRemoving);
                    DBEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                    DataGridPayment.ItemsSource = DBEntities.GetContext().Payments.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
