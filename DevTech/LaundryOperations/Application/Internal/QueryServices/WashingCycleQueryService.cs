using DevTech.LaundryOperations.Application.QueryServices;
using DevTech.LaundryOperations.Domain.Model.Entities;
using DevTech.LaundryOperations.Domain.Model.Queries;
using DevTech.LaundryOperations.Domain.Repositories;
namespace DevTech.LaundryOperations.Application.Internal.QueryServices;

public class WashingCycleQueryService(IWashingCycleRepository repository) : IWashingCycleQueryService
{
    public Task<IEnumerable<WashingCycle>> Handle(GetAllWashingCyclesQuery query, CancellationToken ct)
        => repository.ListAsNoTrackingAsync(ct);
}

