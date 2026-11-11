using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.LaundryOperations.Domain.Repositories;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
namespace DevTech.LaundryOperations.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
public class LaundryResourceRepository(AppDbContext context)
    : BaseRepository<LaundryResource, LaundryResourceId>(context), ILaundryResourceRepository
{
    public async Task<IEnumerable<LaundryResource>> FindByLaundryIdAsync(LaundryId laundryId, CancellationToken ct)
        => await Context.Set<LaundryResource>().AsNoTracking().Where(r => r.LaundryId == laundryId).ToListAsync(ct);
}

