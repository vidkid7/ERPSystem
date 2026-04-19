using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.TaskModule;

public class TaskComment : BaseEntity
{
    public int TaskItemId { get; set; }
    public TaskItem TaskItem { get; set; } = null!;
    public string Comment { get; set; } = string.Empty;
    public int? CommentedById { get; set; }
    public DateTime CommentDate { get; set; } = DateTime.UtcNow;
}
