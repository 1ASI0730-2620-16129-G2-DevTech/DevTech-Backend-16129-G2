using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.LaundryOperations.Domain.Repositories;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
namespace DevTech.LaundryOperations.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
public class LaundryOrderRepository(AppDbContext context)
    : BaseRepository<LaundryOrder, LaundryOrderId>(context), ILaundryOrderRepository
{
    public Task<LaundryOrder?> FindByOrderIdAsync(Guid orderId, CancellationToken ct)
        => Context.Set<LaundryOrder>().FirstOrDefaultAsync(o => o.OrderId == orderId, ct);
}

