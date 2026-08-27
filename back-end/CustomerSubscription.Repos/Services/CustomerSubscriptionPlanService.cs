using CustomerSubscription.Dal.Dal;
using CustomerSubscription.Dal.Models;
using CustomerSubscription.Repos.Interfaces;
using CustomerSubscription.Shared;
using CustomerSubscription.Shared.Interfaces;
using CustomerSubscription.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerSubscription.Repos
{
    public class CustomerSubscriptionPlanService : ICustomerSubscriptionPlanService
    {
        private readonly AppDbContext _context;
        private readonly ISubscriptionPricingService _subscriptionPricingService;

        public CustomerSubscriptionPlanService(AppDbContext context, 
            ISubscriptionPricingService subscriptionPricingService)
        {
            _context = context;
            _subscriptionPricingService = subscriptionPricingService; 
        }

        // Get all customer subscriptipn plans
        async Task<List<CustomerSubscriptionPlanDto>> ICustomerSubscriptionPlanService.GetCustomerSubscriptionPlansAsync(int customerId)
        {
            return await _context.CustomerSubscriptionPlans
                 .AsNoTracking()
                 .Where(s => s.CustomerId == customerId)
                 .Include(s => s.Customer)
                 .Include(s => s.Plan)
                 .OrderByDescending(s => s.StartDate)
                 .Select(s => new CustomerSubscriptionPlanDto
                 {
                     Id = s.Id,
                     CustomerId = s.CustomerId,
                     CustomerName = s.Customer.Name,
                     PlanName = s.Plan.PlanName,
                     StartDate = s.StartDate,
                     EndDate = s.EndDate,
                     Status = s.Status,
                     PlanId = s.PlanId    
                 })
                 .ToListAsync();
        }

        public async Task<PlanChangeCostDto> ChangePlanAsync(int customerSubscriptionId, int newPlanId, string changedBy)
        {
            CustomerSubscriptionPlan subscription = new CustomerSubscriptionPlan();

            // 1. Load the subscription (tracked, since we need to update it)
            try
            {
                 subscription = await _context.CustomerSubscriptionPlans
               .FirstOrDefaultAsync(s => s.Id == customerSubscriptionId);
            }
            catch (Exception ex) { }

            if (subscription == null)
                throw new KeyNotFoundException($"Subscription {customerSubscriptionId} not found.");

            var newPlanExists = await _context.SubscriptionPlans
                .AnyAsync(p => p.Id == newPlanId);

            if (!newPlanExists)
                throw new KeyNotFoundException($"Plan {newPlanId} not found.");

            if (subscription.PlanId == newPlanId)
                throw new InvalidOperationException("Customer is already on this plan.");

            // 2. Recalculate cost server-side (never trust a client-supplied cost)
            var costResult = await _subscriptionPricingService.CalculatePlanChangeCostAsync(
                customerSubscriptionId, subscription.PlanId, newPlanId);

            // 3. Apply plan change + save history atomically
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                subscription.PlanId = newPlanId;
                subscription.ModifiedDate = DateTime.UtcNow;
                subscription.ModifiedBy = changedBy;

                var history = new PlanChangeHistory
                {
                    CustomerSubscriptionId = customerSubscriptionId,
                    ExistingPlanId = costResult.ExistingPlanId,
                    NewPlanId = costResult.NewPlanId,
                    ExistingPlanRemainingCredit = costResult.ExistingPlanRemainingCredit,
                    NewPlanProratedCost = costResult.NewPlanProratedCost,
                    NetCost = costResult.NetCost,
                    RemainingDays = costResult.RemainingDays,
                    TotalDaysInCycle = costResult.TotalDaysInCycle,
                    ChangedOn = DateTime.UtcNow,
                    ChangedBy = changedBy
                };

                _context.PlanChangeHistories.Add(history);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return costResult;
        }
    }
}
