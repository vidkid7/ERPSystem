namespace UltimateERP.Domain.Common;

public abstract class AuditableEntity : BaseEntity, IAuditableEntity
{
    public int? CreatedByUserId { get; set; }
    public int? ModifiedByUserId { get; set; }

    int? IAuditableEntity.ModifiedBy
    {
        get => ModifiedByUserId;
        set => ModifiedByUserId = value;
    }
}
