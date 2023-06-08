import { createReducer, on } from '@ngrx/store';
import * as authenticationActions from './authentication.actions';

export interface State {
  token: string | null;
  error: any | null;
}

export const initialState: State = {
  token: null,
  error: null,
};

export const identityReducer = createReducer(
initialState,
on(authenticationActions.loginSuccess, (state, { token }) => ({ 
    ...state, 
    token 
})),
on(authenticationActions.loginFailure, (state, { error }) => ({ 
    ...state, 
    error 
})),
on(authenticationActions.registerSuccess, (state, { token }) => ({ 
    ...state, 
token 
})),
on(authenticationActions.registerFailure, (state, { error }) => ({ 
    ...state, 
    error 
}))
);
  