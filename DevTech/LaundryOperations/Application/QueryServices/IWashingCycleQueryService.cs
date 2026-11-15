using DevTech.LaundryOperations.Domain.Model.Entities;
using DevTech.LaundryOperations.Domain.Model.Queries;

namespace DevTech.LaundryOperations.Application.QueryServices;

public interface IWashingCycleQueryService
{
    Task<IEnumerable<WashingCycle>> Handle(
        GetAllWashingCyclesQuery query,
        CancellationToken cancellationToken);
}
