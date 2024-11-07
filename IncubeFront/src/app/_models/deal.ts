export interface Deal {
  deal_Id: string; // Matches C# Deal_Id
  related_Fund?: string; // Matches C# Related_Fund
  deal_Name: string;
  facility?: number; // Adjusted to match the decimal type
  country?: string;
  sector?: string;
  subsector?: string;
  investment_date?: Date | string;
  maturity_date?: Date | string;
  opening_fee?: number;
  availability_period?: number;
  underwriting_IRR?: number;
  underwriting_MOIC?: number;
  underwriting_NAV?: number;
  comments?: string;
  loan?: string; // Matches C# Loan
  instrument_Dealddbbb?: string; // Matches C# Instrument_Dealddbbb
  extension_Period?: string;
  first_Utilization?: number;
  purchase_Discount_Rate?: number;
  exit_fee?: number;
  minimum_Multiple?: number;
  eURIBOR_Interest_Rate?: string; // Matches C# EURIBOR_Interest_Rate
  cashInterest_Rate_1st?: number;
  cashInterest_End_1st?: Date | string;
  cashInterest_Rate_2nd?: number;
  cashInterest_End_2nd?: Date | string;
  cashInterest_Rate_3rd?: number;
  cashInterest_End_3rd?: Date | string;
  pikInterest_Rate_1st?: number;
  pikInterest_End_1st?: Date | string;
  pikInterest_Rate_2nd?: number;
  pikInterest_End_2nd?: Date | string;
  pikInterest_Rate_3rd?: number;
  pikInterest_End_3rd?: Date | string;
  piycInterest_Rate_1st?: number;
  piycInterest_End_1st?: Date | string;
  piycInterest_Rate_2nd?: number;
  piycInterest_End_2nd?: Date | string;
  piycInterest_Rate_3rd?: number;
  piycInterest_End_3rd?: Date | string;
  others?: string;
  year_Base?: number;
  interest_Period?: string;
  default_interest_rate?: number;
  undrawn_fee?: number;
  undrawn_fee_periods?: string;
  default_Capitalization_Periods?: string;
  acceleration_Date?: Date | string;
  share_Premium?: number;
  lender?: string;
  borrower?: string;
  client?: string;
  percent_Master_Fund?: number;
  percent_Coinvestor?: number;
  country_Code?: string;
  client_Country_Code?: string;
  asset_Class?: string;
  product?: string;
  strategy?: string;
  grouping?: string; // Matches C# Grouping
  loan_Type?: string;
  seniority?: string;
  capital_Repayment?: string;
  coupon?: string;
  interest_Rate_Type?: string;
  thematic_Vs_Opportunistic?: string;
  theme?: string;
  origination?: string;
  sponsorship?: string;
  repeat_Counterparty?: string;
  deal_Source?: string;
  instrument_LoanTemplateddbb?: string;
}
