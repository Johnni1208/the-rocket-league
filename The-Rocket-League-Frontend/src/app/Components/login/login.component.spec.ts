import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginComponent } from './login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;

  beforeEach((() => {
    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        FormsModule,
        RouterTestingModule,
        HttpClientTestingModule],
      declarations: [LoginComponent],
    })
      .compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    component.ngOnInit();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('loginForm should be initialized OnInit', () => {
    expect(component.loginForm).toBeDefined();
  });

  it('loginForm should be invalid if nothing is given', () => {
    expect(component.loginForm.valid).toBeFalsy();
  });

  it('loginForm should be invalid if only one is given', () => {
    component.loginForm.controls['username'].setValue('test');
    expect(component.loginForm.valid).toBeFalsy();

    component.loginForm.controls['username'].setValue('');
    component.loginForm.controls['password'].setValue('test');
    expect(component.loginForm.valid).toBeFalsy();
  });

  it('loginForm should be valid if every field is given', () => {
    component.loginForm.controls['username'].setValue('test');
    component.loginForm.controls['password'].setValue('test');
    expect(component.loginForm.valid).toBeTruthy();
  });
});
