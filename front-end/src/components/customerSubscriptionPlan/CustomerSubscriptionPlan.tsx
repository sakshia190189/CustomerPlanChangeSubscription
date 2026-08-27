import React, { useState, useEffect, useCallback } from "react";
import { CFormSelect } from "@coreui/react-pro";
import { customerSubscriptionPlanApiService } from "../../services/customerSubscriptionPlanApiService";
import { CButton } from "@coreui/react-pro";
import { CustomerSubscriptionPlanDto } from "../../models/CustomerSubscriptionPlanDto";
import { subscriptionPlanApiService } from "../../services/subscriptionPlanApiService";
import { SubscriptionPlanDto } from "../../models/SubscriptionPlanDto";
import { subscriptionPricingApiService } from "../../services/subscriptionPricingApiService";
import PlanChangeSummary from "../planChangeSummary/planChangeSummary";
import { PlanChangeCostDto } from "../../models/PlanChangeCostDto";
const SafeButton = CButton as unknown as React.FC<React.PropsWithChildren<any>>;

interface CustomerPlansProps {
  customerId: number;
  expandedIds: Set<number>;
}

export interface CustomerPlanView extends CustomerSubscriptionPlanDto {
  PlanName: string;
  MonthlyCharge: number;
}

function formatDate(value: string | null): string {
  if (!value) return "—";
  const d = new Date(value);
  if (isNaN(d.getTime())) return value;
  return d.toLocaleDateString("en-US", {
    month: "2-digit",
    day: "2-digit",
    year: "numeric",
  });
}

function formatCurrency(value: number): string {
  return value.toLocaleString("en-US", { style: "currency", currency: "USD" });
}

function statusBadgeClass(status: string): string {
  switch (status?.toLowerCase()) {
    case "active":
      return "bg-green-100 text-green-800";
    case "cancelled":
      return "bg-red-100 text-red-800";
    case "expired":
      return "bg-gray-100 text-gray-600";
    default:
      return "bg-yellow-100 text-yellow-800";
  }
}

