using CustomerSubscription.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSubscription.Services.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController( ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("getCustomers")]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _customerService.GetCustomersAsync();

            return Ok(customers);
        }
    }
}
