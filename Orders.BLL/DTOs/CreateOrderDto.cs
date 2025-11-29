using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orders.BLL.DTOs
{
    public class CreateOrderDto
    {
        [Required(ErrorMessage = "Customer name is required")]
        public string CustomerName { get; set; } = null!;

        [Required(ErrorMessage = "Product is required")]
        public string Product { get; set; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
    }
}
