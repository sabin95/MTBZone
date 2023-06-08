import { createAction, props } from '@ngrx/store';
import { UserLogin } from './models/userLogin.model';
import { UserRegister } from './models/userRegister.model';

export const login = createAction(
  '[Identity] Login',
  props<{ user: UserLogin }>()
);

export const loginSuccess = createAction(
  '[Identity] Login Success',
  props<{ token: string }>()
);

export const loginFailure = createAction(
  '[Identity] Login Failure',
  props<{ error: any }>()
);

export const register = createAction(
  '[Identity] Register',
  props<{ user: UserRegister }>()
);

export const registerSuccess = createAction(
  '[Identity] Register Success',
  props<{ token: string }>()
);

export const registerFailure = createAction(
  '[Identity] Register Failure',
  props<{ error: any }>()
);
  