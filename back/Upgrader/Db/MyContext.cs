using Microsoft.EntityFrameworkCore;
using Upgrader.Users;

public class MyContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DbSet<User> Users { get; set; }
    public DbSet<TgProfile> TgProfiles { get; set; }

    public MyContext(DbContextOptions<MyContext> options, IConfiguration configuration)
        : base(options)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TgProfile>(entity =>
        {
            entity.HasKey(tp => tp.TelegramId);

            entity.HasOne<User>()
                .WithOne(u => u.TgProfile)
                .HasForeignKey<TgProfile>(tp => tp.TelegramId)
                .HasPrincipalKey<User>(u => u.TelegramId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<User>()
            .HasIndex(u => u.TelegramId)
            .IsUnique();
    }
}