using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project10_PostgreSQLToDoListApp
{
    public partial class FrmCategory: Form
    {
        public FrmCategory()
        {
            InitializeComponent();
        }
        string connectionString = ("Server=localhost;port=5432;Database=DbProject10ToDoApp;user Id=postgres;Password=Gunes123");

        void CategoryList()
        {
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            string query = "Select * From categories order by CategoryId";
            var command = new NpgsqlCommand(query, conn);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            DGVCategory.DataSource = dt;
        }
        private void FrmCategory_Load(object sender, EventArgs e)
        {
            CategoryList();
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            CategoryList();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            string query = "insert into Categories (CategoryName) values (@categoryName)";
            using (var command = new NpgsqlCommand(query,conn))
            {
                command.Parameters.AddWithValue("@categoryName",txtCatName.Text);
                command.ExecuteNonQuery();
                MessageBox.Show("Category Added Successfully");
                CategoryList();
            }
            conn.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtCatId.Text);

            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            string query = "Delete From Categories Where CategoryId=@categoryId";
            using (var command = new NpgsqlCommand(query,conn))
            {
                command.Parameters.AddWithValue("@categoryId",id);
                command.ExecuteNonQuery();
                MessageBox.Show("Category Deleted Successfully!");
                CategoryList();
            }
            conn.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtCatId.Text);
            string categoryName = txtCatName.Text;

            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            string query = "Update Categories Set CategoryName=@categoryName where CategoryId=@categoryId";
            using (var command = new NpgsqlCommand(query,conn))
            {
                command.Parameters.AddWithValue("@categoryId",id);
                command.Parameters.AddWithValue("@categoryName",categoryName);
                command.ExecuteNonQuery();
                MessageBox.Show("Category Updated Successfully");
                CategoryList();
            }
            conn.Close();
        }

        private void btnGetById_Click(object sender, EventArgs e)
        {
            // ID ye göre getirme işlemi
            int id = int.Parse(txtCatId.Text);
            using (var connection=new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string query = "Select * From Categories where CategoryId=@categoryId";
                using (var command =new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@categoryId", id);
                    using (var adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        DGVCategory.DataSource = dt;
                    }
                }
                connection.Close();
            }
        }
    }
}
