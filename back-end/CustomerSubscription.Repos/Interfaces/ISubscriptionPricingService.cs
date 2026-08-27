using CustomerSubscription.Shared.Models;

namespace CustomerSubscription.Shared.Interfaces
{
    public interface ISubscriptionPricingService
    {
        Task<PlanChangeCostDto> CalculatePlanChangeCostAsync(int customerSubscriptionId, int existingPlanId, int newPlanId);
    }
}