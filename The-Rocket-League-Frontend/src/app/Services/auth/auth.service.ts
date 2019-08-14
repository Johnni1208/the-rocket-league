import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { User } from '../../Models/user';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly AUTH_TOKEN_KEY = environment.authTokenKey;
  private baseUrl = environment.baseUrl + '/auth';

  constructor(private http: HttpClient) { }

  static getAuthToken(): string {
    return localStorage.getItem(environment.authTokenKey);
  }

  static getDecodedToken(): any {
    const token = localStorage.getItem(environment.authTokenKey);
    const jwtHelper = new JwtHelperService();
    return jwtHelper.decodeToken(token);
  }

  login(user: User): Observable<any> {
    return this.http.post(this.baseUrl + '/login', user)
      .pipe(
        map((response: any) => {
          if (response) {
            this.setAuthToken(response.token);
          }
        })
      );
  }

  logout(): void {
    localStorage.removeItem(this.AUTH_TOKEN_KEY);
  }

  register(user: User): Observable<any> {
    return this.http.post(this.baseUrl + '/register', user);
  }

  setAuthToken(token: string) {
    localStorage.setItem(this.AUTH_TOKEN_KEY, token);
  }

  isLoggedIn(): boolean {
    const jwtHelper = new JwtHelperService();

    const token = localStorage.getItem(this.AUTH_TOKEN_KEY);
    if (token) {
      return !jwtHelper.isTokenExpired(token);
    }

    return false;
  }
}
