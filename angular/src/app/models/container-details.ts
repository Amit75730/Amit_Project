interface DemurrageFees {
    FeesDue: number;
    FeesPaid: number;
  }
  export interface containerdetails {
    id: string;
    containerNumber: string;
    tradeType: string;
    status: string;
    origin: string;
    vesselCode: number;
    vesselName: string;
    flightNumber: string;
    transactionSetControlNumber: number;
    demurrage_fees?: DemurrageFees;
    //demurrage_fees?: number;
  }