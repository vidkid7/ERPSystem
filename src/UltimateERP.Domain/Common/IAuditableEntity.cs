namespace UltimateERP.Domain.Common;

public interface IAuditableEntity
{
    int? CreatedByUserId { get; set; }
    DateTime CreatedDate { get; set; }
    int? ModifiedBy { get; set; }
    DateTime? ModifiedDate { get; set; }
}
