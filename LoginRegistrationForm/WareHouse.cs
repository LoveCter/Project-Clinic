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
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LoginRegistrationForm
{

    public partial class WareHouse : Form
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
        SELECT 
        p.ProductID, 
        p.Product_Name,  
        p.Product_Suplier, 
        p.Quantity, 
        p.EXP, 
        p.Price,
        r.ReceiptDay 
    FROM Products p

    INNER JOIN Receipt r ON p.ProductID = r.ProductID
    ORDER BY CAST(p.ProductID AS INT)"

; // Kết hợp hai bảng

            adapter.SelectCommand = command;
            table.Clear();
            adapter.Fill(table);
            dgvWarehouse.DataSource = table;


        }
        public WareHouse()
        {
            InitializeComponent();
        }

        private void WareHouse_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(str);
            connection.Open();
            loaddata();
        }
        private void CheckLowStock()
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"
        SELECT ProductID, Product_Name, Quantity, MinimumStock
        FROM Products
        WHERE Quantity < MinimumStock";
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable lowStockTable = new DataTable();
            adapter.Fill(lowStockTable);

            if (lowStockTable.Rows.Count > 0)
            {
                dgvWarehouse.DataSource = lowStockTable;
                MessageBox.Show("Some products are low on stock!", "Warning");
            }
            else
            {
                MessageBox.Show("All products have sufficient stock.", "Info");
            }
        }





        private void btnSupplier_Click(object sender, EventArgs e)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"
        SELECT Product_Suplier, COUNT(ProductID) AS ProductCount, SUM(Quantity) AS TotalQuantity
        FROM Products
        GROUP BY Product_Suplier";
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable statsTable = new DataTable();
            adapter.Fill(statsTable);

            dgvWarehouse.DataSource = statsTable; // Hiển thị kết quả
        }


        private void UpdateStock(string productId, int quantity, string transactionType)
        {
            try
            {
                SqlCommand command = connection.CreateCommand();

                // Tạo lệnh SQL cho giao dịch nhập hoặc xuất kho
                if (transactionType == "Import")
                {
                    command.CommandText = @"
                UPDATE Products
                SET Quantity = Quantity + @Quantity
                WHERE ProductID = @ProductID;
                
                INSERT INTO Transactions (ProductID, TransactionType, Quantity, TransactionDate)
                VALUES (@ProductID, 'Import', @Quantity, GETDATE());";
                }
                else if (transactionType == "Export")
                {
                    command.CommandText = @"
                UPDATE Products
                SET Quantity = Quantity - @Quantity
                WHERE ProductID = @ProductID AND Quantity >= @Quantity;
                
                INSERT INTO Transactions (ProductID, TransactionType, Quantity, TransactionDate)
                VALUES (@ProductID, 'Export', @Quantity, GETDATE());";
                }

                // Thêm tham số
                command.Parameters.AddWithValue("@ProductID", productId);
                command.Parameters.AddWithValue("@Quantity", quantity);

                // Thực thi lệnh
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show($"{transactionType} successful!", "Success");
                }
                else
                {
                    MessageBox.Show("Operation failed. Check the Product ID or Quantity.", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
            }
        }

        private void btnCheckLowStock_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Tạo command để lấy các sản phẩm có số lượng dưới 10
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
            SELECT ProductID, Product_Name, Quantity
            FROM Products
            WHERE Quantity < 10"; // Lọc các sản phẩm có Quantity nhỏ hơn 10
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable lowStockTable = new DataTable();

                // Đổ dữ liệu vào DataTable
                adapter.Fill(lowStockTable);

                // Kiểm tra có sản phẩm nào dưới ngưỡng tồn kho không
                if (lowStockTable.Rows.Count > 0)
                {
                    dgvWarehouse.DataSource = lowStockTable; // Hiển thị danh sách sản phẩm trong DataGridView
                    MessageBox.Show("Some products are low on stock (less than 10)!", "Low Stock Warning");
                }
                else
                {
                    MessageBox.Show("All products have sufficient stock.", "Info");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error");
            }
        }


        private void btnImport_Click_1(object sender, EventArgs e)
        {
            try
            {
                string productId = txtID.Text.Trim(); // ID sản phẩm là chuỗi
                int quantity = int.Parse(txtQuantity.Text);  // Số lượng nhập từ textbox

                UpdateStock(productId, quantity, "Import");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
            }
        }

        //private void btnExport_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // Lấy ProductID và Quantity từ textbox
        //        string productId = txtProductID.Text.Trim(); // ProductID là chuỗi, dùng Trim để loại bỏ khoảng trắng
        //        int quantity = int.Parse(txtQuantity.Text);  // Số lượng nhập từ textbox

        //        // Gọi hàm UpdateStock để xử lý xuất kho
        //        UpdateStock(productId, quantity, "Export");
        //    }
        //    catch (FormatException)
        //    {
        //        MessageBox.Show("Invalid input. Please enter a valid Product ID and Quantity.", "Input Error");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("An error occurred: " + ex.Message, "Error");
        //    }
        //}
        private void btnHistory_Click(object sender, EventArgs e)
        {

            LoadTransactionHistory();

        }
        private void LoadTransactionHistory()
        {
            try
            {
                // Tạo truy vấn để lấy lịch sử giao dịch
                string query = @"
            SELECT 
                T.TransactionID,
                P.Product_Name,
                T.TransactionType,
                T.Quantity,
                T.TransactionDate
            FROM 
                Transactions T
            JOIN 
                Products P ON T.ProductID = P.ProductID
            ORDER BY 
                T.TransactionDate DESC";

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable historyTable = new DataTable();

                // Đổ dữ liệu vào DataTable
                adapter.Fill(historyTable);

                // Hiển thị dữ liệu lên DataGridView
                dgvWarehouse.DataSource = historyTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error");
            }
        }



        //private void btnReturn_Click(object sender, EventArgs e)
        //{
        //    {
        //        try
        //        {
        //            // Truy vấn để hiển thị lại dữ liệu Products
        //            string query = @"
        //    SELECT 
        //        ProductID AS [Product ID],
        //        Product_Name AS [Product Name],
        //        Type AS [Type],
        //        Product_Suplier AS [Supplier],
        //        Quantity AS [Quantity],
        //        EXP AS [Expiry Date]
        //    FROM 
        //        Products";

        //            SqlCommand command = new SqlCommand(query, connection);
        //            SqlDataAdapter adapter = new SqlDataAdapter(command);
        //            DataTable productTable = new DataTable();

        //            // Đổ dữ liệu vào DataTable
        //            adapter.Fill(productTable);

        //            // Hiển thị dữ liệu lên dgv1
        //            dgvWarehouse.DataSource = productTable;
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("An error occurred: " + ex.Message, "Error");
        //        }
        //    }
        //}



        private void btnCheckExpire_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo command để lấy các sản phẩm đã hết hạn hoặc sắp hết hạn
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
            SELECT 
                ProductID, 
                Product_Name, 
                Type, 
                Product_Suplier, 
                Quantity, 
                EXP,
                CASE 
                    WHEN EXP < GETDATE() THEN 'Expired'
                    WHEN EXP BETWEEN GETDATE() AND DATEADD(DAY, 7, GETDATE()) THEN 'Expiring Soon'
                END AS Status
            FROM Products
            WHERE EXP < GETDATE() OR EXP BETWEEN GETDATE() AND DATEADD(DAY, 7, GETDATE())";
                // Lấy sản phẩm đã hết hạn hoặc sắp hết hạn trong 7 ngày tới

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable expireTable = new DataTable();

                // Đổ dữ liệu vào DataTable
                adapter.Fill(expireTable);

                // Kiểm tra có sản phẩm nào cần cảnh báo không
                if (expireTable.Rows.Count > 0)
                {
                    dgvWarehouse.DataSource = expireTable; // Hiển thị các sản phẩm lên DataGridView
                    MessageBox.Show("Some products have expired or are expiring soon!", "Warning");
                }
                else
                {
                    MessageBox.Show("No expired or expiring products found.", "Check Complete");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error");
            }
        }

        private void dgv1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int rowIndex = dgvWarehouse.CurrentRow.Index;
                txtID.Text = dgvWarehouse.Rows[rowIndex].Cells["ProductID"].Value.ToString();
                txtName.Text = dgvWarehouse.Rows[rowIndex].Cells["Product_Name"].Value.ToString();
                txtPrice.Text = dgvWarehouse.Rows[rowIndex].Cells["Price"].Value.ToString();
                txtQuantity.Text = dgvWarehouse.Rows[rowIndex].Cells["Quantity"].Value.ToString();
                EXP.Text = dgvWarehouse.Rows[rowIndex].Cells["EXP"].Value.ToString();
                ReceiptDay.Text = dgvWarehouse.Rows[rowIndex].Cells["ReceiptDay"].Value.ToString();
                txtSupplier.Text = dgvWarehouse.Rows[rowIndex].Cells["Product_Suplier"].Value.ToString();

            }
        }

        private void btnPrintImport_Click(object sender, EventArgs e)
        {
            if (dgvWarehouse.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để in!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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
                    Paragraph title = new Paragraph("Pharmaceutical Invoice \n\n", FontFactory.GetFont("Arial", 18));
                    title.Alignment = Element.ALIGN_CENTER;
                    pdfDoc.Add(title);

                    // Bảng dữ liệu
                    PdfPTable table = new PdfPTable(dgvWarehouse.Columns.Count)
                    {
                        WidthPercentage = 100
                    };

                    // Thêm tiêu đề cột
                    foreach (DataGridViewColumn column in dgvWarehouse.Columns)
                    {
                        table.AddCell(new Phrase(column.HeaderText));
                    }

                    // Thêm dữ liệu hàng
                    foreach (DataGridViewRow row in dgvWarehouse.Rows)
                    {
                        if (row.IsNewRow) continue;
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            table.AddCell(new Phrase(cell.Value?.ToString() ?? ""));
                        }
                    }

                    pdfDoc.Add(table);
                    pdfDoc.Close();

                    MessageBox.Show("Hóa đơn đã được lưu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }












            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu nhập vào có hợp lệ
            if (string.IsNullOrWhiteSpace(txtID.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtSupplier.Text) ||
                string.IsNullOrWhiteSpace(EXP.Text) ||
                string.IsNullOrWhiteSpace(txtQuantity.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text) ||
                string.IsNullOrWhiteSpace(ReceiptDay.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên không âm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Giá phải là số thực không âm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!DateTime.TryParse(EXP.Text, out DateTime expDate))
            {
                MessageBox.Show("Ngày hết hạn không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!DateTime.TryParse(ReceiptDay.Text, out DateTime receiptDate))
            {
                MessageBox.Show("Ngày nhận không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(str))
                {
                    connection.Open();

                    // Kiểm tra xem ProductID đã tồn tại hay chưa
                    string checkQuery = "SELECT COUNT(*) FROM Products WHERE ProductID = @ProductID";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@ProductID", txtID.Text);
                    int productExists = (int)checkCommand.ExecuteScalar();

                    if (productExists > 0)
                    {
                        // Nếu sản phẩm đã tồn tại, cập nhật số lượng (logic của Import)
                        string updateQuery = @"
                    UPDATE Products
                    SET Quantity = Quantity + @Quantity
                    WHERE ProductID = @ProductID;
                    
                    INSERT INTO Transactions (ProductID, TransactionType, Quantity, TransactionDate)
                    VALUES (@ProductID, 'Import', @Quantity, GETDATE());";
                        SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@ProductID", txtID.Text);
                        updateCommand.Parameters.AddWithValue("@Quantity", quantity);
                        updateCommand.ExecuteNonQuery();
                        MessageBox.Show("Cập nhật số lượng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        // Nếu sản phẩm chưa tồn tại, thêm sản phẩm mới (logic của Add)
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                // Thêm dữ liệu vào bảng Products
                                string addProductQuery = @"
                            INSERT INTO Products (ProductID, Product_Name, Product_Suplier, Quantity, EXP, Price)
                            VALUES (@ProductID, @ProductName, @Supplier, @Quantity, @EXP, @Price)";
                                SqlCommand addProductCommand = new SqlCommand(addProductQuery, connection, transaction);
                                addProductCommand.Parameters.AddWithValue("@ProductID", txtID.Text);
                                addProductCommand.Parameters.AddWithValue("@ProductName", txtName.Text);
                                addProductCommand.Parameters.AddWithValue("@Supplier", txtSupplier.Text);
                                addProductCommand.Parameters.AddWithValue("@Quantity", quantity);
                                addProductCommand.Parameters.AddWithValue("@EXP", expDate);
                                addProductCommand.Parameters.AddWithValue("@Price", price);
                                addProductCommand.ExecuteNonQuery();

                                // Thêm dữ liệu vào bảng Receipt
                                string addReceiptQuery = @"
                            INSERT INTO Receipt (ProductID, ReceiptDay)
                            VALUES (@ProductID, @ReceiptDay)";
                                SqlCommand addReceiptCommand = new SqlCommand(addReceiptQuery, connection, transaction);
                                addReceiptCommand.Parameters.AddWithValue("@ProductID", txtID.Text);
                                addReceiptCommand.Parameters.AddWithValue("@ReceiptDay", receiptDate);
                                addReceiptCommand.ExecuteNonQuery();

                                // Commit transaction
                                transaction.Commit();

                                MessageBox.Show("Thêm sản phẩm mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch
                            {
                                transaction.Rollback();
                                MessageBox.Show("Đã xảy ra lỗi khi thêm sản phẩm mới. Thay đổi đã được hoàn tác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
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

        // Hàm xóa trắng các trường nhập
        private void ClearInputs()
        {
            txtID.Clear();
            txtName.Clear();
            txtSupplier.Clear();
            txtQuantity.Clear();
            txtPrice.Clear();

        }

   

      

        private void btnReturn_Click_1(object sender, EventArgs e)
        {
            loaddata(); 
        }
    }
}





