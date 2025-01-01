export class ServiceRequestModel {

    serviceRequestId: number;
    assetId: number;
    userId: number;
    userName: string;
    description: string;
    requestStatus: string;
    requestDate: Date

    constructor() {
        this.serviceRequestId = 0;
        this.assetId = 0;
        this.userId = 0;
        this.userName = "";
        this.description = ""
        this.requestStatus = "";
        this.requestDate = new Date();
    }
}