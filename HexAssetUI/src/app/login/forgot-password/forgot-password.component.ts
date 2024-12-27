import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms'; 
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [FormsModule], 
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss'],
})
export class ForgotPasswordComponent {
  email: string = '';
  newPassword: string = '';
  userSrv = inject(UserService);
  router = inject(Router);
  toaster= inject(ToastrService)

  onForgotPassword() {

    if (!this.email || !this.newPassword) {
      alert('Please enter both email and new password.');
      return;
    }

    this.userSrv.onForgotPassword(this.email, this.newPassword).subscribe({
      next: () => {
        this.toaster.success('Password Updated Successfully');
        this.router.navigateByUrl('login');
      },
      error: (err) => {
        console.error(err);
        this.toaster.error('Failed to update password.');
      },
    });
  }
}
