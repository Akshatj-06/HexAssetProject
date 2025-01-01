export class AssetRequestModel {
  assetRequestId: number;
  assetId: number;
  userId: number;
  userName: string;
  item: string;
  requestStatus: string;
  requestDate: Date;

  constructor() {
    this.assetRequestId = 0;
    this.assetId = 0;
    this.userId = 0;
    this.userName = "";
    this.item = "";
    this.requestStatus = "";
    this.requestDate = new Date();
  }
}