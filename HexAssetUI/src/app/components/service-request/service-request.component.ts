import { Component, inject, OnInit } from '@angular/core';
import { ServiceRequestModel } from '../../models/ServiceRequest';
import { ServiceRequestService } from '../../services/service-request.service';
import { FormsModule } from '@angular/forms';
import { jwtDecode } from 'jwt-decode';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-service-request',
  imports: [FormsModule],
  templateUrl: './service-request.component.html',
  styleUrls: ['./service-request.component.scss']
})
export class ServiceRequestComponent implements OnInit {

  isAdmin: boolean = false;
  serviceRequestList: ServiceRequestModel[] = [];
  serviceRequestObj: ServiceRequestModel = new ServiceRequestModel();
  serviceRequestSrv = inject(ServiceRequestService);
  toaster = inject(ToastrService);

  ngOnInit(): void {
    const token = localStorage.getItem('jwtToken');
    if (token) {
      const decodedToken: any = jwtDecode(token);
      this.isAdmin = decodedToken.role === 'Admin'; 
    }
    this.getServiceRequest();
  }

 
  getServiceRequest() {
    this.serviceRequestSrv.getServiceRequest()
      .pipe(
        catchError((error) => {
          this.toaster.error('Failed to load service requests. Please try again.', 'Error');
          return of([]); 
        })
      )
      .subscribe((result: any) => {
        this.serviceRequestList = result;
      });
  }


  onEdit(data: ServiceRequestModel) {
    this.serviceRequestObj = data;
  }


  onSaveServiceRequest() {
    this.serviceRequestSrv.onSaveServiceRequest(this.serviceRequestObj)
      .pipe(
        catchError((error) => {
          this.toaster.error('Failed to save the service request. Please try again.', 'Error');
          return of(null); 
        })
      )
      .subscribe((result: any) => {
        if (result) {
          this.toaster.success('Service Request Saved Successfully', 'Success');
          this.getServiceRequest(); 
        }
      });
  }


  onUpdateServiceRequest() {
    this.serviceRequestSrv.onUpdateServiceRequest(this.serviceRequestObj)
      .pipe(
        catchError((error) => {
          this.toaster.error('Failed to update the service request. Please try again.', 'Error');
          return of(null); 
        })
      )
      .subscribe((result: any) => {
        if (result) {
          this.toaster.info('Service Request Updated Successfully', 'Info');
          this.getServiceRequest(); 
        }
      });
  }


  deleteServiceRequest(id: number) {
    const isDelete = confirm('Are you sure you want to delete the Service Request?');
    if (isDelete) {
      this.serviceRequestSrv.onDeleteServiceRequest(id)
        .pipe(
          catchError((error) => {
            this.toaster.error('Failed to delete the service request. Please try again.', 'Error');
            return of(null); 
          })
        )
        .subscribe((result: any) => {
          if (result) {
            this.toaster.warning('Service Request Deleted Successfully', 'Warning');
            this.getServiceRequest(); 
          }
        });
    }
  }


  resetServiceRequest() {
    this.serviceRequestObj = new ServiceRequestModel();
  }
}
