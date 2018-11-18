namespace App
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьКакToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.правкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сбросToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.вГрадацияхСерогоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.собельToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.графикиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.контрастированиеToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.grayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.линейноеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.эквализацияToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.yIQToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.линейноеToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.эквализацияToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.дисторсияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.комплексированиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.кластеризацияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.правкаToolStripMenuItem,
            this.графикиToolStripMenuItem,
            this.контрастированиеToolStripMenuItem1,
            this.infoToolStripMenuItem,
            this.дисторсияToolStripMenuItem,
            this.комплексированиеToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(898, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьToolStripMenuItem,
            this.сохранитьКакToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.открытьToolStripMenuItem.Text = "Открыть";
            this.открытьToolStripMenuItem.Click += new System.EventHandler(this.открытьToolStripMenuItem_Click);
            // 
            // сохранитьКакToolStripMenuItem
            // 
            this.сохранитьКакToolStripMenuItem.Name = "сохранитьКакToolStripMenuItem";
            this.сохранитьКакToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.сохранитьКакToolStripMenuItem.Text = "Сохранить как...";
            this.сохранитьКакToolStripMenuItem.Click += new System.EventHandler(this.сохранитьКакToolStripMenuItem_Click);
            // 
            // правкаToolStripMenuItem
            // 
            this.правкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сбросToolStripMenuItem,
            this.вГрадацияхСерогоToolStripMenuItem,
            this.собельToolStripMenuItem,
            this.toolStripMenuItem1,
            this.кластеризацияToolStripMenuItem});
            this.правкаToolStripMenuItem.Name = "правкаToolStripMenuItem";
            this.правкаToolStripMenuItem.Size = new System.Drawing.Size(112, 20);
            this.правкаToolStripMenuItem.Text = "Преобразования";
            // 
            // сбросToolStripMenuItem
            // 
            this.сбросToolStripMenuItem.Name = "сбросToolStripMenuItem";
            this.сбросToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.сбросToolStripMenuItem.Text = "Сброс";
            this.сбросToolStripMenuItem.Click += new System.EventHandler(this.сбросToolStripMenuItem_Click);
            // 
            // вГрадацияхСерогоToolStripMenuItem
            // 
            this.вГрадацияхСерогоToolStripMenuItem.Name = "вГрадацияхСерогоToolStripMenuItem";
            this.вГрадацияхСерогоToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.вГрадацияхСерогоToolStripMenuItem.Text = "В градациях серого";
            this.вГрадацияхСерогоToolStripMenuItem.Click += new System.EventHandler(this.вГрадацияхСерогоToolStripMenuItem_Click);
            // 
            // собельToolStripMenuItem
            // 
            this.собельToolStripMenuItem.Name = "собельToolStripMenuItem";
            this.собельToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.собельToolStripMenuItem.Text = "Собель";
            this.собельToolStripMenuItem.Click += new System.EventHandler(this.собельToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem1.Text = "Дисперсия";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(109, 22);
            this.toolStripMenuItem2.Text = "15 x 15";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(109, 22);
            this.toolStripMenuItem3.Text = "31 x 31";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(109, 22);
            this.toolStripMenuItem4.Text = "40 x 40";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // графикиToolStripMenuItem
            // 
            this.графикиToolStripMenuItem.Name = "графикиToolStripMenuItem";
            this.графикиToolStripMenuItem.Size = new System.Drawing.Size(92, 20);
            this.графикиToolStripMenuItem.Text = "Гистограмма";
            this.графикиToolStripMenuItem.Click += new System.EventHandler(this.графикиToolStripMenuItem_Click);
            // 
            // контрастированиеToolStripMenuItem1
            // 
            this.контрастированиеToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.grayToolStripMenuItem,
            this.yIQToolStripMenuItem1});
            this.контрастированиеToolStripMenuItem1.Name = "контрастированиеToolStripMenuItem1";
            this.контрастированиеToolStripMenuItem1.Size = new System.Drawing.Size(122, 20);
            this.контрастированиеToolStripMenuItem1.Text = "Контрастирование";
            // 
            // grayToolStripMenuItem
            // 
            this.grayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.линейноеToolStripMenuItem,
            this.эквализацияToolStripMenuItem1});
            this.grayToolStripMenuItem.Name = "grayToolStripMenuItem";
            this.grayToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.grayToolStripMenuItem.Text = "Gray";
            // 
            // линейноеToolStripMenuItem
            // 
            this.линейноеToolStripMenuItem.Name = "линейноеToolStripMenuItem";
            this.линейноеToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.линейноеToolStripMenuItem.Text = "Линейное";
            this.линейноеToolStripMenuItem.Click += new System.EventHandler(this.линейноеToolStripMenuItem_Click);
            // 
            // эквализацияToolStripMenuItem1
            // 
            this.эквализацияToolStripMenuItem1.Name = "эквализацияToolStripMenuItem1";
            this.эквализацияToolStripMenuItem1.Size = new System.Drawing.Size(144, 22);
            this.эквализацияToolStripMenuItem1.Text = "Эквализация";
            this.эквализацияToolStripMenuItem1.Click += new System.EventHandler(this.эквализацияToolStripMenuItem1_Click);
            // 
            // yIQToolStripMenuItem1
            // 
            this.yIQToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.линейноеToolStripMenuItem1,
            this.эквализацияToolStripMenuItem2});
            this.yIQToolStripMenuItem1.Name = "yIQToolStripMenuItem1";
            this.yIQToolStripMenuItem1.Size = new System.Drawing.Size(98, 22);
            this.yIQToolStripMenuItem1.Text = "YIQ";
            // 
            // линейноеToolStripMenuItem1
            // 
            this.линейноеToolStripMenuItem1.Name = "линейноеToolStripMenuItem1";
            this.линейноеToolStripMenuItem1.Size = new System.Drawing.Size(144, 22);
            this.линейноеToolStripMenuItem1.Text = "Линейное";
            this.линейноеToolStripMenuItem1.Click += new System.EventHandler(this.линейноеToolStripMenuItem1_Click);
            // 
            // эквализацияToolStripMenuItem2
            // 
            this.эквализацияToolStripMenuItem2.Name = "эквализацияToolStripMenuItem2";
            this.эквализацияToolStripMenuItem2.Size = new System.Drawing.Size(144, 22);
            this.эквализацияToolStripMenuItem2.Text = "Эквализация";
            this.эквализацияToolStripMenuItem2.Click += new System.EventHandler(this.эквализацияToolStripMenuItem2_Click);
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(134, 20);
            this.infoToolStripMenuItem.Text = "Показатели качества";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // дисторсияToolStripMenuItem
            // 
            this.дисторсияToolStripMenuItem.Name = "дисторсияToolStripMenuItem";
            this.дисторсияToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.дисторсияToolStripMenuItem.Text = "Дисторсия";
            this.дисторсияToolStripMenuItem.Click += new System.EventHandler(this.дисторсияToolStripMenuItem_Click);
            // 
            // комплексированиеToolStripMenuItem
            // 
            this.комплексированиеToolStripMenuItem.Name = "комплексированиеToolStripMenuItem";
            this.комплексированиеToolStripMenuItem.Size = new System.Drawing.Size(127, 20);
            this.комплексированиеToolStripMenuItem.Text = "Комплексирование";
            this.комплексированиеToolStripMenuItem.Click += new System.EventHandler(this.комплексированиеToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(898, 504);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // кластеризацияToolStripMenuItem
            // 
            this.кластеризацияToolStripMenuItem.Name = "кластеризацияToolStripMenuItem";
            this.кластеризацияToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.кластеризацияToolStripMenuItem.Text = "Кластеризация";
            this.кластеризацияToolStripMenuItem.Click += new System.EventHandler(this.кластеризацияToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 528);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "App";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьКакToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem правкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem вГрадацияхСерогоToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сбросToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem графикиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem собельToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem дисторсияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem контрастированиеToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem grayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem линейноеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem эквализацияToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem yIQToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem линейноеToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem эквализацияToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem комплексированиеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem кластеризацияToolStripMenuItem;
    }
}

