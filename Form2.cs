﻿using System;
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
    public partial class Form2 : Form
    {
        // Ссылка на первую форму
        private Form1 form1;

        public Form2(Form1 form1, int x, int y)
        {
            InitializeComponent();
            chart1.ChartAreas[0].AxisX.Title = "Дата";
            chart1.ChartAreas[0].AxisY.Title = "Стоимость";
            chart1.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
            chart1.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
            this.form1 = form1;

            // Установить местоположение второй формы
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new System.Drawing.Point(x, y);
        }

        
        

        int Glubina = 1;
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
        //private void IsFlagOrPennant(List<Data> DataList)
        //{
        //    // Логика для распознавания паттернов "Флаги и вымпелы"
        //    bool flagOrPennant = false;

        //    for (int i = 1; i < DataList.Count - 1; i++)
        //    {
        //        if (DataList[i].Value > DataList[i - 1].Value * 1.1)
        //        {
        //            // Найден резкий рост
        //            for (int j = i + 1; j < DataList.Count - 1; j++)
        //            {
        //                if (Math.Abs(DataList[j].Value - DataList[i].Value) < 0.05 * DataList[i].Value)
        //                {
        //                    // Найдена консолидация
        //                    flagOrPennant = true;
        //                }
        //                else if (flagOrPennant)
        //                {
        //                    // Консолидация завершена

        //                }
        //            }
        //        }
        //    }


        //}
        private void ReverseflagOrPennant(List<Data> dataList, List<MaxANdMins> maxAndMins)
        {
            int counter = 0;
            for (int a = 0; a < maxAndMins.Count - 1; a++)
            {
                if ((maxAndMins[a].max == true && maxAndMins[a + 1].max == true) || (maxAndMins[a].max == false && maxAndMins[a + 1].max == false))
                {
                    counter = 0;
                }
                else
                {
                    counter++;
                }
                if (counter >= 8)
                {
                    var entery = maxAndMins[a - 7];
                    var turn1 = maxAndMins[a - 6];
                    var turn2 = maxAndMins[a - 5];
                    var turn3 = maxAndMins[a - 4];
                    var turn4 = maxAndMins[a - 3];
                    var turn5 = maxAndMins[a - 2];
                    var turn6 = maxAndMins[a - 1];
                    var exit = maxAndMins[a - 0];
                    if (entery.Znach > exit.Znach && entery.Znach > turn1.Znach && exit.Znach < turn6.Znach && procents(entery.Znach, exit.Znach) >= 5)
                    {
                        Console.WriteLine(((exit.Znach - entery.Znach) / exit.Znach) * 100 + "--");
                        counter--;
                        var MaxOfMax = max(max(turn1.Znach, turn3.Znach), turn5.Znach);
                        var MinOfMax = min(min(turn1.Znach, turn3.Znach), turn5.Znach);
                        var MaxOfMins = max(max(turn2.Znach, turn4.Znach), turn6.Znach);
                        var MinOfMins = min(min(turn2.Znach, turn4.Znach), turn6.Znach);
                        //flag
                        if (procents(turn1.Znach, turn3.Znach) < 5 && procents(turn3.Znach, turn5.Znach) < 5 && procents(turn2.Znach, turn4.Znach) < 5 && procents(turn4.Znach, turn6.Znach) < 5)
                        {
                            counter = 0;
                            if (chart1.Series.IsUniqueName("Перевернутый флаг") == false)
                            {
                                chart1.Series.RemoveAt(chart1.Series.IndexOf("Перевернутый флаг"));
                            }
                            Series series = new Series("Перевернутый флаг");
                            series.ChartType = SeriesChartType.Line;
                            series.BorderWidth = 2;
                            series.Points.AddXY(entery.Date, entery.Znach);
                            series.Points.AddXY(turn1.Date, turn1.Znach);
                            //series.Points.AddXY(turn3.Date, turn3.Znach);
                            series.Points.AddXY(turn5.Date, turn5.Znach);
                            series.Points.AddXY(turn6.Date, turn6.Znach);
                            //series.Points.AddXY(turn4.Date, turn4.Znach);
                            series.Points.AddXY(turn2.Date, turn2.Znach);
                            chart1.Series.Add(series);

                        }
                        //wimpel
                        else if (min(min(turn1.Znach, turn3.Znach), turn5.Znach) == turn5.Znach)
                        {
                            counter = 0;
                            if (chart1.Series.IsUniqueName("Перевернутый вымпел") == false)
                            {
                                chart1.Series.RemoveAt(chart1.Series.IndexOf("Перевернутый вымпел"));
                            }
                            Series series = new Series("Перевернутый вымпел");
                            series.BorderWidth = 2;
                            //series.BorderColor = Color.Purple;
                            series.ChartType = SeriesChartType.Line;
                            series.Points.AddXY(entery.Date, entery.Znach);
                            series.Points.AddXY(turn1.Date, turn1.Znach);
                            //series.Points.AddXY(turn3.Date, turn3.Znach);
                            series.Points.AddXY(turn5.Date, turn5.Znach);
                            series.Points.AddXY(turn6.Date, turn6.Znach);
                            //series.Points.AddXY(turn4.Date, turn4.Znach);
                            series.Points.AddXY(turn2.Date, turn2.Znach);
                            chart1.Series.Add(series);
                        }
                    }
                }
            }
        }
        private void flagOrPennant(List<Data> dataList, List<MaxANdMins> maxAndMins)
        {
            int counter = 0;
            for (int a = 0; a < maxAndMins.Count - 1; a++)
            {
                if ((maxAndMins[a].max == true && maxAndMins[a + 1].max == true) || (maxAndMins[a].max == false && maxAndMins[a + 1].max == false))
                {
                    counter = 0;
                }
                else
                {
                    counter++;
                }
                if (counter >= 8)
                {
                    var entery = maxAndMins[a - 7];
                    var turn1 = maxAndMins[a - 6];
                    var turn2 = maxAndMins[a - 5];
                    var turn3 = maxAndMins[a - 4];
                    var turn4 = maxAndMins[a - 3];
                    var turn5 = maxAndMins[a - 2];
                    var turn6 = maxAndMins[a - 1];
                    var exit = maxAndMins[a - 0];
                    if(entery.Znach < exit.Znach && entery.Znach<turn1.Znach && exit.Znach>turn6.Znach && (((exit.Znach - entery.Znach) / exit.Znach) * 100 >= 5))
                    {
                        Console.WriteLine(((exit.Znach - entery.Znach) / exit.Znach) * 100 + "--");
                        counter --;
                        var MaxOfMax = max(max(turn1.Znach, turn3.Znach), turn5.Znach);
                        var MinOfMax = min(min(turn1.Znach, turn3.Znach), turn5.Znach);
                        var MaxOfMins = max(max(turn2.Znach, turn4.Znach), turn6.Znach);
                        var MinOfMins = min(min(turn2.Znach, turn4.Znach), turn6.Znach);
                        //flag
                        if (procents(turn1.Znach,turn3.Znach)<5 && procents(turn3.Znach,turn5.Znach)<5 && procents(turn2.Znach,turn4.Znach)<5 && procents(turn4.Znach,turn6.Znach)<5)
                        {
                            counter = 0;
                            if (chart1.Series.IsUniqueName("флаг") == false)
                            {
                                chart1.Series.RemoveAt(chart1.Series.IndexOf("флаг"));
                            }
                            Series series = new Series("флаг");
                            series.ChartType = SeriesChartType.Line;
                            series.BorderWidth = 2;
                            series.Points.AddXY(entery.Date, entery.Znach);
                            series.Points.AddXY(turn1.Date, turn1.Znach);
                            //series.Points.AddXY(turn3.Date, turn3.Znach);
                            series.Points.AddXY(turn5.Date, turn5.Znach);
                            series.Points.AddXY(turn6.Date, turn6.Znach);
                            //series.Points.AddXY(turn4.Date, turn4.Znach);
                            series.Points.AddXY(turn2.Date, turn2.Znach);
                            chart1.Series.Add(series);

                        }
                        //wimpel
                        else if (min(min(turn1.Znach, turn3.Znach),turn5.Znach)==turn5.Znach) 
                        {
                            counter = 0;
                            if (chart1.Series.IsUniqueName("Вымпел") == false)
                            {
                                chart1.Series.RemoveAt(chart1.Series.IndexOf("Вымпел"));
                            }
                            Series series = new Series("Вымпел"); 
                            series.BorderWidth = 2;
                            //series.BorderColor = Color.Purple;
                            series.ChartType = SeriesChartType.Line;
                            series.Points.AddXY(entery.Date, entery.Znach);
                            series.Points.AddXY(turn1.Date, turn1.Znach);
                            //series.Points.AddXY(turn3.Date, turn3.Znach);
                            series.Points.AddXY(turn5.Date, turn5.Znach);
                            series.Points.AddXY(turn6.Date, turn6.Znach);
                            //series.Points.AddXY(turn4.Date, turn4.Znach);
                            series.Points.AddXY(turn2.Date, turn2.Znach);
                            chart1.Series.Add(series);
                        }
                    }
                }
            }
        }
        private double procents (double a, double b)
        {
            if (a > b)
            {
                return (a - b) / a * 100;
            }
            else
            { 
                return(b-a) / b * 100;
            }
        }
        private void DoubleTopAndBottom(List<Data> dataList, List<MaxANdMins> maxAndMins, bool inverted){
            int counter = 0;
            for (int a = 0; a < maxAndMins.Count - 1; a++)
            {
                if ((maxAndMins[a].max == true && maxAndMins[a + 1].max == true) || (maxAndMins[a].max == false && maxAndMins[a + 1].max == false))
                {
                    counter = 0;
                   
                }
                else
                {
                    counter++;
                }
                
                
                    if (inverted)
                    {
                        if (counter >= 5 && maxAndMins[a].max == true)
                        {
                            counter -= 2;
                            var min1 = maxAndMins[a - 4]; //max
                            var max1 = maxAndMins[a - 3]; //min
                            var min2 = maxAndMins[a - 2]; //max
                            var max2 = maxAndMins[a - 1]; //min
                            var min3 = maxAndMins[a]; //max
                            var maxmax = max(max1.Znach, max2.Znach);
                            var minmax = min(max1.Znach, max2.Znach);
                            if ((((maxmax - minmax) / maxmax) * 100 <= 3) && min1.Znach > min2.Znach && min3.Znach > min2.Znach)
                            //Console.WriteLine((((maxmax - minmax) / maxmax) * 100) + " - Pocents"+ maxmax+" - maxmax"+minmax+" - minmax");
                            {
                                counter = 0;
                                if (chart1.Series.IsUniqueName("Перевернутая двойная вершина") == false)
                                {
                                    chart1.Series.RemoveAt(chart1.Series.IndexOf("Перевернутая двойная вершина"));
                                }
                                if (chart1.Series.IsUniqueName("шея") == false)
                                {
                                    chart1.Series.RemoveAt(chart1.Series.IndexOf("шея"));
                                }
                                Series series = new Series("Перевернутая двойная вершина");
                                Series series1 = new Series("шея");
                                series.BorderWidth = 2;
                                series.ChartType = SeriesChartType.Line;
                                series1.ChartType = SeriesChartType.Line;
                                series.Points.AddXY(min1.Date, min1.Znach);
                                series.Points.AddXY(max1.Date, max1.Znach);
                                series.Points.AddXY(min2.Date, min2.Znach);
                                series.Points.AddXY(max2.Date, max2.Znach);
                                series.Points.AddXY(min3.Date, min3.Znach);
                                series1.Points.AddXY(min2.Date, min2.Znach);
                                chart1.Series.Add(series);
                                // chart1.Series.Add(series1);
                            }
                    }
                        
                    }
                    else
                    {
                        if (counter >= 5 && maxAndMins[a].max == false)
                        {
                        counter -= 2;
                        var min1 = maxAndMins[a - 4];
                        var max1 = maxAndMins[a - 3];
                        var min2 = maxAndMins[a - 2];
                        var max2 = maxAndMins[a - 1];
                        var min3 = maxAndMins[a];
                        var maxmax = max(max1.Znach, max2.Znach);
                        var minmax = min(max1.Znach, max2.Znach);
                        if ((((maxmax - minmax) / maxmax) * 100 <= 3) && min1.Znach < min2.Znach && min3.Znach < min2.Znach)
                        {
                            counter = 0;
                            if (chart1.Series.IsUniqueName("Двойная вершина") == false)
                            {
                                chart1.Series.RemoveAt(chart1.Series.IndexOf("Двойная вершина"));
                            }
                            Series series = new Series("Двойная вершина");
                            series.BorderWidth = 3;
                            series.ChartType = SeriesChartType.Line;

                            series.Points.AddXY(min1.Date, min1.Znach);
                            series.Points.AddXY(max1.Date, max1.Znach);
                            series.Points.AddXY(min2.Date, min2.Znach);
                            series.Points.AddXY(max2.Date, max2.Znach);
                            series.Points.AddXY(min3.Date, min3.Znach);
                            chart1.Series.Add(series);



            
        }
       
                    }
                        
                    }
                    
                
            }
        }
        private void HeadAndShoulders(List<Data> dataList, List<MaxANdMins> maxAndMins, bool inverted)
        {
            

            int counter = 0;
           for(int a = 0; a < maxAndMins.Count-1; a++)
            {
                
                if ((maxAndMins[a].max == true && maxAndMins[a+1].max == true)||(maxAndMins[a].max == false && maxAndMins[a+1].max == false))
                {
                    
                    counter = 0;                   
                }
                else
                {
                    counter++;
                }
                if (inverted)
                {
                    if (counter >= 7 && maxAndMins[a].max == true)
                    {
                        //перевернута, поэтому плечи и подмышки поменяны по значению.
                        counter -= 2;
                        var low1 = maxAndMins[a - 6].Znach; //max
                        var leftShoulder = maxAndMins[a - 5].Znach; //min
                        var leftArmpit = maxAndMins[a - 4].Znach; //max
                        var Head = maxAndMins[a - 3].Znach; //min
                        var RightArmpit = maxAndMins[a - 2].Znach; //max
                        var RightShoulder = maxAndMins[a - 1].Znach; //min
                        var low2 = maxAndMins[a].Znach; // max
                        //proverka na golovu i plechi
                        if (leftShoulder > Head && RightShoulder > Head && Math.Abs(leftShoulder - RightShoulder) < 0.05 * Head &&(leftArmpit< min(low1,low2) && RightArmpit < min(low1, low2)))
                        {

                            
                            //proverka na raztyanutosty
                            // if (Math.Abs(leftShoulder - rightShoulder) < 0.05 * head)
                            //{
                            // Найден паттерн "Голова и плечи"
                            if (chart1.Series.IsUniqueName("Перевернутая голова и плечи") == false)
                            {
                                chart1.Series.RemoveAt(chart1.Series.IndexOf("Перевернутая голова и плечи"));
                            }
                            Series series = new Series("Перевернутая голова и плечи");
                            series.ChartType = SeriesChartType.Line;
                            series.BorderWidth = 3;
                            series.BorderColor = Color.Purple;
                            series.Points.AddXY(maxAndMins[a - 6].Date, low1);
                            series.Points.AddXY(maxAndMins[a - 5].Date, leftShoulder);
                            series.Points.AddXY(maxAndMins[a - 4].Date, leftArmpit);
                            series.Points.AddXY(maxAndMins[a - 3].Date, Head);
                            series.Points.AddXY(maxAndMins[a - 2].Date, RightArmpit);
                            series.Points.AddXY(maxAndMins[a - 1].Date, RightShoulder);
                            series.Points.AddXY(maxAndMins[a].Date, low2);
                            series.Points.AddXY(maxAndMins[a - 6].Date, low1);
                            chart1.Series.Add(series);

                           

                            //}
                        }

                    }
                }
                else
                {
                    if (counter >= 7 && maxAndMins[a].max == false)
                    {
                        
                        counter -=2 ;
                        var low1 = maxAndMins[a  - 6].Znach;
                        var leftShoulder = maxAndMins[a - 5].Znach;
                        var leftArmpit = maxAndMins[a - 4].Znach;
                        var Head = maxAndMins[a - 3].Znach;
                        var RightArmpit = maxAndMins[a - 2].Znach;
                        var RightShoulder = maxAndMins[a - 1].Znach;
                        var low2 = maxAndMins[a].Znach;
                        //proverka na golovu i plechi
                        if (leftShoulder < Head && RightShoulder < Head)
                        {
                            //Console.WriteLine("abc");
                            //Console.WriteLine(low1+"-" + leftShoulder +"-"+ leftArmpit + "-" + Head + "-" + RightArmpit + "-" + RightShoulder + "-" + low2 + "-");
                            //proverka na raztyanutosty
                            if (Math.Abs(leftShoulder - RightShoulder) < 0.05 * Head && (leftArmpit > max (low1,low2) && RightArmpit > max (low1, low2)))
                            {
                                
                                if (chart1.Series.IsUniqueName("Голова и плечи") == false)
                                {
                                    chart1.Series.RemoveAt(chart1.Series.IndexOf("Голова и плечи"));
                                }
                                Series series = new Series("Голова и плечи");
                                series.ChartType = SeriesChartType.Line;
                                series.BorderWidth = 3;
                                series.Points.AddXY(maxAndMins[a - 6].Date, low1);
                                series.Points.AddXY(maxAndMins[a - 5].Date, leftShoulder);
                                series.Points.AddXY(maxAndMins[a - 4].Date, leftArmpit);
                                series.Points.AddXY(maxAndMins[a - 3].Date, Head);
                                series.Points.AddXY(maxAndMins[a - 2].Date, RightArmpit);
                                series.Points.AddXY(maxAndMins[a - 1].Date, RightShoulder);
                                series.Points.AddXY(maxAndMins[a].Date, low2);
                                series.Points.AddXY(maxAndMins[a - 6].Date, low1);
                                chart1.Series.Add(series);
                                counter = 0;

                                
                            }
                        }

                    }
                }
               

            }
            
        }
        List<MaxANdMins> IndexofLocalMaximums = new List<MaxANdMins>();
        private double max(double a, double b)
        {
            if(a >b)
            {
                return a;
            }
            else {
                return b;
            }
        }
        private double min(double a, double b)
        {
            if (a < b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }
        private void RollingWindowAlghoritm(List<Data> DataList, int order)
        {
            IndexofLocalMaximums.Clear();
            
                for (int i = order; i < DataList.Count - order; i++)
                {
                    if (CheckForNeighborsMax(DataList, i, order))
                    {
                        //IndexofLocalMaximums.Add(i);
                        IndexofLocalMaximums.Add(new MaxANdMins {index = i, Date = DataList[i].Date, Znach = DataList[i].Value, max=true});
                    }
                    if (CheckForNeighborsMin(DataList, i, order))
                    {
                        // IndexofLocalMaximums.Add(i);
                        IndexofLocalMaximums.Add(new MaxANdMins { index = i, Date = DataList[i].Date, Znach = DataList[i].Value, max = false });
                    }
                }
            //foreach (MaxANdMins index in IndexofLocalMaximums)
            //{
            //    Console.WriteLine(index.index + "--- " + index.Date + "--" + index.Znach + "--" + index.max + "--");
            //}

        }

        private bool CheckForNeighborsMax(List<Data> dataList, int index, int order)
        {
            for (int i = 1; i <= order; i++)
            {
                if (dataList[index].Value <= dataList[index - i].Value || dataList[index].Value <= dataList[index + i].Value)
                {
                    return false;
                }
            }
            return true;
        }
        private bool CheckForNeighborsMin(List<Data> dataList, int index, int order)
        {
            for (int i = 1; i <= order; i++)
            {
                if (dataList[index].Value >= dataList[index - i].Value || dataList[index].Value >= dataList[index + i].Value)
                {
                    return false;
                }
            }
            return true;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            List<Data> Data;
            Data = DataBank.DataList;
            DrawChart(Data);
            checkBox1.CheckState = CheckState.Unchecked;
            checkBox2.CheckState = CheckState.Unchecked;
            checkBox3.CheckState = CheckState.Unchecked;
            checkBox4.CheckState = CheckState.Unchecked;
            checkBox5.CheckState = CheckState.Unchecked;
            RollingWindowAlghoritm(Data, Glubina);
            //HeadAndShoulders(Data, IndexofLocalMaximums, true);
            //HeadAndShoulders(Data, IndexofLocalMaximums, false);
            //DoubleTopAndBottom(Data, IndexofLocalMaximums, true);
            //DoubleTopAndBottom(Data, IndexofLocalMaximums, false);
            //flagOrPennant(Data, IndexofLocalMaximums);
            //ReverseflagOrPennant(Data, IndexofLocalMaximums);
            //RollingWindowAlghoritm(Data, 2);
            //IsTriangle(Data);
            //IsFlagOrPennant(Data);
            //IsDoubleBottom(Data);
            //IsDoubleTop(Data);
            //HeadAndShoulders(Data);
        }
        private void algo(List<Data> Data)
        {
            Glubina = Int32.Parse(numericUpDown1.Value.ToString());
            RollingWindowAlghoritm(Data, Glubina);
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                
                checkBox2.CheckState = CheckState.Unchecked;
                checkBox3.CheckState = CheckState.Unchecked;
                checkBox4.CheckState = CheckState.Unchecked;
                checkBox5.CheckState = CheckState.Unchecked;
                richTextBox1.Text = "Паттерн \"Голова и плечи\" сигнализирует о развороте восходящего тренда в нисходящий.";
                List<Data> Data;
                Data = DataBank.DataList;
                
                HeadAndShoulders(Data, IndexofLocalMaximums, false);
                if (chart1.Series.IsUniqueName("Голова и плечи") == true)
                {
                    MessageBox.Show("Паттерн не найден");
                    checkBox1.CheckState = CheckState.Unchecked;
                }
            }
            else //передаем название для удаления
               {
                if (chart1.Series.IsUniqueName("Голова и плечи") == true)
                {
                }
                else
                {
                    chart1.Series.RemoveAt(chart1.Series.IndexOf("Голова и плечи"));
                }
                 }
        }


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            List<Data> Data;
            Data = DataBank.DataList;
            algo(Data);
        }

      
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                
                checkBox1.CheckState = CheckState.Unchecked;

                checkBox3.CheckState = CheckState.Unchecked;
                checkBox4.CheckState = CheckState.Unchecked;
                checkBox5.CheckState = CheckState.Unchecked;
                checkBox6.CheckState = CheckState.Unchecked;
                List<Data> Data;
                Data = DataBank.DataList;
                richTextBox1.Text = "Перевернутая \"Голова и плечи\" сигнализирует о развороте нисходящего тренда в восходящий.";
                HeadAndShoulders(Data, IndexofLocalMaximums, true);
                if (chart1.Series.IsUniqueName("Перевернутая голова и плечи") == true)
                {
                    MessageBox.Show("Паттерн не найден");
                    checkBox2.CheckState = CheckState.Unchecked;
                }
            }
            else //передаем название для удаления
            {
                if (chart1.Series.IsUniqueName("Перевернутая голова и плечи") == true)
                {
                }
                else
                {
                    chart1.Series.RemoveAt(chart1.Series.IndexOf("Перевернутая голова и плечи"));
                }
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
           
            if (checkBox3.Checked == true)
            {
                checkBox1.CheckState = CheckState.Unchecked;
                checkBox2.CheckState = CheckState.Unchecked;
                checkBox4.CheckState = CheckState.Unchecked;
                checkBox5.CheckState = CheckState.Unchecked;
                checkBox6.CheckState = CheckState.Unchecked;
                List<Data> Data;
                Data = DataBank.DataList;
                richTextBox1.Text = "Двойная вершина сигнализирует о возможном развороте восходящего тренда в нисходящий.";
                DoubleTopAndBottom(Data, IndexofLocalMaximums, false);
                if (chart1.Series.IsUniqueName("Двойная вершина") == true)
                {
                    MessageBox.Show("Паттерн не найден");
                    checkBox3.CheckState = CheckState.Unchecked;
                }
            }
            else //передаем название для удаления
            {
                if (chart1.Series.IsUniqueName("Двойная вершина") == true)
                {
                }
                else
                {
                    chart1.Series.RemoveAt(chart1.Series.IndexOf("Двойная вершина"));
                }
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                
                checkBox1.CheckState = CheckState.Unchecked;
                checkBox2.CheckState = CheckState.Unchecked;
                checkBox3.CheckState = CheckState.Unchecked;
                checkBox5.CheckState = CheckState.Unchecked;
                checkBox6.CheckState = CheckState.Unchecked;
                List<Data> Data;
                Data = DataBank.DataList;
                richTextBox1.Text = "Двойное дно сигнализирует о возможном развороте нисходящего тренда в восходящий.";
                DoubleTopAndBottom(Data, IndexofLocalMaximums, true);
                if (chart1.Series.IsUniqueName("Перевернутая двойная вершина") == true)
                {
                    MessageBox.Show("Паттерн не найден");
                    checkBox4.CheckState = CheckState.Unchecked;
                }
            }
            else //передаем название для удаления
            {
                if (chart1.Series.IsUniqueName("Перевернутая двойная вершина") == true)
                {
                }
                else
                {
                    chart1.Series.RemoveAt(chart1.Series.IndexOf("Перевернутая двойная вершина"));
                }
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                
                checkBox1.CheckState = CheckState.Unchecked;
                checkBox2.CheckState = CheckState.Unchecked;
                checkBox3.CheckState = CheckState.Unchecked;
                checkBox4.CheckState = CheckState.Unchecked;
                checkBox6.CheckState = CheckState.Unchecked;
                richTextBox1.Text = "Эти паттерны считаются паттернами продолжения тренда. Они сигнализируют о краткосрочной паузе в тренде, после которой тренд, скорее всего, продолжится в том же направлении.";
                List<Data> Data;
                Data = DataBank.DataList;

                flagOrPennant(Data, IndexofLocalMaximums);
                if (chart1.Series.IsUniqueName("флаг") == true && chart1.Series.IsUniqueName("Вымпел") == true)
                {
                    MessageBox.Show("Паттерн не найден");
                    checkBox5.CheckState = CheckState.Unchecked;
                }
            }
            else //передаем название для удаления
            {
                if (chart1.Series.IsUniqueName("флаг") == true)
                {
                }
                else
                {
                    chart1.Series.RemoveAt(chart1.Series.IndexOf("флаг"));
                }
                if (chart1.Series.IsUniqueName("Вымпел") == true)
                {
                }
                else
                {
                    chart1.Series.RemoveAt(chart1.Series.IndexOf("Вымпел"));
                }
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
            {
                
                checkBox1.CheckState = CheckState.Unchecked;
                checkBox2.CheckState = CheckState.Unchecked;
                checkBox3.CheckState = CheckState.Unchecked;
                checkBox4.CheckState = CheckState.Unchecked;
                checkBox5.CheckState = CheckState.Unchecked;
                richTextBox1.Text = "Эти паттерны также считаются паттернами продолжения тренда, но они указывают на продолжение нисходящего тренда.";
                List<Data> Data;
                Data = DataBank.DataList;

                ReverseflagOrPennant(Data, IndexofLocalMaximums);
                if (chart1.Series.IsUniqueName("Перевернутый флаг") == true && chart1.Series.IsUniqueName("Перевернутый вымпел") == true)
                {
                    MessageBox.Show("Паттерн не найден");
                    checkBox6.CheckState = CheckState.Unchecked;
                }
            }
            else //передаем название для удаления
            {
                if (chart1.Series.IsUniqueName("Перевернутый флаг") == true)
                {
                }
                else
                {
                    chart1.Series.RemoveAt(chart1.Series.IndexOf("Перевернутый флаг"));
                }
                if (chart1.Series.IsUniqueName("Перевернутый вымпел") == true)
                {
                }
                else
                {
                    chart1.Series.RemoveAt(chart1.Series.IndexOf("Перевернутый вымпел"));
                }
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

                    Paragraph subTitle = new Paragraph("Ниже представлен график с найденным на нём паттерном:", font);
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

                    

                    pdfDoc.Close();
                }

                MessageBox.Show("PDF документ успешно создан!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void label2_Click(object sender, EventArgs e)
        {
            // Показать первую форму
            form1.Show();

            // Закрыть текущую форму
            this.Close();
        }
    }
}
