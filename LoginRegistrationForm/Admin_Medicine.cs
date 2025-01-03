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

    public partial class Admin_Medicine : Form
    {
        SqlConnection connection;
        SqlCommand command;
        string str = "Data Source=LOVECTERS;Initial Catalog=user;Integrated Security=True;Encrypt=False";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        void loaddata()
        {
            command = connection.CreateCommand();
            command.CommandText = @"
    SELECT TOP (1000) 
        [ProductID], 
        [Product_name], 
        [Price], 
        [Quantity],
        [EXP]
    FROM [user].[dbo].[Products_Medicine];
";
            adapter.SelectCommand = command;
            table.Clear();
            adapter.Fill(table);
            dgv2.DataSource = table;

        }
        public Admin_Medicine()
        {
            InitializeComponent();
        }

        private void Admin_Medicine_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(str);
            connection.Open();
            loaddata();

        }

        private void dgv2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int rowIndex = dgv2.CurrentRow.Index;
                txtID.Text = dgv2.Rows[rowIndex].Cells["ProductID"].Value.ToString();
                txtPDName.Text = dgv2.Rows[rowIndex].Cells["Product_name"].Value.ToString();
                txtPrice.Text = dgv2.Rows[rowIndex].Cells["Price"].Value.ToString();
                txtQuantity.Text = dgv2.Rows[rowIndex].Cells["Quantity"].Value.ToString();
                EXP.Text = dgv2.Rows[rowIndex].Cells["EXP"].Value.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text) ||
                string.IsNullOrWhiteSpace(txtPDName.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text) ||
                string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Mở kết nối
                using (SqlConnection connection = new SqlConnection(str))
                {
                    connection.Open();

                    // Bắt đầu transaction
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        // Lệnh thêm dữ liệu vào bảng Employee
                        using (SqlCommand cmdEmployee = new SqlCommand(
                            "INSERT INTO Products_Medicine (ProductID, Product_name, Price, Quantity, EXP) VALUES (@ProductID, @Product_name, @Price, @Quantity, @EXP)",
                            connection,
                            transaction))

                        {                                                     
                            cmdEmployee.Parameters.AddWithValue("@ProductID", txtID.Text);
                            cmdEmployee.Parameters.AddWithValue("@Product_name", txtPDName.Text);
                            cmdEmployee.Parameters.AddWithValue("@Price", txtPrice.Text);
                            cmdEmployee.Parameters.AddWithValue("@Quantity", txtQuantity.Text);
                            cmdEmployee.Parameters.AddWithValue("@EXP", EXP.Text);
                            cmdEmployee.ExecuteNonQuery();
                        }




                        // Commit transaction
                        transaction.Commit();
                        MessageBox.Show("Thêm dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    connection.Close();
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
            txtPDName.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
            EXP.Value = DateTime.Now;

        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text) ||
                string.IsNullOrWhiteSpace(txtPDName.Text) ||
                string.IsNullOrWhiteSpace(txtQuantity.Text) ||
                string.IsNullOrWhiteSpace(EXP.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Mở kết nối
                using (SqlConnection connection = new SqlConnection(str))
                {
                    connection.Open();

                    // Bắt đầu transaction
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        // Lệnh cập nhật dữ liệu vào bảng Products_Medicine
                        using (SqlCommand cmdUpdate = new SqlCommand(
                            "UPDATE Products_Medicine SET Product_name = @Product_name, Price = @Price, Quantity = @Quantity, EXP = @EXP WHERE ProductID = @ID",
                            connection, transaction))
                        {
                            cmdUpdate.Parameters.AddWithValue("@ID", txtID.Text);
                            cmdUpdate.Parameters.AddWithValue("@Product_name", txtPDName.Text);
                            cmdUpdate.Parameters.AddWithValue("@Price", txtPrice.Text);
                            cmdUpdate.Parameters.AddWithValue("@Quantity", txtQuantity.Text);

                            // Xử lý ngày hết hạn
                            if (EXP.Value != null)
                            {
                                cmdUpdate.Parameters.AddWithValue("@EXP", EXP.Value.ToString("yyyy-MM-dd"));  // Định dạng ngày
                            }
                            else
                            {
                                cmdUpdate.Parameters.AddWithValue("@EXP", DBNull.Value);  // Nếu không có ngày, set NULL
                            }

                            cmdUpdate.ExecuteNonQuery();
                        }

                        // Commit transaction
                        transaction.Commit();
                        MessageBox.Show("Cập nhật dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    connection.Close();
                }

                // Reload dữ liệu sau khi cập nhật
                loaddata();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                else if (selectedColumn == "Quantity")
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

            dgv2.DataSource = dv; // Cập nhật lại dữ liệu hiển thị trong DataGridView
        }

        private void cbSuplier_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }



}