using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LoginRegistrationForm
{
    public partial class EmployeeForm : Form
    {
        SqlConnection connection;
        SqlCommand command;
        string str = "Data Source=LOVECTERS;Initial Catalog=user;Integrated Security=True;Encrypt=False";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        void loaddata()
        {
            command = connection.CreateCommand();
            command.CommandText = "select e.* , d.email,d.username,d.password from Employee e inner join Register d ON e.ID=d.ID ";
            adapter.SelectCommand = command;
            table.Clear();
            adapter.Fill(table);
            dgv1.DataSource = table;
           
        }
        public EmployeeForm()
        {
            InitializeComponent();
        }


       
    



        private void btnImport_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (.jpg; *.jpeg; *.png; *.bmp)|.jpg;*.jpeg;*.png;*.bmp",
                Title = "Select an Image"
            };

            // Kiểm tra nếu người dùng chọn file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Load ảnh vào PictureBox
                try
                {
                    pictureBox1.Image = new Bitmap(openFileDialog.FileName);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private int selectedRowIndex = -1;
        private void dgv1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRowIndex = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                int rowIndex = dgv1.CurrentRow.Index;
                txtName.Text = dgv1.Rows[rowIndex].Cells["Name"].Value.ToString();
                txtPhone.Text = dgv1.Rows[rowIndex].Cells["PhoneNumber"].Value.ToString(); // Giả sử cột PhoneNumber là kiểu số
                                                                                                          // ... các dòng tương tự
                cbRole.Text = dgv1.Rows[rowIndex].Cells["Role"].Value.ToString();
                cbYearsOfWork.Text = dgv1.Rows[rowIndex].Cells["YearsOfWork"].Value.ToString();
                txtAccount.Text = dgv1.Rows[rowIndex].Cells["username"].Value.ToString();
                txtPass.Text = dgv1.Rows[rowIndex].Cells["password"].Value.ToString();
                txtEmail.Text = dgv1.Rows[rowIndex].Cells["email"].Value.ToString();
                txtID.Text = dgv1.Rows[rowIndex].Cells["ID"].Value.ToString();
            }    
                
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu tất cả các trường bắt buộc đã được điền
            if (string.IsNullOrWhiteSpace(txtID.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(cbRole.Text) ||
                string.IsNullOrWhiteSpace(cbYearsOfWork.Text) ||
                string.IsNullOrWhiteSpace(txtAccount.Text) ||
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
                            "INSERT INTO Employee (ID, Name, PhoneNumber, Role, YearsOfWork) VALUES (@ID, @Name, @PhoneNumber, @Role, @YearsOfWork)",
                            connection,
                            transaction))
                        {
                            cmdEmployee.Parameters.AddWithValue("@ID", txtID.Text);
                            cmdEmployee.Parameters.AddWithValue("@Name", txtName.Text);
                            cmdEmployee.Parameters.AddWithValue("@PhoneNumber", txtPhone.Text);
                            cmdEmployee.Parameters.AddWithValue("@Role", cbRole.Text);
                            cmdEmployee.Parameters.AddWithValue("@YearsOfWork", cbYearsOfWork.Text);

                            cmdEmployee.ExecuteNonQuery();
                        }

                        // Lệnh thêm dữ liệu vào bảng Register
                        using (SqlCommand cmdRegister = new SqlCommand(
                            "INSERT INTO Register (ID, email, username, password) VALUES (@ID, @Email, @Username, @Password)",
                            connection,
                            transaction))
                        {
                            cmdRegister.Parameters.AddWithValue("@ID", txtID.Text);
                            cmdRegister.Parameters.AddWithValue("@Email", txtEmail.Text);
                            cmdRegister.Parameters.AddWithValue("@Username", txtAccount.Text);
                            cmdRegister.Parameters.AddWithValue("@Password", txtPass.Text);

                            cmdRegister.ExecuteNonQuery();
                        }

                        // Commit transaction
                        transaction.Commit();
                        MessageBox.Show("Thêm dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // Reload dữ liệu sau khi thêm
                loaddata();
                //dgv1.Refresh();
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
            //cbRole.Items.Clear();
            cbRole.DataSource = null; // Clear the DataSource
            cbRole.Items.Clear(); // Clear the Items collection

            //cbYearsOfWork.Items.Clear();
            txtAccount.Clear();
            txtPass.Clear();
            txtEmail.Clear();
        }



        private string selectedColumn = ""; // Biến toàn cục để lưu tên cột

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
       

        private void textBox1_TextChanged(object sender, EventArgs e)
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
                else if (selectedColumn == "PhoneNumber" || selectedColumn == "YearsOfWork")
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

            dgv1.DataSource = dv; // Cập nhật lại dữ liệu hiển thị trong DataGridView
        }




        private void EmployeeForm_Load(object sender, EventArgs e)
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtID.Text = "";
            txtName.Text = "";
            txtPhone.Text = "";
            txtAccount.Text = "";
            cbRole.SelectedIndex = -1;
            cbYearsOfWork.SelectedIndex = -1;
            txtPass.Text = "";
            txtEmail.Text = "";
        }
        private int GetIDOfSelectedRow()
        {
            if (dgv1.SelectedRows.Count > 0)
            {
                return Convert.ToInt32(dgv1.SelectedRows[0].Cells["ID"].Value);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa.");
                return -1; // Hoặc một giá trị khác để biểu thị không có dòng nào được chọn
            }
        }


        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (selectedRowIndex >= 0)
            {
                int id = Convert.ToInt32(dgv1.Rows[selectedRowIndex].Cells["ID"].Value);
                string deleteQuery = "DELETE FROM Employee WHERE ID = @ID";

                try
                {
                    using (SqlConnection connection = new SqlConnection(str))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ID", id);
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
        //private int lastUpdatedRowIndex = -1;
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtAccount.Text) ||
                string.IsNullOrWhiteSpace(txtPass.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(cbRole.Text) ||
                string.IsNullOrWhiteSpace(cbYearsOfWork.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int idToUpdate = Convert.ToInt32(txtID.Text); // Capture the ID of the row being updated


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
                                @"UPDATE Employee SET Name = @Name, PhoneNumber = @PhoneNumber, Role = @Role, YearsOfWork = @YearsOfWork WHERE ID = @ID",
                                connection, transaction))
                            {
                                commandEmployee.Parameters.AddWithValue("@ID", txtID.Text);
                                commandEmployee.Parameters.AddWithValue("@Name", txtName.Text);
                                commandEmployee.Parameters.AddWithValue("@PhoneNumber", txtPhone.Text);
                                commandEmployee.Parameters.AddWithValue("@YearsOfWork", cbYearsOfWork.Text);
                                commandEmployee.Parameters.AddWithValue("@Role", cbRole.Text);
                                commandEmployee.ExecuteNonQuery();
                            }

                            // Update Register table
                            using (SqlCommand commandRegister = new SqlCommand(
                                @"UPDATE Register SET username = @Account, password = @Pass, email = @Email WHERE ID = @ID",
                                connection, transaction))
                            {
                                commandRegister.Parameters.AddWithValue("@ID", txtID.Text);
                                commandRegister.Parameters.AddWithValue("@Account", txtAccount.Text);
                                commandRegister.Parameters.AddWithValue("@Pass", txtPass.Text);
                                commandRegister.Parameters.AddWithValue("@Email", txtEmail.Text);
                                commandRegister.ExecuteNonQuery();
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

            /*var updatedRowIndex = -1;

            foreach (DataGridViewRow row in dgv1.Rows)
            {
                if (Convert.ToInt32(row.Cells["ID"].Value) == idToUpdate)
                {
                    row.DefaultCellStyle.BackColor = Color.LightBlue;
                    row.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
                    break;
                }
            }

            */

        }

        private void dgv1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    
}
 

