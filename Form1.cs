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
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string connectionString = ("Server=localhost;port=5432;Database=DbProject10ToDoApp;user Id=postgres;Password=Gunes123");

        void ToDoList()
        {
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            string query = "Select * From ToDoLists";
            var command = new NpgsqlCommand(query, conn);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            DGVToDo.DataSource = dt;
            conn.Close();
        }

        void CategoryList()
        {
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            string query = "Select * From Categories";
            var command = new NpgsqlCommand(query, conn);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CategoryId";
            cmbCategory.DataSource=dt;
            conn.Close();
        }

        private void btnCatList_Click(object sender, EventArgs e)
        {
            FrmCategory category = new FrmCategory();
            category.Show();
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            ToDoList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ToDoList();
            CategoryList();
        }

        private void btnAllList_Click(object sender, EventArgs e)
        {
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            string query = "Select todolistid,title,description,status,priority, categoryname From ToDoLists inner join Categories on todolists.categoryid = categories.categoryid";
            var command = new NpgsqlCommand(query, conn);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            DGVToDo.DataSource = dt;
            conn.Close();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            int categoryId = int.Parse(cmbCategory.SelectedValue.ToString());
            string title = txtTitle.Text;
            string priority = txtPriority.Text;
            string description = txtExplanation.Text;
            string status= "B'0'";

            if(rbCompleted.Checked) { status = "B'1'"; }
            if(rbNotCompleted.Checked) { status = "B'0'"; }

            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "insert into ToDoLists (title,description,status,priority,categoryid) values (@title,@description,@status,@priority,@categoryid)";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@priority", priority);
                command.Parameters.AddWithValue("@categoryid", categoryId);
                command.ExecuteNonQuery();
                MessageBox.Show("TODO List Added Successfully");
                ToDoList();
            }
            connection.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtProcessId.Text);

            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            string query = "Delete From ToDoLists Where ToDoListId=@toDoListId";
            using (var command = new NpgsqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@toDoList", id);
                command.ExecuteNonQuery();
                MessageBox.Show("List Deleted Successfully!");
                ToDoList();
            }
            conn.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtProcessId.Text);
            string title = txtTitle.Text;
            string priority = txtPriority.Text;
            string description = txtExplanation.Text;
            int categoryId = int.Parse(cmbCategory.SelectedValue.ToString());
            string status = "B'0'";

            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            string query = "Update ToDoLists Set Title=@title, Description=@description, Status=@status,Priority=@priority, CategoryId=@categoryid where ToDoListId=@todolistid";
            using (var command = new NpgsqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@todolistid", id);
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@categoryid", categoryId);
                command.Parameters.AddWithValue("@priority", priority);
                command.ExecuteNonQuery();
                MessageBox.Show("List Updated Successfully");
                ToDoList();
            }
            conn.Close();
        }

        private void btnCompleted_Click(object sender, EventArgs e)
        {
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            string query = "Select * From ToDoLists Where Status = '1'";
            var command = new NpgsqlCommand(query, conn);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            DGVToDo.DataSource = dt;
            conn.Close();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            string query = "Select * From ToDoLists Where Status = '0'";
            var command = new NpgsqlCommand(query, conn);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            DGVToDo.DataSource = dt;
            conn.Close();
        }
    }
}
