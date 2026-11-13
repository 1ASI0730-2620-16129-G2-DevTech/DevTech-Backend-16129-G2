using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using DevTech.OrderManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using DevTech.PickupsDeliveries.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;
namespace DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddInterceptors(new DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors.AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyLaundryOperationsConfiguration();
        builder.ApplyOrderManagementConfiguration();
        builder.ApplyPickupsDeliveriesConfiguration();
        builder.UseSnakeCaseNamingConvention();
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<LaundryOrder>().Where(x => x.State == EntityState.Modified))
            entry.Entity.RefreshVersion();
        foreach (var entry in ChangeTracker.Entries<LaundryResource>().Where(x => x.State == EntityState.Modified))
            entry.Entity.RefreshVersion();
        return base.SaveChangesAsync(cancellationToken);
    }
}

