import { TestBed } from '@angular/core/testing';

import { AuthService } from './auth.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { CookieService } from 'ngx-cookie-service';
import { environment } from '../../../environments/environment';

describe('AuthService', () => {
  const testToken = environment.testToken;
  let service: AuthService;
  let cookieService: CookieService;
  const AUTH_TOKEN_COOKIE_NAME = 'authToken';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CookieService]
    });

    service = TestBed.get(AuthService);
    cookieService = TestBed.get(CookieService);
  });

  afterEach(() => {
    cookieService.deleteAll();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('setAuthToken() should set a token into the cookie space', () => {
    service.setAuthToken(testToken);
    expect(cookieService.get(AUTH_TOKEN_COOKIE_NAME)).toBe(testToken);
  });

  it('setAuthToken() should change the old authCookie to new values', () => {
    service.setAuthToken(testToken);
    service.setAuthToken('abcdef');
    expect(cookieService.get(AUTH_TOKEN_COOKIE_NAME)).toBe('abcdef');
  });

  it('deleteAuthToken() should delete the authToken in cookie space', () => {
    service.setAuthToken(testToken);
    service.deleteAuthToken();
    expect(cookieService.get(AUTH_TOKEN_COOKIE_NAME)).toBe('');
  });

  it('getAuthToken() should return the authToken cookie if it got set', () => {
    service.setAuthToken(testToken);
    expect(service.getAuthToken()).toBe(testToken);
  });

  it('getAuthToken() should return "" if no authToken is found in cookie space', () => {
    expect(service.getAuthToken()).toBe('');
  });
});
