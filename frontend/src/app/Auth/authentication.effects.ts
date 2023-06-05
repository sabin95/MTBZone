import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { AuthService } from './AuthService.service';
import * as authenticationActions from './authentication.actions';

@Injectable()
export class IdentityEffects {
  constructor(private actions$: Actions, private authService: AuthService) {}

  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(authenticationActions.login),
      mergeMap(action =>
        this.authService.login(action.user).pipe(
            map(response => authenticationActions.loginSuccess(response)),
          catchError(error => of(authenticationActions.loginFailure({ error })))
        )
      )
    )
  );
}
