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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _122_Sargas.Pages.AdminPages
{
    /// <summary>
    /// Interaction logic for AddCategoryPage.xaml
    /// </summary>
    public partial class AddCategoryPage : Page
    {
        /// <summary>
        /// Текущая редактируемая или создаваемая категория
        /// </summary>
        private Category _currentCategory = new Category();

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddCategoryPage"/>.
        /// </summary>
        /// <param name="selectedCategory">
        /// Категория для редактирования. Если null, создаётся новая категория.
        /// </param>
        /// <remarks>
        /// При передаче существующей категории страница работает в режиме редактирования.
        /// При передаче null создаётся новая пустая категория.
        /// </remarks>
        public AddCategoryPage(Category selectedCategory)
        {
            InitializeComponent();
            if (selectedCategory != null)
                _currentCategory = selectedCategory;

            DataContext = _currentCategory;
        }

        /// <summary>
        /// Обработчик нажатия кнопки сохранения категории.
        /// Проверяет валидность данных и сохраняет категорию в базу данных.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        /// <remarks>
        /// Проверки:
        /// - Название категории не должно быть пустым
        /// Если ID категории = 0, добавляет новую запись в базу данных.
        /// В противном случае обновляет существующую запись.
        /// </remarks>
        private void ButtonSaveCategory_Click(object sender, RoutedEventArgs
e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentCategory.Name))
                errors.AppendLine("Укажите название категории!");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentCategory.ID == 0)
                DBEntities.GetContext().Categories.Add(_currentCategory);

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
            TBCategoryName.Text = "";
        } 
    
    }
}
