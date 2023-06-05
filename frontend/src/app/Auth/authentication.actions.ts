import { createAction, props } from '@ngrx/store';
import { UserLogin } from './models/userLogin.model';

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
