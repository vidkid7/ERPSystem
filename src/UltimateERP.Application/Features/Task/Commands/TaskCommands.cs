using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Task.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.TaskModule;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Task.Commands;

// Create Task
public record CreateTaskCommand(CreateTaskDto Task) : IRequest<ApiResponse<TaskItemDto>>;

public class CreateTaskValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Task.TaskTitle).NotEmpty().MaximumLength(200);
    }
}

public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, ApiResponse<TaskItemDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateTaskHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async System.Threading.Tasks.Task<ApiResponse<TaskItemDto>> Handle(CreateTaskCommand request, CancellationToken ct)
    {
        var dto = request.Task;
        var priority = TicketPriority.Medium;
        if (!string.IsNullOrWhiteSpace(dto.Priority))
            Enum.TryParse(dto.Priority, out priority);

        var task = new TaskItem
        {
            TaskTitle = dto.TaskTitle,
            TaskDescription = dto.TaskDescription,
            TaskTypeId = dto.TaskTypeId,
            AssignedToId = dto.AssignedToId,
            AssignedById = dto.AssignedById,
            DueDate = dto.DueDate,
            DueDateBS = dto.DueDateBS,
            Priority = priority,
            Status = Domain.Enums.TaskStatus.Pending
        };

        _db.TaskItems.Add(task);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<TaskItemDto>.Success(_mapper.Map<TaskItemDto>(task), "Task created");
    }
}

// Update Task Status
public record UpdateTaskStatusCommand(UpdateTaskStatusDto Update) : IRequest<ApiResponse<TaskItemDto>>;

public class UpdateTaskStatusHandler : IRequestHandler<UpdateTaskStatusCommand, ApiResponse<TaskItemDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public UpdateTaskStatusHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async System.Threading.Tasks.Task<ApiResponse<TaskItemDto>> Handle(UpdateTaskStatusCommand request, CancellationToken ct)
    {
        var task = await _db.TaskItems.FindAsync(new object[] { request.Update.TaskId }, ct);
        if (task is null) return ApiResponse<TaskItemDto>.Failure("Task not found");

        if (!Enum.TryParse<Domain.Enums.TaskStatus>(request.Update.Status, out var status))
            return ApiResponse<TaskItemDto>.Failure($"Invalid status: {request.Update.Status}");

        task.Status = status;
        if (status == Domain.Enums.TaskStatus.Completed)
            task.CompletionDate = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        return ApiResponse<TaskItemDto>.Success(_mapper.Map<TaskItemDto>(task), "Task status updated");
    }
}

// Assign Task
public record AssignTaskCommand(AssignTaskDto Dto) : IRequest<ApiResponse<TaskItemDto>>;

public class AssignTaskValidator : AbstractValidator<AssignTaskCommand>
{
    public AssignTaskValidator()
    {
        RuleFor(x => x.Dto.TaskId).GreaterThan(0);
        RuleFor(x => x.Dto.AssignedToId).GreaterThan(0);
    }
}

public class AssignTaskHandler : IRequestHandler<AssignTaskCommand, ApiResponse<TaskItemDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public AssignTaskHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async System.Threading.Tasks.Task<ApiResponse<TaskItemDto>> Handle(AssignTaskCommand request, CancellationToken ct)
    {
        var task = await _db.TaskItems.FindAsync(new object[] { request.Dto.TaskId }, ct);
        if (task is null) return ApiResponse<TaskItemDto>.Failure("Task not found");

        task.AssignedToId = request.Dto.AssignedToId;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<TaskItemDto>.Success(_mapper.Map<TaskItemDto>(task), "Task assigned");
    }
}

// Add Task Comment
public record AddTaskCommentCommand(AddTaskCommentDto Dto) : IRequest<ApiResponse<TaskCommentDto>>;

public class AddTaskCommentValidator : AbstractValidator<AddTaskCommentCommand>
{
    public AddTaskCommentValidator()
    {
        RuleFor(x => x.Dto.TaskItemId).GreaterThan(0);
        RuleFor(x => x.Dto.Comment).NotEmpty().MaximumLength(2000);
    }
}

public class AddTaskCommentHandler : IRequestHandler<AddTaskCommentCommand, ApiResponse<TaskCommentDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public AddTaskCommentHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async System.Threading.Tasks.Task<ApiResponse<TaskCommentDto>> Handle(AddTaskCommentCommand request, CancellationToken ct)
    {
        var task = await _db.TaskItems.FindAsync(new object[] { request.Dto.TaskItemId }, ct);
        if (task is null) return ApiResponse<TaskCommentDto>.Failure("Task not found");

        var comment = new TaskComment
        {
            TaskItemId = request.Dto.TaskItemId,
            Comment = request.Dto.Comment,
            CommentedById = request.Dto.CommentedById,
            CommentDate = DateTime.UtcNow
        };

        _db.TaskComments.Add(comment);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<TaskCommentDto>.Success(_mapper.Map<TaskCommentDto>(comment), "Comment added");
    }
}
