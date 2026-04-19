using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Setup.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Setup;

namespace UltimateERP.Application.Features.Setup.Commands;

// ── Branch Commands ────────────────────────────────────────────────────

public record CreateBranchCommand(CreateBranchDto Branch) : IRequest<ApiResponse<BranchDto>>;

public class CreateBranchValidator : AbstractValidator<CreateBranchCommand>
{
    public CreateBranchValidator()
    {
        RuleFor(x => x.Branch.Code).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Branch.Name).NotEmpty().MaximumLength(200);
    }
}

public class CreateBranchHandler : IRequestHandler<CreateBranchCommand, ApiResponse<BranchDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateBranchHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<BranchDto>> Handle(CreateBranchCommand request, CancellationToken ct)
    {
        if (await _db.Branches.AnyAsync(b => b.Code == request.Branch.Code, ct))
            return ApiResponse<BranchDto>.Failure($"Branch code '{request.Branch.Code}' already exists");

        var entity = _mapper.Map<Branch>(request.Branch);
        _db.Branches.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<BranchDto>.Success(_mapper.Map<BranchDto>(entity), "Branch created");
    }
}

public record UpdateBranchCommand(int Id, CreateBranchDto Branch) : IRequest<ApiResponse<BranchDto>>;

public class UpdateBranchHandler : IRequestHandler<UpdateBranchCommand, ApiResponse<BranchDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public UpdateBranchHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<BranchDto>> Handle(UpdateBranchCommand request, CancellationToken ct)
    {
        var entity = await _db.Branches.FindAsync(new object[] { request.Id }, ct);
        if (entity is null) return ApiResponse<BranchDto>.Failure("Branch not found");

        if (await _db.Branches.AnyAsync(b => b.Code == request.Branch.Code && b.Id != request.Id, ct))
            return ApiResponse<BranchDto>.Failure($"Branch code '{request.Branch.Code}' already exists");

        entity.Code = request.Branch.Code;
        entity.Name = request.Branch.Name;
        entity.Alias = request.Branch.Alias;
        entity.Address = request.Branch.Address;
        entity.Phone = request.Branch.Phone;
        entity.Email = request.Branch.Email;
        entity.PAN = request.Branch.PAN;

        await _db.SaveChangesAsync(ct);
        return ApiResponse<BranchDto>.Success(_mapper.Map<BranchDto>(entity), "Branch updated");
    }
}

public record DeleteBranchCommand(int Id) : IRequest<ApiResponse<bool>>;

public class DeleteBranchHandler : IRequestHandler<DeleteBranchCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _db;

    public DeleteBranchHandler(IApplicationDbContext db) { _db = db; }

    public async Task<ApiResponse<bool>> Handle(DeleteBranchCommand request, CancellationToken ct)
    {
        var entity = await _db.Branches.FindAsync(new object[] { request.Id }, ct);
        if (entity is null) return ApiResponse<bool>.Failure("Branch not found");

        entity.IsDeleted = true;
        entity.IsActive = false;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<bool>.Success(true, "Branch deleted");
    }
}

// ── CostClass Commands ────────────────────────────────────────────────

public record CreateCostClassCommand(CreateCostClassDto CostClass) : IRequest<ApiResponse<CostClassDto>>;

public class CreateCostClassValidator : AbstractValidator<CreateCostClassCommand>
{
    public CreateCostClassValidator()
    {
        RuleFor(x => x.CostClass.Code).NotEmpty().MaximumLength(20);
        RuleFor(x => x.CostClass.Name).NotEmpty().MaximumLength(200);
    }
}

public class CreateCostClassHandler : IRequestHandler<CreateCostClassCommand, ApiResponse<CostClassDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateCostClassHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<CostClassDto>> Handle(CreateCostClassCommand request, CancellationToken ct)
    {
        if (await _db.CostClasses.AnyAsync(c => c.Code == request.CostClass.Code, ct))
            return ApiResponse<CostClassDto>.Failure($"CostClass code '{request.CostClass.Code}' already exists");

        var entity = _mapper.Map<CostClass>(request.CostClass);
        _db.CostClasses.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<CostClassDto>.Success(_mapper.Map<CostClassDto>(entity), "CostClass created");
    }
}

// ── DocumentType Commands ─────────────────────────────────────────────

public record CreateDocumentTypeCommand(CreateDocumentTypeDto DocumentType) : IRequest<ApiResponse<DocumentTypeDto>>;

public class CreateDocumentTypeValidator : AbstractValidator<CreateDocumentTypeCommand>
{
    public CreateDocumentTypeValidator()
    {
        RuleFor(x => x.DocumentType.Code).NotEmpty().MaximumLength(20);
        RuleFor(x => x.DocumentType.Name).NotEmpty().MaximumLength(200);
    }
}

public class CreateDocumentTypeHandler : IRequestHandler<CreateDocumentTypeCommand, ApiResponse<DocumentTypeDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateDocumentTypeHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<DocumentTypeDto>> Handle(CreateDocumentTypeCommand request, CancellationToken ct)
    {
        var entity = _mapper.Map<DocumentType>(request.DocumentType);
        _db.DocumentTypes.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<DocumentTypeDto>.Success(_mapper.Map<DocumentTypeDto>(entity), "DocumentType created");
    }
}

// ── EntityNumbering Commands ──────────────────────────────────────────

public record CreateEntityNumberingCommand(CreateEntityNumberingDto Numbering) : IRequest<ApiResponse<EntityNumberingDto>>;

public class CreateEntityNumberingHandler : IRequestHandler<CreateEntityNumberingCommand, ApiResponse<EntityNumberingDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateEntityNumberingHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<EntityNumberingDto>> Handle(CreateEntityNumberingCommand request, CancellationToken ct)
    {
        var entity = _mapper.Map<EntityNumbering>(request.Numbering);
        _db.EntityNumberings.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<EntityNumberingDto>.Success(_mapper.Map<EntityNumberingDto>(entity), "Entity numbering created");
    }
}
