import axios from "axios";
import { SubscriptionPlanDto } from "../models/SubscriptionPlanDto";

const subscriptionPlansUrl = "/api/subscriptionPlan";

export const subscriptionPlanApiService = {
  getSubscriptionPlans: async () =>
    await axios.get<SubscriptionPlanDto[]>(
      `${subscriptionPlansUrl}/getSubscriptionPlans`,
    ),
};
