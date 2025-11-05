using CoffeeManagement.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CoffeeManagement.Forms
{
    public partial class frmReport : Form
    {
        private readonly OrderService orderService = new OrderService();
        private readonly ProductService productService = new ProductService();

        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void LoadReport()
        {
            try
            {
                // Get all orders and products
                List<Models.Order> orders = orderService.GetAll();
                List<Models.Product> products = productService.GetAll();

                // Join orders with products and include dates
                var reportData = from o in orders
                                 join p in products on o.ProductId equals p.Id
                                 select new
                                 {
                                     OrderID = o.Id,
                                     ProductName = p.Name,
                                     Category = p.Category,
                                     Quantity = o.Quantity,
                                     Total = o.Total,
                                     ProductCreated = p.CreatedAt, // Product creation date
                                     OrderDate = o.OrderDate        // Order date
                                 };

                dgvReport.DataSource = reportData.ToList();

                // Format date columns if exist
                if (dgvReport.Columns["ProductCreated"] != null)
                    dgvReport.Columns["ProductCreated"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";

                if (dgvReport.Columns["OrderDate"] != null)
                    dgvReport.Columns["OrderDate"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";

                // Show summary
                lblTotalIncome.Text = $"Total Income: ${reportData.Sum(x => x.Total):0.00}";
                lblTotalQuantity.Text = $"Total Quantity: {reportData.Sum(x => x.Quantity)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmDashboard dashboard = new frmDashboard(); // open the correct dashboard form
            dashboard.ShowDialog();
            this.Close();
        }

        private void dgvReport_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
