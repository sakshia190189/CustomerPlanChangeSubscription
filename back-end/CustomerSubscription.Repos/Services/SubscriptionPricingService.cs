using CustomerSubscription.Dal.Dal;
using CustomerSubscription.Dal.Models;
using CustomerSubscription.Shared.Interfaces;
using CustomerSubscription.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerSubscription.Repos.Services
{
    public class SubscriptionPricingService : ISubscriptionPricingService
    {
        private readonly AppDbContext _context;

        public SubscriptionPricingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PlanChangeCostDto> CalculatePlanChangeCostAsync(
     int customerSubscriptionId, int existingPlanId, int newPlanId)
        {
            var subscription = await _context.CustomerSubscriptionPlans
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == customerSubscriptionId);

            if (subscription == null)
                throw new KeyNotFoundException($"Subscription {customerSubscriptionId} not found.");

            SubscriptionPlan existingPlan = new SubscriptionPlan();

            try { 
             existingPlan = await _context.SubscriptionPlans
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == existingPlanId);
                 }
            catch(Exception ex)
            {

            }

            if (existingPlan == null)
                throw new KeyNotFoundException($"Existing plan {existingPlanId} not found.");

            var newPlan = await _context.SubscriptionPlans
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == newPlanId);

            if (newPlan == null)
                throw new KeyNotFoundException($"New plan {newPlanId} not found.");

            if (existingPlanId == newPlanId)
                throw new InvalidOperationException("Existing plan and new plan cannot be the same.");

            // --- Proration math ---
            var cycleStart = subscription.StartDate;
            var cycleEnd = subscription.EndDate ?? cycleStart.AddMonths(1);

            int totalDays = (cycleEnd - cycleStart).Days;
            if (totalDays <= 0)
                throw new InvalidOperationException("Invalid billing cycle: EndDate must be after StartDate.");

            var asOfDate = DateTime.UtcNow;
            int remainingDays = Math.Max((cycleEnd - asOfDate).Days, 0);

            decimal dailyRateExisting = existingPlan.MonthlyCharge / totalDays;
            decimal dailyRateNew = newPlan.MonthlyCharge / totalDays;

            decimal remainingCredit = Math.Round(dailyRateExisting * remainingDays, 2);
            decimal newPlanProrated = Math.Round(dailyRateNew * remainingDays, 2);
            decimal netCost = newPlanProrated - remainingCredit;

            return new PlanChangeCostDto
            {
                CustomerSubscriptionId = customerSubscriptionId,
                ExistingPlanId = existingPlan.Id,
                ExistingPlanName = existingPlan.PlanName,
                NewPlanId = newPlan.Id,
                NewPlanName = newPlan.PlanName,
                ExistingPlanRemainingCredit = remainingCredit,
                NewPlanProratedCost = newPlanProrated,
                NetCost = netCost,
                RemainingDays = remainingDays,
                TotalDaysInCycle = totalDays,
            };
        }
    }
}
