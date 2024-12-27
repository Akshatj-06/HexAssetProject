import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ServiceRequestModel } from '../models/ServiceRequest';

@Injectable({
  providedIn: 'root'
})
export class ServiceRequestService {

  apiUrl: string ="https://localhost:7209/api/ServiceRequest/"

  constructor(private http:HttpClient) { }

  getServiceRequest(){
    return this.http.get(this.apiUrl +"GetServiceRequest")
  }

  onSaveServiceRequest(obj: ServiceRequestModel){
    return this.http.post(this.apiUrl+"AddServiceRequest",obj)
  }

  onUpdateServiceRequest(obj: ServiceRequestModel){
    return this.http.put(this.apiUrl +"UpdateServiceRequest/"+obj.serviceRequestId,obj)
  }

  onDeleteServiceRequest(id: number){
    return this.http.delete(this.apiUrl +"DeleteServiceRequest/"+id)
  }
}
