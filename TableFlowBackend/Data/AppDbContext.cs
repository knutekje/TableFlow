using Microsoft.EntityFrameworkCore;
using TableFlowBackend.Models;

namespace TableFlowBackend.Data;

public class AppDbContext : DbContext
{
    public DbSet<Table> Tables { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Waitlist> Waitlists { get; set; } 

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed data or custom configurations
    }
}