import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-user',
  imports: [],
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss'
})
export class UserComponent {

  userList: any[]=[];
  constructor(private http: HttpClient){

  }
  getUser(){
    this.http.get("https://localhost:7209/api/User/GetUser").subscribe((result:any)=>{
      this.userList= result;
    })
  }
}
