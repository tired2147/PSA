namespace PSA
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.менюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьКакToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.какPDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.какDOCXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadData = new System.Windows.Forms.Button();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.Form2LabelClick = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonExportDocx = new System.Windows.Forms.Button();
            this.pageSetupDialog2 = new System.Windows.Forms.PageSetupDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.buttonExportPdf = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel11.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Azure;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.менюToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1169, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // менюToolStripMenuItem
            // 
            this.менюToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.загрузитьToolStripMenuItem,
            this.сохранитьКакToolStripMenuItem});
            this.менюToolStripMenuItem.Name = "менюToolStripMenuItem";
            this.менюToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.менюToolStripMenuItem.Text = "Меню";
            // 
            // загрузитьToolStripMenuItem
            // 
            this.загрузитьToolStripMenuItem.Name = "загрузитьToolStripMenuItem";
            this.загрузитьToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.загрузитьToolStripMenuItem.Text = "Загрузить данные";
            this.загрузитьToolStripMenuItem.Click += new System.EventHandler(this.загрузитьToolStripMenuItem_Click);
            // 
            // сохранитьКакToolStripMenuItem
            // 
            this.сохранитьКакToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.какPDFToolStripMenuItem,
            this.какDOCXToolStripMenuItem});
            this.сохранитьКакToolStripMenuItem.Name = "сохранитьКакToolStripMenuItem";
            this.сохранитьКакToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.сохранитьКакToolStripMenuItem.Text = "Сохранить как";
            this.сохранитьКакToolStripMenuItem.Visible = false;
            // 
            // какPDFToolStripMenuItem
            // 
            this.какPDFToolStripMenuItem.Name = "какPDFToolStripMenuItem";
            this.какPDFToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.какPDFToolStripMenuItem.Text = "Как PDF";
            this.какPDFToolStripMenuItem.Click += new System.EventHandler(this.какPDFToolStripMenuItem_Click);
            // 
            // какDOCXToolStripMenuItem
            // 
            this.какDOCXToolStripMenuItem.Name = "какDOCXToolStripMenuItem";
            this.какDOCXToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.какDOCXToolStripMenuItem.Text = "Как DOCX";
            this.какDOCXToolStripMenuItem.Click += new System.EventHandler(this.какDOCXToolStripMenuItem_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            this.справкаToolStripMenuItem.Click += new System.EventHandler(this.справкаToolStripMenuItem_Click);
            // 
            // downloadData
            // 
            this.downloadData.BackColor = System.Drawing.Color.Silver;
            this.downloadData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.downloadData.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.downloadData.Location = new System.Drawing.Point(9, 8);
            this.downloadData.Margin = new System.Windows.Forms.Padding(1);
            this.downloadData.Name = "downloadData";
            this.downloadData.Size = new System.Drawing.Size(186, 48);
            this.downloadData.TabIndex = 1;
            this.downloadData.Text = "Загрузить данные";
            this.downloadData.UseVisualStyleBackColor = false;
            this.downloadData.Click += new System.EventHandler(this.downloadData_Click);
            // 
            // Form2LabelClick
            // 
            this.Form2LabelClick.AutoSize = true;
            this.Form2LabelClick.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Form2LabelClick.Location = new System.Drawing.Point(17, 10);
            this.Form2LabelClick.Name = "Form2LabelClick";
            this.Form2LabelClick.Size = new System.Drawing.Size(268, 20);
            this.Form2LabelClick.TabIndex = 8;
            this.Form2LabelClick.Text = "Найти паттерны на граффике";
            this.Form2LabelClick.Click += new System.EventHandler(this.Form2LabelClick_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label6.Location = new System.Drawing.Point(50, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 33);
            this.label6.TabIndex = 7;
            this.label6.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label5.Location = new System.Drawing.Point(50, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 33);
            this.label5.TabIndex = 6;
            this.label5.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(52, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 33);
            this.label4.TabIndex = 5;
            this.label4.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(52, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 33);
            this.label3.TabIndex = 4;
            this.label3.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(51, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 33);
            this.label1.TabIndex = 2;
            this.label1.Text = "0";
            // 
            // buttonExportDocx
            // 
            this.buttonExportDocx.BackColor = System.Drawing.Color.Silver;
            this.buttonExportDocx.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonExportDocx.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonExportDocx.Location = new System.Drawing.Point(12, 8);
            this.buttonExportDocx.Margin = new System.Windows.Forms.Padding(1);
            this.buttonExportDocx.Name = "buttonExportDocx";
            this.buttonExportDocx.Size = new System.Drawing.Size(186, 48);
            this.buttonExportDocx.TabIndex = 7;
            this.buttonExportDocx.Text = "Экспортировать как DOCX";
            this.buttonExportDocx.UseVisualStyleBackColor = false;
            this.buttonExportDocx.Click += new System.EventHandler(this.buttonExportDocx_Click);
            // 
            // buttonExportPdf
            // 
            this.buttonExportPdf.BackColor = System.Drawing.Color.Silver;
            this.buttonExportPdf.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonExportPdf.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonExportPdf.Location = new System.Drawing.Point(10, 8);
            this.buttonExportPdf.Margin = new System.Windows.Forms.Padding(1);
            this.buttonExportPdf.Name = "buttonExportPdf";
            this.buttonExportPdf.Size = new System.Drawing.Size(186, 48);
            this.buttonExportPdf.TabIndex = 8;
            this.buttonExportPdf.Text = "Экспортировать как PDF";
            this.buttonExportPdf.UseVisualStyleBackColor = false;
            this.buttonExportPdf.Click += new System.EventHandler(this.buttonExportPdf_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(52, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 33);
            this.label2.TabIndex = 3;
            this.label2.Text = "0";
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.checkBox5);
            this.panel1.Controls.Add(this.chart1);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.checkBox2);
            this.panel1.Controls.Add(this.checkBox3);
            this.panel1.Controls.Add(this.checkBox4);
            this.panel1.Location = new System.Drawing.Point(34, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(649, 505);
            this.panel1.TabIndex = 2;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(17, 476);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(103, 17);
            this.checkBox5.TabIndex = 7;
            this.checkBox5.Text = "Тренд графика";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
            // 
            // chart1
            // 
            this.chart1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chart1.BackColor = System.Drawing.Color.Azure;
            chartArea1.BorderWidth = 0;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Alignment = System.Drawing.StringAlignment.Center;
            legend1.BackColor = System.Drawing.Color.Azure;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Row;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            this.chart1.Size = new System.Drawing.Size(649, 466);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(563, 476);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(71, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Медиана";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckStateChanged += new System.EventHandler(this.checkBox1_CheckStateChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(150, 476);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(99, 17);
            this.checkBox2.TabIndex = 4;
            this.checkBox2.Text = "Левый предел";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckStateChanged += new System.EventHandler(this.checkBox2_CheckStateChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(275, 476);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(105, 17);
            this.checkBox3.TabIndex = 5;
            this.checkBox3.Text = "Правый предел";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckStateChanged += new System.EventHandler(this.checkBox3_CheckStateChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(413, 476);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(119, 17);
            this.checkBox4.TabIndex = 6;
            this.checkBox4.Text = "Среднее значение";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel2.AutoSize = true;
            this.panel2.BackColor = System.Drawing.Color.LightCoral;
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(684, 69);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(226, 131);
            this.panel2.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label7.Location = new System.Drawing.Point(14, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 20);
            this.label7.TabIndex = 4;
            this.label7.Text = "Медиана";
            // 
            // panel3
            // 
            this.panel3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel3.AutoSize = true;
            this.panel3.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Location = new System.Drawing.Point(684, 343);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(226, 131);
            this.panel3.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label8.Location = new System.Drawing.Point(14, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 20);
            this.label8.TabIndex = 5;
            this.label8.Text = "Дисперсия";
            // 
            // panel4
            // 
            this.panel4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel4.AutoSize = true;
            this.panel4.BackColor = System.Drawing.Color.SandyBrown;
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Location = new System.Drawing.Point(684, 206);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(226, 131);
            this.panel4.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label9.Location = new System.Drawing.Point(14, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(130, 20);
            this.label9.TabIndex = 4;
            this.label9.Text = "Левый предел";
            // 
            // panel5
            // 
            this.panel5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel5.AutoSize = true;
            this.panel5.BackColor = System.Drawing.Color.MediumTurquoise;
            this.panel5.Controls.Add(this.label10);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Location = new System.Drawing.Point(917, 206);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(226, 131);
            this.panel5.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label10.Location = new System.Drawing.Point(14, 13);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(141, 20);
            this.label10.TabIndex = 4;
            this.label10.Text = "Правый предел";
            // 
            // panel6
            // 
            this.panel6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel6.AutoSize = true;
            this.panel6.BackColor = System.Drawing.Color.YellowGreen;
            this.panel6.Controls.Add(this.label11);
            this.panel6.Controls.Add(this.label1);
            this.panel6.Location = new System.Drawing.Point(916, 69);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(226, 131);
            this.panel6.TabIndex = 11;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label11.Location = new System.Drawing.Point(14, 13);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(167, 20);
            this.label11.TabIndex = 5;
            this.label11.Text = "Среднее значение";
            // 
            // panel7
            // 
            this.panel7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel7.BackColor = System.Drawing.Color.Plum;
            this.panel7.Controls.Add(this.label12);
            this.panel7.Controls.Add(this.label6);
            this.panel7.Location = new System.Drawing.Point(917, 343);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(226, 131);
            this.panel7.TabIndex = 11;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label12.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label12.Location = new System.Drawing.Point(14, 13);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(156, 20);
            this.label12.TabIndex = 5;
            this.label12.Text = "Стоимость акций";
            // 
            // panel8
            // 
            this.panel8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel8.AutoSize = true;
            this.panel8.Controls.Add(this.Form2LabelClick);
            this.panel8.Location = new System.Drawing.Point(770, 500);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(306, 42);
            this.panel8.TabIndex = 12;
            this.panel8.Visible = false;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.downloadData);
            this.panel9.Location = new System.Drawing.Point(245, 601);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(203, 66);
            this.panel9.TabIndex = 13;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.buttonExportDocx);
            this.panel10.Location = new System.Drawing.Point(703, 601);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(207, 66);
            this.panel10.TabIndex = 14;
            this.panel10.Visible = false;
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.buttonExportPdf);
            this.panel11.Location = new System.Drawing.Point(916, 601);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(207, 66);
            this.panel11.TabIndex = 15;
            this.panel11.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(1169, 728);
            this.Controls.Add(this.panel11);
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Анализатор акций";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem менюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.Button downloadData;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;

        private System.Windows.Forms.Button buttonExportDocx;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button buttonExportPdf;

        private System.Windows.Forms.Label Form2LabelClick;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.ToolStripMenuItem загрузитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьКакToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem какPDFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem какDOCXToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBox5;
    }
}

