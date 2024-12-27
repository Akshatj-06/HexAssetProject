import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AssetModel } from '../models/Asset';

@Injectable({
  providedIn: 'root'
})
export class AssetService {

  apiUrl: string = "https://localhost:7209/api/Asset/"

  constructor(private http:HttpClient) { }

  getAsset(){
    return this.http.get(this.apiUrl +"GetAsset")
  }

  onSaveAsset(obj: AssetModel){
    return this.http.post(this.apiUrl +"AddAsset",obj)
  }

  onUpdateAsset(obj: AssetModel){
    return this.http.put(this.apiUrl +"UpdateAsset/"+obj.assetId,obj)
  }

  onDeleteAsset(id: number){
    return this.http.delete(this.apiUrl +"DeleteAsset/"+id)
  }
}
