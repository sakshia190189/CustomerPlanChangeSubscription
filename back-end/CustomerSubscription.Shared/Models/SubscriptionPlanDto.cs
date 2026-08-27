
namespace CustomerSubscription.Shared.Models
{
    public class SubscriptionPlanDto
    {
        public int Id { get; set; }
        public string PlanName { get; set; }
        public decimal MonthlyCharge { get; set; }
        public bool IsActive { get; set; } = true;
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
