using CoffeeManagement.Models;
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
    public partial class frmOrder : Form
    {
        private readonly OrderService orderService = new OrderService();
        private readonly ProductService productService = new ProductService();
        private List<Product> products;
        public frmOrder()
        {
            InitializeComponent();
        }

        private void frmOrder_Load(object sender, EventArgs e)
        {
            LoadProducts();
            LoadOrders();
        }
        private void LoadProducts()
        {
            products = productService.GetAll();
            cbProduct.DataSource = products;
            cbProduct.DisplayMember = "Name";
            cbProduct.ValueMember = "Id";
        }

        private void LoadOrders()
        {
            dataGridOrders.DataSource = null;
            dataGridOrders.DataSource = orderService.GetAll();

            if (dataGridOrders.Columns["OrderDate"] != null)
                dataGridOrders.Columns["OrderDate"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";

        }

        private void cbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbProduct.SelectedItem is Product selected)
                txtPrice.Text = selected.Price.ToString();
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtPrice.Text, out var price) && int.TryParse(txtQuantity.Text, out var qty))
                txtTotal.Text = (price * qty).ToString("0.00");
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var order = new Order
                {
                    ProductId = (int)cbProduct.SelectedValue,
                    Quantity = int.Parse(txtQuantity.Text),
                    Total = decimal.Parse(txtTotal.Text)
                };
                orderService.Add(order);
                MessageBox.Show("✅ Order added!");
                LoadOrders();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearFields()
        {
            txtQuantity.Clear();
            txtTotal.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridOrders.CurrentRow != null)
            {
                int id = (int)dataGridOrders.CurrentRow.Cells["Id"].Value;
                orderService.Delete(id);
                MessageBox.Show("🗑️ Order deleted!");
                LoadOrders();
                ClearFields();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridOrders.CurrentRow != null)
            {
                try
                {
                    var order = new Order
                    {
                        Id = (int)dataGridOrders.CurrentRow.Cells["Id"].Value,
                        ProductId = (int)cbProduct.SelectedValue,
                        Quantity = int.Parse(txtQuantity.Text),
                        Total = decimal.Parse(txtTotal.Text)
                    };
                    orderService.Update(order);
                    MessageBox.Show("✅ Order updated!");
                    LoadOrders();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dataGridOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dataGridOrders.Rows[e.RowIndex];
                cbProduct.Text = row.Cells["ProductName"].Value.ToString();
                txtQuantity.Text = row.Cells["Quantity"].Value.ToString();
                txtTotal.Text = row.Cells["Total"].Value.ToString();
                // Correct column name for order date
                if (dataGridOrders.Columns["OrderDate"] != null && row.Cells["OrderDate"].Value != null)
                    txtCreatedAt.Text = ((DateTime)row.Cells["OrderDate"].Value).ToString("yyyy-MM-dd HH:mm");

   

                var product = products.FirstOrDefault(p => p.Name == cbProduct.Text);
                if (product != null)
                    txtPrice.Text = product.Price.ToString();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            //btnReport f = new btnReport();
            this.Hide();
            frmDashboard f = new frmDashboard();
            f.ShowDialog();
            this.Show();
        }
    }
}
