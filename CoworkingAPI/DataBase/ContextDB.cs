using CoworkingAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace CoworkingAPI.DataBase
{
    public class ContextDB : DbContext
    {
        public ContextDB(DbContextOptions<ContextDB> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Space> Spaces { get; set; }
        public DbSet<Workplace> Workplaces { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка отношений
                modelBuilder.Entity<Booking>()
                    .HasOne(b => b.User)
                    .WithMany(u => u.Bookings)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.NoAction); // Изменено на NoAction

                modelBuilder.Entity<Booking>()
                    .HasOne(b => b.Workplace)
                    .WithMany(w => w.Bookings)
                    .HasForeignKey(b => b.WorkplaceId)
                    .OnDelete(DeleteBehavior.NoAction);
                modelBuilder.Entity<User>()
                    .HasMany(u => u.OwnedSpaces)
                    .WithOne(s => s.Owner)
                    .HasForeignKey(s => s.OwnerId)
                    .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<User>()
                .HasMany(u => u.OwnedSpaces)
                .WithOne(s => s.Owner)
                .HasForeignKey(s => s.OwnerId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Bookings)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Space>()
                .HasMany(s => s.Workplaces)
                .WithOne(w => w.Space)
                .HasForeignKey(w => w.SpaceId);

            modelBuilder.Entity<Workplace>()
                .HasMany(w => w.Bookings)
                .WithOne(b => b.Workplace)
                .HasForeignKey(b => b.WorkplaceId);

            // Инициализация ролей
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            );
        }
    }
}
