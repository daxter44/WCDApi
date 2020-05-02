using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCDApi.Entity;

namespace WCDApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext() 
        {
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=Marcinek44;database=WCD");
            }
        }

        public DbSet<MonitoredItem> MonitoredItems { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MonitoredItem>().ToTable("MonitoredItem");
            modelBuilder.Entity<User>().ToTable("Users");
        }

    }
}
