using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Excel = Microsoft.Office.Interop.Excel;

namespace LoginRegistrationForm
{
    public partial class AnalysisReport : Form
    {
        private SqlConnection connection;

        public AnalysisReport()
        {
            InitializeComponent();
            // Khởi tạo kết nối SQL
            connection = new SqlConnection("Data Source=LOVECTERS;Initial Catalog=user;Integrated Security=True;Encrypt=False");
        }

        private void AnalysisReport_Load(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                string query = "SELECT OrderID FROM Ordered";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        GlobalCounter.OrderButtonClickCount = Convert.ToInt32(result);
                    }
                    else
                    {
                        GlobalCounter.OrderButtonClickCount = 0;
                    }
                }
                // Hiển thị doanh thu
                decimal revenue = GetRevenue();
                txtNumRevenue.Text = revenue.ToString("C"); // Định dạng tiền tệ

                // Các giá trị khác (nếu có)
                int lowStockCount = GetLowStockCount();
                txtNumStock.Text = lowStockCount.ToString();

                int expiredCount = GetExpiredCount();
                txtNumExpired.Text = expiredCount.ToString();

                txtNumOrders.Text = GlobalCounter.OrderButtonClickCount.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private decimal GetRevenue()
        {
            try
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT SUM((O.Price - P.Price) * O.Qty) AS TotalRevenue
                    FROM Order2 O
                    JOIN Products P ON O.ID = P.ProductID"; // Tính tổng Revenue

                object result = command.ExecuteScalar(); // Lấy kết quả từ SQL
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving revenue: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0; // Trả về 0 nếu xảy ra lỗi
            }
        }

        private int GetLowStockCount()
        {
            try
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT COUNT(*) 
                    FROM Products
                    WHERE Quantity < 10"; // Truy vấn đếm số lượng tồn kho thấp

                int count = Convert.ToInt32(command.ExecuteScalar()); // Lấy kết quả từ SQL
                return count;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving low stock count: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0; // Trả về 0 nếu xảy ra lỗi
            }
        }

        private int GetExpiredCount()
        {
            try
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT COUNT(*) 
                    FROM Products
                    WHERE EXP < GETDATE() 
                       OR EXP BETWEEN GETDATE() AND DATEADD(DAY, 7, GETDATE())"; // Truy vấn đếm số sản phẩm hết hạn hoặc sắp hết hạn

                int count = Convert.ToInt32(command.ExecuteScalar()); // Lấy kết quả từ SQL
                return count;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving expired count: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0; // Trả về 0 nếu xảy ra lỗi
            }
        }


        private void txtNumOrders_TextChanged(object sender, EventArgs e)
        {
            // Sự kiện TextChanged (có thể bỏ trống nếu không cần logic thêm)
        }

        private void txtNumStock_TextChanged(object sender, EventArgs e)
        {
            // Sự kiện TextChanged (có thể bỏ trống nếu không cần logic thêm)
        }
        
        private void cbReport_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cbReport.SelectedItem.ToString() == "Low Stock")
            {
                LoadLowStockData();
            }
            else if (cbReport.SelectedItem.ToString() == "Price")
            {
                LoadPriceData();
            }
        }

        private void LoadLowStockData()
        {
            try
            {
                connection.Open();
                string query = "SELECT Product_name, Quantity FROM Products";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Cấu hình biểu đồ
                chart1.Series.Clear();
                Series series = new Series
                {
                    Name = "Quantity",
                    ChartType = SeriesChartType.Column,
                    IsValueShownAsLabel = true
                };

                chart1.Series.Add(series);
                chart1.ChartAreas[0].AxisX.Title = "Product Name";
                chart1.ChartAreas[0].AxisY.Title = "Quantity";

                foreach (DataRow row in dataTable.Rows)
                {
                    string productName = row["Product_name"].ToString();
                    int quantity = Convert.ToInt32(row["Quantity"]);

                    int pointIndex = series.Points.AddXY(productName, quantity);

                    // Tô đỏ nếu Quantity < 10
                    if (quantity < 10)
                    {
                        series.Points[pointIndex].Color = Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void LoadPriceData()
        {
            try
            {
                connection.Open();
                string query = "SELECT Product_name, Price FROM Products";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Cấu hình biểu đồ
                chart1.Series.Clear();
                Series series = new Series
                {
                    Name = "Price",
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2,
                    Color = Color.Blue,
                    IsValueShownAsLabel = true
                };

                chart1.Series.Add(series);
                chart1.ChartAreas[0].AxisX.Title = "Product Name";
                chart1.ChartAreas[0].AxisY.Title = "Price";

                foreach (DataRow row in dataTable.Rows)
                {
                    string productName = row["Product_name"].ToString();
                    decimal price = Convert.ToDecimal(row["Price"]);

                    series.Points.AddXY(productName, price);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            try
            {
                // Khởi tạo Excel Application
                Excel.Application excelApp = new Excel.Application();
                if (excelApp == null)
                {
                    MessageBox.Show("Excel is not properly installed!");
                    return;
                }

                // Tạo workbook và worksheet
                Excel.Workbook workbook = excelApp.Workbooks.Add();
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];
                worksheet.Name = "Chart Data";

                // Thêm tiêu đề cột
                worksheet.Cells[1, 1] = "Product Name";
                worksheet.Cells[1, 2] = "Value";

                // Lấy dữ liệu từ biểu đồ
                Series series = chart1.Series[0];
                for (int i = 0; i < series.Points.Count; i++)
                {
                    string productName = series.Points[i].AxisLabel;
                    double value = series.Points[i].YValues[0];

                    worksheet.Cells[i + 2, 1] = productName;
                    worksheet.Cells[i + 2, 2] = value;
                }

                // Định dạng tiêu đề
                Excel.Range headerRange = worksheet.get_Range("A1", "B1");
                headerRange.Font.Bold = true;
                headerRange.Interior.Color = Color.LightGray;
                headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // Tạo biểu đồ trong Excel
                Excel.ChartObjects chartObjects = (Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
                Excel.ChartObject chartObject = chartObjects.Add(300, 50, 400, 300);
                Excel.Chart chart = chartObject.Chart;

                // Gắn dữ liệu cho biểu đồ
                Excel.Range dataRange = worksheet.get_Range("A1", "B" + (series.Points.Count + 1));
                chart.SetSourceData(dataRange, Type.Missing);

                // Đặt loại biểu đồ giống biểu đồ đang hiển thị
                chart.ChartType = series.ChartType == SeriesChartType.Column
                    ? Excel.XlChartType.xlColumnClustered
                    : Excel.XlChartType.xlLine;

                // Hiển thị Excel
                excelApp.Visible = true;

                MessageBox.Show("Exported chart data to Excel successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void txtNumRevenue_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
