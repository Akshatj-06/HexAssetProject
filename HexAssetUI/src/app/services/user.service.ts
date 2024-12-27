import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl: string = "https://localhost:7209/api/User/";

  constructor(private http: HttpClient) { }

  onForgotPassword(email: string, newPassword: string) {
    return this.http.post(this.apiUrl + 'ForgotPassword', {
      email: email,
      newPassword: newPassword,
    });
  }
}
