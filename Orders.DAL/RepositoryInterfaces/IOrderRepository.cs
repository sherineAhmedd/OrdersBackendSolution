using Orders.DAL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orders.DAL.RepositoryInterfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(Guid id);
        Task<Order> AddAsync(Order order);
        Task<bool> DeleteAsync(Guid id);
    }
}
