namespace UltimateERP.Application.Features.Account.Services;

/// <summary>
/// Validates that voucher debit and credit totals balance.
/// Core accounting rule: Total Debits = Total Credits.
/// </summary>
public static class VoucherValidator
{
    public static (bool IsValid, string? Error) ValidateBalance(decimal totalDebit, decimal totalCredit)
    {
        if (totalDebit <= 0 && totalCredit <= 0)
            return (false, "Voucher must have at least one debit or credit entry");

        if (Math.Abs(totalDebit - totalCredit) > 0.01m)
            return (false, $"Voucher is not balanced. Debit: {totalDebit}, Credit: {totalCredit}, Difference: {Math.Abs(totalDebit - totalCredit)}");

        return (true, null);
    }

    public static string GenerateVoucherNumber(string prefix, int currentNumber, int width = 6)
    {
        return $"{prefix}{currentNumber.ToString().PadLeft(width, '0')}";
    }
}
