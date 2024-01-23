using System;
using Microsoft.EntityFrameworkCore;
using Aircraft_project.Models;
using System.Collections.Generic;

namespace Aircraft_project.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Users> Users { get; set; }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Orders> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Aircraft> Aircraft { get; set; }

        public DbSet<Cart> Cart { get; set; }

        public DbSet<Contact> Contact { get; set; }
        public object ShoppingCartItems { get; internal set; }
    }
}

