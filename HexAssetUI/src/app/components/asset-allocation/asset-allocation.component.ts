import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AssetAllocationModel } from '../../models/AssetAllocation';
import { AssetAllocationServiceService } from '../../services/asset-allocation-service.service';
import { jwtDecode } from 'jwt-decode';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-asset-allocation',
  imports: [FormsModule, CommonModule],
  templateUrl: './asset-allocation.component.html',
  styleUrl: './asset-allocation.component.scss'
})
export class AssetAllocationComponent {

  isAdmin: boolean = false;
  assetAllocationList: AssetAllocationModel[] = [];
  assetAllocationObj: AssetAllocationModel = new AssetAllocationModel();
  assetAllocationSrv = inject(AssetAllocationServiceService);
  toaster = inject(ToastrService);

  ngOnInit(): void {
    const token = localStorage.getItem('jwtToken');
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);
        this.isAdmin = decodedToken.role === 'Admin';
      } catch (error) {
        this.toaster.error('Failed to decode token. Please log in again.', 'Error');
        console.error('Token decode error:', error);
      }
    }
    this.getAssetAllocation();
  }

  getAssetAllocation() {
    this.assetAllocationSrv.getAssetAllocation().subscribe({
      next: (result: any) => {
        this.assetAllocationList = result;
      },
      error: (error: any) => {
        this.toaster.error('Failed to fetch asset allocation data.', 'Error');
        console.error('Error fetching asset allocations:', error);
      }
    });
  }

  onEdit(data: AssetAllocationModel) {
    this.assetAllocationObj = { ...data };
  }

  onSaveAssetAllocation() {
    this.assetAllocationSrv.onSaveAssetAllocation(this.assetAllocationObj).subscribe({
      next: (result: any) => {
        this.toaster.success('Asset allocated successfully', 'Success');
        this.getAssetAllocation();
        this.resetAssetAllocation();
      },
      error: (error: any) => {
        this.toaster.error('Failed to allocate asset.', 'Error');
        console.error('Error saving asset allocation:', error);
      }
    });
  }

  onUpdateAssetAllocation() {
    this.assetAllocationSrv.onUpdateAssetAllocation(this.assetAllocationObj).subscribe({
      next: (result: any) => {
        this.toaster.info('Asset allocation updated successfully', 'Info');
        this.getAssetAllocation();
        this.resetAssetAllocation();
      },
      error: (error: any) => {
        this.toaster.error('Failed to update asset allocation.', 'Error');
        console.error('Error updating asset allocation:', error);
      }
    });
  }

  deleteAssetAllocation(id: number) {
    const isDelete = confirm('Are you sure you want to delete the asset allocation?');
    if (isDelete) {
      this.assetAllocationSrv.onDeleteAssetAllocation(id).subscribe({
        next: (result: any) => {
          this.toaster.warning('Asset allocation deleted successfully', 'Warning');
          this.getAssetAllocation();
        },
        error: (error: any) => {
          this.toaster.error('Failed to delete asset allocation.', 'Error');
          console.error('Error deleting asset allocation:', error);
        }
      });
    }
  }

  resetAssetAllocation() {
    this.assetAllocationObj = new AssetAllocationModel();
  }
}
