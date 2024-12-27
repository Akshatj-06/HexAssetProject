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

  isAdmin: boolean = false;
  userId: number | null = null;

  ngOnInit(): void {
    const token = localStorage.getItem('jwtToken');
    if (token) {
      const decodedToken: any = jwtDecode(token);
      this.isAdmin = decodedToken.role === 'Admin';
      this.userId = decodedToken.userId; 
    }
  }

  onLogout() {
    this.loginSrv.onLogout();
    this.toaster.success('Logged out Successfully', 'Success');
    this.router.navigateByUrl('login');
  }
}
