using Microsoft.EntityFrameworkCore;
using Orders.DAL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orders.DAL.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }

    }
}
