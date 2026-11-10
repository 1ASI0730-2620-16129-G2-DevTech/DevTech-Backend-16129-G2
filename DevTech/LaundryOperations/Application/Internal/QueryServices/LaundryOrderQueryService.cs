using DevTech.LaundryOperations.Application.QueryServices;
using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.Queries;
using DevTech.LaundryOperations.Domain.Repositories;

namespace DevTech.LaundryOperations.Application.Internal.QueryServices;

public class LaundryOrderQueryService(
    ILaundryOrderRepository laundryOrderRepository)
    : ILaundryOrderQueryService
{
    public async Task<LaundryOrder?> Handle(
        GetLaundryOrderByIdQuery query,
        CancellationToken cancellationToken)
    {
        return await laundryOrderRepository.FindByIdAsNoTrackingAsync(
            query.LaundryOrderId,
            cancellationToken);
    }

    public async Task<IEnumerable<LaundryOrder>> Handle(
        GetAllLaundryOrdersQuery query,
        CancellationToken cancellationToken)
    {
        return await laundryOrderRepository.ListAsNoTrackingAsync(
            cancellationToken);
    }
}
