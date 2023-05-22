import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from './models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) { }

  login(user: User) {
    return this.http.post<{ token: string }>('https://localhost:7124/api/identity/token', user);
  }
}
