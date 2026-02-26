using PayerPortalAutomation.Application.Repositories;
using PayerPortalAutomation.Domain.Entities;

namespace PayerPortalAutomation.Infrastructure.Repositories;

public class PayerTargetCsvRepository : IPayerTargetRepository
{
    private readonly string _filePath = "payer_targets.csv";

    public async Task<PayerTarget> GetAsync(string payerId, string taxId)
    {
        var lines = await File.ReadAllLinesAsync(_filePath);

        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split(',');

            if (parts[0] == payerId && parts[1] == taxId)
            {
                return new PayerTarget
                {
                    PayerId = parts[0],
                    TaxId = parts[1],
                    PortalUrl = parts[2],
                    Username = parts[3],
                    Password = parts[4],
                    DefaultMinDelayMs = int.Parse(parts[5]),
                    DefaultMaxConcurrency = int.Parse(parts[6])
                };
            }
        }

        return null;
    }
}