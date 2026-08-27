export interface PlanChangeCostDto {
  CustomerSubscriptionId: number;
  ExistingPlanId: number;
  ExistingPlanName: string;
  NewPlanId: number;
  NewPlanName: string;
  ExistingPlanRemainingCredit: number;
  NewPlanProratedCost: number;
  NetCost: number;
  RemainingDays: number;
  TotalDaysInCycle: number;
  CalculatedOn: string;
}
