using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace _122_Sargas.Pages.AdminPages
{
    /// <summary>
    /// Interaction logic for DiagrammPage.xaml
    /// </summary>
    /// <remarks>
    /// Функциональность:
    /// - Выбор пользователя для анализа
    /// - Выбор типа диаграммы (Bar, Pie, Column и др.)
    /// - Экспорт данных в Word с таблицами и статистикой
    /// - Экспорт данных в Excel с детальной разбивкой по категориям
    /// </remarks>
    public partial class DiagrammPage : Page
    {
        /// <summary>
        /// Контекст базы данных для работы с данными
        /// </summary>
        private DBEntities _context = new DBEntities();

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DiagrammPage"/>.
        /// </summary>
        /// <remarks>
        /// Создаёт область диаграммы и серию данных.
        /// Заполняет ComboBox списком пользователей и типов диаграмм.
        /// </remarks>
        public DiagrammPage()
        {
            InitializeComponent();
            ChartPayments.ChartAreas.Add(new ChartArea("Main"));
            var currentSeries = new Series("Платежи")
            {
                IsValueShownAsLabel = true
            };
            ChartPayments.Series.Add(currentSeries);
            ComboBoxUser.ItemsSource = _context.Users.ToList();
            ComboBoxDiagram.ItemsSource = Enum.GetValues(typeof(SeriesChartType));
        }

        /// <summary>
        /// Обработчик изменения выбора в ComboBox пользователя или типа диаграммы.
        /// Обновляет диаграмму с учётом выбранного пользователя и типа визуализации.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        /// <remarks>
        /// Для каждой категории подсчитывает общую сумму платежей выбранного пользователя (Цена × Количество).
        /// Обновляет только при наличии выбранного пользователя и типа диаграммы.
        /// </remarks>
        private void UpdateChart(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxUser.SelectedItem is User currentUser && ComboBoxDiagram.SelectedItem is SeriesChartType currentType)
            {
                Series currentSeries = ChartPayments.Series.FirstOrDefault();
                currentSeries.ChartType = currentType;
                currentSeries.Points.Clear();

                var categoriesList = _context.Categories.ToList();
                foreach (var category in categoriesList)
                {
                    currentSeries.Points.AddXY(category.Name,
                        _context.Payments.ToList().Where(u => u.User == currentUser && u.Category == category).Sum(u => u.Price * u.Num));
                }
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки экспорта в Word.
        /// Создаёт отчёт с таблицами платежей для каждого пользователя.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        /// <remarks>
        /// Создаваемый отчёт включает:
        /// - Таблицу платежей по категориям для каждого пользователя
        /// - Самый дорогой платёж (выделен красным)
        /// - Самый дешёвый платёж (выделен зелёным)
        /// - Автоматическую нумерацию страниц
        /// - Дату создания в заголовке
        /// Сохраняет документ в форматах DOCX и PDF по пути D:\Payments
        /// </remarks>
        private void ButtonWord_Click(object sender, RoutedEventArgs e)
        {
            var allUsers = _context.Users.ToList();
            var allCategories = _context.Categories.ToList();

            var application = new Word.Application();
            Word.Document document = application.Documents.Add();

            foreach (var user in allUsers)
            {
                Word.Paragraph userParagraph = document.Paragraphs.Add();
                Word.Range userRange = userParagraph.Range;
                userRange.Text = user.FIO;
                userParagraph.set_Style("Заголовок");
                userRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                userRange.InsertParagraphAfter();
                document.Paragraphs.Add(); 

                Word.Paragraph tableParagraph = document.Paragraphs.Add();
                Word.Range tableRange = tableParagraph.Range;
                Word.Table paymentsTable = document.Tables.Add(tableRange, allCategories.Count() + 1, 2);
                paymentsTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                paymentsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                paymentsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                Word.Range cellRange;
                cellRange = paymentsTable.Cell(1, 1).Range;
                cellRange.Text = "Категория";
                cellRange = paymentsTable.Cell(1, 2).Range;
                cellRange.Text = "Сумма расходов";
                paymentsTable.Rows[1].Range.Font.Name = "Times New Roman";
                paymentsTable.Rows[1].Range.Font.Size = 14;
                paymentsTable.Rows[1].Range.Bold = 1;
                paymentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                for (int i = 0; i < allCategories.Count(); i++)
                {
                    var currentCategory = allCategories[i];
                    cellRange = paymentsTable.Cell(i + 2, 1).Range;
                    cellRange.Text = currentCategory.Name;
                    cellRange.Font.Name = "Times New Roman";
                    cellRange.Font.Size = 12;
                    cellRange = paymentsTable.Cell(i + 2, 2).Range;
                    cellRange.Text = user.Payments.ToList()
                        .Where(u => u.Category == currentCategory)
                        .Sum(u => u.Num * u.Price)
                        .ToString("N2") + " руб.";
                    cellRange.Font.Name = "Times New Roman";
                    cellRange.Font.Size = 12;
                }
                document.Paragraphs.Add(); 

                Payment maxPayment = user.Payments.OrderByDescending(u => u.Price * u.Num).FirstOrDefault();
                if (maxPayment != null)
                {
                    Word.Paragraph maxPaymentParagraph = document.Paragraphs.Add();
                    Word.Range maxPaymentRange = maxPaymentParagraph.Range;
                    maxPaymentRange.Text = $"Самый дорогостоящий платеж - {maxPayment.Name} за {(maxPayment.Price * maxPayment.Num).ToString("N2")} " +
                                          $"руб. от {maxPayment.Date.ToString("dd.MM.yyyy")}";
                    maxPaymentParagraph.set_Style("Подзаголовок");
                    maxPaymentRange.Font.Color = Word.WdColor.wdColorDarkRed;
                    maxPaymentRange.InsertParagraphAfter();
                }
                document.Paragraphs.Add();

                Payment minPayment = user.Payments.OrderBy(u => u.Price * u.Num).FirstOrDefault();
                if (minPayment != null)
                {
                    Word.Paragraph minPaymentParagraph = document.Paragraphs.Add();
                    Word.Range minPaymentRange = minPaymentParagraph.Range;
                    minPaymentRange.Text = $"Самый дешевый платеж - {minPayment.Name} за {(minPayment.Price * minPayment.Num).ToString("N2")} " +
                                          $"руб. от {minPayment.Date.ToString("dd.MM.yyyy")}";
                    minPaymentParagraph.set_Style("Подзаголовок");
                    minPaymentRange.Font.Color = Word.WdColor.wdColorDarkGreen;
                    minPaymentRange.InsertParagraphAfter();
                }

                if (user != allUsers.LastOrDefault())
                    document.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
            }

            foreach (Word.Section section in document.Sections)
            {
                Word.HeaderFooter footer = section.Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];
                footer.PageNumbers.Add(Word.WdPageNumberAlignment.wdAlignPageNumberCenter);
            }

            foreach (Word.Section section in document.Sections)
            {
                Word.Range headerRange = section.Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                headerRange.Fields.Add(headerRange, Word.WdFieldType.wdFieldPage);
                headerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                headerRange.Font.ColorIndex = Word.WdColorIndex.wdBlack;
                headerRange.Font.Size = 10;
                headerRange.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }

            application.Visible = true;
            document.SaveAs2(@"D:\Payments.docx");
            document.SaveAs2(@"D:\Payments.pdf", Word.WdExportFormat.wdExportFormatPDF);
        }

        /// <summary>
        /// Обработчик нажатия кнопки экспорта в Excel.
        /// Создаёт книгу Excel с листами для каждого пользователя и общим итогом.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        /// <remarks>
        /// Создаваемый файл включает:
        /// - Отдельный лист для каждого пользователя (название = ФИО)
        /// - Группировку платежей по категориям
        /// - Автоматические формулы для расчёта сумм
        /// - Промежуточные итоги по категориям
        /// - Лист "Общий итог" с суммой всех платежей всех пользователей
        /// - Форматирование таблиц с границами
        /// - Автоматическую подгонку ширины столбцов
        /// </remarks>
        private void ButtonExcel_Click(object sender, RoutedEventArgs e)
        {
            var allUsers = _context.Users.ToList().OrderBy(u => u.FIO).ToList();

            var application = new Excel.Application();
            application.SheetsInNewWorkbook = allUsers.Count();
            Excel.Workbook workbook = application.Workbooks.Add(Type.Missing);

            decimal grandTotal = 0;

            for (int i = 0; i < allUsers.Count(); i++)
            {
                int startRowIndex = 1;
                Excel.Worksheet worksheet = application.Worksheets.Item[i + 1];
                worksheet.Name = allUsers[i].FIO;

                worksheet.Cells[1][startRowIndex] = "Дата платежа";
                worksheet.Cells[2][startRowIndex] = "Название";
                worksheet.Cells[3][startRowIndex] = "Стоимость";
                worksheet.Cells[4][startRowIndex] = "Количество";
                worksheet.Cells[5][startRowIndex] = "Сумма";
                Excel.Range columlHeaderRange = worksheet.Range[worksheet.Cells[1][1], worksheet.Cells[5][1]];
                columlHeaderRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                columlHeaderRange.Font.Bold = true;
                startRowIndex++;

                var userCategories = allUsers[i].Payments.OrderBy(u => u.Date).GroupBy(u => u.Category).OrderBy(u => u.Key.Name);

                foreach (var groupCategory in userCategories)
                {
                    Excel.Range headerRange = worksheet.Range[worksheet.Cells[1][startRowIndex], worksheet.Cells[5][startRowIndex]];
                    headerRange.Merge();
                    headerRange.Value = groupCategory.Key.Name;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Italic = true;
                    startRowIndex++;

                    foreach (var payment in groupCategory)
                    {
                        worksheet.Cells[1][startRowIndex] = payment.Date.ToString("dd.MM.yyyy");
                        worksheet.Cells[2][startRowIndex] = payment.Name;
                        worksheet.Cells[3][startRowIndex] = payment.Price;
                        (worksheet.Cells[3][startRowIndex] as Excel.Range).NumberFormat = "0.00";
                        worksheet.Cells[4][startRowIndex] = payment.Num;
                        worksheet.Cells[5][startRowIndex].Formula = $"=C{startRowIndex}*D{startRowIndex}";
                        (worksheet.Cells[5][startRowIndex] as Excel.Range).NumberFormat = "0.00";
                        startRowIndex++;
                    }

                    Excel.Range sumRange = worksheet.Range[worksheet.Cells[1][startRowIndex], worksheet.Cells[4][startRowIndex]];
                    sumRange.Merge();
                    sumRange.Value = "ИТОГО:";
                    sumRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    worksheet.Cells[5][startRowIndex].Formula = $"=SUM(E{startRowIndex - groupCategory.Count()}:E{startRowIndex - 1})";
                    sumRange.Font.Bold = worksheet.Cells[5][startRowIndex].Font.Bold = true;

                    grandTotal += groupCategory.Sum(p => p.Price * p.Num);
                    startRowIndex++;
                }

                Excel.Range rangeBorders = worksheet.Range[worksheet.Cells[1][1], worksheet.Cells[5][startRowIndex - 1]];
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                rangeBorders.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;
                rangeBorders.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;

                worksheet.Columns.AutoFit();
            }

            Excel.Worksheet summarySheet = workbook.Worksheets.Add(After: workbook.Worksheets[workbook.Worksheets.Count]);
            summarySheet.Name = "Общий итог";
            summarySheet.Cells[1, 1] = "Общий итог:";
            summarySheet.Cells[1, 2] = grandTotal;
            (summarySheet.Cells[1, 2] as Excel.Range).NumberFormat = "0.00";

            Excel.Range summaryRange = summarySheet.Range[summarySheet.Cells[1, 1], summarySheet.Cells[1, 2]];
            summaryRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
            summaryRange.Font.Bold = true;

            summarySheet.Columns.AutoFit();

            application.Visible = true;
        }

    }
}