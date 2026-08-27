
namespace CustomerSubscription.Dal.Models
{
    public class PlanChangeHistory
    {
        public int Id { get; set; }
        public int CustomerSubscriptionId { get; set; }
        public int ExistingPlanId { get; set; }
        public int NewPlanId { get; set; }

        public decimal ExistingPlanRemainingCredit { get; set; }
        public decimal NewPlanProratedCost { get; set; }
        public decimal NetCost { get; set; }

        public int RemainingDays { get; set; }
        public int TotalDaysInCycle { get; set; }

        public DateTime ChangedOn { get; set; } = DateTime.UtcNow;
        public string ChangedBy { get; set; }

        // Navigation
        public CustomerSubscriptionPlan CustomerSubscription { get; set; }
        public SubscriptionPlan ExistingPlan { get; set; }
        public SubscriptionPlan NewPlan { get; set; }
    }
}
