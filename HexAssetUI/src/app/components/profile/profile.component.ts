import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user.service';
import { UserModel } from '../../models/User';
import { jwtDecode } from 'jwt-decode';


@Component({
  selector: 'app-profile',
  imports: [FormsModule, CommonModule],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  userId: number = 0; // Initialize userId as 0
  userProfile: UserModel = new UserModel(); // Use the UserModel class for userProfile
  userService = inject(UserService);
  toaster = inject(ToastrService);


  ngOnInit(): void {
    const token = localStorage.getItem('jwtToken');
    if (token) {
      const decodedToken: any = jwtDecode(token);
      this.userId = decodedToken.UserId; // Assign UserId from token
    }
    this.loadUserProfile();
  }
  

  // Method to load user profile
  loadUserProfile(): void {
    if (this.userId) {
      this.userService.getUserProfile(this.userId).pipe(
        catchError((error) => {
          this.toaster.error('Failed to load user profile. Please try again.', 'Error');
          console.error(error); // Helpful for debugging
          return of(null);
        })
      ).subscribe((response) => {
        if (response) {
          this.userProfile = response;  // Map the response to the userProfile object
        }
      });
    } else {
      this.toaster.error('User ID is missing or invalid.', 'Error');
    }
  }

  // Method to update user profile
  updateProfile(): void {
    if (this.userId) {
      this.userService.updateUserProfile(this.userId, this.userProfile).pipe(
        catchError((error) => {
          this.toaster.error('Failed to update profile. Please try again.', 'Error');
          
          return of(null);
        })
      ).subscribe((result) => {
        if (result) {
          this.toaster.success('Profile updated successfully!', 'Success');
        }
      });
    } else {
      this.toaster.error('User ID is missing or invalid.', 'Error');
    }
  }

}
