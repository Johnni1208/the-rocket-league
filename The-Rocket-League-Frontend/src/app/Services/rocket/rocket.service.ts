import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {AuthService} from '../auth/auth.service';
import {JwtHelperService} from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class RocketService {

  private baseUrl: string = environment.baseUrl + '/rocket';

  constructor(private http: HttpClient, private authService: AuthService) {
  }

  addRocket() {
    const token = AuthService.getAuthToken();
    const jwtHelper = new JwtHelperService();
    const userId = jwtHelper.decodeToken(token).NameIdentifier;
    return this.http.post(this.baseUrl + '/' + userId, null);
  }
}
