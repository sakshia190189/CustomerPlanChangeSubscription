using CustomerSubscription.Shared;
using CustomerSubscription.Shared.Models;

namespace CustomerSubscription.Repos.Interfaces
{
    public interface ICustomerSubscriptionPlanService
    {
        public Task<List<CustomerSubscriptionPlanDto>> GetCustomerSubscriptionPlansAsync(int customerId);

        public Task<PlanChangeCostDto> ChangePlanAsync(int customerSubscriptionId, int newPlanId, string changedBy);
    }
}
