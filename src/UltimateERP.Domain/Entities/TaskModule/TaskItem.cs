using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Security;

namespace UltimateERP.Domain.Entities.TaskModule;

public class TaskItem : BaseEntity
{
    public string TaskTitle { get; set; } = string.Empty;
    public string? TaskDescription { get; set; }
    public int? TaskTypeId { get; set; }
    public int? AssignedToId { get; set; }
    public User? AssignedTo { get; set; }
    public int? AssignedById { get; set; }
    public User? AssignedBy { get; set; }
    public DateTime? DueDate { get; set; }
    public string? DueDateBS { get; set; }
    public Enums.TicketPriority Priority { get; set; }
    public Enums.TaskStatus Status { get; set; }
    public DateTime? CompletionDate { get; set; }
    public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
}
