export class AssetModel {
    assetId: number;
    assetName: string;
    assetCategory: string;
    assetModel: string;
    assetValue: number;
    currentStatus: string;

    constructor() {
        this.assetId = 0;
        this.assetName = "";
        this.assetCategory = "";
        this.assetModel = "";
        this.assetValue = 0;
        this.currentStatus = "";

    }
}