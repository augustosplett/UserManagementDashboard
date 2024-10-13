using Microsoft.EntityFrameworkCore;
using UserManagementDashboard.Models;

namespace UserManagementDashboard.Data;

public class UserManagementDashboardContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}

