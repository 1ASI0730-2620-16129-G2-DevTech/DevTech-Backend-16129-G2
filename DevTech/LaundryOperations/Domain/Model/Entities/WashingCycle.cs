namespace DevTech.LaundryOperations.Domain.Model.Entities;

public record WashingCycle(Guid Id)
{
        public WashingCycle() : this(Guid.NewGuid())
        {
        }
}