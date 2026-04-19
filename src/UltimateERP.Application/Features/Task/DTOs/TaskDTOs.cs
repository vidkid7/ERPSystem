namespace UltimateERP.Application.Features.Task.DTOs;

public class TaskItemDto
{
    public int Id { get; set; }
    public string TaskTitle { get; set; } = string.Empty;
    public string? TaskDescription { get; set; }
    public int? TaskTypeId { get; set; }
    public int? AssignedToId { get; set; }
    public string? AssignedToName { get; set; }
    public int? AssignedById { get; set; }
    public string? AssignedByName { get; set; }
    public DateTime? DueDate { get; set; }
    public string? DueDateBS { get; set; }
    public string? Priority { get; set; }
    public string? Status { get; set; }
    public DateTime? CompletionDate { get; set; }
    public bool IsActive { get; set; }
}

public class CreateTaskDto
{
    public string TaskTitle { get; set; } = string.Empty;
    public string? TaskDescription { get; set; }
    public int? TaskTypeId { get; set; }
    public int? AssignedToId { get; set; }
    public int? AssignedById { get; set; }
    public DateTime? DueDate { get; set; }
    public string? DueDateBS { get; set; }
    public string? Priority { get; set; }
}

public class UpdateTaskStatusDto
{
    public int TaskId { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class TaskCommentDto
{
    public int Id { get; set; }
    public int TaskItemId { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int? CommentedById { get; set; }
    public DateTime CommentDate { get; set; }
    public bool IsActive { get; set; }
}

public class AssignTaskDto
{
    public int TaskId { get; set; }
    public int AssignedToId { get; set; }
}

public class AddTaskCommentDto
{
    public int TaskItemId { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int? CommentedById { get; set; }
}
