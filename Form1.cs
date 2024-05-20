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
        public Form1()
        {
            InitializeComponent();
        }
        List<Data> Data = new List<Data>();

        private void OpenFile()
        {
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.RowCount = 12;
            dataGridView1.Rows.Clear();
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
                            string[] parts = line.Split(';'); // Разделить строку по запятой

                            if (parts.Length >= 2)
                            {
                                if (double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double value)
                                    && DateTime.TryParseExact(parts[1].Trim(), "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                                {
                                    Data.Add(new Data { Value = value, Date = date });
                                }
                                else
                                {
                                    Console.WriteLine($"Ошибка при разборе строки: {line}");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Некорректный формат строки: {line}");
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
        double med;
        double left;
        double right;
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
            med = CalculateMedian(Data);

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
            left = CalculateLeftBound(Data);
            right = CalculateRightBound(Data);

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

            dataGridView2.Rows[0].Cells[0].Value = "Среднее значение: " + Math.Round(CalculateMean(Data), 2);
            dataGridView2.Rows[1].Cells[0].Value = "Медиана: " + Math.Round(CalculateMedian(Data), 2);
            dataGridView2.Rows[2].Cells[0].Value = "Дисперсия: " + Math.Round(CalculateVariance(Data), 2);
            dataGridView2.Rows[3].Cells[0].Value = "Левый предел: " + Math.Round(CalculateLeftBound(Data), 2);
            dataGridView2.Rows[4].Cells[0].Value = "Правый предел: " + Math.Round(CalculateRightBound(Data), 2);
            dataGridView2.Rows[5].Cells[0].Value = "Относит. частота: ";

            foreach(var i in CalculateRelativeFrequency(Data))
            {
                dataGridView2.Rows[6].Cells[0].Value = dataGridView2.Rows[6].Cells[0].Value + i.Value.ToString() + "; ";
            }


            Console.WriteLine(CalculateMean(Data) + "        " + CalculateMedian(Data)
                + "           " + CalculateVariance(Data) + "          " + CalculateLeftBound(Data) + "        " + CalculateRightBound(Data)
                + "           " + CalculateRelativeFrequency(Data));

            //dataGridView2.Rows[0].Cells[0].Value = Convert.ToString("Средняя арифметическая взвешенная: ") + Math.Round(Convert.ToDouble(sredVelich), 2);

        }


        private void DrawChart(List<Data> DataList)
        {
            // Очистить существующие серии данных на графике
            chart1.Series.Clear();

            // Создать новую серию данных для графика
            Series series = new Series("Data Series");
            series.ChartType = SeriesChartType.Line; // Выбрать тип графика (линейный)

            // Добавить данные в серию
            foreach (var data in DataList)
            {
                series.Points.AddXY(data.Date, data.Value);
            }

            // Добавить серию данных на график
            chart1.Series.Add(series);

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void downloadData_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        int shetchik1 = 0;
        int shetchik2 = 0;
        int shetchik3 = 0;
        private void drawOnSeries (double ChtoRisovat, int nomer, string nazvanie,int shetchik)
        {
            
            Series ser = new Series(nazvanie);
            ser.ChartType = SeriesChartType.Line;
            //тут можно будет указать цвета и прочую хуету

            if (shetchik%2==0)
            {



                foreach (var data in Data)
                {
                    ser.Points.AddXY(data.Date, ChtoRisovat);
                }
                chart1.Series.Add(ser);
            }
            else
            {
                chart1.Series.RemoveAt(nomer);
            }
            

            
        
    }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            drawOnSeries(med, 1, "медиана",shetchik1);
            shetchik1++;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            drawOnSeries(left, 2, "Левый предел", shetchik2) ;
            shetchik2++;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            drawOnSeries(right, 3, "Правый предел", shetchik3);
            shetchik3++;
        }
    }
}
