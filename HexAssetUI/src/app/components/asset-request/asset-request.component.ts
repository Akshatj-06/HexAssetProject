import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AssetRequestModel } from '../../models/AssetRequest';
import { AssetRequestService } from '../../services/asset-request.service';
import { jwtDecode } from 'jwt-decode';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-asset-request',
  imports: [FormsModule, CommonModule],
  templateUrl: './asset-request.component.html',
  styleUrls: ['./asset-request.component.scss']
})
export class AssetRequestComponent implements OnInit {

  isAdmin: boolean = false;
  assetRequestList: AssetRequestModel[] = [];
  assetRequestObj: AssetRequestModel = new AssetRequestModel();
  assetRequestSrv = inject(AssetRequestService);
  toaster = inject(ToastrService);

  ngOnInit(): void {
    const token = localStorage.getItem('jwtToken');
    if (token) {
      const decodedToken: any = jwtDecode(token);
      this.isAdmin = decodedToken.role === 'Admin';
    }
    this.getAssetRequest();
  }

  getAssetRequest() {
    this.assetRequestSrv.getAssetRequest()
      .pipe(
        catchError((error) => {
          this.toaster.error('Failed to load asset requests. Please try again.', 'Error');
          return of([]); // Return an empty array on error
        })
      )
      .subscribe((result: any) => {
        this.assetRequestList = result;
      });
  }

  onEdit(data: AssetRequestModel) {
    this.assetRequestObj = { ...data };
  }

  onSaveAssetRequest() {
    this.assetRequestSrv.onSaveAssetRequest(this.assetRequestObj)
      .pipe(
        catchError((error) => {
          this.toaster.error('Failed to save the asset request. Please try again.', 'Error');
          return of(null); 
        })
      )
      .subscribe((result: any) => {
        if (result) {
          this.toaster.success('Asset Request Saved Successfully', 'Success');
          this.getAssetRequest();
        }
      });
  }

  onUpdateAssetRequest() {
    this.assetRequestSrv.onUpdateAssetRequest(this.assetRequestObj)
      .pipe(
        catchError((error) => {
          this.toaster.error('Failed to update the asset request. Please try again.', 'Error');
          return of(null); 
        })
      )
      .subscribe((result: any) => {
        if (result) {
          this.toaster.info('Asset Request Updated Successfully', 'Info');
          this.getAssetRequest();
        }
      });
  }

  deleteAssetRequest(id: number) {
    const isDelete = confirm('Are you sure you want to delete the asset request?');
    if (isDelete) {
      this.assetRequestSrv.onDeleteAssetRequest(id)
        .pipe(
          catchError((error) => {
            this.toaster.error('Failed to delete the asset request. Please try again.', 'Error');
            return of(null); 
          })
        )
        .subscribe((result: any) => {
          if (result) {
            this.toaster.warning('Asset Request Deleted Successfully', 'Warning');
            this.getAssetRequest();
          }
        });
    }
  }

  resetAssetRequest() {
    this.assetRequestObj = new AssetRequestModel();
  }
}
