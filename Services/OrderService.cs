using CoffeeManagement.Models;
using CoffeeManagement.Repositories;
using System;
using System.Collections.Generic;

namespace CoffeeManagement.Services
{
    public class OrderService
    {
        private readonly OrderRepository _repo = new OrderRepository();

        public List<Order> GetAll() => _repo.GetAll();

        public void Add(Order order)
        {
            if (order.Quantity <= 0)
                throw new Exception("Quantity must be greater than 0.");
            _repo.Add(order);
        }

        public void Update(Order order)
        {
            if (order.Id <= 0)
                throw new Exception("Invalid Order ID.");
            _repo.Update(order);
        }

        public void Delete(int id)
        {
            if (id <= 0)
                throw new Exception("Invalid Order ID.");
            _repo.Delete(id);
        }
    }
}
