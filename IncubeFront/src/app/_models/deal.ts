export interface Deal {
  dealId: number;
    alias: string;
    fund: string;
    signingDate: Date;
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