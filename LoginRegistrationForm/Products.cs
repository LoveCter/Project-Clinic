using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginRegistrationForm
{
    public partial class Products : Form
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
    FROM [user].[dbo].[Products];
";
            adapter.SelectCommand = command;
            table.Clear();
            adapter.Fill(table);
            dgv2.DataSource = table;

        }

        public Products()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(str);
            connection.Open();
            loaddata();
            LoadOrders();
            LoadCustomerNames();
        }

        void LoadOrders()
        {
            try
            {
                // Câu lệnh SQL để lấy dữ liệu từ bảng Order
                string query = @"
            SELECT ID, Name, Qty, Price, Total
            FROM [user].[dbo].[Order];
        ";

                SqlDataAdapter orderAdapter = new SqlDataAdapter(query, connection);
                DataTable orderTable = new DataTable();
                orderAdapter.Fill(orderTable);

                // Xóa các cột cũ (nếu có) để tránh bị lặp
                dgvOrders.Columns.Clear();

                // Gán dữ liệu cho DataGridView
                dgvOrders.DataSource = orderTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu từ cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            CalculateTotals();
        }

        private void LoadCustomerNames()
        {
            try
            {
                // Câu lệnh SQL để lấy dữ liệu từ bảng Customer
                string query = "SELECT NameCus FROM Customer";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    // Xóa các mục cũ trong ComboBox (nếu có)
                    cbCus.Items.Clear();

                    // Nạp dữ liệu từ cột NameCus vào ComboBox
                    while (reader.Read())
                    {
                        cbCus.Items.Add(reader["NameCus"].ToString());
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu từ bảng Customer: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbCus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCus.SelectedItem != null)
            {
                string selectedCustomer = cbCus.SelectedItem.ToString();
                MessageBox.Show("Khách hàng được chọn: " + selectedCustomer, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Không có khách hàng nào được chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgv2_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                int rowIndex = dgv2.CurrentRow.Index;
                txtID.Text = dgv2.Rows[rowIndex].Cells["ProductID"].Value.ToString();
                txtName.Text = dgv2.Rows[rowIndex].Cells["Product_name"].Value.ToString();
                txtPrice.Text = dgv2.Rows[rowIndex].Cells["Price"].Value.ToString();
                //txtQuantity.Text = dgv2.Rows[rowIndex].Cells["Quantity"].Value.ToString();
                EXP.Text = dgv2.Rows[rowIndex].Cells["EXP"].Value.ToString();
            }
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text) ||
       string.IsNullOrWhiteSpace(txtName.Text) ||
       string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy dữ liệu từ các ô nhập liệu
            string productID = txtID.Text;
            string productName = txtName.Text;
            int quantity = int.Parse(txtQuantity.Text);

            try
            {
                decimal cost;

                // Truy vấn để lấy giá trị Cost từ bảng Product
                string costQuery = "SELECT Cost FROM Products WHERE ProductID = @ProductID";
                using (SqlCommand costCommand = new SqlCommand(costQuery, connection))
                {
                    costCommand.Parameters.AddWithValue("@ProductID", productID);
                    object result = costCommand.ExecuteScalar();

                    if (result == null)
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm với ID đã nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    cost = Convert.ToDecimal(result); 
                }

                // Tính tổng tiền cho sản phẩm (Cost * Quantity)
                decimal totalPrice = cost * quantity;

                // Tạo câu lệnh SQL để chèn dữ liệu vào bảng Order
                string query = @"
            INSERT INTO [user].[dbo].[Order] (ID, Name, Price, Qty, Total)
            VALUES (@ID, @Name, @Price, @Qty, @Total);
        ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Gắn giá trị tham số
                    command.Parameters.AddWithValue("@ID", productID);
                    command.Parameters.AddWithValue("@Name", productName);
                    command.Parameters.AddWithValue("@Price", cost); // Dùng giá trị Cost
                    command.Parameters.AddWithValue("@Qty", quantity);
                    command.Parameters.AddWithValue("@Total", totalPrice);

                    // Thực thi lệnh SQL
                    command.ExecuteNonQuery();
                }

                // Hiển thị thông báo thành công
                MessageBox.Show("Dữ liệu đã được thêm vào cơ sở dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Tải lại dữ liệu cho DataGridView Orders
                LoadOrders();
                CalculateTotals();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show("Lỗi khi thêm dữ liệu vào cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Reset các ô nhập liệu sau khi thêm xong (tuỳ chọn)
            txtID.Clear();
            txtName.Clear();
            txtQuantity.Clear();

        }

        private void dgvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int rowIndex = dgvOrders.CurrentRow.Index;
                txtID.Text = dgvOrders.Rows[rowIndex].Cells["ID"].Value.ToString();
                txtName.Text = dgvOrders.Rows[rowIndex].Cells["Name"].Value.ToString();
                txtQuantity.Text = dgvOrders.Rows[rowIndex].Cells["Qty"].Value.ToString();
                txtPrice.Text = dgvOrders.Rows[rowIndex].Cells["Price"].Value.ToString();
                //EXP.Text = dgvOrders.Rows[rowIndex].Cells["EXP"].Value.ToString();
            }
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu ô ID trống
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy ProductID từ ô nhập liệu
            string productID = txtID.Text;

            try
            {
                // Tạo câu lệnh SQL để xóa sản phẩm khỏi bảng Products
                string query = @"
            DELETE FROM [user].[dbo].[Order]
            WHERE ID = @ID;
        ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Gắn giá trị tham số
                    command.Parameters.AddWithValue("@ID", productID);

                    // Thực thi lệnh SQL
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Hiển thị thông báo thành công
                        MessageBox.Show("Sản phẩm đã được xóa khỏi cơ sở dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Làm mới dữ liệu hiển thị
                        LoadOrders();
                        CalculateTotals();
                    }
                    else
                    {
                        // Nếu không tìm thấy sản phẩm để xóa
                        MessageBox.Show("Không tìm thấy sản phẩm với ID này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show("Lỗi khi xóa sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Reset các ô nhập liệu sau khi xóa xong (tùy chọn)
            txtID.Clear();
            txtName.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
        }

        private void CalculateTotals()
        {
            int totalQuantity = 0;
            decimal totalAmount = 0;

            // Duyệt qua tất cả các hàng trong DataGridView
            foreach (DataGridViewRow row in dgvOrders.Rows)
            {
                if (row.Cells["Qty"].Value != null && row.Cells["Total"].Value != null)
                {
                    // Cộng dồn số lượng và tổng tiền
                    totalQuantity += Convert.ToInt32(row.Cells["Qty"].Value);
                    totalAmount += Convert.ToDecimal(row.Cells["Total"].Value);
                }
            }

            // Hiển thị tổng số lượng và tổng tiền vào TextBox
            textBox2.Text = totalQuantity.ToString();
            txtTotal.Text = totalAmount.ToString("N2"); // Format số thập phân
        }

        private void dgv2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            if (dgvOrders.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để in!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();

                    // Xóa dữ liệu khỏi bảng Order
                    foreach (DataGridViewRow row in dgvOrders.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string deleteQuery = "DELETE FROM [user].[dbo].[Order] WHERE ID = @ID";
                        using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                        {
                            deleteCmd.Parameters.AddWithValue("@ID", row.Cells["ID"].Value);
                            deleteCmd.ExecuteNonQuery();
                        }
                    }

                    // Lưu dữ liệu vào bảng Order2
                    foreach (DataGridViewRow row in dgvOrders.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string checkQuery = "SELECT COUNT(*) FROM Order2 WHERE ID = @ID";
                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@ID", row.Cells["ID"].Value);
                            int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                            if (count > 0)
                            {
                                string updateQuery = "UPDATE Order2 SET Qty = Qty + @Qty, Total = Total + @Total WHERE ID = @ID";
                                using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                                {
                                    updateCmd.Parameters.AddWithValue("@ID", row.Cells["ID"].Value);
                                    updateCmd.Parameters.AddWithValue("@Qty", row.Cells["Qty"].Value);
                                    updateCmd.Parameters.AddWithValue("@Total", row.Cells["Total"].Value);
                                    updateCmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                string insertQuery = "INSERT INTO Order2 (ID, Name, Qty, Price, Total) VALUES (@ID, @Name, @Qty, @Price, @Total)";
                                using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                                {
                                    insertCmd.Parameters.AddWithValue("@ID", row.Cells["ID"].Value);
                                    insertCmd.Parameters.AddWithValue("@Name", row.Cells["Name"].Value);
                                    insertCmd.Parameters.AddWithValue("@Qty", row.Cells["Qty"].Value);
                                    insertCmd.Parameters.AddWithValue("@Price", row.Cells["Price"].Value);
                                    insertCmd.Parameters.AddWithValue("@Total", row.Cells["Total"].Value);
                                    insertCmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }



                    // Cập nhật số lượng trong kho và thêm giao dịch
                    foreach (DataGridViewRow row in dgvOrders.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string productId = row.Cells["ID"].Value.ToString();  // ID sản phẩm
                        int quantity = Convert.ToInt32(row.Cells["Qty"].Value); // Số lượng

                        // Cập nhật số lượng tồn kho
                        string updateStockQuery = @"
                UPDATE Products
                SET Quantity = Quantity - @Quantity
                WHERE ProductID = @ProductID AND Quantity >= @Quantity;

                INSERT INTO Transactions (ProductID, TransactionType, Quantity, TransactionDate)
                VALUES (@ProductID, 'Export', @Quantity, GETDATE());";

                        using (SqlCommand stockCmd = new SqlCommand(updateStockQuery, conn))
                        {
                            stockCmd.Parameters.AddWithValue("@ProductID", productId);
                            stockCmd.Parameters.AddWithValue("@Quantity", quantity);
                            stockCmd.ExecuteNonQuery();
                        }
                        loaddata();
                    }

                    MessageBox.Show("Bạn đã order thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu hoặc xóa dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();

                    // Kiểm tra xem có bản ghi nào trong bảng Ordered chưa
                    string checkQuery = "SELECT COUNT(*) FROM Ordered";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count == 0)
                        {
                            // Nếu chưa có, thêm mới với OrderID = 1
                            string insertQuery = "INSERT INTO Ordered (OrderID) VALUES (1)";
                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                            {
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Nếu đã có, tăng OrderID lên 1
                            string updateQuery = "UPDATE Ordered SET OrderID = OrderID + 1";
                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                            {
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật số lần đặt hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            DialogResult result = MessageBox.Show("Bạn có muốn in hóa đơn không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Nếu chọn Yes, tiến hành in hóa đơn
            if (result == DialogResult.Yes)
            {
                // Tạo và lưu file PDF (phần này giữ nguyên như mã hiện tại)
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF Files|*.pdf",
                    Title = "Lưu hóa đơn"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Tạo tài liệu PDF
                        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                        PdfWriter.GetInstance(pdfDoc, new FileStream(saveFileDialog.FileName, FileMode.Create));
                        pdfDoc.Open();

                        // Tiêu đề
                        Paragraph title = new Paragraph("MedBill \n\n", FontFactory.GetFont("Arial", 18));
                        title.Alignment = Element.ALIGN_CENTER;
                        pdfDoc.Add(title);

                        // Thêm thông tin khách hàng
                        string customerName = cbCus.Text; // Lấy tên khách hàng từ ComboBox
                        if (!string.IsNullOrEmpty(customerName))
                        {
                            Paragraph customerInfo = new Paragraph($"Customer: {customerName}\n\n", FontFactory.GetFont("Arial", 12));
                            customerInfo.Alignment = Element.ALIGN_LEFT;
                            pdfDoc.Add(customerInfo);
                        }

                        // Bảng dữ liệu
                        PdfPTable table = new PdfPTable(dgvOrders.Columns.Count)
                        {
                            WidthPercentage = 100
                        };

                        // Thêm tiêu đề cột
                        foreach (DataGridViewColumn column in dgvOrders.Columns)
                        {
                            table.AddCell(new Phrase(column.HeaderText, FontFactory.GetFont("Arial", 10)));
                        }

                        // Thêm dữ liệu hàng
                        foreach (DataGridViewRow row in dgvOrders.Rows)
                        {
                            if (row.IsNewRow) continue;
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                table.AddCell(new Phrase(cell.Value?.ToString() ?? "", FontFactory.GetFont("Arial", 10)));
                            }
                        }

                        pdfDoc.Add(table);

                        // Tính tổng số lượng và tổng tiền
                        int totalQuantity = 0;
                        decimal totalAmount = 0;
                        foreach (DataGridViewRow row in dgvOrders.Rows)
                        {
                            if (row.IsNewRow) continue;
                            totalQuantity += Convert.ToInt32(row.Cells["Qty"].Value);
                            totalAmount += Convert.ToDecimal(row.Cells["Total"].Value);
                        }

                        // Thêm tổng số lượng và tổng tiền
                        Paragraph totals = new Paragraph($"\nTotal Quantity: {totalQuantity}\nTotal Amount: {totalAmount:N2}", FontFactory.GetFont("Arial", 12));
                        totals.Alignment = Element.ALIGN_RIGHT;
                        pdfDoc.Add(totals);

                        pdfDoc.Close();

                        MessageBox.Show("Hóa đơn đã được lưu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lưu hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
            else
            {
                MessageBox.Show("Hủy in hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // Xóa dữ liệu khỏi DataGridView
            if (dgvOrders.DataSource is DataTable dt)
            {
                dt.Clear();
            }
            else
            {
                dgvOrders.Rows.Clear();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}