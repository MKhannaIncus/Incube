export interface Transaction {
   transaction_Id: number;
   related_Deal_Id: number;
   transaction_Date: Date;
    occurred?: boolean;
    amountDueBOP?: number;
    principalBOP?: number;
    cashInterestBOP?: number;
    pikInterestBOP?: number;
    undrawnInterestBOP?: number;
    cashInterestRate?: number;
    pikInterestRate?: number;
    undrawnFeeInterestRate?: number;
    undrawnAmount?: number;
    cashInterestAccrued?: number;
    pikInterestAccrued?: number;
    undrawnInterestAccrued?: number;
    repayment?: number;
    repaymentCashInterest?: number;
    repaymentPikInterest?: number;
    repaymentPrincipal?: number;
    repaymentUndrawnFees?: number;
    capitalized?: number;
    principalEOP?: number;
    cashInterestEOP?: number;
    pikInterestEOP?: number;
    undrawnInterestEOP?: number;
    amountDueEOP?: number;
    drawdown?: number;
  }
  