import { CustomerDto } from "../models/CustomerDto";
import axios from "axios";

const customerUrl = "/api/customer";

export const customerApiService = {
  getCustomers: async () =>
    await axios.get<CustomerDto[]>(`${customerUrl}/getCustomers`),
};
