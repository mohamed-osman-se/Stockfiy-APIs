
using Api.Models;
using Api.Modles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<AppUser>
{


    public DbSet<Stock> Stocks { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Portfolio> portfolios { get; set; } = null!;
    public DbSet<PasswordResetCode> passwordResetCodes { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Portfolio>(p => p.HasKey(p => new { p.AppUserId, p.StockId }));

        builder.Entity<Portfolio>().
        HasOne(u => u.appUser).
        WithMany(u => u.portfolios).
        HasForeignKey(p => p.AppUserId);


        builder.Entity<Portfolio>().
        HasOne(u => u.stock).
        WithMany(u => u.portfolios).
        HasForeignKey(p => p.StockId);

        List<IdentityRole> roles = new List<IdentityRole>
    {
        new IdentityRole
        {
            Id = "1",
            Name = "Admin",
            NormalizedName = "ADMIN"
        },
        new IdentityRole
        {
            Id = "2",
            Name = "User",
            NormalizedName = "USER"
        }
    };

        builder.Entity<IdentityRole>().HasData(roles);
    }


}