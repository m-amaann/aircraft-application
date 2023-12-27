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

        public DbSet<Users> User { get; set; }
    }
}

