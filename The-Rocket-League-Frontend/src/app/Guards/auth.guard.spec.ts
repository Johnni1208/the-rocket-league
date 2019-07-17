import { fakeAsync, TestBed } from '@angular/core/testing';

import { AuthGuard } from './auth.guard';
import { AuthService } from '../Services/auth/auth.service';
import { environment } from '../../environments/environment';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { LoginComponent } from '../Components/login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertifyService } from '../Services/alertify/alertify.service';

describe('AuthGuard', () => {
  let authService: AuthService;
  let guard: AuthGuard;
  const router = {
    navigate: jasmine.createSpy('navigate')
  };
  const alertify = {
    error: jasmine.createSpy('error')
  };
  const testToken = environment.testToken;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AuthGuard, AuthService, { provide: Router, useValue: router }, { provide: AlertifyService, useValue: alertify }],
      imports: [
        ReactiveFormsModule,
        FormsModule,
        HttpClientTestingModule,
        RouterTestingModule.withRoutes([
          { path: 'login', component: LoginComponent }
        ]),
      ],
      declarations: [LoginComponent]
    });
    authService = TestBed.get(AuthService);
    guard = TestBed.get(AuthGuard);
  });

  afterEach(() => {
    localStorage.clear();
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });

  it('canActivate() should return true if the user is logged in', () => {
    authService.setAuthToken(testToken);
    expect(guard.canActivate()).toBeTruthy();
  });

  it('canActivate() should return false if the user isn\'t logged in', () => {
    expect(guard.canActivate()).toBeFalsy();
  });

  it('canActivate() should show alertify error message if the user isn\'t logged in', fakeAsync(() => {
    expect(guard.canActivate()).toBeFalsy();
    expect(alertify.error).toHaveBeenCalledWith('Du bist nicht eingeloggt!');
  }));

  it('canActivate() should navigate to /login if the user isn\'t logged in', fakeAsync(() => {
    expect(guard.canActivate()).toBeFalsy();
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  }));
});
