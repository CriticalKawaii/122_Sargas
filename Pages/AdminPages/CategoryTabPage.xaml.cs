using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace _122_Sargas.Pages.AdminPages
{
    /// <summary>
    /// Interaction logic for CategoryTabPage.xaml
    /// </summary>
    public partial class CategoryTabPage : Page
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CategoryTabPage"/>.
        /// </summary>
        public CategoryTabPage()
        {
            InitializeComponent();
            DataGridCategory.ItemsSource = DBEntities.GetContext().Categories.ToList();
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
        /// Это необходимо для отображения изменений, сделанных на других страницах.
        /// </remarks>
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                DBEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                DataGridCategory.ItemsSource = DBEntities.GetContext().Categories.ToList();
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки редактирования категории.
        /// Переходит на страницу редактирования выбранной категории.
        /// </summary>
        /// <param name="sender">Кнопка редактирования в строке DataGrid</param>
        /// <param name="e">Аргументы события</param>
        /// <remarks>
        /// Извлекает объект Category из DataContext кнопки и передаёт его в конструктор страницы редактирования.
        /// </remarks>
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddCategoryPage((sender as Button).DataContext as Category));
        }

        /// <summary>
        /// Обработчик нажатия кнопки добавления новой категории.
        /// Переходит на страницу создания новой категории.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddCategoryPage(null));
        }

        /// <summary>
        /// Обработчик нажатия кнопки удаления категорий.
        /// Удаляет выбранные в таблице категории после подтверждения.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        /// <remarks>
        /// Запрашивает подтверждение у пользователя перед удалением.
        /// Показывает количество выбранных для удаления элементов.
        /// При возникновении ошибки (например, категория используется в платежах) показывает сообщение об ошибке.
        /// </remarks>
        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            var categoryForRemoving = DataGridCategory.SelectedItems.Cast<Category>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {categoryForRemoving.Count()} элементов ? ", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {

                    DBEntities.GetContext().Categories.RemoveRange(categoryForRemoving);
                    DBEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                    DataGridCategory.ItemsSource = DBEntities.GetContext().Categories.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
