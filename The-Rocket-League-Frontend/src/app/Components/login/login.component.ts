import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../Services/auth/auth.service';
import { User } from '../../Models/user';
import { AlertifyService } from '../../Services/alertify/alertify.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  isLoading = false;

  constructor(private fb: FormBuilder,
              private authService: AuthService,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.createLoginForm();
  }

  createLoginForm() {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  login() {
    if (this.loginForm.valid) {
      const user: User = Object.assign({}, this.loginForm.value);

      this.isLoading = true;

      this.authService.login(user).subscribe(() => {
        this.alertify.success('Login successfully');
      }, error => {
        this.alertify.error('Es konnte nicht eingeloggt werden.');
      }, () => {
        this.isLoading = false;
      });
    }
  }
}
