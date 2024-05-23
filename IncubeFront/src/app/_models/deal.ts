export interface Deal {
    deal_Id: number;
    deal_Name: string;
    fund: string;
    investment_date: Date;
    borrowerOne: string;
    borrowerTwo?: string; 
    ownershipOne: string;
    percentOwnershipOne: string;
    typeInvestment: string;
    typeInvestor: string;
    strategy: string;
    sector: string;
    subSector: string;
    navType: string;
    nationalAmount: string;
    guarantee?: string;
    description: string;
    status: string;
    dealPerformance: string;
  }