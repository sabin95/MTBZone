import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserLogin } from './models/userLogin.model';
import { UserRegister } from './models/userRegister.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7110/api/';
  constructor(private http: HttpClient) { }

  login(user: UserLogin): Observable<any> {
    const url = `${this.apiUrl}identity/generateToken`;
    return this.http.post(url, user);
  }

  register(user: UserRegister): Observable<any> {
    const url = `${this.apiUrl}identity/register`;
    return this.http.post(url, user);
  }   
}