using CustomerSubscription.Dal.Dal;
using CustomerSubscription.Repos.Interfaces;
using CustomerSubscription.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerSubscription.Repos.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerDto>> GetCustomersAsync()
        {
            return await _context.Customers
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new CustomerDto()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Phone = c.Phone
                })
                .ToListAsync();
        }
    }
}
