using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginRegistrationForm
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            //btnEmployee.Click += btnEmployee_Click;
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            EmployeeForm employeeForm = new EmployeeForm();
            AddControl(employeeForm);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            //this.Enabled = false; // This will disable the entire form


        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    EmployeeForm employee = new EmployeeForm();
        //    AddControl(employee);
        //}

        private void AddControl(Form form)
        {
            form.TopLevel = false;
            mainpnl.Controls.Clear();
            mainpnl.Controls.Add(form);
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Show();
        }


      
       

        private void label1_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Nếu người dùng chọn "Có", thì thoát chương trình
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

     

        private void btnSignOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Nếu người dùng chọn "Có", thì thoát chương trình
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void picAvatar_Click(object sender, EventArgs e)
        {

        }

        private void btnChange_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (.jpg; *.jpeg; *.png; *.bmp)|.jpg;*.jpeg;*.png;*.bmp",
                Title = "Select an Image"
            };

            // Kiểm tra nếu người dùng chọn file
           
        }

        private void btnMedicine_Click(object sender, EventArgs e)
        {
            Products medicine= new Products();
            AddControl(medicine);
        }

      

        private void btnAnalysis_Click(object sender, EventArgs e)
        {
           AnalysisReport Analysisform = new AnalysisReport();
            AddControl(Analysisform);
        }

        private void btnSuplier_Click(object sender, EventArgs e)
        {
            Suplier suplierform = new Suplier();
            AddControl(suplierform);
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            Customer customerform = new Customer();
            AddControl(customerform);
        }

        private void btnWarehouse_Click(object sender, EventArgs e)
        {
            WareHouse warehouseform = new WareHouse();  
            AddControl (warehouseform); 
        }
    }
}
