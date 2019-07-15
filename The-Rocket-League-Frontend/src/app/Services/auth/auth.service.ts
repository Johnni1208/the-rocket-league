import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { User } from '../../Models/user';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = environment.baseUrl + '/auth';
  private readonly AUTH_TOKEN_COOKIE_NAME = 'authToken';

  constructor(private http: HttpClient, private cookieService: CookieService) { }

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

  register(user: User): Observable<any> {
    return this.http.post(this.baseUrl + '/register', user);
  }

  setAuthToken(token: string) {
    this.cookieService.set(this.AUTH_TOKEN_COOKIE_NAME, token);
  }

  deleteAuthToken() {
    this.cookieService.delete(this.AUTH_TOKEN_COOKIE_NAME);
  }

  getAuthToken(): string {
    return this.cookieService.get(this.AUTH_TOKEN_COOKIE_NAME);
  }
}
