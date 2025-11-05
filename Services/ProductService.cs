using CoffeeManagement.Models;
using CoffeeManagement.Repositories;
using System.Collections.Generic;

namespace CoffeeManagement.Services
{
    public class ProductService
    {
        private readonly ProductRepository _repo = new ProductRepository();

        public List<Product> GetAll() => _repo.GetAll();

        public void Add(Product p)
        {
            if (string.IsNullOrWhiteSpace(p.Name))
                throw new System.Exception("Name cannot be empty.");
            if (p.Price <= 0)
                throw new System.Exception("Price must be positive.");

            _repo.Add(p);
        }

        public void Update(Product p)
        {
            if (p.Id <= 0)
                throw new System.Exception("Invalid ID.");
            _repo.Update(p);
        }

        public void Delete(int id)
        {
            if (id <= 0)
                throw new System.Exception("Invalid ID.");

            try
            {
                _repo.Delete(id);
            }
            catch (Npgsql.PostgresException ex) when (ex.SqlState == "23503")
            {
                // 23503 = foreign key violation
                throw new System.Exception("Cannot delete product. There are orders linked to it.");
            }
        }
    }
}
