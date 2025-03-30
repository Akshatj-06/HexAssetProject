import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginModel } from '../models/Login';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  apiUrl: string = "https://localhost:7209/api/User/";

  private isLoggedInSubject = new BehaviorSubject<boolean>(!!localStorage.getItem('jwtToken'));
  isLoggedIn$: Observable<boolean> = this.isLoggedInSubject.asObservable();

  private isAdminSubject = new BehaviorSubject<boolean>(localStorage.getItem('userRole') === 'Admin');
  isAdmin$: Observable<boolean> = this.isAdminSubject.asObservable();

  constructor(private http: HttpClient) { }

  onLogin(obj: LoginModel) {
    return this.http.post<any>(this.apiUrl + "login", obj).pipe(
      tap((result) => {
        if (result) {
          localStorage.setItem('jwtToken', result.token);
          localStorage.setItem('userRole', result.role);
          localStorage.setItem('userName', result.username);

          this.isLoggedInSubject.next(true);
          this.isAdminSubject.next(result.role === 'Admin');
        }
      })
    );
  }

  onLogout() {
    localStorage.removeItem('jwtToken');
    localStorage.removeItem('userRole');
    localStorage.removeItem('userName');

    this.isLoggedInSubject.next(false);
    this.isAdminSubject.next(false);
  }
}
