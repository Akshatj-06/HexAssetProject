import { Component, inject, OnInit } from '@angular/core';
import { AssetService } from '../../services/asset.service';
import { AssetModel } from '../../models/Asset';
import { FormsModule } from '@angular/forms';
import { jwtDecode } from 'jwt-decode';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'app-asset',
  imports: [FormsModule, CommonModule],
  templateUrl: './asset.component.html',
  styleUrls: ['./asset.component.scss']
})
export class AssetComponent implements OnInit {

  isAdmin: boolean = false;
  assetList: AssetModel[] = [];
  filteredAssets: AssetModel[] = [];
  paginatedAssets: AssetModel[] = [];
  assetObj: AssetModel = new AssetModel();
  currentPage: number = 1;
  pageSize: number = 6;
  selectedCategory: string = ''; 
  showSearch: boolean = false;

  assetSrv = inject(AssetService);
  toaster = inject(ToastrService);

  ngOnInit(): void {
    const token = localStorage.getItem('jwtToken');
    if (token) {
      const decodedToken: any = jwtDecode(token);
      this.isAdmin = decodedToken.role === 'Admin';
    }
    this.getAsset();
  }

  getAsset() {
    this.assetSrv.getAsset()
      .pipe(
        catchError((error) => {
          this.toaster.error('Failed to load assets. Please try again.', 'Error');
          return of([]);
        })
      )
      .subscribe((result: any) => {
        this.assetList = result;
        this.filteredAssets = [...this.assetList]; // Initialize filteredAssets with all assets
        this.updatePaginatedAssets();
      });
  }

  // Method to filter assets based on the selected category
  filterAssets() {
    if (this.selectedCategory) {
      this.filteredAssets = this.assetList.filter(asset => asset.assetCategory === this.selectedCategory);
    } else {
      this.filteredAssets = [...this.assetList]; // If no category selected, show all assets
    }
    this.currentPage = 1; // Reset to the first page after filtering
    this.updatePaginatedAssets();
  }

  get totalPages(): number {
    return Math.ceil(this.filteredAssets.length / this.pageSize);
  }

  updatePaginatedAssets() {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedAssets = this.filteredAssets.slice(startIndex, endIndex);
  }

  nextPage() {
    if ((this.currentPage * this.pageSize) < this.filteredAssets.length) {
      this.currentPage++;
      this.updatePaginatedAssets();
    }
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePaginatedAssets();
    }
  }

  onEdit(data: AssetModel) {
    this.assetObj = { ...data }; 
  }

  onSaveAsset() {
    this.assetSrv.onSaveAsset(this.assetObj)
      .pipe(
        catchError((error) => {
          if (error.status === 403) {
            this.toaster.error('You are not authorized to perform this action', 'Authorization Error');
          } else {
            this.toaster.error('Failed to save the asset. Please try again.', 'Error');
          }
          return of(null);
        })
      )
      .subscribe((result: any) => {
        if (result) {
          this.toaster.success('Asset saved successfully', 'Success');
          this.getAsset();
        }
      });
  }

  onUpdateAsset() {
    this.assetSrv.onUpdateAsset(this.assetObj)
      .pipe(
        catchError((error) => {
          this.toaster.error('Failed to update the asset. Please try again.', 'Error');
          return of(null);
        })
      )
      .subscribe((result: any) => {
        if (result) {
          this.toaster.info('Asset updated successfully', 'Info');
          this.getAsset();
        }
      });
  }

  deleteAsset(id: number) {
    const isDelete = confirm('Are you sure you want to delete the asset?');
    if (isDelete) {
      this.assetSrv.onDeleteAsset(id)
        .pipe(
          catchError((error) => {
            this.toaster.error('Failed to delete the asset. Please try again.', 'Error');
            return of(null);
          })
        )
        .subscribe((result: any) => {
          if (result) {
            this.toaster.warning('Asset deleted successfully', 'Warning');
            this.getAsset();
          }
        });
    }
  }

  resetAsset() {
    this.assetObj = new AssetModel();
  }

  toggleSearch() {
    this.showSearch = !this.showSearch;
  }

  getAssetImageUrl(assetId: number): string {
    return `assets/images/${assetId}.jpg`; 
}
}
