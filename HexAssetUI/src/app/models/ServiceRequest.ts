export class ServiceRequestModel {

    serviceRequestId: number;
    assetId: number;
    userId: number;
    description: string;
    requestStatus: string;
    requestDate: Date

    constructor() {
        this.serviceRequestId = 0;
        this.assetId = 0;
        this.userId = 0;
        this.description = ""
        this.requestStatus = "";
        this.requestDate = new Date();
    }
}