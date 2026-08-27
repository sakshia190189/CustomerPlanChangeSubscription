import React, { useState, useEffect, useCallback } from "react";
import { CButton, CSmartTable } from "@coreui/react-pro";
import { customerApiService } from "../../services/customerApiService";
import { CustomerDto } from "../../models/CustomerDto";
import CustomerSubscriptionPlan from "../customerSubscriptionPlan/CustomerSubscriptionPlan";

interface SmartDataTableHeader {
  key: keyof CustomerDto;
  text: string;
  sortable?: boolean;
  invisible?: boolean;
  transform?: (value: any, allValues?: CustomerDto) => React.ReactNode;
}

interface RowClickMeta {
  rowIndex: number;
  rowData: CustomerDto;
  event: React.MouseEvent;
}

const CustomerGrid: React.FC = () => {
  const SafeButton = CButton as unknown as React.FC<
    React.PropsWithChildren<any>
  >;
  const [expandedIds, setExpandedIds] = useState<Set<number>>(new Set());
  const toggleExpanded = (id: number): void => {
    setExpandedIds((prev) => {
      const next = new Set(prev);
      next.has(id) ? next.delete(id) : next.add(id);
      return next;
    });
  };
  const [rows, setRows] = useState<CustomerDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const fetchCustomers = useCallback(async (): Promise<void> => {
    setLoading(true);
    setError(null);
    try {
      const response = await customerApiService.getCustomers();
      setRows(response.data);
    } catch (err) {
      const message =
        err instanceof Error ? err.message : "Failed to load customers";
      setError(message);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchCustomers();
  }, [fetchCustomers]);

  const handleRowClick = (
    event: React.MouseEvent,
    meta: RowClickMeta,
  ): void => {
    // Hook this up to a detail panel / edit modal / route navigation
    console.log("Customer clicked:", meta.rowData);
  };

  if (error) {
    return (
      <div className="p-4 text-red-600">
        Error loading customers: {error}
        <button
          onClick={fetchCustomers}
          className="ml-3 px-3 py-1 bg-gray-100 rounded hover:bg-gray-200"
        >
          Retry
        </button>
      </div>
    );
  }

  return (
    <div className="p-4">
      <div className="flex items-center justify-between mb-3">
        <h2 className="text-lg font-semibold">Customers</h2>
        <button
          onClick={fetchCustomers}
          className="px-3 py-1.5 text-sm bg-blue-600 text-white rounded hover:bg-blue-700"
        >
          Refresh
        </button>
      </div>
      <CSmartTable
        items={(rows ?? []) as any[]}
        columns={[
          { key: "Name", label: "Name" },
          { key: "Phone", label: "Phone" },
          { key: "Email", label: "Email" },
          { key: "show_plans", label: "", filter: false, sorter: false },
        ]}
        scopedColumns={{
          show_plans: (item) => {
            const customer = item as CustomerDto;
            return (
              <td>
                <SafeButton
                  color="primary"
                  variant="outline"
                  size="sm"
                  onClick={() => toggleExpanded(customer.Id)}
                >
                  {expandedIds.has(customer.Id) ? "Hide Plans" : "Show Plans"}
                </SafeButton>
              </td>
            );
          },
          details: (item: any) => {
            const customer = item as CustomerDto;
            if (!expandedIds.has(customer.Id)) {
              return null;
            }
            return (
              <CustomerSubscriptionPlan
                customerId={customer.Id}
                expandedIds={expandedIds}
              />
            );
          },
        }}
        // details={Array.from(expandedIds)}
      ></CSmartTable>
    </div>
  );
};

export default CustomerGrid;
