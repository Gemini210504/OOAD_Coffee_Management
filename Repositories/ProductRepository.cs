using CoffeeManagement.Database;
using CoffeeManagement.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CoffeeManagement.Repositories
{
    public class ProductRepository
    {
        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT id, name, category, price, created_at FROM products ORDER BY id";
                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Category = reader.GetString(2),
                            Price = reader.GetDecimal(3),
                            CreatedAt = reader.GetDateTime(4) // <-- read created_at here
                        });
                    }
                }
            }
            return products;
        }

        public void Add(Product product)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                // Don't include created_at here, PostgreSQL will set it automatically
                string query = "INSERT INTO products (name, category, price) VALUES (@n, @c, @p)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@n", product.Name);
                    cmd.Parameters.AddWithValue("@c", product.Category);
                    cmd.Parameters.AddWithValue("@p", product.Price);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Product product)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "UPDATE products SET name=@n, category=@c, price=@p WHERE id=@id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@n", product.Name);
                    cmd.Parameters.AddWithValue("@c", product.Category);
                    cmd.Parameters.AddWithValue("@p", product.Price);
                    cmd.Parameters.AddWithValue("@id", product.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                // Check if any orders exist for this product
                string checkQuery = "SELECT COUNT(*) FROM orders WHERE product_id=@id";
                using (var checkCmd = new NpgsqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@id", id);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                        throw new Exception("Cannot delete product. There are orders linked to it.");
                }

                // Delete product if no orders
                string query = "DELETE FROM products WHERE id=@id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
