import axios from "axios";
import { PlanChangeCostDto } from "../models/PlanChangeCostDto";

const subscriptionPricingUrl = "api/subscriptionpricing";

export const subscriptionPricingApiService = {
  getPlanChangeCost: async (
    customerSubscriptionId: number,
    existingPlanId: number,
    newPlanId: number,
  ) =>
    await axios.get<PlanChangeCostDto>(
      `${subscriptionPricingUrl}/${customerSubscriptionId}/plan-change-cost`,
      { params: { existingPlanId, newPlanId } },
    ),
};
export {};
