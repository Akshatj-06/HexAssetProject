import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { LoginService } from '../services/login.service';
import { ToastrService } from 'ngx-toastr';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-header',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'], 
})
export class HeaderComponent implements OnInit {
  loginSrv = inject(LoginService);
  router = inject(Router);
  toaster = inject(ToastrService);

  isLoggedIn: boolean = false;
  isAdmin: boolean = false;
  userId: number | null = null;
  username: string | null = null;

  ngOnInit(): void {
    this.updateUserState();

    // Subscribe to login state changes
    this.loginSrv.isLoggedIn$.subscribe((status) => {
      this.isLoggedIn = status;
      this.updateUserState(); // Recheck token details when login state changes
    });
  }

  updateUserState() {
    const token = localStorage.getItem('jwtToken');
    if (token) {
      const decodedToken: any = jwtDecode(token);
      this.isAdmin = decodedToken.role === 'Admin';
      this.userId = decodedToken.userId;
      this.username = decodedToken.Name;
      this.isLoggedIn = true;
    } else {
      this.isLoggedIn = false;
      this.isAdmin = false;
      this.userId = null;
      this.username = null;
    }
  }
  

  onLogout() {
    this.loginSrv.onLogout();
    this.isLoggedIn = false;
    this.toaster.success('Logged out Successfully', 'Success');
    this.router.navigateByUrl('login');
  }
}
