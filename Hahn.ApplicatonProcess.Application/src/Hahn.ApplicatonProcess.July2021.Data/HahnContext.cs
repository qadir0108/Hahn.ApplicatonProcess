using Hahn.ApplicatonProcess.July2021.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.July2021.Data
{
    public class HahnContext : DbContext
    {

        public HahnContext() { }

        public HahnContext(DbContextOptions<HahnContext> options) : base(options)
        {
            // To save time;
            // After working with UseSqlServer, I found, had to UseInMemoryDatabase
            this.Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Asset> Assets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(p => p.Assets)
                .WithMany(p => p.Users);
        }
    }
}
