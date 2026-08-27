namespace CustomerSubscription.Shared.Models
{
    public class PlanChangeCostDto
    {
        public int CustomerSubscriptionId { get; set; }
        public int ExistingPlanId { get; set; }
        public string ExistingPlanName { get; set; }
        public int NewPlanId { get; set; }
        public string NewPlanName { get; set; }

        public decimal ExistingPlanRemainingCredit { get; set; }
        public decimal NewPlanProratedCost { get; set; }
        public decimal NetCost { get; set; }   // positive = customer owes; negative = refund/credit

        public int RemainingDays { get; set; }
        public int TotalDaysInCycle { get; set; }
    }
}
