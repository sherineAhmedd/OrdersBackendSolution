

namespace Orders.BLL.DTOs
{
    public class OrderReadDto
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string Product { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
