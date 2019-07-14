import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterComponent } from './register.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;

  beforeEach((() => {
    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        FormsModule,
        RouterTestingModule,
        HttpClientTestingModule],
      declarations: [RegisterComponent],
    })
      .compileComponents();

    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    component.ngOnInit();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('registerForm should be initialized OnInit', () => {
    expect(component.registerForm).toBeDefined();
  });

  it('registerForm should be invalid if nothing is given', () => {
    expect(component.registerForm.valid).toBeFalsy();
  });

  it('registerForm should be invalid if only one is given', () => {
    component.registerForm.controls['username'].setValue('test');
    expect(component.registerForm.valid).toBeFalsy();

    component.registerForm.controls['username'].setValue('');
    component.registerForm.controls['password'].setValue('test');
    expect(component.registerForm.valid).toBeFalsy();

    component.registerForm.controls['username'].setValue('');
    component.registerForm.controls['password'].setValue('');
    component.registerForm.controls['gamePin'].setValue('1234');
    expect(component.registerForm.valid).toBeFalsy();
  });

  it('registerForm should be valid if every field is given', () => {
    component.registerForm.controls['username'].setValue('test');
    component.registerForm.controls['password'].setValue('test');
    component.registerForm.controls['gamePin'].setValue('1234');
    expect(component.registerForm.valid).toBeTruthy();
  });
});
