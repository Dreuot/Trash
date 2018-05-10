namespace Graph
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
            this.алгоритмДейкстрыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.методДинамическогоПрограммированияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.предустановкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.дейкстраToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.динамПрогрToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.алгоритмДейкстрыToolStripMenuItem,
            this.методДинамическогоПрограммированияToolStripMenuItem,
            this.предустановкиToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(675, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // алгоритмДейкстрыToolStripMenuItem
            // 
            this.алгоритмДейкстрыToolStripMenuItem.Name = "алгоритмДейкстрыToolStripMenuItem";
            this.алгоритмДейкстрыToolStripMenuItem.Size = new System.Drawing.Size(131, 20);
            this.алгоритмДейкстрыToolStripMenuItem.Text = "Алгоритм Дейкстры";
            this.алгоритмДейкстрыToolStripMenuItem.Click += new System.EventHandler(this.алгоритмДейкстрыToolStripMenuItem_Click);
            // 
            // методДинамическогоПрограммированияToolStripMenuItem
            // 
            this.методДинамическогоПрограммированияToolStripMenuItem.Name = "методДинамическогоПрограммированияToolStripMenuItem";
            this.методДинамическогоПрограммированияToolStripMenuItem.Size = new System.Drawing.Size(256, 20);
            this.методДинамическогоПрограммированияToolStripMenuItem.Text = "Метод динамического программирования";
            this.методДинамическогоПрограммированияToolStripMenuItem.Click += new System.EventHandler(this.методДинамическогоПрограммированияToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Количество узлов:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(120, 25);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(38, 20);
            this.textBox1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Расстояния:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(88, 55);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(506, 20);
            this.textBox2.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(600, 49);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(63, 30);
            this.button1.TabIndex = 5;
            this.button1.Text = "Создать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(15, 85);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(467, 363);
            this.richTextBox1.TabIndex = 6;
            this.richTextBox1.Text = "";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(488, 85);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(175, 363);
            this.richTextBox2.TabIndex = 7;
            this.richTextBox2.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(287, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Начальный узел:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(386, 25);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(41, 20);
            this.textBox3.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(536, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Конечный";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(599, 25);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(63, 20);
            this.textBox4.TabIndex = 11;
            // 
            // предустановкиToolStripMenuItem
            // 
            this.предустановкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.дейкстраToolStripMenuItem,
            this.динамПрогрToolStripMenuItem});
            this.предустановкиToolStripMenuItem.Name = "предустановкиToolStripMenuItem";
            this.предустановкиToolStripMenuItem.Size = new System.Drawing.Size(103, 20);
            this.предустановкиToolStripMenuItem.Text = "Предустановки";
            // 
            // дейкстраToolStripMenuItem
            // 
            this.дейкстраToolStripMenuItem.Name = "дейкстраToolStripMenuItem";
            this.дейкстраToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.дейкстраToolStripMenuItem.Text = "Дейкстра";
            this.дейкстраToolStripMenuItem.Click += new System.EventHandler(this.дейкстраToolStripMenuItem_Click);
            // 
            // динамПрогрToolStripMenuItem
            // 
            this.динамПрогрToolStripMenuItem.Name = "динамПрогрToolStripMenuItem";
            this.динамПрогрToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.динамПрогрToolStripMenuItem.Text = "Динам. прогр.";
            this.динамПрогрToolStripMenuItem.Click += new System.EventHandler(this.динамПрогрToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 458);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Graph";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem алгоритмДейкстрыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem методДинамическогоПрограммированияToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.ToolStripMenuItem предустановкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem дейкстраToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem динамПрогрToolStripMenuItem;
    }
}

