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
//using iTextSharp.text;
//using iTextSharp.text.pdf;
using System.IO;
//using Microsoft.Office.Interop;
//using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

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

            if (Data.Count > 0)
            {
                DialogResult result = MessageBox.Show("Открыть новый файл?\nВсе подсчеты будут потеряны!.", "Сообщение",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
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
                                Console.WriteLine($"Ошибка при разборе строки и невозможность исправить: {line}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            foreach(var i in Data)
            {
                Console.WriteLine(i.Value + " " + i.Date);
            }
            DrawChart(Data);
            Razchet();
           
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
            double Pocents(List<Data> dataList)
            {
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
            Console.WriteLine(proc+"");
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

            label1.Text = "Среднее значение: " + Math.Round(CalculateMean(Data), 2);
            label2.Text = "Медиана: " + Math.Round(CalculateMedian(Data), 2);
            label3.Text = "Дисперсия: " + Math.Round(CalculateVariance(Data), 2);
            label4.Text = "Левый предел: " + Math.Round(CalculateLeftBound(Data), 2);
            label5.Text = "Правый предел: " + Math.Round(CalculateRightBound(Data), 2);
            if (proc>0)
            {
                label6.Text = "Акции выросли на: " + Math.Round(proc, 2)+ "%";
                label6.ForeColor = Color.Green;
            }
            else
            {
                label6.Text = "Акции упали на: " + Math.Round(-proc, 2) + "%";
                label6.ForeColor = Color.Red;
                
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

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Авторы:\nИванов О.Н. \nБарановский Д.Ю. \nСитникова Елизавета ");
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                //передаем что нарисовать и название
                DrawChart(srznach, "Mush");
            }
            else //передаем название для удаления
                chart1.Series.RemoveAt(chart1.Series.IndexOf("Mush"));
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

        

    }
}
