namespace UltimateERP.Application.Features.Finance.Services;

public static class EMICalculationService
{
    public record EMIScheduleItem(
        int EMINumber,
        DateTime DueDate,
        decimal EMIAmount,
        decimal PrincipalComponent,
        decimal InterestComponent,
        decimal OutstandingBalance);

    public static decimal CalculateEMI(decimal principal, decimal annualRate, int tenureMonths)
    {
        if (annualRate == 0)
            return Math.Round(principal / tenureMonths, 2);

        var r = annualRate / 12 / 100;
        var pow = (decimal)Math.Pow((double)(1 + r), tenureMonths);
        var emi = principal * r * pow / (pow - 1);
        return Math.Round(emi, 2);
    }

    public static List<EMIScheduleItem> GenerateSchedule(
        decimal principal, decimal annualRate, int tenureMonths, DateTime startDate)
    {
        var emi = CalculateEMI(principal, annualRate, tenureMonths);
        var r = annualRate / 12 / 100;
        var outstanding = principal;
        var schedule = new List<EMIScheduleItem>();

        for (int i = 1; i <= tenureMonths; i++)
        {
            var interest = Math.Round(outstanding * r, 2);
            var principalPart = emi - interest;

            if (i == tenureMonths)
            {
                principalPart = outstanding;
                interest = Math.Round(outstanding * r, 2);
                emi = principalPart + interest;
                outstanding = 0;
            }
            else
            {
                outstanding -= principalPart;
            }

            schedule.Add(new EMIScheduleItem(
                i,
                startDate.AddMonths(i),
                Math.Round(emi, 2),
                Math.Round(principalPart, 2),
                Math.Round(interest, 2),
                Math.Round(outstanding, 2)));
        }

        return schedule;
    }

    public static decimal CalculateRebate(decimal outstandingAmount, decimal rebatePercent)
    {
        return Math.Round(outstandingAmount * rebatePercent / 100, 2);
    }

    public static decimal CalculatePenalty(decimal overdueAmount, decimal penaltyPercent)
    {
        return Math.Round(overdueAmount * penaltyPercent / 100, 2);
    }
}
