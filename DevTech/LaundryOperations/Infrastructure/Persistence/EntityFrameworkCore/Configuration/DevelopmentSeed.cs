using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.Entities;
using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;
namespace DevTech.LaundryOperations.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public static class DevelopmentSeed
{
    public static readonly Guid LaundryId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static async Task InitializeAsync(AppDbContext db, CancellationToken ct = default)
    {
        // Only called explicitly with --init-db in Development, against a new database.
        // Migrations are applied by Program before seeding.
        if (!await db.Set<WashingCycle>().AnyAsync(ct))
            db.AddRange(new WashingCycle("Cotton 40 C", 60, "cotton"), new WashingCycle("Delicate", 30, "delicate"));
        if (!await db.Set<LaundryResource>().AnyAsync(ct))
            db.AddRange(new LaundryResource(new LaundryId(LaundryId), "Washer 1", 10),
                new LaundryResource(new LaundryId(LaundryId), "Washer 2", 15));
        await db.SaveChangesAsync(ct);
    }
}

