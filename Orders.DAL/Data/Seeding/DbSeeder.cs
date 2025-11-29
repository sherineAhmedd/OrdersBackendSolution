using Orders.DAL.Context;
using Orders.DAL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orders.DAL.Data.Seeding
{
    public static class DbSeeder
    {
        public static void SeedData(AppDbContext context)
        {
            if (!context.Orders.Any())
            {
                context.Orders.AddRange(
                    new Order { CustomerName = "Alice", Product = "Laptop", Amount = 1500 },
                    new Order { CustomerName = "Bob", Product = "Phone", Amount = 800 },
                    new Order { CustomerName = "Charlie", Product = "Tablet", Amount = 600 }
                );
                context.SaveChanges();
            }
        }

    }
}
