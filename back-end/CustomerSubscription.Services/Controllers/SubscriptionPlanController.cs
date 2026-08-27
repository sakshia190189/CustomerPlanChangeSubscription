using CustomerSubscription.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSubscription.Services.Controllers
{
    [Route("api/subscriptionPlan")]
    public class SubscriptionPlanController : ControllerBase
    {
        private readonly ISubscriptionPlanService _subscriptionPlanService;

        public SubscriptionPlanController(ISubscriptionPlanService subscriptionPlanService)
        {
            _subscriptionPlanService = subscriptionPlanService;
        }

        [HttpGet("getSubscriptionPlans")]

        public async Task<IActionResult> GetSubscriptionPlans()
        {
            var plans = await _subscriptionPlanService.GetSubscriptionPlansAsync();

            return Ok(plans);
        }
    }
}
