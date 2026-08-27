import React from "react";
import { PlanChangeCostDto } from "../../models/PlanChangeCostDto";

interface PlanChangeSummaryProps {
  planCost: PlanChangeCostDto | null;
}

const formatCurrency = (value: number): string =>
  value.toLocaleString("en-US", {
    style: "currency",
    currency: "USD",
  });

const planChangeSummary: React.FC<PlanChangeSummaryProps> = ({ planCost }) => {
  if (!planCost) return null;

  return (
    <div className="mb-4 p-4 bg-blue-50 border rounded">
      <h5 className="font-semibold mb-3">Plan Change Summary</h5>

      <div>
        <strong>Current Plan:</strong> {planCost.ExistingPlanName}
      </div>

      <div>
        <strong>New Plan:</strong> {planCost.NewPlanName}
      </div>

      <div>
        <strong>Remaining Credit:</strong>{" "}
        {formatCurrency(planCost?.ExistingPlanRemainingCredit ?? 0)}
      </div>

      <div>
        <strong>Prorated Cost:</strong>{" "}
        {formatCurrency(planCost.NewPlanProratedCost)}
      </div>

      <div>
        <strong>Remaining Days:</strong> {planCost.RemainingDays}
      </div>

      <div className="mt-2 text-lg font-bold">
        Net Cost: {formatCurrency(planCost.NetCost)}
      </div>
    </div>
  );
};

export default planChangeSummary;
