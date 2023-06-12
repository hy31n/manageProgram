using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MGMT
{
    public partial class Form3 : Form
    {
        MySqlConnection connection = new MySqlConnection("Server=localhost;Port=3306;Database=pos_dataset;Uid=root");
        MySqlCommand command;
        MySqlDataAdapter adapter;
        DataTable table = new DataTable();

        public Form3()
        {
            InitializeComponent();
        }

        int selectedRow;

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void LoadData()
        {
            string sql = "Server=localhost;Port=3306;Database=pos_dataset;Uid=root";
            MySqlConnection con = new MySqlConnection(sql);
            MySqlCommand cmd_db = new MySqlCommand("SELECT * FROM sales_table;", con);

            try
            {
                MySqlDataAdapter sda = new MySqlDataAdapter();
                sda.SelectCommand = cmd_db;
                DataTable dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bSource = new BindingSource();

                bSource.DataSource = dbdataset;
                dataGridView1.DataSource = bSource;
                sda.Update(dbdataset);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            textBox1.Text = "";
            textBox6.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        public void searchData(string valueToSearch)
        {
            string query = "SELECT * FROM sales_table WHERE CONCAT(`Name`, `Price`, `Count`, `Total`) like '%" + valueToSearch + "%'";
            command = new MySqlCommand(query, connection);
            adapter = new MySqlDataAdapter(command);
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            searchData("");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("검색 정보를 입력해주세요");
            }
            else
            {
                string valueToSearch = textBox1.Text.ToString();
                searchData(valueToSearch);
                
                textBox1.Text = "";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string constring = "Server=localhost;Port=3306;Database=pos_dataset;Uid=root";

            if (textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "")
            {
                MessageBox.Show("항목을 정확히 입력해주세요");
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
            }
            else
            {
                decimal Price = decimal.Parse(textBox4.Text);
                decimal Count = decimal.Parse(textBox5.Text);
                decimal Total = Price * Count;

                textBox6.Text = Total.ToString();

                
                string Query = "update pos_dataset.sales_table set No ='" + this.textBox2.Text + "',Name='" + this.textBox3.Text + "',Price='" + this.textBox4.Text + "'," +
                    "Count='" + this.textBox5.Text + "',Total='" + this.textBox6.Text + "' where No ='" + this.textBox2.Text + "'";
                MySqlConnection conDataBase = new MySqlConnection(constring);
                MySqlCommand cmdDatabase = new MySqlCommand(Query, conDataBase);
                MySqlDataReader myReader;

                try
                {
                    conDataBase.Open();
                    myReader = cmdDatabase.ExecuteReader();
                    MessageBox.Show("수정완료");

                    while (myReader.Read())
                    {

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            LoadData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[selectedRow];
            textBox2.Text = row.Cells[0].Value.ToString();
            textBox3.Text = row.Cells[1].Value.ToString();
            textBox4.Text = row.Cells[2].Value.ToString();
            textBox5.Text = row.Cells[3].Value.ToString();
            textBox6.Text = row.Cells[4].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string constring = "Server=localhost;Port=3306;Database=pos_dataset;Uid=root";
            if (textBox6.Text == "")
            {
                MessageBox.Show("삭제 할 항목을 찾지 못했습니다.");
            }
            else
            {

                string Query = "delete from pos_dataset.sales_table where No ='" + this.textBox2.Text + "';";
                MySqlConnection conDataBase = new MySqlConnection(constring);
                MySqlCommand cmdDatabase = new MySqlCommand(Query, conDataBase);
                MySqlDataReader myReader;

                try
                {
                    conDataBase.Open();
                    myReader = cmdDatabase.ExecuteReader();
                    MessageBox.Show("삭제완료");

                    while (myReader.Read())
                    {

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                LoadData();
            }
        }
    }
}
