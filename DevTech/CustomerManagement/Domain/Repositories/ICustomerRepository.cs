using DevTech.CustomerManagement.Domain.Model.Aggregates;
using DevTech.CustomerManagement.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Repositories;

namespace DevTech.CustomerManagement.Domain.Repositories;

/// <summary>
///     Represents the Customer repository in the application. 
/// </summary>
/// <remarks>
///     Extends <see cref="IBaseRepository{TEntity,TKey}" /> for the <see cref="Customer" /> entity,
///     using <see cref="CustomerId" /> as its key.
/// </remarks>
public interface ICustomerRepository : IBaseRepository<Customer, CustomerId>
{
    /// <summary>
    ///     Finds a customer by its document number.
    /// </summary>
    /// <param name="document">
    ///     The document number of the customer to find.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    ///     A task that resolves to the matching <see cref="Customer" />,
    /// </returns>
    Task<Customer?> FindByDocumentAsync(DocumentNumber document, CancellationToken cancellationToken);
    
    /// <summary>
    ///     Finds a customer by its user id.
    /// </summary>
    /// <param name="userId">
    ///     The user id of the customer to find.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    ///     A task that resolves to the matching <see cref="Customer" />,
    /// </returns>
    Task<Customer?> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}