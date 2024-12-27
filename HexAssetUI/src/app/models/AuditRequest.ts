export class AuditRequestModel {
    auditId: number;
    userId: 0;
    allocationId: 0;
    item: string;
    auditStatus: string;
    auditDate: Date

    constructor() {
        this.auditId = 0;
        this.userId = 0;
        this.allocationId=0;
        this.item = "";
        this.auditStatus = "";
        this.auditDate = new Date();
    }
}