using BattleShipGame.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BattleShipGame.Database;

public class BattleShipGameContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public BattleShipGameContext(DbContextOptions<BattleShipGameContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(user => user.Login).IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}