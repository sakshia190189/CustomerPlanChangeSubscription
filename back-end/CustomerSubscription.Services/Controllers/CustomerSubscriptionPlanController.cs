using CustomerSubscription.Repos.Interfaces;
using CustomerSubscription.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSubscription.Services.Controllers
{
    [Route("api/customerSubscriptionPlan")]
    [ApiController]
    public class CustomerSubscriptionPlanController : ControllerBase
    {
        private readonly ICustomerSubscriptionPlanService _customerSubscriptionPlanService;

        public CustomerSubscriptionPlanController(
            ICustomerSubscriptionPlanService customerSubscriptionPlanService)
        {
            _customerSubscriptionPlanService = customerSubscriptionPlanService;
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerSubscriptionPlans(int customerId)
        {
            var subscriptions = await _customerSubscriptionPlanService.GetCustomerSubscriptionPlansAsync(customerId);

            if (!subscriptions.Any())
                return NotFound($"No subscriptions found for customer {customerId}");

            return Ok(subscriptions);
        }

        [HttpPost("{customerSubscriptionId}/change-plan")]
        public async Task<ActionResult<PlanChangeCostDto>> ChangePlan(
            int customerSubscriptionId,
            //, 
            //int newPlanId
            [FromBody] ChangePlanRequestDto request
            )
        {
            try
            {
                var result = await _customerSubscriptionPlanService.ChangePlanAsync(
                    customerSubscriptionId, request.NewPlanId, User.Identity?.Name ?? "system");

                return Ok(null);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
