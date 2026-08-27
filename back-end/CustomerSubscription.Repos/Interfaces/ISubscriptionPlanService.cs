using CustomerSubscription.Shared.Models;

namespace CustomerSubscription.Repos.Interfaces
{
    public interface ISubscriptionPlanService
    {
        public Task<List<SubscriptionPlanDto>> GetSubscriptionPlansAsync();
    }
}
