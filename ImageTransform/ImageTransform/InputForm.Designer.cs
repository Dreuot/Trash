namespace ImageTransform
{
    partial class InputForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DataTB = new System.Windows.Forms.TextBox();
            this.MessageLBL = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DataTB
            // 
            this.DataTB.Location = new System.Drawing.Point(12, 56);
            this.DataTB.Name = "DataTB";
            this.DataTB.Size = new System.Drawing.Size(229, 20);
            this.DataTB.TabIndex = 0;
            // 
            // MessageLBL
            // 
            this.MessageLBL.Location = new System.Drawing.Point(12, 30);
            this.MessageLBL.Name = "MessageLBL";
            this.MessageLBL.Size = new System.Drawing.Size(229, 23);
            this.MessageLBL.TabIndex = 1;
            this.MessageLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(92, 82);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Ввод";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // InputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 125);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.MessageLBL);
            this.Controls.Add(this.DataTB);
            this.Name = "InputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Input";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox DataTB;
        private System.Windows.Forms.Label MessageLBL;
        private System.Windows.Forms.Button button1;
    }
}