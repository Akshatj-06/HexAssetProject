export class AssetRequestModel {
  assetRequestId: number;
  assetId: number;
  userId: number;
  item: string;
  requestStatus: string;
  requestDate: Date;

  constructor() {
    this.assetRequestId = 0;
    this.assetId = 0;
    this.userId = 0;
    this.item = "";
    this.requestStatus = "";
    this.requestDate = new Date();
  }
}