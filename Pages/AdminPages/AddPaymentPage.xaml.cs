using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace _122_Sargas.Pages.AdminPages
{
    /// <summary>
    /// Interaction logic for AddPaymentPage.xaml
    /// </summary>
    public partial class AddPaymentPage : Page
    {
        /// <summary>
        /// Текущий редактируемый или создаваемый платёж
        /// </summary>
        private Payment _currentPayment = new Payment();

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddPaymentPage"/>.
        /// Загружает списки категорий и пользователей для выбора.
        /// </summary>
        /// <param name="selectedPayment">
        /// Платёж для редактирования. Если null, создаётся новый платёж.
        /// </param>
        /// <remarks>
        /// Заполняет ComboBox категориями и пользователями из базы данных.
        /// При передаче существующего платежа страница работает в режиме редактирования.
        /// </remarks>
        public AddPaymentPage(Payment selectedPayment)
        {
            InitializeComponent();
            CBCategory.ItemsSource = DBEntities.GetContext().Categories.ToList();
            CBCategory.DisplayMemberPath = "Name";

            CBUser.ItemsSource = DBEntities.GetContext().Users.ToList();
            CBUser.DisplayMemberPath = "FIO";

            if (selectedPayment != null)
                _currentPayment = selectedPayment;

            DataContext = _currentPayment;
        }

        /// <summary>
        /// Обработчик нажатия кнопки сохранения платежа.
        /// Проверяет валидность всех данных и сохраняет платёж в базу данных.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        /// <remarks>
        /// Обязательные поля для заполнения:
        /// - Дата платежа
        /// - Количество
        /// - Цена
        /// - Пользователь (UserID)
        /// - Категория (CategoryID)
        /// Если ID платежа = 0, добавляет новую запись в базу данных.
        /// В противном случае обновляет существующую запись.
        /// </remarks>
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentPayment.Date.ToString()))
                errors.AppendLine("Укажите дату!");
            if (string.IsNullOrWhiteSpace(_currentPayment.Num.ToString()))
                errors.AppendLine("Укажите количество!");
            if (string.IsNullOrWhiteSpace(_currentPayment.Price.ToString()))
                errors.AppendLine("Укажите цену");
            if (string.IsNullOrWhiteSpace(_currentPayment.UserID.ToString()))
                errors.AppendLine("Укажите клиента!");
            if (string.IsNullOrWhiteSpace(_currentPayment.CategoryID.ToString()))
                errors.AppendLine("Укажите категорию!");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentPayment.ID == 0)
                DBEntities.GetContext().Payments.Add(_currentPayment);

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
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void ButtonClean_Click(object sender, RoutedEventArgs e)
        {
            TBPaymentName.Text = "";
            TBAmount.Text = "";
            TBCount.Text = "";
            TBDate.Text = "";
            CBCategory.Text = "";
            CBUser.Text = "";
        }
    }
}
