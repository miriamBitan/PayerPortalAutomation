using PayerPortalAutomation.Domain.Entities;

namespace PayerPortalAutomation.Application.Repositories;

public interface IPayerTargetRepository
{
    Task<PayerTarget> GetAsync(string payerId, string taxId);
}