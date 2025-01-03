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
using System.Xml.Linq;

namespace LoginRegistrationForm
{
    public partial class Suplier : Form
    {
        SqlConnection connection;
        SqlCommand command;
        string str = "Data Source=LOVECTERS;Initial Catalog=user;Integrated Security=True;Encrypt=False";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();

        void loaddata()
        {
            command = connection.CreateCommand();
            //command.CommandText = "select e.* , d.Type from Suplier e inner join Products d ON e.IDNCC=d.Product_Suplier";
            command.CommandText = "select * from Suplier";

            adapter.SelectCommand = command;
            table.Clear();
            adapter.Fill(table);
            dgv5.DataSource = table;

        }
        public Suplier()
        {
            InitializeComponent();
        }

        private void Suplier_Load(object sender, EventArgs e)
        {

            // Khởi tạo kết nối và tải dữ liệu
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
        private void dgv5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRowIndex = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                int rowIndex = dgv5.CurrentRow.Index;
                txtSupName.Text = dgv5.Rows[rowIndex].Cells["TenNCC"].Value.ToString();
                txtPhone.Text = dgv5.Rows[rowIndex].Cells["PhoneNCC"].Value.ToString(); // Giả sử cột PhoneNumber là kiểu số
                txtID.Text = dgv5.Rows[rowIndex].Cells["MaNCC"].Value.ToString();
                txtNote.Text = dgv5.Rows[rowIndex].Cells["Note"].Value.ToString();
                //txtTypeMedicine.Text = dgv5.Rows[rowIndex].Cells["Type"].Value.ToString();
            }
        }
        private void ClearInputs()
        {
            txtID.Clear();
            txtSupName.Clear();
            txtPhone.Clear();
            txtNote.Clear();
            //txtTypeMedicine.Clear();
        }


        private void picADD_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text) ||
                string.IsNullOrWhiteSpace(txtSupName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtNote.Text))
            //string.IsNullOrWhiteSpace(txtTypeMedicine.Text))
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
                        // Lệnh thêm dữ liệu vào bảng Suplier
                        using (SqlCommand cmdEmployee = new SqlCommand(
                            "INSERT INTO Suplier (MaNCC, TenNCC, PhoneNCC, Note) VALUES (@MaNCC, @TenNCC, @PhoneNCC, @Note)",
                            connection,
                            transaction))
                        {
                            cmdEmployee.Parameters.AddWithValue("@MaNCC", txtID.Text);
                            cmdEmployee.Parameters.AddWithValue("@TenNCC", txtSupName.Text);
                            cmdEmployee.Parameters.AddWithValue("@PhoneNCC", txtPhone.Text);
                            cmdEmployee.Parameters.AddWithValue("@Note", txtNote.Text);

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
        private int GetIDOfSelectedRow()
        {
            if (dgv5.SelectedRows.Count > 0)
            {
                return Convert.ToInt32(dgv5.SelectedRows[0].Cells["ID"].Value);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa.");
                return -1; // Hoặc một giá trị khác để biểu thị không có dòng nào được chọn
            }
        }
        private void picDelete_Click(object sender, EventArgs e)
        {
            if (selectedRowIndex >= 0)
            {
                string id = dgv5.Rows[selectedRowIndex].Cells["MaNCC"].Value.ToString();
                string deleteQuery = "DELETE FROM Suplier WHERE MaNCC = @MaNCC";

                try
                {
                    using (SqlConnection connection = new SqlConnection(str))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@MaNCC", id);
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

        private void picUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text) ||
                string.IsNullOrWhiteSpace(txtSupName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtNote.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //int idToUpdate = Convert.ToInt32(txtID.Text); // Capture the ID of the row being updated


            try
            {
                using (SqlConnection connection = new SqlConnection(str))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Update Employee table
                            using (SqlCommand commandEmployee = new SqlCommand(
                                @"UPDATE Suplier SET MaNCC = @MaNCC, TenNCC = @TenNCC, PhoneNCC = @PhoneNCC, Note = @Note  WHERE MaNCC = @MaNCC",
                                connection, transaction))
                            {
                                commandEmployee.Parameters.AddWithValue("@MaNCC", txtID.Text);
                                commandEmployee.Parameters.AddWithValue("@TenNCC", txtSupName.Text);
                                commandEmployee.Parameters.AddWithValue("@Note", txtNote.Text);
                                commandEmployee.Parameters.AddWithValue("@PhoneNCC", txtPhone.Text);
                                commandEmployee.ExecuteNonQuery();
                            }



                            transaction.Commit();
                            MessageBox.Show("Cập nhật dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            throw; // Re-throw the exception for further handling (optional)
                        }
                    }

                    loaddata(); // Reload data after successful update
                }
            }
            catch (Exception ex)
            {
                // Handle any uncaught exceptions here (optional)
                MessageBox.Show("Lỗi nghiêm trọng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void picReload_Click(object sender, EventArgs e)
        {
            txtID.Text = "";
            txtSupName.Text = "";
            txtPhone.Text = "";
            txtNote.Text = "";

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
                else if (selectedColumn == "PhoneNCC")
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

            dgv5.DataSource = dv; // Cập nhật lại dữ liệu hiển thị trong DataGridView
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
    