const CustomerSubscriptionPlan: React.FC<CustomerPlansProps> = ({
  customerId,
  expandedIds,
}) => {
  const [planHistory, setPlanHistory] = useState<CustomerPlanView[]>([]);
  const [availablePlans, setAvailablePlans] = useState<SubscriptionPlanDto[]>(
    [],
  );
  const [selectedPlanId, setSelectedPlanId] = useState<string>("");
  const [loading, setLoading] = useState<boolean>(true);
  const [assigning, setAssigning] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [planCost, setPlanCost] = useState<PlanChangeCostDto | null>(null);

  const loadData = useCallback(async (): Promise<void> => {
    setLoading(true);
    setError(null);
    try {
      const [historyRes, plansRes] = await Promise.all([
        customerSubscriptionPlanApiService.getCustomerSubscriptionPlans(
          customerId,
        ),
        subscriptionPlanApiService.getSubscriptionPlans(),
      ]);
      const plans: SubscriptionPlanDto[] = plansRes.data;
      const joined: CustomerPlanView[] = historyRes.data.map(
        (csp: CustomerSubscriptionPlanDto) => {
          const plan = plans.find((p) => p.Id === csp.PlanId);
          //const plan = historyRes.data.find((p) => p.customerId === customerId);

          return {
            ...csp,
            //PlanName: plan?.planName ?? "Unknown Plan",
            MonthlyCharge: plan?.MonthlyCharge ?? 0,
            PlanName: csp.PlanName ?? "Unknown Plan",
            StartDate: csp.StartDate,
            EndDate: csp.EndDate,
          };
        },
      );
      setAvailablePlans(plans.filter((p) => p.IsActive));
      //setAvailablePlans(plans);
      setPlanHistory(joined);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to load plans");
    } finally {
      setLoading(false);
    }
  }, [customerId]);

  useEffect(() => {
    loadData();
  }, [loadData]);

  const handleShowCost = async () => {
    if (!selectedPlanId) return;

    const activeSubscription = planHistory.find((p) => p.Status === "Active");

    if (!activeSubscription) {
      setError("No active subscription found.");
      return;
    }

    const response = await subscriptionPricingApiService.getPlanChangeCost(
      activeSubscription.Id,
      activeSubscription.PlanId,
      Number(selectedPlanId),
    );

    setPlanCost(response.data);
  };

  const handleAssignPlan = async (): Promise<void> => {
    if (!selectedPlanId) return;
    setAssigning(true);
    setError(null);
    try {
      const activeSubscription = planHistory.find((p) => p.Status === "Active");

      if (!activeSubscription) {
        setError("No active subscription found.");
        return;
      }

      await customerSubscriptionPlanApiService.changePlan({
        customerSubscriptionId: activeSubscription.Id,
        newPlanId: Number(selectedPlanId),
      });
      setSelectedPlanId("");
      await loadData(); // refresh the history table to show the new row
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to assign plan");
    } finally {
      setAssigning(false);
    }
  };

  if (loading) {
    return <div className="p-3 text-sm text-gray-500">Loading plans…</div>;
  }

  return (
    <div className="p-4 bg-gray-50 border-t border-gray-200">
      {error && <div className="mb-3 text-sm text-red-600">{error}</div>}

      {/* Plan history table */}
      <table className="w-full text-sm mb-4">
        <thead>
          <tr className="text-left text-gray-600 border-b border-gray-300">
            <th className="pb-2 font-medium">Plan</th>
            <th className="pb-2 font-medium">Monthly Charge</th>
            <th className="pb-2 font-medium">Start Date</th>
            <th className="pb-2 font-medium">End Date</th>
            <th className="pb-2 font-medium">Status</th>
          </tr>
        </thead>
        <tbody>
          {planHistory.length === 0 ? (
            <tr>
              <td colSpan={5} className="py-3 text-gray-500">
                No plans assigned to this customer yet.
              </td>
            </tr>
          ) : (
            planHistory.map((plan) => (
              //   <tr key={plan.Id} className="border-b border-gray-100">
              <tr key={1} className="border-b border-gray-100">
                <td className="py-2">{plan.PlanName}</td>
                {<td className="py-2">{formatCurrency(plan.MonthlyCharge)}</td>}
                <td className="py-2">{formatDate(plan.StartDate)}</td>
                <td className="py-2">{formatDate(plan.EndDate)}</td>
              </tr>
            ))
          )}
        </tbody>
      </table>

      {planCost && <PlanChangeSummary planCost={planCost} />}

      {/* Assign new plan */}
      <div className="flex items-end gap-3 pt-2 border-t border-gray-200">
        <div className="flex-1 max-w-xs">
          <label className="block text-xs font-medium text-gray-600 mb-1">
            Assign New Plan
          </label>
          <CFormSelect
            value={selectedPlanId}
            onChange={(e: React.ChangeEvent<HTMLSelectElement>) =>
              setSelectedPlanId(e.target.value)
            }
          >
            <option value="">Select a plan…</option>
            {availablePlans.map((plan) => (
              <option key={plan.Id} value={plan.Id}>
                {plan.PlanName}— {formatCurrency(plan.MonthlyCharge)}/mo
              </option>
            ))}
          </CFormSelect>
        </div>
        <SafeButton
          color="info"
          size="sm"
          disabled={!selectedPlanId}
          onClick={handleShowCost}
        >
          Show Plan Cost
        </SafeButton>
        <SafeButton
          color="primary"
          size="sm"
          disabled={!selectedPlanId || assigning}
          onClick={handleAssignPlan}
        >
          {assigning ? "Assigning…" : "Assign Plan"}
        </SafeButton>
      </div>
    </div>
  );
};

export default CustomerSubscriptionPlan;
