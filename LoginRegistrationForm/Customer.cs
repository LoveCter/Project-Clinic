using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginRegistrationForm
{
    public partial class Customer : Form
    {
        SqlConnection connection;
        SqlCommand command;
        string str = "Data Source=LOVECTERS;Initial Catalog=user;Integrated Security=True;Encrypt=False";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        void loaddata()
        {
            command = connection.CreateCommand();
            command.CommandText = "Select * From Customer ";
            adapter.SelectCommand = command;
            table.Clear();
            adapter.Fill(table);
            dgv3.DataSource = table;

        }
        public Customer()
        {
            InitializeComponent();
        }

        private void Customer_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(str);
            try
            {
                connection.Open();
                loaddata(); // Gọi hàm load dữ liệu từ DataTable vào DataGridView                
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private int selectedRowIndex = -1;

        private void dgv3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRowIndex = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                int rowIndex = dgv3.CurrentRow.Index;
                txtID.Text = dgv3.Rows[rowIndex].Cells["IDCus"].Value.ToString();
                txtName.Text = dgv3.Rows[rowIndex].Cells["NameCus"].Value.ToString();
                txtPhone.Text = dgv3.Rows[rowIndex].Cells["PhoneNumberCus"].Value.ToString();
                txtUsername.Text = dgv3.Rows[rowIndex].Cells["Account"].Value.ToString();
                txtPass.Text = dgv3.Rows[rowIndex].Cells["Password"].Value.ToString();
                txtEmail.Text = dgv3.Rows[rowIndex].Cells["Email"].Value.ToString();
            }
        }
        private void dgv3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu tất cả các trường bắt buộc đã được điền
            if (string.IsNullOrWhiteSpace(txtID.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPass.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(str))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        // Lệnh thêm dữ liệu vào bảng Employee
                        using (SqlCommand cmdEmployee = new SqlCommand(
                            "INSERT INTO Customer (IdCus, NameCus, PhoneNumberCus, Account, Password, Email) VALUES (@IdCus, @NameCus, @PhoneNumberCus, @Account, @Password, @Email)",
                            connection,
                            transaction))
                        {
                            cmdEmployee.Parameters.AddWithValue("@IdCus", txtID.Text);
                            cmdEmployee.Parameters.AddWithValue("@NameCus", txtName.Text);
                            cmdEmployee.Parameters.AddWithValue("@PhoneNumberCus", txtPhone.Text);
                            cmdEmployee.Parameters.AddWithValue("@Account", txtUsername.Text);
                            cmdEmployee.Parameters.AddWithValue("@Password", txtPass.Text);
                            cmdEmployee.Parameters.AddWithValue("@Email", txtEmail.Text);


                            cmdEmployee.ExecuteNonQuery();
                        }

                        // Commit transaction
                        transaction.Commit();
                        MessageBox.Show("Thêm dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // Reload dữ liệu sau khi thêm
                loaddata();
                
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearInputs()
        {
            txtID.Clear();
            txtName.Clear();
            txtPhone.Clear();
            txtUsername.Clear();
            txtPass.Clear();
            txtEmail.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu tất cả các trường bắt buộc đã được điền
            if (string.IsNullOrWhiteSpace(txtID.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPass.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"UPDATE Customer 
                                    SET NameCus = @NameCus, 
                                        PhoneNumberCus = @PhoneNumberCus, 
                                        Account = @Account, 
                                        Password = @Password, 
                                        Email = @Email 
                                    WHERE IdCus = @IdCus";

                    command.Parameters.AddWithValue("@IdCus", txtID.Text);
                    command.Parameters.AddWithValue("@NameCus", txtName.Text);
                    command.Parameters.AddWithValue("@PhoneNumberCus", txtPhone.Text);
                    command.Parameters.AddWithValue("@Account", txtUsername.Text);
                    command.Parameters.AddWithValue("@Password", txtPass.Text);
                    command.Parameters.AddWithValue("@Email", txtEmail.Text);
                    command.ExecuteNonQuery();

                    MessageBox.Show("Cập nhật dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                loaddata(); // Reload data after successful update
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedRowIndex >= 0)
            {
                int id = Convert.ToInt32(dgv3.Rows[selectedRowIndex].Cells["IdCus"].Value);
                string deleteQuery = "DELETE FROM Customer WHERE IdCus = @IdCus";

                try
                {
                    using (SqlConnection connection = new SqlConnection(str))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@IdCus", id);
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa dữ liệu thành công!");
                                // Cập nhật lại DataGridView
                                loaddata();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy dữ liệu để xóa.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa dữ liệu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa.");
            }
        }

        private string selectedColumn = "";
        private void cbSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lấy giá trị đã chọn trong ComboBox
            selectedColumn = cbSearch.SelectedItem.ToString();

            // Kiểm tra nếu giá trị không hợp lệ
            if (string.IsNullOrWhiteSpace(selectedColumn))
            {
                MessageBox.Show("Vui lòng chọn cột cần tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(selectedColumn))
            {
                MessageBox.Show("Vui lòng chọn cột cần tìm kiếm trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string filterValue = txtSearch.Text;
            DataView dv = table.DefaultView;

            if (string.IsNullOrWhiteSpace(filterValue))
            {
                dv.RowFilter = ""; // Xóa bộ lọc nếu không nhập gì
            }
            else
            {
                if (selectedColumn == "ALL")
                {
                    // Tìm kiếm trên tất cả các cột (từ trái sang phải)
                    string filterExpression = string.Join(" OR ", table.Columns
                        .Cast<DataColumn>()
                        .Select(column => $"CONVERT([{column.ColumnName}], 'System.String') LIKE '{filterValue}%'"));

                    dv.RowFilter = filterExpression;
                }
                else if (selectedColumn == "PhoneNumberCus")
                {
                    // Chuyển đổi cột số thành chuỗi và tìm kiếm từ trái sang phải
                    dv.RowFilter = $"CONVERT({selectedColumn}, 'System.String') LIKE '{filterValue}%'";
                }
                else
                {
                    // Áp dụng LIKE (từ trái sang phải) cho các cột chuỗi
                    dv.RowFilter = $"{selectedColumn} LIKE '{filterValue}%'";
                }
            }

            dgv3.DataSource = dv; // Cập nhật lại dữ liệu hiển thị trong DataGridView
        }

    }
}
