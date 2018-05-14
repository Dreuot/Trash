namespace ImageTransform
{
    partial class MainForm
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.преобразоватьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сольИПерецToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.медианныйФильтрToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x7ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x9ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.гаусToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.фильтрГауссаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.чернобелоеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.градацииСерогоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сброситьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.границыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.собельToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.всеНаправленияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.морфологическиеОперацииToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.эрозияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.дилекцияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.афинныеПреобразованияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.поворотToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.растяжениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отражениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.вертикальноToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.горизонтальноToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сдвигToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.совмещениеИзображенийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выбратьИзображенияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выполнитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сброToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.верхШляпыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.низШляпыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(18, 42);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(952, 732);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.преобразоватьToolStripMenuItem,
            this.границыToolStripMenuItem,
            this.морфологическиеОперацииToolStripMenuItem,
            this.афинныеПреобразованияToolStripMenuItem,
            this.совмещениеИзображенийToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1912, 35);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(65, 29);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(166, 30);
            this.открытьToolStripMenuItem.Text = "Открыть";
            this.открытьToolStripMenuItem.Click += new System.EventHandler(this.открытьToolStripMenuItem_Click);
            // 
            // преобразоватьToolStripMenuItem
            // 
            this.преобразоватьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сольИПерецToolStripMenuItem,
            this.медианныйФильтрToolStripMenuItem,
            this.гаусToolStripMenuItem,
            this.фильтрГауссаToolStripMenuItem,
            this.чернобелоеToolStripMenuItem,
            this.градацииСерогоToolStripMenuItem,
            this.сброситьToolStripMenuItem});
            this.преобразоватьToolStripMenuItem.Name = "преобразоватьToolStripMenuItem";
            this.преобразоватьToolStripMenuItem.Size = new System.Drawing.Size(152, 29);
            this.преобразоватьToolStripMenuItem.Text = "Преобразовать";
            // 
            // сольИПерецToolStripMenuItem
            // 
            this.сольИПерецToolStripMenuItem.Name = "сольИПерецToolStripMenuItem";
            this.сольИПерецToolStripMenuItem.Size = new System.Drawing.Size(256, 30);
            this.сольИПерецToolStripMenuItem.Text = "Соль и перец";
            this.сольИПерецToolStripMenuItem.Click += new System.EventHandler(this.сольИПерецToolStripMenuItem_Click);
            // 
            // медианныйФильтрToolStripMenuItem
            // 
            this.медианныйФильтрToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.x3ToolStripMenuItem,
            this.x5ToolStripMenuItem,
            this.x7ToolStripMenuItem,
            this.x9ToolStripMenuItem});
            this.медианныйФильтрToolStripMenuItem.Name = "медианныйФильтрToolStripMenuItem";
            this.медианныйФильтрToolStripMenuItem.Size = new System.Drawing.Size(256, 30);
            this.медианныйФильтрToolStripMenuItem.Text = "Медианный фильтр";
            // 
            // x3ToolStripMenuItem
            // 
            this.x3ToolStripMenuItem.Name = "x3ToolStripMenuItem";
            this.x3ToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.x3ToolStripMenuItem.Text = "3x3";
            this.x3ToolStripMenuItem.Click += new System.EventHandler(this.x3ToolStripMenuItem_Click);
            // 
            // x5ToolStripMenuItem
            // 
            this.x5ToolStripMenuItem.Name = "x5ToolStripMenuItem";
            this.x5ToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.x5ToolStripMenuItem.Text = "5x5";
            this.x5ToolStripMenuItem.Click += new System.EventHandler(this.x5ToolStripMenuItem_Click);
            // 
            // x7ToolStripMenuItem
            // 
            this.x7ToolStripMenuItem.Name = "x7ToolStripMenuItem";
            this.x7ToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.x7ToolStripMenuItem.Text = "7x7";
            this.x7ToolStripMenuItem.Click += new System.EventHandler(this.x7ToolStripMenuItem_Click);
            // 
            // x9ToolStripMenuItem
            // 
            this.x9ToolStripMenuItem.Name = "x9ToolStripMenuItem";
            this.x9ToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.x9ToolStripMenuItem.Text = "9x9";
            this.x9ToolStripMenuItem.Click += new System.EventHandler(this.x9ToolStripMenuItem_Click);
            // 
            // гаусToolStripMenuItem
            // 
            this.гаусToolStripMenuItem.Name = "гаусToolStripMenuItem";
            this.гаусToolStripMenuItem.Size = new System.Drawing.Size(256, 30);
            this.гаусToolStripMenuItem.Text = "Гаусс";
            this.гаусToolStripMenuItem.Click += new System.EventHandler(this.гаусToolStripMenuItem_Click);
            // 
            // фильтрГауссаToolStripMenuItem
            // 
            this.фильтрГауссаToolStripMenuItem.Name = "фильтрГауссаToolStripMenuItem";
            this.фильтрГауссаToolStripMenuItem.Size = new System.Drawing.Size(256, 30);
            this.фильтрГауссаToolStripMenuItem.Text = "Фильтр Гаусса";
            this.фильтрГауссаToolStripMenuItem.Click += new System.EventHandler(this.фильтрГауссаToolStripMenuItem_Click);
            // 
            // чернобелоеToolStripMenuItem
            // 
            this.чернобелоеToolStripMenuItem.Name = "чернобелоеToolStripMenuItem";
            this.чернобелоеToolStripMenuItem.Size = new System.Drawing.Size(256, 30);
            this.чернобелоеToolStripMenuItem.Text = "Черно-белое";
            this.чернобелоеToolStripMenuItem.Click += new System.EventHandler(this.чернобелоеToolStripMenuItem_Click);
            // 
            // градацииСерогоToolStripMenuItem
            // 
            this.градацииСерогоToolStripMenuItem.Name = "градацииСерогоToolStripMenuItem";
            this.градацииСерогоToolStripMenuItem.Size = new System.Drawing.Size(256, 30);
            this.градацииСерогоToolStripMenuItem.Text = "Градации серого";
            this.градацииСерогоToolStripMenuItem.Click += new System.EventHandler(this.градацииСерогоToolStripMenuItem_Click);
            // 
            // сброситьToolStripMenuItem
            // 
            this.сброситьToolStripMenuItem.Name = "сброситьToolStripMenuItem";
            this.сброситьToolStripMenuItem.Size = new System.Drawing.Size(256, 30);
            this.сброситьToolStripMenuItem.Text = "Сбросить";
            this.сброситьToolStripMenuItem.Click += new System.EventHandler(this.сброситьToolStripMenuItem_Click);
            // 
            // границыToolStripMenuItem
            // 
            this.границыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.собельToolStripMenuItem,
            this.всеНаправленияToolStripMenuItem});
            this.границыToolStripMenuItem.Name = "границыToolStripMenuItem";
            this.границыToolStripMenuItem.Size = new System.Drawing.Size(96, 29);
            this.границыToolStripMenuItem.Text = "Границы";
            // 
            // собельToolStripMenuItem
            // 
            this.собельToolStripMenuItem.Name = "собельToolStripMenuItem";
            this.собельToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.собельToolStripMenuItem.Text = "Собель";
            this.собельToolStripMenuItem.Click += new System.EventHandler(this.собельToolStripMenuItem_Click);
            // 
            // всеНаправленияToolStripMenuItem
            // 
            this.всеНаправленияToolStripMenuItem.Name = "всеНаправленияToolStripMenuItem";
            this.всеНаправленияToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.всеНаправленияToolStripMenuItem.Text = "Все направления";
            this.всеНаправленияToolStripMenuItem.Click += new System.EventHandler(this.всеНаправленияToolStripMenuItem_Click);
            // 
            // морфологическиеОперацииToolStripMenuItem
            // 
            this.морфологическиеОперацииToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.эрозияToolStripMenuItem,
            this.дилекцияToolStripMenuItem,
            this.верхШляпыToolStripMenuItem,
            this.низШляпыToolStripMenuItem});
            this.морфологическиеОперацииToolStripMenuItem.Name = "морфологическиеОперацииToolStripMenuItem";
            this.морфологическиеОперацииToolStripMenuItem.Size = new System.Drawing.Size(263, 29);
            this.морфологическиеОперацииToolStripMenuItem.Text = "Морфологические операции";
            // 
            // эрозияToolStripMenuItem
            // 
            this.эрозияToolStripMenuItem.Name = "эрозияToolStripMenuItem";
            this.эрозияToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.эрозияToolStripMenuItem.Text = "Дилатация";
            this.эрозияToolStripMenuItem.Click += new System.EventHandler(this.эрозияToolStripMenuItem_Click);
            // 
            // дилекцияToolStripMenuItem
            // 
            this.дилекцияToolStripMenuItem.Name = "дилекцияToolStripMenuItem";
            this.дилекцияToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.дилекцияToolStripMenuItem.Text = "Эрозия";
            this.дилекцияToolStripMenuItem.Click += new System.EventHandler(this.дилекцияToolStripMenuItem_Click);
            // 
            // афинныеПреобразованияToolStripMenuItem
            // 
            this.афинныеПреобразованияToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.поворотToolStripMenuItem,
            this.растяжениеToolStripMenuItem,
            this.отражениеToolStripMenuItem,
            this.сдвигToolStripMenuItem});
            this.афинныеПреобразованияToolStripMenuItem.Name = "афинныеПреобразованияToolStripMenuItem";
            this.афинныеПреобразованияToolStripMenuItem.Size = new System.Drawing.Size(243, 29);
            this.афинныеПреобразованияToolStripMenuItem.Text = "Афинные преобразования";
            // 
            // поворотToolStripMenuItem
            // 
            this.поворотToolStripMenuItem.Name = "поворотToolStripMenuItem";
            this.поворотToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.поворотToolStripMenuItem.Text = "Поворот";
            this.поворотToolStripMenuItem.Click += new System.EventHandler(this.поворотToolStripMenuItem_Click);
            // 
            // растяжениеToolStripMenuItem
            // 
            this.растяжениеToolStripMenuItem.Name = "растяжениеToolStripMenuItem";
            this.растяжениеToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.растяжениеToolStripMenuItem.Text = "Растяжение";
            this.растяжениеToolStripMenuItem.Click += new System.EventHandler(this.растяжениеToolStripMenuItem_Click);
            // 
            // отражениеToolStripMenuItem
            // 
            this.отражениеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.вертикальноToolStripMenuItem,
            this.горизонтальноToolStripMenuItem});
            this.отражениеToolStripMenuItem.Name = "отражениеToolStripMenuItem";
            this.отражениеToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.отражениеToolStripMenuItem.Text = "Отражение";
            // 
            // вертикальноToolStripMenuItem
            // 
            this.вертикальноToolStripMenuItem.Name = "вертикальноToolStripMenuItem";
            this.вертикальноToolStripMenuItem.Size = new System.Drawing.Size(220, 30);
            this.вертикальноToolStripMenuItem.Text = "Вертикально";
            this.вертикальноToolStripMenuItem.Click += new System.EventHandler(this.вертикальноToolStripMenuItem_Click);
            // 
            // горизонтальноToolStripMenuItem
            // 
            this.горизонтальноToolStripMenuItem.Name = "горизонтальноToolStripMenuItem";
            this.горизонтальноToolStripMenuItem.Size = new System.Drawing.Size(220, 30);
            this.горизонтальноToolStripMenuItem.Text = "Горизонтально";
            this.горизонтальноToolStripMenuItem.Click += new System.EventHandler(this.горизонтальноToolStripMenuItem_Click);
            // 
            // сдвигToolStripMenuItem
            // 
            this.сдвигToolStripMenuItem.Name = "сдвигToolStripMenuItem";
            this.сдвигToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.сдвигToolStripMenuItem.Text = "Сдвиг";
            this.сдвигToolStripMenuItem.Click += new System.EventHandler(this.сдвигToolStripMenuItem_Click);
            // 
            // совмещениеИзображенийToolStripMenuItem
            // 
            this.совмещениеИзображенийToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.выбратьИзображенияToolStripMenuItem,
            this.выполнитьToolStripMenuItem,
            this.сброToolStripMenuItem});
            this.совмещениеИзображенийToolStripMenuItem.Name = "совмещениеИзображенийToolStripMenuItem";
            this.совмещениеИзображенийToolStripMenuItem.Size = new System.Drawing.Size(247, 29);
            this.совмещениеИзображенийToolStripMenuItem.Text = "Совмещение изображений";
            // 
            // выбратьИзображенияToolStripMenuItem
            // 
            this.выбратьИзображенияToolStripMenuItem.Name = "выбратьИзображенияToolStripMenuItem";
            this.выбратьИзображенияToolStripMenuItem.Size = new System.Drawing.Size(280, 30);
            this.выбратьИзображенияToolStripMenuItem.Text = "Выбрать изображение";
            this.выбратьИзображенияToolStripMenuItem.Click += new System.EventHandler(this.выбратьИзображенияToolStripMenuItem_Click);
            // 
            // выполнитьToolStripMenuItem
            // 
            this.выполнитьToolStripMenuItem.Name = "выполнитьToolStripMenuItem";
            this.выполнитьToolStripMenuItem.Size = new System.Drawing.Size(280, 30);
            this.выполнитьToolStripMenuItem.Text = "Выполнить";
            this.выполнитьToolStripMenuItem.Click += new System.EventHandler(this.выполнитьToolStripMenuItem_Click);
            // 
            // сброToolStripMenuItem
            // 
            this.сброToolStripMenuItem.Name = "сброToolStripMenuItem";
            this.сброToolStripMenuItem.Size = new System.Drawing.Size(280, 30);
            this.сброToolStripMenuItem.Text = "Сброс";
            this.сброToolStripMenuItem.Click += new System.EventHandler(this.сброToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(987, 42);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(912, 732);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // верхШляпыToolStripMenuItem
            // 
            this.верхШляпыToolStripMenuItem.Name = "верхШляпыToolStripMenuItem";
            this.верхШляпыToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.верхШляпыToolStripMenuItem.Text = "Верх шляпы";
            this.верхШляпыToolStripMenuItem.Click += new System.EventHandler(this.верхШляпыToolStripMenuItem_Click);
            // 
            // низШляпыToolStripMenuItem
            // 
            this.низШляпыToolStripMenuItem.Name = "низШляпыToolStripMenuItem";
            this.низШляпыToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.низШляпыToolStripMenuItem.Text = "Низ шляпы";
            this.низШляпыToolStripMenuItem.Click += new System.EventHandler(this.низШляпыToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1912, 786);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImageTransform";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem преобразоватьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сольИПерецToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ToolStripMenuItem медианныйФильтрToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x7ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x9ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem гаусToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem фильтрГауссаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem границыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem собельToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem всеНаправленияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem морфологическиеОперацииToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem эрозияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem дилекцияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem чернобелоеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сброситьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem афинныеПреобразованияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem поворотToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem растяжениеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem градацииСерогоToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отражениеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem вертикальноToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem горизонтальноToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сдвигToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem совмещениеИзображенийToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сброToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выбратьИзображенияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выполнитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem верхШляпыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem низШляпыToolStripMenuItem;
    }
}

