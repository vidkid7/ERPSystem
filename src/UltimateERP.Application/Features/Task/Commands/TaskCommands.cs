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
