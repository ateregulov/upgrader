using Microsoft.EntityFrameworkCore;
using Upgrader.Features.Courses;
using Upgrader.Features.ReferralSystem;
using Upgrader.Features.Tasks;
using Upgrader.Features.Transactions;
using Upgrader.Users;

public class MyContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DbSet<User> Users { get; set; }
    public DbSet<TgProfile> TgProfiles { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Upgrader.Features.Tasks.Task> Tasks { get; set; }
    public DbSet<TaskResult> TaskResults { get; set; }
    public DbSet<TaskResultImage> TaskResultsImages { get; set; }
    public DbSet<CoursePurchase> CoursePurchases { get; set; }
    public DbSet<Referral> Referrals { get; set; }
    public DbSet<RefCode> RefCodes { get; set; }

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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyContext).Assembly);

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