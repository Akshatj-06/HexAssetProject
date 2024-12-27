import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginModel } from '../models/Login';
import { LoginService } from '../services/login.service';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'] 
})
export class LoginComponent {

  loginObj: LoginModel = new LoginModel();
  loginSrv = inject(LoginService);
  router = inject(Router);
  toaster = inject(ToastrService);

  onLogin() {
    this.loginSrv.onLogin(this.loginObj)
      .pipe(
        catchError((error) => {
       
          if (error.status === 401) {
            this.toaster.error('Invalid credentials. Please try again.', 'Login Failed');
          } else if (error.status === 400) {
            this.toaster.error('Bad Request. Please check your inputs.', 'Login Failed');
          } else {
            this.toaster.error('Something went wrong. Please try later.', 'Login Error');
          }
       
          return of(null);
        })
      )
      .subscribe((result: any) => {
        if (result) {
          this.toaster.success('Logged in Successfully', 'Success');
          localStorage.setItem('jwtToken', result.token);
          this.router.navigateByUrl('header/Home');
        }
      });
  }

 

  onForgotPassword() {
    this.router.navigateByUrl('forgot-password');
  }
}
