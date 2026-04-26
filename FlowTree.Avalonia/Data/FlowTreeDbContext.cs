using FlowTree.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTree.Avalonia.Data
{
    public class FlowTreeDbContext : DbContext
    {
        private readonly string _connectionString;
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<CardContainer> CardContainers { get; set; }
        public FlowTreeDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        // Connects to the database using the provided connection string.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        // Seeds the database with initial data, in this case, a default app setting.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppSetting>().HasData(
                new AppSetting
                {
                    Id = 1,
                    Key = "CreatedBy",
                    Value = "FlowTree"
                }
            );
        }

    }
}
