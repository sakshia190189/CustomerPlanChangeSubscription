using CustomerSubscription.Shared.Models;
namespace CustomerSubscription.Repos.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetCustomersAsync();
    }
}
