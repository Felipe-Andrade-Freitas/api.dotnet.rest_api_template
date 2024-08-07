namespace Domain.Common.Contracts;

public abstract class AuditableEntity : AuditableEntity<Guid>
{
    public AuditableEntity()
    {
        Id = Guid.NewGuid();
    }
}

public abstract class AuditableEntity<T> : BaseEntity<T>, ISoftDelete
{
    public DateTime? DeletedOn { get; set; }
    public Guid? DeletedBy { get; set; }
}