using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.Queries;

namespace DevTech.LaundryOperations.Application.QueryServices;

public interface ILaundryResourceQueryService
{
    Task<IEnumerable<LaundryResource>> Handle(
        GetAllLaundryResourcesByLaundryIdQuery query,
        CancellationToken cancellationToken);
}