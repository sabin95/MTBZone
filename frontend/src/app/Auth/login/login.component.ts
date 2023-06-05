import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { State } from '../authentication.reducer';
import { Store } from '@ngrx/store';
import * as authenticationActions from '../authentication.actions';
import { UserLogin } from '../models/userLogin.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less']
})
export class LoginComponent implements OnInit {
  hide = true;
  loginForm: FormGroup;

  constructor(private store: Store<State>) { } 

  ngOnInit() {
    this.loginForm = new FormGroup({
      'email': new FormControl(null, [Validators.required, Validators.email]),
      'password': new FormControl(null, Validators.required),
    });
  }

  onSubmit() {
    console.log(this.loginForm.value); // This should print an object containing email and password
    if (this.loginForm.valid) {
      const { email, password } = this.loginForm.value;
      const user: UserLogin = { email, password };
      this.store.dispatch(authenticationActions.login({user}));
    }
  }
}
