export interface Transaction {
   transaction_Id: number;
   related_Deal_Id: number;
   transaction_Date: Date;
    occurred?: boolean;
    amount_Due_BOP?: number;
    principal_BOP?: number;
    cash_Interest_BOP?: number;
    pik_Interest_BOP?: number;
    undrawn_Interest_BOP?: number;
    cash_Interest_Rate?: number;
    pik_Interest_Rate?: number;
    undrawnFeeInterestRate?: number;
    undrawn_Amount?: number;
    cash_Interest_Accrued?: number;
    pik_Interest_Accrued?: number;
    undrawn_Interest_Accrued?: number;
    repayment?: number;
    repayment_CashInterest?: number;
    repayment_PikInterest?: number;
    repayment_Principal?: number;
    repayment_UndrawnFees?: number;
    capitalized?: number;
    principal_EOP?: number;
    cash_Interest_EOP?: number;
    pik_Interest_EOP?: number;
    undrawn_Interest_EOP?: number;
    amount_Due_EOP?: number;
    drawdown?: number;
  }
  