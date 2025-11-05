using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;


namespace CoffeeManagement.Database
{
    public class DatabaseConnection
    {
        private static readonly string connectionString =
            "Host=localhost;Port=5432;Username=postgres;Password=1127;Database=coffeedb;";

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
