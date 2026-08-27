using CustomerSubscription.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerSubscription.Dal.Dal
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

        public DbSet<CustomerSubscriptionPlan> CustomerSubscriptionPlans { get; set; }

        public DbSet<PlanChangeHistory> PlanChangeHistories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");
                entity.HasKey(c => c.Id);         
            });

            modelBuilder.Entity<SubscriptionPlan>(entity =>
            {
                entity.ToTable("SubscriptionPlan");
                entity.HasKey(c => c.Id);        
            });

            modelBuilder.Entity<CustomerSubscriptionPlan>(entity =>
            {
                entity.ToTable("CustomerSubscriptionPlan");
                entity.HasKey(c => c.Id);
            });

            modelBuilder.Entity<PlanChangeHistory>()
            .ToTable("PlanChangeHistory");
        }
    }
}
