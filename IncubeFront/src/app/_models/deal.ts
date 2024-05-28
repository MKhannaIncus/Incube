export interface Deal {
  deal_Id: number;
  deal_Name: string;
  client_Id?: number;
  facility?: number;
  asset_Id?: number;
  country: string;
  sector: string;
  subsector: string;
  investment_date?: Date;
  maturity_date?: Date;
  opening_fee?: number;
  minimum_multiple?: number;
  IRR?: number;
  MOIC?: number;
  NAV?: number;
  availability_period?: number;
  availability_fee?: number;
  intercompany_loan?: boolean;
  entity_Id?: number;
  interest_Id?: number;
  amortization_type: string;
  ownership_Id?: number;
  LTV_Entry?: number;
}
