using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCDApi.DataBase.Entity;

namespace WCDApi.DataBase.Data
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
            
        }

        public DbSet<MonitoredItem> MonitoredItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MonitoredHistoryItem> MonitoredHistory { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MonitoredHistoryItem>().ToTable("MonitoredHistory");
            modelBuilder.Entity<MonitoredItem>().ToTable("MonitoredItem");
            modelBuilder.Entity<User>().ToTable("Users");
        }

    }
}
