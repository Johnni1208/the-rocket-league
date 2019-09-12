import { TestBed } from '@angular/core/testing';

import { AuthService } from './auth.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { environment } from '../../../environments/environment';

describe('AuthService', () => {
  const testToken = environment.testToken;
  let service: AuthService;
  const AUTH_TOKEN_KEY = environment.authTokenKey;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });

    service = TestBed.get(AuthService);
  });

  afterEach(() => {
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('logout() should delete the authToken in localstorage', () => {
    service.setAuthToken(testToken);
    service.logout();
    expect(localStorage.getItem(AUTH_TOKEN_KEY)).toBe(null);
  });

  describe('setAuthToken()', () => {
    it('should set a token into the localstorage', () => {
      service.setAuthToken(testToken);
      expect(localStorage.getItem(AUTH_TOKEN_KEY)).toBe(testToken);
    });

    it('should change the old authToken to new values', () => {
      service.setAuthToken(testToken);
      service.setAuthToken('abcdef');
      expect(localStorage.getItem(AUTH_TOKEN_KEY)).toBe('abcdef');
    });
  });

  describe('getAuthToken()', () => {
    it('should return the authToken if it got set', () => {
      service.setAuthToken(testToken);
      expect(AuthService.getAuthToken()).toBe(testToken);
    });

    it('should return null if no authToken is found in localstorage', () => {
      expect(AuthService.getAuthToken()).toBe(null);
    });
  });

  describe('getDecodedToken()', () => {
    it('should return the decoded authToken if it got set', () => {
      service.setAuthToken(testToken);
      const decodedToken = AuthService.getDecodedToken();
      expect(decodedToken === testToken).toBeFalsy();
    });

    it('should return null if no authToken is found in localstorage', () => {
      expect(AuthService.getDecodedToken()).toBe(null);
    });
  });

  describe('isLoggedIn()', () => {
    it('should return true if there is any valid authToken', () => {
      service.setAuthToken(testToken);
      expect(service.isLoggedIn()).toBeTruthy();
    });

    it('should return false if there isn\'t any valid authToken', () => {
      expect(service.isLoggedIn()).toBeFalsy();
    });
  });
});
