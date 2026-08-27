using CustomerSubscription.Shared.Models;
using System;
namespace CustomerSubscription.Dal.Models
{
    public class CustomerSubscriptionPlan
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = "Active";   // Active, Cancelled, Paused, Expired
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public Customer Customer { get; set; }
        public SubscriptionPlan Plan { get; set; }
    }
}
