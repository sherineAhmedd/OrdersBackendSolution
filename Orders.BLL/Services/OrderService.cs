using Microsoft.Extensions.Logging;
using Orders.BLL.DTOs;
using Orders.BLL.Exceptions;
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
        private readonly ILogger<OrderService> _logger;
        public OrderService(IOrderRepository orderRepository , ICacheService cache ,ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _cache = cache;
            _logger = logger;
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
            _logger.LogInformation("Creating order for customer {CustomerName} with product {Product}", dto.CustomerName, dto.Product);

            var createdOrder = await _orderRepository.AddAsync(order);
            _logger.LogInformation("Order {OrderId} created successfully", createdOrder.OrderId);


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
            _logger.LogInformation("Attempting to delete order {OrderId}", id);
            var deleted = await _orderRepository.DeleteAsync(id);
            if (!deleted)
            {
                throw new NotFoundException($"Order with id {id} not found.");
            }
                string key = $"order:{id}";
                await _cache.RemoveAsync(key); // remove from Redis 
            _logger.LogInformation("Order {OrderId} deleted and cache removed", id);
            return true;
         }
            
       

        public async Task<List<OrderReadDto>> GetAllOrdersAsync()
        {
            _logger.LogInformation("Fetching all orders from database.");
            var orders = await _orderRepository.GetAllAsync();
            _logger.LogInformation("{Count} orders retrieved", orders.Count);

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
            if (cachedOrder != null)
            {
                _logger.LogInformation("Cache hit for order {OrderId}", id);
                return cachedOrder; //yes found 
            }
            _logger.LogInformation("Cache miss for order {OrderId}. Fetching from DB.", id);
            //if not will get from db 
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                throw new NotFoundException($"Order with id {id} not found.");
            }
            await _cache.SetAsync(key, order, TimeSpan.FromMinutes(5)); //store it in redis cache 
            _logger.LogInformation("Order {OrderId} cached for 5 minutes", id);
            return new OrderReadDto
                {
                    OrderId = order.OrderId,
                    CustomerName = order.CustomerName,
                    Product = order.Product,
                    Amount = order.Amount,
                    CreatedAt = order.CreatedAt
                };
        }
    }
}
