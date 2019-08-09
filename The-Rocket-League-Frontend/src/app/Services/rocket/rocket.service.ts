import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { AuthService } from '../auth/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';
import { Rocket } from '../../Models/rocket';

@Injectable({
  providedIn: 'root'
})
export class RocketService {

  private baseUrl: string = environment.baseUrl + '/rockets';

  constructor(private http: HttpClient) { }

  addRocket(): Observable<any> {
    const token = AuthService.getAuthToken();
    const jwtHelper = new JwtHelperService();
    const userId = jwtHelper.decodeToken(token).nameid;
    return this.http.post(this.baseUrl + '/' + userId, '');
  }

  getAllRockets(): Observable<Rocket[]> {
    return this.http.get<Rocket[]>(this.baseUrl);
  }
}
