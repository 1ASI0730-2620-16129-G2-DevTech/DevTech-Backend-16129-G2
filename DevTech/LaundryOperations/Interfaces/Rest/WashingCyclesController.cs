using DevTech.LaundryOperations.Application.CommandServices;
using DevTech.LaundryOperations.Application.QueryServices;
using DevTech.LaundryOperations.Domain.Model.Commands;
using DevTech.LaundryOperations.Domain.Model.Queries;
using DevTech.LaundryOperations.Domain.Model.Errors;
using DevTech.LaundryOperations.Interfaces.Rest.Resources;
using DevTech.LaundryOperations.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;
namespace DevTech.LaundryOperations.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Tags("Washing cycles")]
public class WashingCyclesController(IWashingCycleQueryService queryService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get washing cycles, optionally filtered by compatible type")]
    [ProducesResponseType(typeof(IEnumerable<WashingCycleResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? compatibleType, CancellationToken cancellationToken)
    {
        var cycles = await queryService.Handle(new GetAllWashingCyclesQuery(), cancellationToken);
        return Ok(cycles.Where(cycle => string.IsNullOrWhiteSpace(compatibleType) ||
            cycle.CompatibleType.Equals(compatibleType, StringComparison.OrdinalIgnoreCase))
            .Select(WashingCycleResourceFromEntityAssembler.ToResourceFromEntity));
    }
}

