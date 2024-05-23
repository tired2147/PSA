﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PSA
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
       
        private void chart1_Click(object sender, EventArgs e)
        {

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
        
        
        private void HeadAndShoulders(List<Data> DataList)
        {
            
            for (int i = 1; i < DataList.Count - 1; i++)
            {
                if (DataList[i].Value > DataList[i - 1].Value && DataList[i].Value > DataList[i + 1].Value)
                {
                    // Найден пик
                    double leftShoulder = DataList[i - 1].Value;
                    var leftShoulderDate = DataList[i - 1].Date;
                    double head = DataList[i].Value;
                    var HeadDate = DataList[i].Date;
                    double rightShoulder = DataList[i + 1].Value;
                    var RightShoulderDate = DataList[i + 1].Date;

                    if (leftShoulder < head && rightShoulder < head && Math.Abs(leftShoulder - rightShoulder) < 0.05 * head)
                    {
                        // Найден паттерн "Голова и плечи"
                        Series series = new Series("Голова и плечи");
                        series.ChartType = SeriesChartType.Line;
                        series.Points.AddXY(leftShoulderDate, leftShoulder);
                        series.Points.AddXY(HeadDate, head);
                        series.Points.AddXY(RightShoulderDate, rightShoulder);
                        chart1.Series.Add(series);
                    }
                }
            }

        }
        private void IsDoubleTop(List<Data> DataList)
        {
            // Логика для распознавания паттерна "Двойная вершина"
            for (int i = 1; i < DataList.Count - 2; i++)
            {
                if (DataList[i].Value > DataList[i - 1].Value && DataList[i].Value > DataList[i + 1].Value)
                {
                    // Найден первый пик
                    double firstTop = DataList[i].Value;
                    var firstTopDate = DataList[i].Date;

                    for (int j = i + 2; j < DataList.Count - 1; j++)
                    {
                        if (DataList[j].Value > DataList[j - 1].Value && DataList[j].Value > DataList[j + 1].Value)
                        {
                            // Найден второй пик
                            double secondTop = DataList[j].Value;
                            var SecondTopDate = DataList[j].Date;

                            if (Math.Abs(firstTop - secondTop) < 0.05 * firstTop)
                            {

                                Series series = new Series("Двойная вершина");
                                series.ChartType = SeriesChartType.Line;
                                series.Points.AddXY(firstTopDate, firstTop);
                                series.Points.AddXY(SecondTopDate, secondTop);
                                chart1.Series.Add(series);
                                break;
                            }
                        }
                    }
                }
            }

            
        }

        private void IsDoubleBottom(List<Data> DataList)
        {
            // Логика для распознавания паттерна "Двойное дно"
            for (int i = 1; i < DataList.Count - 2; i++)
            {
                if (DataList[i].Value < DataList[i - 1].Value && DataList[i].Value < DataList[i + 1].Value)
                {
                    // Найден первый минимум
                    double firstBottoma = DataList[i-1].Value;
                    var aa = DataList[i-1].Date;
                    double firstBottom = DataList[i].Value;
                    var a = DataList[i].Date;
                    double firstBottomb = DataList[i+1].Value;
                    var ab = DataList[i+1].Date;
                    for (int j = i + 2; j < DataList.Count - 1; j++)
                    {
                        if (DataList[j].Value < DataList[j - 1].Value && DataList[j].Value < DataList[j + 1].Value)
                        {
                            // Найден второй минимум
                            double secondBottoma = DataList[j-1].Value;
                            var ba = DataList[j-1].Date; 
                            double secondBottom = DataList[j].Value;
                            var b = DataList[j].Date;
                            double secondBottomb = DataList[j+1].Value;
                            var bb = DataList[j+1].Date;

                            if (Math.Abs(firstBottom - secondBottom) < 0.05 * firstBottom)
                            {
                                // Найден паттерн "Двойное дно"
                                if(chart1.Series.IsUniqueName("Двойное дно")==false)
                                {
                                    chart1.Series.RemoveAt(chart1.Series.IndexOf("Двойное дно"));
                                }
                                Series series = new Series("Двойное дно");
                                series.ChartType = SeriesChartType.Line;
                                series.Points.AddXY(aa, firstBottoma-10);
                                series.Points.AddXY(a, firstBottom-10);
                                series.Points.AddXY(ab, firstBottomb -10);
                                series.Points.AddXY(ba, secondBottoma - 10);
                                series.Points.AddXY(b, secondBottom- 10);
                                series.Points.AddXY(bb, secondBottomb -10);
                                series.Points.AddXY(bb, secondBottomb + 10);
                               series.Points.AddXY(aa, firstBottoma + 10);
                                series.Points.AddXY(aa, firstBottoma - 10);
                                chart1.Series.Add(series);

                            }
                        }
                    }
                }
            }

            
        }

        private void IsFlagOrPennant(List<Data> DataList)
        {
            // Логика для распознавания паттернов "Флаги и вымпелы"
            bool flagOrPennant = false;

            for (int i = 1; i < DataList.Count - 1; i++)
            {
                if (DataList[i].Value > DataList[i - 1].Value * 1.1)
                {
                    // Найден резкий рост
                    for (int j = i + 1; j < DataList.Count - 1; j++)
                    {
                        if (Math.Abs(DataList[j].Value - DataList[i].Value) < 0.05 * DataList[i].Value)
                        {
                            // Найдена консолидация
                            flagOrPennant = true;
                        }
                        else if (flagOrPennant)
                        {
                            // Консолидация завершена
                            
                        }
                    }
                }
            }

            
        }

        private void IsTriangle(List<Data> DataList)
        {
            // Логика для распознавания паттерна "Треугольники"
            int start = 0;
            while (start < DataList.Count - 2)
            {
                int end = start + 1;
                while (end < DataList.Count - 1 && DataList[end].Value != DataList[start].Value)
                {
                    end++;
                }

                if (end < DataList.Count - 1)
                {
                    // Найден треугольник
                    Series series = new Series("Треугольник");
                    series.ChartType = SeriesChartType.Line;
                    series.Points.AddXY(DataList.ElementAt(0).Date, DataList.ElementAt(0).Value);
                    series.Points.AddXY(DataList.Last().Date, DataList.Last().Value);
                    chart1.Series.Add(series);
                }

                start++;
            }

            
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            List<Data> Data;
            Data = DataBank.DataList;
            DrawChart(Data);
            IsTriangle(Data);
            IsFlagOrPennant(Data);
            IsDoubleBottom(Data);
           // IsDoubleTop(Data);
           // HeadAndShoulders(Data);
        }
    }
}
