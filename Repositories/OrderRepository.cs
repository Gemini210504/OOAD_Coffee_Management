using CoffeeManagement.Models;
using CoffeeManagement.Database;
using Npgsql;
using System.Collections.Generic;

namespace CoffeeManagement.Repositories
{
    public class OrderRepository
    {
        public List<Order> GetAll()
        {
            List<Order> orders = new List<Order>();
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = @"SELECT o.id, o.product_id, p.name AS product_name, o.quantity, o.total, o.order_date
                         FROM orders o
                         JOIN products p ON o.product_id = p.id
                         ORDER BY o.id";
                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new Order
                        {
                            Id = reader.GetInt32(0),
                            ProductId = reader.GetInt32(1),
                            ProductName = reader.GetString(2),
                            Quantity = reader.GetInt32(3),
                            Total = reader.GetDecimal(4),
                            OrderDate = reader.GetDateTime(5)  // <-- read order_date
                        });
                    }
                }
            }
            return orders;
        }

        public void Add(Order order)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO orders (product_id, quantity, total) VALUES (@pid, @q, @t)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@pid", order.ProductId);
                    cmd.Parameters.AddWithValue("@q", order.Quantity);
                    cmd.Parameters.AddWithValue("@t", order.Total);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Order order)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "UPDATE orders SET product_id=@pid, quantity=@q, total=@t WHERE id=@id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@pid", order.ProductId);
                    cmd.Parameters.AddWithValue("@q", order.Quantity);
                    cmd.Parameters.AddWithValue("@t", order.Total);
                    cmd.Parameters.AddWithValue("@id", order.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM orders WHERE id=@id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
