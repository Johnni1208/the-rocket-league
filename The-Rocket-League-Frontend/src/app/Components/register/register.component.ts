import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../Services/auth/auth.service';
import { AlertifyService } from '../../Services/alertify/alertify.service';
import { User } from '../../Models/user';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  private registerForm: FormGroup;
  isLoading = false;


  constructor(private fb: FormBuilder,
              private authService: AuthService,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = this.fb.group({
      gamePin: ['', Validators.required],
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  register() {
    if (this.registerForm.valid) {
      const user: User = Object.assign({}, this.registerForm.value);

      this.isLoading = true;

      this.authService.register(user).subscribe(res => {
        this.alertify.success('Registered successfully');
      }, error => {
        this.alertify.error('Du konntest dich nicht registrieren, bitte versuche es nochmal.');
      }, () => {
        this.isLoading = false;
      });
    }
  }
}
