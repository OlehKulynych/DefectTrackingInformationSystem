import { Component } from '@angular/core';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent {
saveUser(_t12: any) {
throw new Error('Method not implemented.');
}
  id: any;
users: any;

}

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private usersUrl = 'https://example.com/users';

  constructor(private http: HttpClient) { }

  getUsers(): Observable<UsersComponent[]> {
    return this.http.get<UsersComponent[]>(this.usersUrl);
  }

  updateUser(user: UsersComponent): Observable<UsersComponent> {
    return this.http.put<UsersComponent>(`${this.usersUrl}/${user.id}`, user);
  }

}
