using CustomerSubscription.Dal.Dal;
using CustomerSubscription.Repos.Interfaces;
using CustomerSubscription.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerSubscription.Repos
{
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        private readonly AppDbContext _context;

        public SubscriptionPlanService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SubscriptionPlanDto>> GetSubscriptionPlansAsync()
        {
            return await _context.SubscriptionPlans
            .AsNoTracking()
            .OrderBy(c => c.PlanName)
            .Select(c => new SubscriptionPlanDto
            {
                Id = c.Id,
                PlanName = c.PlanName,
                MonthlyCharge = c.MonthlyCharge
            })
            .ToListAsync();
        }
    }
}
