using DevTech.LaundryOperations.Domain.Model.Entities;
using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.LaundryOperations.Domain.Repositories;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
namespace DevTech.LaundryOperations.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
public class WashingCycleRepository(AppDbContext context)
    : BaseRepository<WashingCycle, WashingCycleId>(context), IWashingCycleRepository;

