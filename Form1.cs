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
        double median, leftBound, rightBound, srznach, proc;




        private void downloadData_Click(object sender, EventArgs e)
        {
            checkBox1.CheckState = CheckState.Unchecked;
            checkBox2.CheckState = CheckState.Unchecked;
            checkBox3.CheckState = CheckState.Unchecked;
            checkBox4.CheckState = CheckState.Unchecked;
            
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
                    MessageBox.Show(ex.Message);//обработка любых исключений
                }
            }
            
            DrawChart(Data);
            Razchet();
            if (countErrors > 0 || countEmptyCells > 0)
            {
                Console.WriteLine($"Обнаружено:\n *{countEmptyCells} строк, где есть пустые ячейки\n *{countErrors} ошибок, которые не удалось исправить");
                MessageBox.Show($"Обнаружено:\n *{countEmptyCells} строк, где есть пустые ячейки\n *{countErrors} ошибок, которые не удалось исправить");
            }

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
            median = CalculateMedian(Data);

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

                return sumOfSquares / dataList.Count;
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
            leftBound = CalculateLeftBound(Data);
            rightBound = CalculateRightBound(Data);
            srznach = CalculateMean(Data);
            proc = Pocents(Data);

            //относительная частота
            Dictionary<double, double> CalculateRelativeFrequency(List<Data> dataList)
            {
                Dictionary<double, double> frequencyMap = new Dictionary<double, double>();
                int totalCount = dataList.Count;

                foreach (var data in dataList)
                {
                    if (frequencyMap.ContainsKey(data.Value))
                    {
                        frequencyMap[data.Value] += 1.0 / totalCount;
                    }
                    else
                    {
                        frequencyMap[data.Value] = 1.0 / totalCount;
                    }
                }

                return frequencyMap;
            }

            label1.Text = Math.Round(CalculateMean(Data), 2).ToString(); 
            label2.Text = Math.Round(CalculateMedian(Data), 2).ToString();
            label3.Text = Math.Round(CalculateVariance(Data), 2).ToString();
            label4.Text = Math.Round(CalculateLeftBound(Data), 2).ToString();
            label5.Text = Math.Round(CalculateRightBound(Data), 2).ToString();
            if (proc > 0)
            {
                label6.Text = "⬆" + Math.Round(proc, 2)+ "%";
                label6.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                label6.Text = "⬇" + Math.Round(-proc, 2) + "%";
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

        private void Form2LabelClick_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            List<Data> NewData = new List<Data>();
            int counter = 0;
            foreach(var data in Data)
            {
                if(counter > (((Data.Count)*70)/100))
                {
                    NewData.Add(data);
                }
                
                counter++;
            }
            DataBank.DataList = NewData;
            form2.Show();
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

        

        

        private void ExportToDocx(string text, Chart chart, string docxPath)
        {
            // Создаем объект приложения Word
            var wordApp = new Word.Application();
            wordApp.Visible = false; // Сделайте Word видимым, если нужно

            // Создаем новый документ
            var document = wordApp.Documents.Add();

            // Добавляем текст в документ
            Word.Paragraph para = document.Paragraphs.Add();
            para.Range.Text = text;
            para.Range.InsertParagraphAfter();

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
                imagePara.Range.InsertParagraphAfter();

                // Удаляем временный файл
                if (File.Exists(tempImagePath))
                {
                    File.Delete(tempImagePath);
                }
            }

            // Сохраняем документ
            document.SaveAs2(docxPath);

            // Закрываем документ и приложение Word
            document.Close();
            wordApp.Quit();

            MessageBox.Show("Документ успешно создан!");
        }

        private void ExportToPdf(string text, Chart chart, string pdfPath)
        {
            using (MemoryStream chartStream = new MemoryStream())
            {
                chart1.SaveImage(chartStream, ChartImageFormat.Png);
                chartStream.Seek(0, SeekOrigin.Begin);

                using (FileStream fs = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    Document doc = new Document();
                    PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                    
                    doc.Open();

                    doc.Add(new Paragraph(text));

                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(chartStream);
                    doc.Add(img);

                    doc.Close();
                }
            }
            MessageBox.Show("Документ успешно создан!");
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
                    ExportToDocx("ssss", chart1, docxPath);
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
                    ExportToPdf("sdsdasda", chart1, pdfPath);
                }
            }
        }




        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Авторы:\nИванов О.Н. \nБарановский Д.Ю. \nСитникова Е.Д. ");
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
