using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MGMT
{
    public partial class Form1 : Form
    {
        DataTable table = new DataTable();
        public Form1()
        {
            InitializeComponent();

            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Price", typeof(string));
            table.Columns.Add("Count", typeof(string));
            table.Columns.Add("Total", typeof(string));

            dataGridView1.DataSource = table;
            numericUpDown1.Value = 1;
        }

        //취소 버튼
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(item.Index);
            }

            decimal all = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                all += Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value);
            }

            textBox4.Text = all.ToString();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;user=root");
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                label6.Text = "Connected";
                label6.ForeColor = Color.Black;
            }
            else
            {
                label6.Text = "DisConnected";
                label6.ForeColor = Color.Red;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("항목을 정확히 입력해주세요.");
                textBox1.Clear();
                textBox2.Clear();
            }
            else
            {
                decimal price = decimal.Parse(textBox2.Text);
                decimal count = numericUpDown1.Value;
                decimal total = price * count;

                table.Rows.Add(textBox1.Text, textBox2.Text, numericUpDown1.Value, total);
                dataGridView1.DataSource = table;

                textBox1.Clear();
                textBox2.Clear();
                numericUpDown1.Value = 1;

                decimal all = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                {
                    all += Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value);
                }
                textBox4.Text = all.ToString();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form3 dlg = new Form3();
            dlg.ShowDialog();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Port=3306;Database=pos_dataset;Uid=root"))
            {
                conn.Open();
                
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    String Name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    String Price = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    String Count = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    String Total = dataGridView1.Rows[i].Cells[3].Value.ToString();

                    
                    string sql = string.Format("INSERT INTO sales_table(Name,Price,Count,Total,C_Num) VALUES  ('{0}',{1},{2},{3},{4})", @Name, @Price, @Count, @Total, @i);

                    string sql_count = string.Format("update item_table set i_count = i_count - {0} where i_name = '{1}'", @Count, @Name);

                    try
                    {
                        MySqlCommand command = new MySqlCommand(sql, conn);
                        command.ExecuteNonQuery();

                        MySqlCommand c_command = new MySqlCommand(sql_count, conn);
                        c_command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            MessageBox.Show("계산되었습니다.");

            
            int rowCount = dataGridView1.Rows.Count;
            for (int n = 0; n < rowCount; n++)
            {
                if (dataGridView1.Rows[0].IsNewRow == false)
                    dataGridView1.Rows.RemoveAt(0);
            }

            textBox4.Text = "0";
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form4 dlg = new Form4();
            dlg.ShowDialog();
        }
    }
}
