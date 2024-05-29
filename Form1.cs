using System;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Word = Microsoft.Office.Interop.Word;
using System.Drawing.Imaging;

namespace PSA
{
    public partial class Form1 : Form
    {
        private ToolTip toolTip;
        public Form1()
        {
            InitializeComponent();
            InitializeToolTiop();

        }

        List<Data> Data = new List<Data>();
        double median, variance, leftBound, rightBound, srznach, proc, soprotivlenie, podderjka;




        private void downloadData_Click(object sender, EventArgs e)
        {
            
            if (Data.Count > 0)
            {
                DialogResult result = MessageBox.Show("Открыть новый файл?\nВсе подсчеты будут потеряны!.", "Сообщение",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Data.Clear();
                    chart1.Series.Clear();
                    label1.Text = ""; label2.Text = ""; label3.Text = ""; label4.Text = ""; label5.Text = ""; label6.Text = "";
                    OpenFile();
                    
                }

            }
            else
            {
                OpenFile();
            }
        }
        private void OpenFile()
        {
            int countErrors = 0;//счетчик для подсчета ошибок в данных
            int countEmptyCells = 0;//счетчик для подсчета пустых ячеек

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV файл (*.csv)|*.csv";
            ofd.FileName = "";
            ofd.Title = "Открыть";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var reader = new StreamReader(ofd.FileName))
                    {
                        string line;
                        
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split(';');

                            // Проверка на пустые строки или ячейки
                            if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
                            {
                                Console.WriteLine($"Пропуск пустой или некорректной строки: {line}");
                                countEmptyCells++;
                                continue;
                            }

                            string valueStr = parts[0].Trim();
                            string dateStr = parts[1].Trim();

                            // Удаление всех букв из строки перед попыткой парсинга, сохраняя цифры, точки и запятые
                            valueStr = new string(valueStr.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray());
                            dateStr = new string(dateStr.Where(char.IsDigit).ToArray());

                            // Попытка распарсить значение и дату
                            if (double.TryParse(valueStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double value)
                                && DateTime.TryParseExact(dateStr, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                            {
                                Data.Add(new Data { Value = value, Date = date });
                            }
                            else
                            {
                                countErrors++;
                                Console.WriteLine($"Ошибка при разборе строки и невозможность исправить: {line}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка");//обработка любых исключений
                }
            }
            if (countErrors > 0 || countEmptyCells > 0)
            {
                Console.WriteLine($"Обнаружено:\n *{countEmptyCells} строк, где есть пустые ячейки\n *{countErrors} ошибок, которые не удалось исправить");
                MessageBox.Show($"Обнаружено:\n *{countEmptyCells} строк, где есть пустые ячейки\n *{countErrors} ошибок, которые не удалось исправить", "Ошибки в файле",  MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            DrawChart(Data);
            chart1.ChartAreas[0].AxisX.Title = "Дата";
            chart1.ChartAreas[0].AxisY.Title = "Стоимость";
            chart1.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
            chart1.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Arial", 10, FontStyle.Bold);

            checkBox1.CheckState = CheckState.Unchecked;
            checkBox2.CheckState = CheckState.Unchecked;
            checkBox3.CheckState = CheckState.Unchecked;
            checkBox4.CheckState = CheckState.Unchecked;
            panel8.Visible = true;
            panel10.Visible = true;
            panel11.Visible = true;
            сохранитьКакToolStripMenuItem.Visible = true;

            Razchet();

            

            foreach (var i in Data)
            {
                Console.WriteLine(i.Value + " " + i.Date);
            }
        }
        



        private void Razchet()
        {
            //среднее значение
            double CalculateMean(List<Data> dataList)
            {
                if (dataList.Count == 0)
                    return 0;

                double sum = 0;
                foreach (var data in dataList)
                {
                    sum += data.Value;
                }

                return sum / dataList.Count;
            }


            //медиана
            double CalculateMedian(List<Data> dataList)
            {
                if (dataList.Count == 0)
                    return 0;

                List<double> sortedValues = dataList.Select(data => data.Value).OrderBy(value => value).ToList();

                int n = sortedValues.Count;
                if (n % 2 == 1)
                {
                   
                    return sortedValues[n / 2];
                }
                else
                {
                    return (sortedValues[n / 2 - 1] + sortedValues[n / 2]) / 2.0;
                }
                
            }
            

            //дисперсия
            double CalculateVariance(List<Data> dataList)
            {
                if (dataList.Count == 0)
                    return 0;

                double mean = CalculateMean(dataList);
                double sumOfSquares = 0;

                foreach (var data in dataList)
                {
                    sumOfSquares += Math.Pow(data.Value - mean, 2);
                }

                return dataList.Count > 1 ? sumOfSquares / (dataList.Count - 1) : 0;
            }

            //левый предел
            double CalculateLeftBound(List<Data> dataList)
            {
                if (dataList.Count == 0)
                    return 0;

                return dataList.Min(data => data.Value);
            }
            //правый предел
            double CalculateRightBound(List<Data> dataList)
            {
                if (dataList.Count == 0)
                    return 0;

                return dataList.Max(data => data.Value);
            }
            //процент роста или падения акций
            double Pocents(List<Data> dataList)
            {
                if (dataList.Count == 0)
                    return 0;

                double pervoeChislo = dataList.First().Value;
                double posledneeChislo = dataList.ElementAt(dataList.Count-1).Value;
                
                if(pervoeChislo > posledneeChislo)
                {
                    return ((pervoeChislo - posledneeChislo)/pervoeChislo)*100 * (-1);
                }
                else
                {
                    return ((posledneeChislo - pervoeChislo)/posledneeChislo ) * (100) ;
                }
            }
            variance = Math.Round(CalculateVariance(Data), 2);
            median = Math.Round(CalculateMedian(Data),2);
            leftBound = Math.Round(CalculateLeftBound(Data),2);
            rightBound = Math.Round(CalculateRightBound(Data),2);
            srznach = Math.Round(CalculateMean(Data),2);
            proc = Math.Round(Pocents(Data),2);

            ////относительная частота
            //Dictionary<double, double> CalculateRelativeFrequency(List<Data> dataList)
            //{
            //    Dictionary<double, double> frequencyMap = new Dictionary<double, double>();
            //    int totalCount = dataList.Count;

            //    foreach (var data in dataList)
            //    {
            //        if (frequencyMap.ContainsKey(data.Value))
            //        {
            //            frequencyMap[data.Value] += 1.0 / totalCount;
            //        }
            //        else
            //        {
            //            frequencyMap[data.Value] = 1.0 / totalCount;
            //        }
            //    }

            //    return frequencyMap;
            //}

            // Метод для нахождения локальных максимумов
            List<double> FindLocalMaxima(List<Data> dataList)
            {
                List<double> localMaxima = new List<double>();

                for (int i = 1; i < dataList.Count - 1; i++)
                {
                    if (dataList[i].Value > dataList[i - 1].Value && dataList[i].Value > dataList[i + 1].Value)
                    {
                        localMaxima.Add(dataList[i].Value);
                    }
                }

                return localMaxima;
            }
            double FindResistanceLevel(List<Data> dataList)
            {
                if (dataList.Count == 0)
                    return 0;

                if (dataList == null || dataList.Count < 3)
                    throw new ArgumentException("Data list must contain at least 3 data points to find local maxima");

                // Находим локальные максимумы
                List<double> localMaxima = FindLocalMaxima(dataList);

                if (localMaxima.Count == 0)
                    throw new InvalidOperationException("No local maxima found in the data list");

                // Рассчитываем среднее значение локальных максимумов как уровень сопротивления
                double resistanceLevel = localMaxima.Average();
                return resistanceLevel;
            }

            soprotivlenie = FindResistanceLevel(Data);





            label1.Text = Math.Round(CalculateMean(Data), 2).ToString(); 
            label2.Text = Math.Round(CalculateMedian(Data), 2).ToString();
            label3.Text = Math.Round(CalculateVariance(Data), 2).ToString();
            label4.Text = Math.Round(CalculateLeftBound(Data), 2).ToString();
            label5.Text = Math.Round(CalculateRightBound(Data), 2).ToString();
            if (proc > 0)
            {
                label6.Text = "⬆" + proc + "%";
                label6.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                label6.Text = "⬇" + -proc + "%";
                label6.ForeColor = System.Drawing.Color.Red;
                
            }

            


            //Console.WriteLine(CalculateMean(Data) + "        " + CalculateMedian(Data)
            //    + "           " + CalculateVariance(Data) + "          " + CalculateLeftBound(Data) + "        " + CalculateRightBound(Data)
            //    + "           " + CalculateRelativeFrequency(Data));

            //dataGridView2.Rows[0].Cells[0].Value = Convert.ToString("Средняя арифметическая взвешенная: ") + Math.Round(Convert.ToDouble(sredVelich), 2);

        }





        private void DrawChart(List<Data> DataList)
        {
            // Очистить существующие серии данных на графике
            chart1.Series.Clear();

            // Создать новую серию данных для графика
            Series series = new Series("stocks");
            series.ChartType = SeriesChartType.Line; // Выбрать тип графика (линейный)
            series.BorderWidth = 4; //толщина линии
            series.Color = Color.DarkRed;
            

            // Добавить данные в серию
            foreach (var data in DataList)
            {
                series.Points.AddXY(data.Date, data.Value);
            }

            // Добавить серию данных на график
            chart1.Series.Add(series);

        }
        private void DrawChart(double ChtoRisovat, string nazvanie)
        {
            // Создать новую серию данных для графика
            Series series = new Series(nazvanie);
            series.ChartType = SeriesChartType.Line; // Выбрать тип графика (линейный)
            series.BorderWidth = 2;

            // Добавить данные в серию
            foreach (var data in Data)
            {
                series.Points.AddXY(data.Date, ChtoRisovat);
            }

            // Добавить серию данных на график
            chart1.Series.Add(series);
        }
        
        
       

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }



        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                //передаем что нарисовать и название
                DrawChart(median, "median");
            }
            else //передаем название для удаления
                chart1.Series.RemoveAt(chart1.Series.IndexOf("median"));
        }
        private void checkBox2_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                //передаем что нарисовать и название
                DrawChart(leftBound, "leftBound");
            }
            else //передаем название для удаления
                chart1.Series.RemoveAt(chart1.Series.IndexOf("leftBound"));
        }

        private void checkBox3_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                //передаем что нарисовать и название
                DrawChart(rightBound, "rightBound");
            }
            else //передаем название для удаления
                chart1.Series.RemoveAt(chart1.Series.IndexOf("rightBound"));
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                //передаем что нарисовать и название
                DrawChart(srznach, "mush");
            }
            else //передаем название для удаления
                chart1.Series.RemoveAt(chart1.Series.IndexOf("mush"));
        }

        


        private void InitializeToolTiop()
        {
            toolTip = new ToolTip();
            toolTip.AutoPopDelay = 1000000;
            toolTip.InitialDelay = 100;
            toolTip.ReshowDelay = 500;
          
            toolTip.ShowAlways = true;

            toolTip.SetToolTip(this.label1, "Среднее значение, или математическое ожидание, представляет собой среднюю величину набора данных. \nЧтобы вычислить среднее значение, нужно сложить все значения в наборе данных и затем разделить полученную сумму на количество этих значений.");
            toolTip.SetToolTip(this.label2, "Медиана — это значение, которое делит упорядоченный набор данных пополам. \nЕсли количество элементов в наборе данных нечётное, медианой будет центральный элемент. \nЕсли количество элементов чётное, медианой будет среднее значение двух центральных элементов после сортировки данных.");
            toolTip.SetToolTip(this.label3, "Дисперсия измеряет, насколько значения в наборе данных отклоняются от среднего значения. \nЧтобы найти дисперсию, сначала нужно определить среднее значение набора данных. \nЗатем нужно вычислить отклонение каждого значения от среднего, возвести каждое отклонение в квадрат, \nсложить все квадраты отклонений и, наконец, разделить полученную сумму на количество значений в наборе данных.");
            toolTip.SetToolTip(this.label4, "Левый предел функции в определённой точке — это значение, к которому стремится функция, когда переменная приближается к этой точке слева, \nто есть с меньших значений. Это помогает понять поведение функции с одной стороны от данной точки.");
            toolTip.SetToolTip(this.label5, "Правый предел функции в определённой точке — это значение, к которому стремится функция, когда переменная приближается к этой точке справа, \nто есть с больших значений. Это помогает понять поведение функции с другой стороны от данной точки.");
        }

        

        private void PreparationOfReport()
        {
            
        }

        private void ExportToDocx(Chart chart, string docxPath)
        {
            try
            {
                // Создаем объект приложения Word
                var wordApp = new Word.Application();
                wordApp.Visible = false; // Сделайте Word видимым, если нужно

                // Создаем новый документ
                var document = wordApp.Documents.Add();
                document.Content.Font.Name = "Times New Roman";
                document.Content.Font.Size = 14;
                

                // Добавляем текст в документ
                Word.Paragraph para = document.Paragraphs.Add();
                para.Range.Text = "Отчет создан приложением \"Анализатор акций\"";
                para.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                para.Range.Font.Bold = 1;
                para.Range.InsertParagraphAfter();

                Word.Paragraph para2 = document.Paragraphs.Add();
                para2.Range.Text = "Ниже представлен график изменения стоимости акций:";
                para2.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                para2.Range.Font.Bold = 0;
                para2.Range.InsertParagraphAfter();

                // Сохраняем график в MemoryStream
                using (MemoryStream chartStream = new MemoryStream())
                {
                    chart.SaveImage(chartStream, ChartImageFormat.Png);
                    chartStream.Seek(0, SeekOrigin.Begin);

                    // Загружаем изображение из MemoryStream в Interop.PictureDisp
                    var image = System.Drawing.Image.FromStream(chartStream);
                    var tempImagePath = Path.GetTempFileName();
                    image.Save(tempImagePath, ImageFormat.Png);

                    // Вставляем изображение графика в документ
                    Word.Paragraph imagePara = document.Paragraphs.Add();
                    imagePara.Range.InlineShapes.AddPicture(tempImagePath);
                    //imagePara.Range.InsertParagraphAfter();

                    // Удаляем временный файл
                    if (File.Exists(tempImagePath))
                    {
                        File.Delete(tempImagePath);
                    }
                }

                Word.Paragraph para3 = document.Paragraphs.Add();
                para3.Range.Text = $"Среднее значение и медиана составляют {srznach} и {median} соответственно. Если относительная разница этих величин больше определенного " +
                    $"порога (например, 10% или 20%), это может быть индикатором асимметрии или наличия выбросов.";
                para3.Range.Text += $"Нижний и верхний пределы - {leftBound} и {rightBound}. Показывают наименьшую и наибольшую цену";
                para3.Range.Text += $"Показатель дисперсии - {variance}. Дисперсия в данных о стоимости акций показывает степень разброса или вариативности " +
                    $"этих цен относительно среднего значения.";
                if(proc > 0)
                {
                    para3.Range.Text += $"Изменение стоимости акции относительно первоначальной цены - {proc}%";
                }
                else para3.Range.Text += $"Изменение стоимости акции относительно первоначальной цены - {-proc}%";

                para3.Range.InsertParagraphAfter();

                // Сохраняем документ
                document.SaveAs2(docxPath);

                // Закрываем документ и приложение Word
                document.Close();
                wordApp.Quit();

                MessageBox.Show("DOCX документ успешно создан!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void ExportToPdf(Chart chart, string pdfPath)
        {
            try
            {
                // Создаем новый документ PDF
                using (FileStream fs = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    Document pdfDoc = new Document(PageSize.A4);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, fs);
                    pdfDoc.Open();

                    // Устанавливаем кириллический шрифт размером 14
                    BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.NORMAL);

                    // Создаем параграфы с использованием кириллического шрифта размером 14
                    Paragraph title = new Paragraph("Отчет создан приложением \"Анализатор акций\"", font);
                    title.Alignment = Element.ALIGN_CENTER;
                    pdfDoc.Add(title);
                    pdfDoc.Add(new Paragraph("\n"));

                    Paragraph subTitle = new Paragraph("Ниже представлен график изменения стоимости акций:", font);
                    subTitle.Alignment = Element.ALIGN_LEFT;
                    pdfDoc.Add(subTitle);
                    pdfDoc.Add(new Paragraph("\n"));

                    // Сохраняем график в MemoryStream и уменьшаем его размер
                    using (MemoryStream chartStream = new MemoryStream())
                    {
                        chart.SaveImage(chartStream, ChartImageFormat.Png);
                        chartStream.Seek(0, SeekOrigin.Begin);

                        // Вставляем изображение графика в документ и уменьшаем его размер
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(chartStream);
                        img.ScalePercent(80f); // Уменьшаем размер на 20%
                        pdfDoc.Add(img);
                    }

                    pdfDoc.Add(new Paragraph("\n"));

                    // Добавляем остальные данные с использованием кириллического шрифта размером 14
                    string text = $"Среднее значение и медиана составляют {srznach} и {median} соответственно. Если относительная разница этих величин больше определенного " +
                        $"порога (например, 10% или 20%), это может быть индикатором асимметрии или наличия выбросов.\n\n" +
                        $"Нижний и верхний пределы - {leftBound} и {rightBound}. Показывают наименьшую и наибольшую цену.\n\n" +
                        $"Показатель дисперсии - {variance}. Дисперсия в данных о стоимости акций показывает степень разброса или вариативности " +
                        $"этих цен относительно среднего значения.\n\n";

                    if (proc > 0)
                        text += $"Изменение стоимости акции относительно первоначальной цены - {proc}%\n";
                    else
                        text += $"Изменение стоимости акции относительно первоначальной цены - {-proc}%\n";

                    Paragraph data = new Paragraph(text, font);
                    pdfDoc.Add(data);

                    pdfDoc.Close();
                }

                MessageBox.Show("PDF документ успешно создан!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void buttonExportDocx_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Word Document|*.docx";
                saveFileDialog.Title = "Save as Word Document";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string docxPath = saveFileDialog.FileName;
                    ExportToDocx(chart1, docxPath);
                }
            }
        }

        private void buttonExportPdf_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PDF Document|*.pdf";
                saveFileDialog.Title = "Save as PDF Document";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string pdfPath = saveFileDialog.FileName;
                    ExportToPdf(chart1, pdfPath);
                }
            }
        }




       private void Form2LabelClick_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this);
            List<Data> NewData = new List<Data>();
            int counter = 0;
            foreach(var data in Data)
            {
                if(counter > (((Data.Count)*80)/100))
                {
                    NewData.Add(data);
                }
                
                counter++;
            }
            DataBank.DataList = NewData;

            // Подписка на событие FormClosed второй формы
            form2.FormClosed += (s, args) => this.Show();

            // Отображение второй формы
            form2.Show();

            // Скрытие первой формы
            this.Hide();

        }




        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данное приложение разработано для оценки стоимости акций компании с помощью первичного статистического анализа. " +
                "Программа позволяет отслеживать такие величины: среднее значение, медиана, дисперсия, левый и правый пределы, а также" +
                " выполняет поиск паттернов на графике для наглядного оценивания движения графика.\n" +
                "Имеется возможность выгрузить отчет в формат DOCX и PDF.\n\n" +
                "Для корректного считывания с csv-файла данные необходимо выстроить в 2 столбца (1-й - Стоимости акций, 2-й - Дата)\n\n" +
                "Авторы:\nИванов О.Н. \nБарановский Д.Ю. \nСитникова Е.Д. ", "О программе");
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            downloadData_Click(sender, e);
        }

        private void какPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonExportPdf_Click(sender, e);
        }

        private void какDOCXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonExportDocx_Click(sender, e);
        }
    }
}
