using Orders.BLL.DTOs;
using Orders.BLL.ServiceInterfaces;
using Orders.DAL.Data.Entities;
using Orders.DAL.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orders.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICacheService _cache;
        public OrderService(IOrderRepository orderRepository , ICacheService cache)
        {
            _orderRepository = orderRepository;
            _cache = cache;
        }
        public async Task<OrderReadDto> CreateOrderAsync(CreateOrderDto dto)
        {
            // Map DTO to Entity
            var order = new Order
            {
                CustomerName = dto.CustomerName,
                Product = dto.Product,
                Amount = dto.Amount
            };

            var createdOrder = await _orderRepository.AddAsync(order);

            // Map Entity to Read DTO
            var readDto = new OrderReadDto
            {
                OrderId = createdOrder.OrderId,
                CustomerName = createdOrder.CustomerName,
                Product = createdOrder.Product,
                Amount = createdOrder.Amount,
                CreatedAt = createdOrder.CreatedAt
            };
            return readDto;


        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            var deleted = await _orderRepository.DeleteAsync(id);
            if(deleted)
            {
                string key = $"order:{id}";
                await _cache.RemoveAsync(key); // remove from Redis 
            }
            return deleted;
        }

        public async Task<List<OrderReadDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            // Map Entity to DTO
            var orderDtos = orders.Select(o => new OrderReadDto
            {
                OrderId = o.OrderId,
                CustomerName = o.CustomerName,
                Product = o.Product,
                Amount = o.Amount,
                CreatedAt = o.CreatedAt
            }).ToList();

            return orderDtos;
        }

        public async Task<OrderReadDto?> GetOrderByIdAsync(Guid id)
        {
            string key = $"order:{id}";
            //check first have in cached or no
            var cachedOrder = await _cache.GetAsync<OrderReadDto>(key);
            if (cachedOrder != null) return cachedOrder; //yes found 

            //if not will get from db 
            var order = await _orderRepository.GetByIdAsync(id);
            if (order != null)
            {
                await _cache.SetAsync(key, order, TimeSpan.FromMinutes(5)); //store it in redis cache 
                return new OrderReadDto
                {
                    OrderId = order.OrderId,
                    CustomerName = order.CustomerName,
                    Product = order.Product,
                    Amount = order.Amount,
                    CreatedAt = order.CreatedAt
                };
            }
            return null;

           
        }
    }
}
