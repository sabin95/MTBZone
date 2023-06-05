import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserLogin } from './models/userLogin.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) { }

  login(user: UserLogin): Observable<{ token: string }> {
    return this.http.post<{ token: string }>('https://localhost:7110/api/identity/generateToken', user);
  }
  
}
