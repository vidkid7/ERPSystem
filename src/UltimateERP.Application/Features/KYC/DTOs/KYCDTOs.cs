namespace UltimateERP.Application.Features.KYC.DTOs;

public class KYCRecordDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? FatherName { get; set; }
    public string? GrandFatherName { get; set; }
    public string? SpouseName { get; set; }
    public string? District { get; set; }
    public string? Province { get; set; }
    public string? Country { get; set; }
    public string? MobileNo { get; set; }
    public string? EmailId { get; set; }
    public string? PANNo { get; set; }
    public string? CitizenshipNo { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? VerifiedDate { get; set; }
}

public class CreateKYCDto
{
    public int CustomerId { get; set; }
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? FatherName { get; set; }
    public string? GrandFatherName { get; set; }
    public string? SpouseName { get; set; }
    public string? WardNo { get; set; }
    public string? District { get; set; }
    public string? Province { get; set; }
    public string? Country { get; set; }
    public string? MobileNo { get; set; }
    public string? EmailId { get; set; }
    public string? PANNo { get; set; }
    public string? CitizenshipNo { get; set; }
    public DateTime? CitizenshipIssuedDate { get; set; }
    public string? CitizenshipIssuedDistrict { get; set; }
    public string? IssuedBy { get; set; }
}

public class UpdateKYCDto
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? FatherName { get; set; }
    public string? GrandFatherName { get; set; }
    public string? SpouseName { get; set; }
    public string? WardNo { get; set; }
    public string? District { get; set; }
    public string? Province { get; set; }
    public string? Country { get; set; }
    public string? MobileNo { get; set; }
    public string? EmailId { get; set; }
    public string? PANNo { get; set; }
    public string? CitizenshipNo { get; set; }
}

public class VerifyKYCDto
{
    public int Id { get; set; }
    public bool IsVerified { get; set; }
    public string? Remarks { get; set; }
}
