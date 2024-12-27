export class AssetAllocationModel {
    allocationId: number;
    assetId: number;
    userId: number;
    allocationDate: Date;
    returnDate: Date;
    allocationStatus: string;


    constructor() {
        this.allocationId = 0;
        this.assetId = 0;
        this.userId = 0;
        this.allocationDate = new Date();
        this.returnDate = new Date();
        this.allocationStatus = "";
    }
}