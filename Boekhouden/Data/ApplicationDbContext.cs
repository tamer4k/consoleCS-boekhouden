using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace Boekhouden.Data
{
    internal class ApplicationDbContext : DbContext
    {
        public DbSet<Invoice> Invoice { get; set; }

        public DbSet<CustomerDiscount> CustomerDiscount { get; set; }

        public DbSet<TransactionRow> TransactionRow { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Boekhouden;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
