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
            this.совмещениеИзображенийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выбратьИзображенияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выполнитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сброToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(635, 476);
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
            this.совмещениеИзображенийToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1275, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.открытьToolStripMenuItem.Text = "Открыть";
            this.открытьToolStripMenuItem.Click += new System.EventHandler(this.открытьToolStripMenuItem_Click);
            // 
            // совмещениеИзображенийToolStripMenuItem
            // 
            this.совмещениеИзображенийToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.выбратьИзображенияToolStripMenuItem,
            this.выполнитьToolStripMenuItem,
            this.сброToolStripMenuItem});
            this.совмещениеИзображенийToolStripMenuItem.Name = "совмещениеИзображенийToolStripMenuItem";
            this.совмещениеИзображенийToolStripMenuItem.Size = new System.Drawing.Size(170, 20);
            this.совмещениеИзображенийToolStripMenuItem.Text = "Совмещение изображений";
            // 
            // выбратьИзображенияToolStripMenuItem
            // 
            this.выбратьИзображенияToolStripMenuItem.Name = "выбратьИзображенияToolStripMenuItem";
            this.выбратьИзображенияToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.выбратьИзображенияToolStripMenuItem.Text = "Выбрать изображение";
            this.выбратьИзображенияToolStripMenuItem.Click += new System.EventHandler(this.выбратьИзображенияToolStripMenuItem_Click);
            // 
            // выполнитьToolStripMenuItem
            // 
            this.выполнитьToolStripMenuItem.Name = "выполнитьToolStripMenuItem";
            this.выполнитьToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.выполнитьToolStripMenuItem.Text = "Выполнить";
            this.выполнитьToolStripMenuItem.Click += new System.EventHandler(this.выполнитьToolStripMenuItem_Click);
            // 
            // сброToolStripMenuItem
            // 
            this.сброToolStripMenuItem.Name = "сброToolStripMenuItem";
            this.сброToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.сброToolStripMenuItem.Text = "Сброс";
            this.сброToolStripMenuItem.Click += new System.EventHandler(this.сброToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(658, 27);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(608, 476);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1275, 511);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
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
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ToolStripMenuItem совмещениеИзображенийToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сброToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выбратьИзображенияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выполнитьToolStripMenuItem;
    }
}

