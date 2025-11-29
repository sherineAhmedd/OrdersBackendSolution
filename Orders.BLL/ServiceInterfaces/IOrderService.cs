

using Orders.BLL.DTOs;
using Orders.DAL.Data.Entities;

namespace Orders.BLL.ServiceInterfaces
{
    public interface IOrderService
    {
        Task<OrderReadDto> CreateOrderAsync(CreateOrderDto order);
        Task<OrderReadDto?> GetOrderByIdAsync(Guid id);
        Task<List<OrderReadDto>> GetAllOrdersAsync();
        Task<bool> DeleteOrderAsync(Guid id);
    }
}
