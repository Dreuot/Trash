using ExpressionEvaluatorLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ValidateTestCaseBuilder.ExpressionEntities;
using ExpressionEvaluatorLib.Lexems;

namespace ValidateTestCaseBuilder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            //ExpressionEvaluator eval1 = ExpressionEvaluatorBuilder.CreateDefault(textBox1.Text);
            ////dataGridView1.DataSource = eval1.OutArray;
            //dataGridView1.Rows.Clear();
            //dataGridView1.ColumnCount = 1;
            //dataGridView1.Columns[0].HeaderText = "Результат";
            //dataGridView1.Rows.Add();
            //dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = eval1.Calculate();
            Calc();
        }

        private void Calc()
        {
            try
            {
                if (radioButton1.Checked)
                {
                    ExpressionEvaluator eval1 = ExpressionEvaluatorBuilder.CreateDefault(textBox1.Text);
                    dataGridView1.Rows.Clear();
                    dataGridView1.ColumnCount = 1;
                    dataGridView1.Columns[0].HeaderText = "Результат";
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = eval1.Calculate();

                }
                else
                {
                    ExpressionEvaluator eval1 = ExpressionEvaluatorBuilder.CreateDefault(textBox1.Text);
                    var operands = eval1.Variables;
                    AccomodationGenerator ag = new AccomodationGenerator(operands.Length, 2);

                    dataGridView1.Rows.Clear();
                    dataGridView1.ColumnCount = operands.Length + 1;
                    for (int i = 0; i < operands.Length; i++)
                    {
                        dataGridView1.Columns[i].Name = operands[i];
                    }
                    dataGridView1.Columns[operands.Length].HeaderText = "Результат";

                    foreach (var item in ag.Inner)
                    {
                        for (int i = 0; i < operands.Length; i++)
                            eval1.SetVariable(operands[i], item[i] == 0 ? false : true);

                        var result = eval1.Calculate();
                        List<string> row = item.Select(l => l == 0 ? "False" : "True").ToList();
                        row.Add(result.ToString());

                        dataGridView1.Rows.Add(row.ToArray());
                        var curRow = dataGridView1.Rows[dataGridView1.Rows.Count - 1];
                        for (int i = 0; i < curRow.Cells.Count; i++)
                        {
                            var cell = curRow.Cells[i];
                            if (cell.Value.ToString() == "True")
                            {
                                cell.Style.BackColor = Color.FromArgb(162, 203, 173);
                            }
                            else
                            {
                                cell.Style.BackColor = Color.FromArgb(226, 112, 120);
                                //cell.Style.ForeColor = Color.White;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Calc();
            }
        }
    }
}
