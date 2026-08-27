// import { httpService } from "./httpService";
// import {
//   CustomerSubscriptionPlanDto,
//   PlanChangeCostDto,
//   ChangePlanRequest,
// } from "../types/customerSubscriptionPlan";

import axios from "axios";
import {
  ChangePlanRequest,
  CustomerSubscriptionPlanDto,
} from "../models/CustomerSubscriptionPlanDto";
import { PlanChangeCostDto } from "../models/PlanChangeCostDto";

const customerSubscriptionPlanUrl = "api/customerSubscriptionPlan";

export const customerSubscriptionPlanApiService = {
  getCustomerSubscriptionPlans: async (customerId: number) =>
    await axios.get<CustomerSubscriptionPlanDto[]>(
      `${customerSubscriptionPlanUrl}/customer/${customerId}`,
    ),

  // changePlan: async (customerSubscriptionId: number, newPlanId: number) =>
  //   await axios.post<PlanChangeCostDto>(
  //     `${customerSubscriptionPlanUrl}/${customerSubscriptionId}/change-plan`,
  //     { newPlanId } as ChangePlanRequest,
  //   ),
  changePlan: async ({
    customerSubscriptionId,
    newPlanId,
  }: {
    customerSubscriptionId: number;
    newPlanId: number;
  }) =>
    await axios.post<PlanChangeCostDto>(
      `${customerSubscriptionPlanUrl}/${customerSubscriptionId}/change-plan`,
      { newPlanId } as ChangePlanRequest,
    ),
};
