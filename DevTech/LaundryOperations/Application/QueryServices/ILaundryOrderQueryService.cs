using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.Queries;

namespace DevTech.LaundryOperations.Application.QueryServices;

public interface ILaundryOrderQueryService
{
    Task<LaundryOrder?> Handle(
        GetLaundryOrderByIdQuery query,
        CancellationToken cancellationToken);

    Task<IEnumerable<LaundryOrder>> Handle(
        GetAllLaundryOrdersQuery query,
        CancellationToken cancellationToken);
}
