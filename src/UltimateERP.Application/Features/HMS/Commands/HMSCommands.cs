using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.HMS.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.HMS;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.HMS.Commands;

// Register Patient
public record RegisterPatientCommand(CreatePatientDto Patient) : IRequest<ApiResponse<PatientDto>>;

public class RegisterPatientValidator : AbstractValidator<RegisterPatientCommand>
{
    public RegisterPatientValidator()
    {
        RuleFor(x => x.Patient.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Patient.LastName).NotEmpty().MaximumLength(100);
    }
}

public class RegisterPatientHandler : IRequestHandler<RegisterPatientCommand, ApiResponse<PatientDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public RegisterPatientHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<PatientDto>> Handle(RegisterPatientCommand request, CancellationToken ct)
    {
        var dto = request.Patient;
        var patient = new Patient
        {
            PatientNumber = $"PAT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            Address = dto.Address,
            City = dto.City,
            Phone = dto.Phone,
            Email = dto.Email,
            RegistrationDate = DateTime.UtcNow
        };

        _db.Patients.Add(patient);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<PatientDto>.Success(_mapper.Map<PatientDto>(patient), "Patient registered");
    }
}

// Create OPD Ticket
public record CreateOPDTicketCommand(CreateOPDTicketDto Ticket) : IRequest<ApiResponse<OPDTicketDto>>;

public class CreateOPDTicketHandler : IRequestHandler<CreateOPDTicketCommand, ApiResponse<OPDTicketDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateOPDTicketHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<OPDTicketDto>> Handle(CreateOPDTicketCommand request, CancellationToken ct)
    {
        var dto = request.Ticket;
        var ticket = new OPDTicket
        {
            TicketNumber = $"OPD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            TicketDate = DateTime.UtcNow,
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            Symptoms = dto.Symptoms,
            Amount = dto.Amount,
            Status = "Open"
        };

        _db.OPDTickets.Add(ticket);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<OPDTicketDto>.Success(_mapper.Map<OPDTicketDto>(ticket), "OPD ticket created");
    }
}

// Create IPD Admission
public record CreateIPDAdmissionCommand(CreateIPDAdmissionDto Admission) : IRequest<ApiResponse<IPDAdmissionDto>>;

public class CreateIPDAdmissionHandler : IRequestHandler<CreateIPDAdmissionCommand, ApiResponse<IPDAdmissionDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateIPDAdmissionHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<IPDAdmissionDto>> Handle(CreateIPDAdmissionCommand request, CancellationToken ct)
    {
        var dto = request.Admission;

        // If bed specified, check availability and mark as occupied
        if (dto.BedId.HasValue)
        {
            var bed = await _db.Beds.FindAsync(new object[] { dto.BedId.Value }, ct);
            if (bed is null) return ApiResponse<IPDAdmissionDto>.Failure("Bed not found");
            if (bed.Status != BedStatus.Available) return ApiResponse<IPDAdmissionDto>.Failure("Bed is not available");
            bed.Status = BedStatus.Occupied;
        }

        var admission = new IPDAdmission
        {
            AdmissionNumber = $"IPD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            AdmissionDate = DateTime.UtcNow,
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            BedId = dto.BedId,
            Diagnosis = dto.Diagnosis,
            Status = IPDStatus.Admitted
        };

        _db.IPDAdmissions.Add(admission);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<IPDAdmissionDto>.Success(_mapper.Map<IPDAdmissionDto>(admission), "Patient admitted");
    }
}

// Discharge Patient
public record DischargePatientCommand(int AdmissionId, int? DischargeTypeId) : IRequest<ApiResponse<IPDAdmissionDto>>;

public class DischargePatientHandler : IRequestHandler<DischargePatientCommand, ApiResponse<IPDAdmissionDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public DischargePatientHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<IPDAdmissionDto>> Handle(DischargePatientCommand request, CancellationToken ct)
    {
        var admission = await _db.IPDAdmissions.Include(a => a.Bed)
            .FirstOrDefaultAsync(a => a.Id == request.AdmissionId, ct);
        if (admission is null) return ApiResponse<IPDAdmissionDto>.Failure("Admission not found");

        admission.Status = IPDStatus.Discharged;
        admission.DischargeDate = DateTime.UtcNow;
        admission.DischargeTypeId = request.DischargeTypeId;

        // Free up bed
        if (admission.Bed is not null)
            admission.Bed.Status = BedStatus.Available;

        await _db.SaveChangesAsync(ct);
        return ApiResponse<IPDAdmissionDto>.Success(_mapper.Map<IPDAdmissionDto>(admission), "Patient discharged");
    }
}
