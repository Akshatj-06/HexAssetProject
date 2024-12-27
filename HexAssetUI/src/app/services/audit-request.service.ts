import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuditRequestModel } from '../models/AuditRequest';

@Injectable({
  providedIn: 'root'
})
export class AuditRequestService {

  apiUrl: string = "https://localhost:7209/api/AuditRequest/"

  constructor(private http:HttpClient) { }

  getAuditRequest(){
    return this.http.get(this.apiUrl +"GetAuditRequest")
  }

  onSaveAuditRequest(obj: AuditRequestModel){
    return this.http.post(this.apiUrl +"AddAuditRequest",obj)
  }

  onUpdateAuditRequest(obj: AuditRequestModel){
    return this.http.put(this.apiUrl +"UpdateAuditRequest/"+obj.auditId,obj)
  }

  onDeleteAuditRequest(id: number){
    return this.http.delete(this.apiUrl +"DeleteAuditRequest/"+id)
  }
}
