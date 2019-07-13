import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { User } from '../../Models/user';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = environment.baseUrl + '/auth';

  constructor(private http: HttpClient) { }

  login(user: User): Observable<any> {
    return this.http.post(this.baseUrl + '/login', user);
  }

}
