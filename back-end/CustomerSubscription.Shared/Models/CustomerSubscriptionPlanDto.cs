
namespace CustomerSubscription.Shared
{
    public class CustomerSubscriptionPlanDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PlanId { get; set; }
        public string CustomerName { get; set; }
        public string PlanName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = "Active";   // Active, Cancelled, Paused, Expired
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
