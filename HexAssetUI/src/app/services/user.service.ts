import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserModel } from '../models/User';
import { Observable } from 'rxjs';

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
  getUserProfile(userId: number): Observable<UserModel> {
    return this.http.get<UserModel>(`${this.apiUrl}GetUserById/${userId}`);
  }
  

  updateUserProfile(userId: number, updatedUserData: any) {
    return this.http.put(`${this.apiUrl}UpdateUser/${userId}`, updatedUserData);
  }
}
