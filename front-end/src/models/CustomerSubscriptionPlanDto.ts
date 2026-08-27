export interface CustomerSubscriptionPlanDto {
  Id: number;
  CustomerName: string;
  PlanName: string;
  StartDate: string;
  EndDate: string | null;
  Status: string;
  CustomerId: number;
  PlanId: number;
}

export interface ChangePlanRequest {
  newPlanId: number;
}
