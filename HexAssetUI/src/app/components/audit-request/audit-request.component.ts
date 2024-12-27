import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuditRequestModel } from '../../models/AuditRequest';
import { AuditRequestService } from '../../services/audit-request.service';
import { jwtDecode } from 'jwt-decode';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'app-audit-request',
  imports: [FormsModule],
  templateUrl: './audit-request.component.html',
  styleUrls: ['./audit-request.component.scss']
})
export class AuditRequestComponent implements OnInit {

  isAdmin: boolean = false;
  auditRequestList: AuditRequestModel[] = [];
  auditRequestObj: AuditRequestModel = new AuditRequestModel();
  auditRequestSrv = inject(AuditRequestService);
  toaster = inject(ToastrService);

  ngOnInit(): void {
    const token = localStorage.getItem('jwtToken');
    if (token) {
      const decodedToken: any = jwtDecode(token);
      this.isAdmin = decodedToken.role === 'Admin';
    }
    this.getAuditRequest();
  }

  getAuditRequest() {
    this.auditRequestSrv.getAuditRequest()
      .pipe(
        catchError((error) => {
          this.toaster.error('Failed to load audit requests. Please try again.', 'Error');
          return of([]); 
        })
      )
      .subscribe((result: any) => {
        this.auditRequestList = result;
      });
  }

  onEdit(data: AuditRequestModel) {
    this.auditRequestObj = { ...data }; 
  }

  onSaveAuditRequest() {
    this.auditRequestSrv.onSaveAuditRequest(this.auditRequestObj)
      .pipe(
        catchError((error) => {
          this.toaster.error('Failed to save the audit request. Please try again.', 'Error');
          return of(null); 
        })
      )
      .subscribe((result: any) => {
        if (result) {
          this.toaster.success('Audit request saved successfully', 'Success');
          this.getAuditRequest();
        }
      });
  }

  onUpdateAuditRequest() {
    this.auditRequestSrv.onUpdateAuditRequest(this.auditRequestObj)
      .pipe(
        catchError((error) => {
          this.toaster.error('Failed to update the audit request. Please try again.', 'Error');
          return of(null); 
        })
      )
      .subscribe((result: any) => {
        if (result) {
          this.toaster.info('Audit request updated successfully', 'Info');
          this.getAuditRequest();
        }
      });
  }

  deleteAuditRequest(id: number) {
    const isDelete = confirm('Are you sure you want to delete the audit request?');
    if (isDelete) {
      this.auditRequestSrv.onDeleteAuditRequest(id)
        .pipe(
          catchError((error) => {
            this.toaster.error('Failed to delete the audit request. Please try again.', 'Error');
            return of(null);
          })
        )
        .subscribe((result: any) => {
          if (result) {
            this.toaster.warning('Audit request deleted successfully', 'Warning');
            this.getAuditRequest();
          }
        });
    }
  }

  resetAudit() {
    this.auditRequestObj = new AuditRequestModel();
  }
}
