using CoffeeManagement.Models;
using CoffeeManagement.Services;
using System;
using System.Windows.Forms;

namespace CoffeeManagement.Forms
{
    public partial class frmProduct : Form
    {
        private readonly ProductService service = new ProductService();

        public frmProduct()
        {
            InitializeComponent();
        }

        private void frmProduct_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            dataGridProducts.DataSource = null;
            dataGridProducts.DataSource = service.GetAll();
         

            if (dataGridProducts.Columns["CreatedAt"] != null)
                dataGridProducts.Columns["CreatedAt"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
        }

       
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtName.Clear();
            txtCategory.Clear();
            txtPrice.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Models.Product p = new Models.Product
                {
                    Name = txtName.Text,
                    Category = txtCategory.Text,
                    Price = decimal.Parse(txtPrice.Text)
                };
                service.Add(p);
                MessageBox.Show("✅ Product added!");
                LoadProducts();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridProducts.CurrentRow != null)
            {
                try
                {
                    Models.Product p = new Models.Product
                    {
                        Id = (int)dataGridProducts.CurrentRow.Cells["Id"].Value,
                        Name = txtName.Text,
                        Category = txtCategory.Text,
                        Price = decimal.Parse(txtPrice.Text)
                    };
                    service.Update(p);
                    MessageBox.Show("✅ Product updated!");
                    LoadProducts();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (dataGridProducts.CurrentRow != null)
            {
                int id = (int)dataGridProducts.CurrentRow.Cells["Id"].Value;
                try
                {
                    service.Delete(id);
                    MessageBox.Show("🗑️ Product deleted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    // Show friendly alert if deletion fails
                    MessageBox.Show(ex.Message, "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void dataGridProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtName.Text = dataGridProducts.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                txtCategory.Text = dataGridProducts.Rows[e.RowIndex].Cells["Category"].Value.ToString();
                txtPrice.Text = dataGridProducts.Rows[e.RowIndex].Cells["Price"].Value.ToString();
                txtCreatedAt.Text = dataGridProducts.Rows[e.RowIndex].Cells["CreatedAt"].Value.ToString();
            }
        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmDashboard dashboard = new frmDashboard();
            dashboard.ShowDialog();
            this.Close();
        }
    }
}
