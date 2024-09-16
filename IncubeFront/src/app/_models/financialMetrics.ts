export interface FinancialMetrics{
    metrics_Id: number;
    deal_Id: number;
    deal: number;
    transaction_Id: number;
    nav: number;
    nav_irr: number;
    nav_moic: number;
    nav_profit: number;
    total_Collections: number;
    total_Invested: number;
    interest_Generated: number;
    accrued_Cash_Interest: number;
    accrued_Pik_Interest: number;
    accrued_Undrawn_Interest: number;
    interest_Payed: number;
    total_Debt: number;
    facility: number;
    undrawn_Amount: number;
}