import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as authenticationActions from '../authentication.actions';
import { UserLogin } from '../models/userLogin.model';
import { UserRegister } from '../models/userRegister.model';
import { State } from '../authentication.reducer';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.less']
})
export class AuthenticationComponent implements OnInit {
  hide = true;
  loginForm: FormGroup;
  registerForm: FormGroup;

  constructor(private store: Store<State>) { }

  ngOnInit() {
    this.loginForm = new FormGroup({
      'email': new FormControl(null, [Validators.required, Validators.email]),
      'password': new FormControl(null, Validators.required),
    });

    this.registerForm = new FormGroup({
      'name': new FormControl(null, Validators.required),
      'email': new FormControl(null, [Validators.required, Validators.email]),
      'password': new FormControl(null, Validators.required),
    });
  }

  onLoginSubmit() {
    if (this.loginForm.valid) {
      const { email, password } = this.loginForm.value;
      const user: UserLogin = { email, password };
      this.store.dispatch(authenticationActions.login({ user }));
    }
  }

  onRegisterSubmit() {
    if (this.registerForm.valid) {
      const { name, email, password } = this.registerForm.value;
      const user: UserRegister = { name, email, password };
      this.store.dispatch(authenticationActions.register({ user }));
    }
  }
}
