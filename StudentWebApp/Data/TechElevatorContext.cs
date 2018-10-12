using Microsoft.EntityFrameworkCore;
using TechElevator.Models;
using System;

namespace TechElevator.Data
{
    public class TechElevatorContext : DbContext
    {
        public TechElevatorContext(DbContextOptions<TechElevatorContext> options) : base(options)
        {
            
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Track> Tracks { get; set; }

        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
