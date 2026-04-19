using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UltimateERP.Application.Features.Integration.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Account;

namespace UltimateERP.Infrastructure.ExternalServices;

public class TallyIntegrationService : ITallyIntegrationService
{
    private readonly IApplicationDbContext _db;
    private readonly ILogger<TallyIntegrationService> _logger;

    public TallyIntegrationService(IApplicationDbContext db, ILogger<TallyIntegrationService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<ImportResultDto> ImportFromTallyXmlAsync(Stream xmlStream)
    {
        _logger.LogInformation("Starting Tally XML import");
        var result = new ImportResultDto();

        try
        {
            var doc = await XDocument.LoadAsync(xmlStream, LoadOptions.None, CancellationToken.None);
            var ledgerElements = doc.Descendants("LEDGER");

            foreach (var element in ledgerElements)
            {
                result.TotalRows++;
                try
                {
                    var name = element.Attribute("NAME")?.Value ?? element.Element("NAME")?.Value;
                    if (string.IsNullOrEmpty(name))
                    {
                        result.ErrorCount++;
                        result.Errors.Add($"Row {result.TotalRows}: Missing ledger name");
                        continue;
                    }

                    var existingLedger = await _db.Ledgers.FirstOrDefaultAsync(l => l.Name == name);
                    if (existingLedger != null)
                    {
                        result.ErrorCount++;
                        result.Errors.Add($"Row {result.TotalRows}: Ledger '{name}' already exists");
                        continue;
                    }

                    var openingBalance = decimal.TryParse(
                        element.Element("OPENINGBALANCE")?.Value ?? "0", out var ob) ? ob : 0;

                    var ledger = new Ledger
                    {
                        Code = $"TALLY-{result.TotalRows:D4}",
                        Name = name,
                        OpeningBalance = openingBalance,
                        IsActive = true
                    };

                    _db.Ledgers.Add(ledger);
                    result.SuccessCount++;
                }
                catch (Exception ex)
                {
                    result.ErrorCount++;
                    result.Errors.Add($"Row {result.TotalRows}: {ex.Message}");
                }
            }

            await _db.SaveChangesAsync();
            _logger.LogInformation("Tally import completed: {Success}/{Total}", result.SuccessCount, result.TotalRows);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Tally XML import failed");
            result.Errors.Add($"XML parsing error: {ex.Message}");
        }

        return result;
    }

    public async Task<byte[]> ExportToTallyXmlAsync(TallyExportRequestDto request)
    {
        _logger.LogInformation("Starting Tally XML export: {ExportType}", request.ExportType);

        var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
        var envelope = new XElement("ENVELOPE",
            new XElement("HEADER",
                new XElement("TALLYREQUEST", "Export"),
                new XElement("TYPE", request.ExportType)));

        var body = new XElement("BODY");

        if (request.ExportType.Equals("Ledgers", StringComparison.OrdinalIgnoreCase))
        {
            var ledgers = await _db.Ledgers
                .Include(l => l.LedgerGroup)
                .Where(l => l.IsActive)
                .ToListAsync();

            foreach (var ledger in ledgers)
            {
                body.Add(new XElement("LEDGER",
                    new XAttribute("NAME", ledger.Name ?? ""),
                    new XElement("NAME", ledger.Name),
                    new XElement("PARENT", ledger.LedgerGroup?.Name ?? ""),
                    new XElement("OPENINGBALANCE", ledger.OpeningBalance),
                    new XElement("CLOSINGBALANCE", ledger.ClosingBalance)));
            }
        }
        else if (request.ExportType.Equals("Vouchers", StringComparison.OrdinalIgnoreCase))
        {
            var query = _db.Vouchers
                .Include(v => v.Details).ThenInclude(d => d.Ledger)
                .AsQueryable();

            if (request.FromDate.HasValue)
                query = query.Where(v => v.VoucherDate >= request.FromDate.Value);
            if (request.ToDate.HasValue)
                query = query.Where(v => v.VoucherDate <= request.ToDate.Value);

            var vouchers = await query.ToListAsync();

            foreach (var voucher in vouchers)
            {
                var vElement = new XElement("VOUCHER",
                    new XElement("VOUCHERNUMBER", voucher.VoucherNumber),
                    new XElement("DATE", voucher.VoucherDate.ToString("yyyyMMdd")),
                    new XElement("NARRATION", voucher.CommonNarration));

                foreach (var detail in voucher.Details)
                {
                    vElement.Add(new XElement("ALLLEDGERENTRIES.LIST",
                        new XElement("LEDGERNAME", detail.Ledger?.Name ?? ""),
                        new XElement("ISDEEMEDPOSITIVE", detail.DebitAmount > 0 ? "Yes" : "No"),
                        new XElement("AMOUNT", detail.DebitAmount > 0 ? -detail.DebitAmount : detail.CreditAmount)));
                }

                body.Add(vElement);
            }
        }

        envelope.Add(body);
        doc.Add(envelope);

        using var ms = new MemoryStream();
        await doc.SaveAsync(ms, SaveOptions.None, CancellationToken.None);
        return ms.ToArray();
    }
}
