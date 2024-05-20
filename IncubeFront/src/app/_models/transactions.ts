export interface Transaction {
    transaction_Id: number;
    related_Deal_Id?: number;
    transaction_Date?: Date;
    amount_Due_BOP?: number;
    principal_BOP?: number;
    cash_Interest_BOP?: number;
    pik_Interest_BOP?: number;
    undrawn_Interest_BOP?: number;
    capitalized?: number;
    repayment?: number;
    pik_Interest_EOP?: number;
    cash_Interest_EOP?: number;
    undrawn_Interest_EOP?: number;
    principal_EOP?: number;
    amount_Due_EOP?: number;
    occurred?: boolean;
    repayment_CashInterest?: number;
    repayment_PIKInterest?: number;
    repayment_Principal?: number;
    repayment_UndrawnFees?: number;
    drawdown?: number;
}