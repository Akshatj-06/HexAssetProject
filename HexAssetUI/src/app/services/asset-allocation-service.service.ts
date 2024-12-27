import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AssetAllocationModel } from '../models/AssetAllocation';

@Injectable({
  providedIn: 'root'
})
export class AssetAllocationServiceService {

  apiUrl: string = "https://localhost:7209/api/AssetAllocation/"

  constructor(private http:HttpClient) { }

  getAssetAllocation(){
    return this.http.get(this.apiUrl +"GetAssetAllocation")
  }

  onSaveAssetAllocation(obj: AssetAllocationModel){
    return this.http.post(this.apiUrl +"AddAssetAllocation",obj)
  }

  onUpdateAssetAllocation(obj: AssetAllocationModel){
    return this.http.put(this.apiUrl +"UpdateAssetAllocation/"+obj.allocationId,obj)
  }

  onDeleteAssetAllocation(id: number){
    return this.http.delete(this.apiUrl +"DeleteAssetAllocation/"+id)
  }

}
