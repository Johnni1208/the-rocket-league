import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../Services/auth/auth.service';
import { AlertifyService } from '../../Services/alertify/alertify.service';
import { User } from '../../Models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  isLoading = false;


  constructor(private fb: FormBuilder,
              private authService: AuthService,
              private alertify: AlertifyService,
              private router: Router) { }

  ngOnInit() {
    this.authService.logout();
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
        this.alertify.success('Erfolgreich registriert');
        this.router.navigate(['/login']);
      }, error => {
        this.alertify.error('Du konntest dich nicht registrieren, bitte versuche es nochmal.');
        this.isLoading = false;
      }, () => {
        this.isLoading = false;
      });
    }
  }
}
