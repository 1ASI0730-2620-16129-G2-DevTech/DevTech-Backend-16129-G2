using DevTech.LaundryOperations.Application.QueryServices;
using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.Queries;
using DevTech.LaundryOperations.Domain.Repositories;
namespace DevTech.LaundryOperations.Application.Internal.QueryServices;

public class LaundryResourceQueryService(ILaundryResourceRepository repository) : ILaundryResourceQueryService
{
    public Task<IEnumerable<LaundryResource>> Handle(GetAllLaundryResourcesByLaundryIdQuery query, CancellationToken ct)
        => repository.FindByLaundryIdAsync(query.LaundryId, ct);
}

