using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Account;

namespace UltimateERP.Domain.Entities.KYC;

public class KYCRecord : BaseEntity
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
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
    public string? CitizenshipFrontPhoto { get; set; }
    public string? CitizenshipBackPhoto { get; set; }
    public string? PANCardPhoto { get; set; }
    public string? ProfilePhoto { get; set; }
    public bool IsVerified { get; set; }
    public int? VerifiedBy { get; set; }
    public DateTime? VerifiedDate { get; set; }
}
