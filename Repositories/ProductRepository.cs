using CoffeeManagement.Models;
using CoffeeManagement.Database;
using Npgsql;
using System.Collections.Generic;

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
                string query = "SELECT * FROM products ORDER BY id";
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
                            Price = reader.GetDecimal(3)
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
