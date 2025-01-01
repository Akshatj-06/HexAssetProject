import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AssetRequestModel } from '../models/AssetRequest';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AssetRequestService {

  apiUrl: string ="https://localhost:7209/api/AssetRequest/"

  

  constructor(private http:HttpClient) { }

  getAssetRequest(): Observable<AssetRequestModel[]> {
    const token = localStorage.getItem('jwtToken');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<AssetRequestModel[]>(`${this.apiUrl}GetAssetRequest`, { headers });
}

  

  onSaveAssetRequest(obj: AssetRequestModel){
    return this.http.post(this.apiUrl+"AddAssetRequest",obj)
  }

  onUpdateAssetRequest(obj: AssetRequestModel){
    return this.http.put(this.apiUrl +"UpdateAssetRequest/"+obj.assetRequestId,obj)
  }

  onDeleteAssetRequest(id: number){
    return this.http.delete(this.apiUrl +"DeleteAssetRequest/"+id)
  }
}
