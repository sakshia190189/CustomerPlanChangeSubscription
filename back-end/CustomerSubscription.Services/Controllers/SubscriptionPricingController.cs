using CustomerSubscription.Shared.Interfaces;
using CustomerSubscription.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSubscription.Services.Controllers
{
    [Route("api/subscriptionPricing")]
    public class SubscriptionPricingController : ControllerBase
    {
        private readonly ISubscriptionPricingService _subscriptionPricingService;

        public SubscriptionPricingController(ISubscriptionPricingService subscriptionPricingService)
        {
            _subscriptionPricingService = subscriptionPricingService;
        }

        [HttpGet("{customerSubscriptionId}/plan-change-cost")]
        public async Task<ActionResult<PlanChangeCostDto>> GetPlanChangeCost(
    int customerSubscriptionId, int existingPlanId, int newPlanId)
        {
            try
            {
                if (existingPlanId == newPlanId)
                {
                    return BadRequest("Existing plan and new plan cannot be the same.");
                }

                var result = await _subscriptionPricingService.CalculatePlanChangeCostAsync(
                    customerSubscriptionId, existingPlanId, newPlanId);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
