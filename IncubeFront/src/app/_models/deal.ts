export interface Deal {
  deal_Id: string; // Changed to string based on "INV3-001"
  deal_Name: string;
  client_Id?: number;
  client?: string; // Added this field to match "Sacyr"
  facility?: number;
  asset_Id?: number;
  country: string;
  sector: string;
  subsector: string;
  status?: string;
  investment_date?: Date | string; // Added string type to handle the ISO string dates
  maturity_date?: Date | string;   // Same here to handle ISO string dates
  opening_fee?: number;
  minimum_multiple?: number;
  irr?: number; // Corrected naming to lower case based on the provided data
  underwriting_IRR?: number;
  moic?: number; // Corrected naming to lower case based on the provided data
  underwriting_MOIC?: number;
  nav?: number;
  underwriting_NAV?: number;
  availability_period?: number;
  availability_fee?: number;
  intercompany_loan?: boolean;
  entity_Id?: number;
  interest_Id?: number;
  amortization_type?: string;
  ownership_Id?: number;
  ltV_Entry?: number; // Corrected field name based on provided data
  comments?: string;
  related_fund_id?: number;
  drawdown?: number; // Added this field based on your data
  cash_Interest_Period?: string; // Added this field based on your data
  piK_Interest_Period?: string;  // Added this field based on your data
  first_CashInterest_Period_Rate?: number;
  first_CashInterest_Period_EndPeriods?: Date | string;
  second_CashInterest_Period_Rate?: number;
  second_CashInterest_Period_EndPeriods?: Date | string;
  third_CashInterest_Period_Rate?: number;
  third_CashInterest_Period_EndPeriods?: Date | string;
  first_PIKInterest_Period_Rate?: number;
  first_PIKInterest_Period_EndPeriods?: Date | string;
  second_PIKInterest_Period_Rate?: number;
  second_PIKInterest_Period_EndPeriods?: Date | string;
  third_PIKInterest_Period_Rate?: number;
  third_PIKInterest_Period_EndPeriods?: Date | string;

  // New fields based on the provided data
  fund?: string; // "Fund III"
  general_Investment_Code?: string; // "INV-045"
  general_Investment_Name?: string; // "Path FIII"
  realization_Date?: Date | string; // "2019-02-28T00:00:00"
  percent_Master_Fund?: number;
  percent_Coinvestors?: number;
  country_Code?: string; // "ESP"
  client_Country_Code?: string; // "ESP"
  asset_Class?: string; // "Infrastructure"
  product?: string; // "Debt"
  strategy?: string; // "Path"
  deal_Grouping?: string; // "Single"
  loan_Type?: string; // "PL"
  seniority?: string; // "Junior"
  capital_Repayment?: string; // "Asset disposal"
  coupon?: string; // "Cash"
  interest_Rate?: string; // "Fixed"
  thematic_vs_Opportunistic?: string; // "Thematic"
  theme?: string; // "Infra HoldCo"
  origination?: string; // "Primary"
  sponsorship?: string; // "Non-sponsored"
  repeat_Counterparty?: string; // "Yes"
  deal_Source?: string; // "Incus"
  instrument?: string; // "Loan"
}
