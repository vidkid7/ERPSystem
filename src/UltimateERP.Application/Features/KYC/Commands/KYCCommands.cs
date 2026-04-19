using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.KYC.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.KYC;

namespace UltimateERP.Application.Features.KYC.Commands;

public record CreateKYCCommand(CreateKYCDto KYC) : IRequest<ApiResponse<KYCRecordDto>>;

public class CreateKYCHandler : IRequestHandler<CreateKYCCommand, ApiResponse<KYCRecordDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateKYCHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<KYCRecordDto>> Handle(CreateKYCCommand request, CancellationToken ct)
    {
        var customer = await _db.Customers.FindAsync(new object[] { request.KYC.CustomerId }, ct);
        if (customer is null) return ApiResponse<KYCRecordDto>.Failure("Customer not found");

        if (await _db.KYCRecords.AnyAsync(k => k.CustomerId == request.KYC.CustomerId, ct))
            return ApiResponse<KYCRecordDto>.Failure("KYC record already exists for this customer");

        var entity = _mapper.Map<KYCRecord>(request.KYC);
        _db.KYCRecords.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<KYCRecordDto>.Success(_mapper.Map<KYCRecordDto>(entity), "KYC record created");
    }
}

public record UpdateKYCCommand(UpdateKYCDto KYC) : IRequest<ApiResponse<KYCRecordDto>>;

public class UpdateKYCHandler : IRequestHandler<UpdateKYCCommand, ApiResponse<KYCRecordDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public UpdateKYCHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<KYCRecordDto>> Handle(UpdateKYCCommand request, CancellationToken ct)
    {
        var entity = await _db.KYCRecords.FindAsync(new object[] { request.KYC.Id }, ct);
        if (entity is null) return ApiResponse<KYCRecordDto>.Failure("KYC record not found");

        if (entity.IsVerified)
            return ApiResponse<KYCRecordDto>.Failure("Cannot update a verified KYC record");

        _mapper.Map(request.KYC, entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<KYCRecordDto>.Success(_mapper.Map<KYCRecordDto>(entity), "KYC record updated");
    }
}

public record VerifyKYCCommand(VerifyKYCDto Verification) : IRequest<ApiResponse<KYCRecordDto>>;

public class VerifyKYCHandler : IRequestHandler<VerifyKYCCommand, ApiResponse<KYCRecordDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public VerifyKYCHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<KYCRecordDto>> Handle(VerifyKYCCommand request, CancellationToken ct)
    {
        var entity = await _db.KYCRecords.FindAsync(new object[] { request.Verification.Id }, ct);
        if (entity is null) return ApiResponse<KYCRecordDto>.Failure("KYC record not found");

        entity.IsVerified = request.Verification.IsVerified;
        entity.VerifiedDate = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<KYCRecordDto>.Success(_mapper.Map<KYCRecordDto>(entity),
            request.Verification.IsVerified ? "KYC verified" : "KYC verification rejected");
    }
}
