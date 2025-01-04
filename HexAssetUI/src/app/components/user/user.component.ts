import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {

  userList: any[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getUser();
  }

  getUser(): void {
    this.http.get("https://localhost:7209/api/User/GetUser").subscribe(
      (result: any) => {
        this.userList = result;
      },
      (error) => {
        console.error("Failed to fetch user data:", error);
      }
    );
  }
}
